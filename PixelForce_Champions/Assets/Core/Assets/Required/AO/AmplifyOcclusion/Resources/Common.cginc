// Amplify Occlusion 2 - Robust Ambient Occlusion for Unity
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

#ifndef AMPLIFY_AO_COMMON_INCLUDED
#define AMPLIFY_AO_COMMON_INCLUDED

#pragma multi_compile_instancing

#include "UnityCG.cginc"

#define PIXEL_RADIUS_LIMIT ( 512 )

#define HALF_MAX        65504.0 // (2 - 2^-10) * 2^15
#define HALF_MAX_MINUS1 65472.0 // (2 - 2^-9) * 2^15
#define DEPTH_EPSILON	1e-6
#define INTENSITY_THRESHOLD 1e-4

inline half DecAO( const half ao )
{
	return sqrt( ao );
}

inline half EncAO( const half ao )
{
	return saturate( ao * ao );
}


float2		_AO_FadeParams;
float4		_AO_FadeValues;

inline half ComputeDistanceFade( const half distance )
{
	return saturate( max( 0.0, distance - _AO_FadeParams.x ) * _AO_FadeParams.y );
}

inline half DecDepth( const float2 aEncodedDepth )
{
	return DecodeFloatRG( aEncodedDepth );
}

inline half2 EncDepth( const half aDepth )
{
	return EncodeFloatRG( aDepth );
}

inline float LinearEyeToSampledDepth( float linearEyeDepth )
{
	return ( 1.0 - linearEyeDepth * _ZBufferParams.w ) / ( linearEyeDepth * _ZBufferParams.z );
}

inline float2 LinearEyeToSampledDepth( float2 linearEyeDepth )
{
	return ( (1.0).xx - linearEyeDepth * ( _ZBufferParams.w ).xx ) / ( linearEyeDepth * ( _ZBufferParams.z ).xx );
}

inline float3 LinearEyeToSampledDepth( float3 linearEyeDepth )
{
	return ( (1.0).xxx - linearEyeDepth * ( _ZBufferParams.w ).xxx ) / ( linearEyeDepth * ( _ZBufferParams.z ).xxx );
}

inline float4 LinearEyeToSampledDepth( float4 linearEyeDepth )
{
	return ( (1.0).xxxx - linearEyeDepth * ( _ZBufferParams.w ).xxxx ) / ( linearEyeDepth * ( _ZBufferParams.z ).xxxx );
}

struct appdata
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};


struct v2f_out
{
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
	UNITY_VERTEX_OUTPUT_STEREO
};


struct v2f_in
{
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	UNITY_VERTEX_INPUT_INSTANCE_ID
	UNITY_VERTEX_OUTPUT_STEREO
};


#if !defined( UNITY_DECLARE_DEPTH_TEXTURE )
#define UNITY_DECLARE_DEPTH_TEXTURE(tex) sampler2D_float tex
#endif

#if !defined( UNITY_DECLARE_SCREENSPACE_TEXTURE )
#define UNITY_DECLARE_SCREENSPACE_TEXTURE(tex) sampler2D tex;
#endif

#if !defined( UNITY_SAMPLE_SCREENSPACE_TEXTURE )
#define UNITY_SAMPLE_SCREENSPACE_TEXTURE(tex, uv) tex2D(tex, uv)
#endif

#if !defined( UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX )
#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(x)
#endif


half4		_AO_Target_TexelSize;

// Jimenez's "Interleaved Gradient Noise"
inline half JimenezNoise( const half2 xyPixelPos )
{
	return frac( 52.9829189 * frac( dot( xyPixelPos, half2( 0.06711056, 0.00583715 ) ) ) );
}


inline void GetSpatialDirections_Offsets_JimenezNoise(	const half2 aScreenPos,
											const half2 aTextureSizeZW,
											out half outNoiseSpatialOffsets,
											out half outNoiseSpatialDirections )
{
#if defined( SHADER_API_D3D9 ) || defined( SHADER_API_MOBILE )
	// Spatial Offsets and Directions - s2016_pbs_activision_occlusion - Slide 93
	const half2 xyPixelPos = ceil( UnityStereoTransformScreenSpaceTex( aScreenPos ) * aTextureSizeZW );
	outNoiseSpatialOffsets = ( 1.0 / 4.0 ) * (half)( frac( ( xyPixelPos.y - xyPixelPos.x ) / 4.0 ) * 4.0 );

	outNoiseSpatialDirections = JimenezNoise( (half2)xyPixelPos );
#else
	// Spatial Offsets and Directions - s2016_pbs_activision_occlusion - Slide 93
	const int2 xyPixelPos = (int2)( UnityStereoTransformScreenSpaceTex( aScreenPos ) * aTextureSizeZW );
	outNoiseSpatialOffsets = ( 1.0 / 4.0 ) * (half)( ( xyPixelPos.y - xyPixelPos.x ) & 3 );

	outNoiseSpatialDirections = JimenezNoise( (half2)xyPixelPos );
#endif
}

inline void GetSpatialDirections_Offsets(	const half2 aScreenPos,
											const half2 aTextureSizeZW,
											out half outNoiseSpatialOffsets,
											out half outNoiseSpatialDirections )
{
#if defined( SHADER_API_D3D9 ) || defined( SHADER_API_MOBILE )
	// Spatial Offsets and Directions - s2016_pbs_activision_occlusion - Slide 93
	const half2 xyPixelPos = ceil( UnityStereoTransformScreenSpaceTex( aScreenPos ) * aTextureSizeZW );
	outNoiseSpatialOffsets = ( 1.0 / 4.0 ) * (half)( frac( ( xyPixelPos.y - xyPixelPos.x ) / 4.0 ) * 4.0 );

	// Noise Spatial Directions
	// X   0  1  2  3
	// Y0 00 05 10 15
	// Y1 04 09 14 03
	// Y2 08 13 02 07
	// Y3 12 01 06 11
	outNoiseSpatialDirections = ( 1.0 / 16.0 ) * (half)( ( ( frac( ( xyPixelPos.x + xyPixelPos.y ) / 4.0 ) * 4.0 ) * 4.0 ) + frac( xyPixelPos.x / 4.0 ) * 4.0 );
#else
	// Spatial Offsets and Directions - s2016_pbs_activision_occlusion - Slide 93
	const int2 xyPixelPos = (int2)( UnityStereoTransformScreenSpaceTex( aScreenPos ) * aTextureSizeZW );
	outNoiseSpatialOffsets = ( 1.0 / 4.0 ) * (half)( ( xyPixelPos.y - xyPixelPos.x ) & 3 );

	// Noise Spatial Directions
	// X   0  1  2  3
	// Y0 00 05 10 15
	// Y1 04 09 14 03
	// Y2 08 13 02 07
	// Y3 12 01 06 11
	outNoiseSpatialDirections = ( 1.0 / 16.0 ) * (half)( ( ( ( xyPixelPos.x + xyPixelPos.y ) & 0x3 ) << 2 ) | ( xyPixelPos.x & 0x3 ) );
#endif
}

#endif
