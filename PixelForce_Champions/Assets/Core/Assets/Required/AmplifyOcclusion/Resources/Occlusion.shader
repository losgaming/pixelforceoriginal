// Amplify Occlusion 2 - Robust Ambient Occlusion for Unity
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

Shader "Hidden/Amplify Occlusion/Occlusion"
{
	CGINCLUDE
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 3.0
		#pragma exclude_renderers gles d3d11_9x n3ds
		#include "GTAO.cginc"

		UNITY_DECLARE_SCREENSPACE_TEXTURE( _AO_Source );

		v2f_out vert( appdata v )
		{
			v2f_out o;
			UNITY_SETUP_INSTANCE_ID( v );
			UNITY_TRANSFER_INSTANCE_ID( v, o );
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

			float4 vertex = v.vertex * float4( 2.0, 2.0, 1.0, 1.0 ) + float4( -1.0, -1.0, 0.0, 0.0 );

			#ifdef UNITY_HALF_TEXEL_OFFSET
			vertex.xy += ( 1.0 / _AO_Target_TexelSize.zw ) * float2( -1.0, 1.0 );
			#endif

			o.pos = vertex;

			#ifdef UNITY_SINGLE_PASS_STEREO
			#if UNITY_UV_STARTS_AT_TOP
			o.uv = float2( v.uv.x, 1.0 - v.uv.y );
			#else
			o.uv = v.uv;
			#endif
			#else
			o.uv = ComputeScreenPos( vertex ).xy;
			#endif

			return o;
		}


		half4 GTAO( const v2f_in ifrag, const int directionCount, const int sampleCount, const int normalSource )
		{
			UNITY_SETUP_INSTANCE_ID( ifrag );
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( ifrag );

			half outDepth;
			half4 outRGBA;

			GetGTAO( ifrag, directionCount, sampleCount / 2, normalSource, outDepth, outRGBA );

			return half4( outRGBA.a, outDepth, 0, 0 );
		}

		half4 CombineDownsampledOcclusionDepth( const v2f_in ifrag )
		{
			UNITY_SETUP_INSTANCE_ID( ifrag );
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( ifrag );

			const half2 screenPos = ifrag.uv.xy;

			const half depthSample = SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, UnityStereoTransformScreenSpaceTex( screenPos ) );

			const half referenceDepth = LinearEyeDepth( depthSample );

			const half intensity = lerp( _AO_Levels.a, _AO_FadeValues.x, ComputeDistanceFade( referenceDepth ) );

			//UNITY_BRANCH
			#if defined(UNITY_REVERSED_Z)
			if( ( depthSample <= DEPTH_EPSILON ) || ( intensity < INTENSITY_THRESHOLD ) )
			#else
			if( ( depthSample >= ( 1.0 - DEPTH_EPSILON ) ) || ( intensity < INTENSITY_THRESHOLD ) )
			#endif
			{
				return half4( 1.0, HALF_MAX, 0, 0 );
			}

			const half2 screenPosPixels = screenPos * _AO_Source_TexelSize.zw;
			const half2 screenPosPixelsFloor = floor( screenPosPixels );
			const half2 screenPosPixelsDelta = screenPosPixels - screenPosPixelsFloor;

			const half2 sPosAdjusted = screenPosPixelsFloor * _AO_Source_TexelSize.xy + half2( 0.5, 0.5 ) * _AO_Source_TexelSize.xy;
			const half s = ( screenPosPixelsDelta.y < 0.5 )?-1.0:1.0;

			half2 odC = UNITY_SAMPLE_SCREENSPACE_TEXTURE( _AO_Source, UnityStereoTransformScreenSpaceTex( sPosAdjusted ) ).rg;

			half2 odL = UNITY_SAMPLE_SCREENSPACE_TEXTURE( _AO_Source, UnityStereoTransformScreenSpaceTex( sPosAdjusted + half2( -1.0, 0.0 ) * _AO_Source_TexelSize.xy ) ).rg;
			half2 odR = UNITY_SAMPLE_SCREENSPACE_TEXTURE( _AO_Source, UnityStereoTransformScreenSpaceTex( sPosAdjusted + half2( +1.0, 0.0 ) * _AO_Source_TexelSize.xy ) ).rg;
			half2 odM = UNITY_SAMPLE_SCREENSPACE_TEXTURE( _AO_Source, UnityStereoTransformScreenSpaceTex( sPosAdjusted + half2(  0.0,   s ) * _AO_Source_TexelSize.xy ) ).rg;

			const half4 o0123 = half4( odC.x, odL.x, odR.x, odM.x );
			const half4 d0123 = half4( odC.y, odL.y, odR.y, odM.y );

			half4 depthWeight0123 = saturate( 1.0 / ( abs( LinearEyeToSampledDepth( d0123 ) - ( depthSample ).xxxx ) * 32768 + 0.95 ) );

			const half4 pixelDeltaWeight = half4( screenPosPixelsDelta.x * screenPosPixelsDelta.y + 0.5,
													1.0 - screenPosPixelsDelta.x,
													screenPosPixelsDelta.x,
													0.80 );

			depthWeight0123 = depthWeight0123 * depthWeight0123 * pixelDeltaWeight;

			half weightOcclusion = dot( o0123, depthWeight0123 );

			const half outOcclusion = saturate( weightOcclusion / dot( ( 1 ).xxxx, depthWeight0123 ) );

			return half4( outOcclusion, referenceDepth, 0, 0 );
		}


		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		UNITY_DECLARE_SCREENSPACE_TEXTURE( _CameraMotionVectorsTexture );
		#else
		sampler2D_half _CameraMotionVectorsTexture;
		#endif

		float4	_CameraMotionVectorsTexture_TexelSize;

		float		_AO_TemporalMotionSensibility;

		inline half2 FetchMotion( const half2 aUV )
		{
			return UNITY_SAMPLE_SCREENSPACE_TEXTURE( _CameraMotionVectorsTexture, UnityStereoTransformScreenSpaceTex( aUV) ).rg;
		}


		inline half FetchNeighborMotion( const half2 aUV, const half aRadiusPixels, const half2 aCentralmv )
		{
			half noiseSpatialOffsets, noiseSpatialDirections;
			GetSpatialDirections_Offsets_JimenezNoise( aUV, _CameraMotionVectorsTexture_TexelSize.zw * 0.5, noiseSpatialOffsets, noiseSpatialDirections );

			const half angle = ( noiseSpatialDirections ) * UNITY_PI;
			const half2 cos_sin = half2( cos( angle ), sin( angle ) );
			const half2 xy = ( aRadiusPixels * noiseSpatialOffsets + 8.0 ).xx * cos_sin;

			const half2 xLyT = FetchMotion( half2( -xy.x, -xy.y ) * _CameraMotionVectorsTexture_TexelSize.xy + aUV );
			const half2 xRyD = FetchMotion( half2( +xy.x, +xy.y ) * _CameraMotionVectorsTexture_TexelSize.xy + aUV );

			const half2 xLyT_v = xLyT - aCentralmv;
			const half2 xRyD_v = xRyD - aCentralmv;

			const half2 sqrLength2 = half2( dot( xLyT_v, xLyT_v ), dot( xRyD_v, xRyD_v ) );
			const half neighborMotion = max( sqrLength2.x, sqrLength2.y );

			const half oneOverPixelDeltaSquared = 1.0 / ( _CameraMotionVectorsTexture_TexelSize.x * _CameraMotionVectorsTexture_TexelSize.x );

			const half outNeighborMotion = saturate( neighborMotion * oneOverPixelDeltaSquared - 1.0 ) * _AO_TemporalMotionSensibility;

			return outNeighborMotion;
		}


		half NeighborMotionIntensity( v2f_in IN )
		{
			UNITY_SETUP_INSTANCE_ID( IN );
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

			const float2 screenPos = IN.uv.xy;

			const half depthSample = SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, UnityStereoTransformScreenSpaceTex( screenPos ) );

			if( depthSample < HALF_MAX )
			{
				half linearEyeDepth = LinearEyeDepth( depthSample );

				const half radius = lerp( _AO_Radius, _AO_FadeValues.y, ComputeDistanceFade( linearEyeDepth ) );
				const half radiusToScreen = radius * _AO_HalfProjScale;
				const half screenRadius = max( min( ( radiusToScreen / linearEyeDepth ), PIXEL_RADIUS_LIMIT ), 2 );

				const half2 cMv = FetchMotion( screenPos );

				const half motionNeighborDisoclusion = FetchNeighborMotion( screenPos, screenRadius, cMv );

				return motionNeighborDisoclusion;
			}
			else
			{
				return 0;
			}
		}


		half4 ClearTemporal( const v2f_in ifrag )
		{
			UNITY_SETUP_INSTANCE_ID( ifrag );
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( ifrag );

			return half4( EncAO( 0.99 ), 1.0, 1.0, 0.0 );
		}

	ENDCG

	SubShader
	{
		ZTest Always
		Cull Off
		ZWrite Off

		// 0-3 => FULL OCCLUSION - LOW QUALITY                    directionCount / sampleCount
		Pass { CGPROGRAM half4 frag( v2f_in IN ) : SV_Target { return GTAO( IN, 2, 4, NORMALS_NONE ); } ENDCG }
		Pass { CGPROGRAM half4 frag( v2f_in IN ) : SV_Target { return GTAO( IN, 2, 4, NORMALS_CAMERA ); } ENDCG }
		Pass { CGPROGRAM half4 frag( v2f_in IN ) : SV_Target { return GTAO( IN, 2, 4, NORMALS_GBUFFER ); } ENDCG }
		Pass { CGPROGRAM half4 frag( v2f_in IN ) : SV_Target { return GTAO( IN, 2, 4, NORMALS_GBUFFER_OCTA_ENCODED ); } ENDCG }

		// 04-07 => FULL OCCLUSION / MEDIUM QUALITY
		Pass { CGPROGRAM half4 frag( v2f_in IN ) : SV_Target { return GTAO( IN, 2, 6, NORMALS_NONE ); } ENDCG }
		Pass { CGPROGRAM half4 frag( v2f_in IN ) : SV_Target { return GTAO( IN, 2, 6, NORMALS_CAMERA ); } ENDCG }
		Pass { CGPROGRAM half4 frag( v2f_in IN ) : SV_Target { return GTAO( IN, 2, 6, NORMALS_GBUFFER ); } ENDCG }
		Pass { CGPROGRAM half4 frag( v2f_in IN ) : SV_Target { return GTAO( IN, 2, 6, NORMALS_GBUFFER_OCTA_ENCODED ); } ENDCG }

		// 08-11 => FULL OCCLUSION - HIGH QUALITY
		Pass { CGPROGRAM half4 frag( v2f_in IN ) : SV_Target { return GTAO( IN, 3, 8, NORMALS_NONE ); } ENDCG }
		Pass { CGPROGRAM half4 frag( v2f_in IN ) : SV_Target { return GTAO( IN, 3, 8, NORMALS_CAMERA ); } ENDCG }
		Pass { CGPROGRAM half4 frag( v2f_in IN ) : SV_Target { return GTAO( IN, 3, 8, NORMALS_GBUFFER ); } ENDCG }
		Pass { CGPROGRAM half4 frag( v2f_in IN ) : SV_Target { return GTAO( IN, 3, 8, NORMALS_GBUFFER_OCTA_ENCODED ); } ENDCG }

		// 12-15 => FULL OCCLUSION / VERYHIGH QUALITY
		Pass { CGPROGRAM half4 frag( v2f_in IN ) : SV_Target { return GTAO( IN, 4, 10, NORMALS_NONE ); } ENDCG }
		Pass { CGPROGRAM half4 frag( v2f_in IN ) : SV_Target { return GTAO( IN, 4, 10, NORMALS_CAMERA ); } ENDCG }
		Pass { CGPROGRAM half4 frag( v2f_in IN ) : SV_Target { return GTAO( IN, 4, 10, NORMALS_GBUFFER ); } ENDCG }
		Pass { CGPROGRAM half4 frag( v2f_in IN ) : SV_Target { return GTAO( IN, 4, 10, NORMALS_GBUFFER_OCTA_ENCODED ); } ENDCG }

		// 16 => CombineDownsampledOcclusionDepth
		Pass { CGPROGRAM half4 frag( v2f_in IN ) : SV_Target { return CombineDownsampledOcclusionDepth( IN );	} ENDCG	}

		// 17 => Neighbor Motion Intensity
		Pass { CGPROGRAM half2 frag( v2f_in IN ) : SV_Target { return NeighborMotionIntensity( IN ); } ENDCG }

		// 18 => Clear Temporal
		Pass { CGPROGRAM half4 frag( v2f_in IN ) : SV_Target { return ClearTemporal( IN ); } ENDCG }
	}
}

