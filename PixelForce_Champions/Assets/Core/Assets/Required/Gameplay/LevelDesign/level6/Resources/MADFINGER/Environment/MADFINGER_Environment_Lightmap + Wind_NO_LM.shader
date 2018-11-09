Shader "MADFINGER/Environment/Lightmap + Wind_NO_LM" {
Properties {
 _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
 _Wind ("Wind params", Vector) = (1,1,1,1)
 _WindEdgeFlutter ("Wind edge fultter factor", Float) = 0.5
 _WindEdgeFlutterFreqScale ("Wind edge fultter freq scale", Float) = 0.5
}
SubShader { 
 LOD 100
 Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Transparent" "RenderType"="Transparent" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Transparent" "RenderType"="Transparent" }
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha
Program "vp" {
SubProgram "opengl " {
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Matrix 5 [_Object2World]
Matrix 9 [_World2Object]
Vector 13 [_Time]
Vector 14 [_Wind]
Vector 15 [_MainTex_ST]
Float 16 [_WindEdgeFlutter]
Float 17 [_WindEdgeFlutterFreqScale]
"!!ARBvp1.0
PARAM c[20] = { { -0.5, 1, 2, 3 },
		state.matrix.mvp,
		program.local[5..17],
		{ 1.975, 0.79299998, 0.375, 0.193 },
		{ 0.30000001, 0.1 } };
TEMP R0;
TEMP R1;
TEMP R2;
TEMP R3;
MOV R0.x, c[0].y;
DP3 R0.x, R0.x, c[8];
ADD R0.w, R0.x, c[16].x;
MOV R0.y, R0.x;
MOV R0.z, c[17].x;
DP3 R0.x, vertex.position, R0.w;
MAD R0.xy, R0.z, c[13].y, R0;
MUL R0, R0.xxyy, c[18];
FRC R0, R0;
MAD R0, R0, c[0].z, c[0].x;
FRC R0, R0;
MAD R0, R0, c[0].z, -c[0].y;
ABS R0, R0;
MAD R1, -R0, c[0].z, c[0].w;
MUL R0, R0, R0;
MUL R1, R0, R1;
MOV R0.xyz, c[14];
ADD R3.xy, R1.xzzw, R1.ywzw;
DP3 R1.z, R0, c[11];
DP3 R1.y, R0, c[10];
DP3 R1.x, R0, c[9];
MUL R0.xy, vertex.normal.xzzw, c[16].x;
MUL R0.xz, R0.xyyw, c[19].y;
MUL R2.xyz, R1, R3.y;
MUL R0.w, vertex.color, c[14];
MUL R2.xyz, vertex.color.w, R2;
MUL R0.y, vertex.color.w, c[19].x;
MAD R0.xyz, R3.xyxw, R0, R2;
MAD R0.xyz, R0, R0.w, vertex.position;
MOV R0.w, vertex.position;
MAD R0.xyz, vertex.color.w, R1, R0;
DP4 result.position.w, R0, c[4];
DP4 result.position.z, R0, c[3];
DP4 result.position.y, R0, c[2];
DP4 result.position.x, R0, c[1];
MOV result.texcoord[2].xyz, vertex.color;
MAD result.texcoord[0].xy, vertex.texcoord[0], c[15], c[15].zwzw;
END
# 37 instructions, 4 R-regs
"
}
SubProgram "d3d9 " {
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
Matrix 0 [glstate_matrix_mvp]
Matrix 4 [_Object2World]
Matrix 8 [_World2Object]
Vector 12 [_Time]
Vector 13 [_Wind]
Vector 14 [_MainTex_ST]
Float 15 [_WindEdgeFlutter]
Float 16 [_WindEdgeFlutterFreqScale]
"vs_2_0
def c17, 1.00000000, 2.00000000, -0.50000000, -1.00000000
def c18, 1.97500002, 0.79299998, 0.37500000, 0.19300000
def c19, 2.00000000, 3.00000000, 0.30000001, 0.10000000
dcl_position0 v0
dcl_normal0 v1
dcl_texcoord0 v2
dcl_color0 v3
mov r0.xyz, c7
dp3 r0.y, c17.x, r0
add r0.x, r0.y, c15
mov r0.z, c12.y
dp3 r0.x, v0, r0.x
mad r0.xy, c16.x, r0.z, r0
mul r0, r0.xxyy, c18
frc r0, r0
mad r0, r0, c17.y, c17.z
frc r0, r0
mad r0, r0, c17.y, c17.w
abs r0, r0
mad r1, -r0, c19.x, c19.y
mul r0, r0, r0
mul r1, r0, r1
add r3.xy, r1.xzzw, r1.ywzw
mov r0.xyz, c10
dp3 r1.z, c13, r0
mov r0.xyz, c8
dp3 r1.x, c13, r0
mov r2.xyz, c9
dp3 r1.y, c13, r2
mul r0.xy, v1.xzzw, c15.x
mul r0.xz, r0.xyyw, c19.w
mul r2.xyz, r1, r3.y
mul r0.w, v3, c13
mul r2.xyz, v3.w, r2
mul r0.y, v3.w, c19.z
mad r0.xyz, r3.xyxw, r0, r2
mad r0.xyz, r0, r0.w, v0
mov r0.w, v0
mad r0.xyz, v3.w, r1, r0
dp4 oPos.w, r0, c3
dp4 oPos.z, r0, c2
dp4 oPos.y, r0, c1
dp4 oPos.x, r0, c0
mov oT2.xyz, v3
mad oT0.xy, v2, c14, c14.zwzw
"
}
SubProgram "d3d11 " {
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 64
Vector 16 [_Wind]
Vector 32 [_MainTex_ST]
Float 48 [_WindEdgeFlutter]
Float 52 [_WindEdgeFlutterFreqScale]
ConstBuffer "UnityPerCamera" 128
Vector 0 [_Time]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
Matrix 192 [_Object2World]
Matrix 256 [_World2Object]
BindCB  "$Globals" 0
BindCB  "UnityPerCamera" 1
BindCB  "UnityPerDraw" 2
"vs_4_0
eefiecedijcpkfcidcgchfpcnlmnfffgpnhdiaaiabaaaaaaleagaaaaadaaaaaa
cmaaaaaapeaaaaaageabaaaaejfdeheomaaaaaaaagaaaaaaaiaaaaaajiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaakbaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapaaaaaakjaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
ahafaaaalaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaadaaaaaaapadaaaalaaaaaaa
abaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaaljaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaafaaaaaaapapaaaafaepfdejfeejepeoaafeebeoehefeofeaaeoepfc
enebemaafeeffiedepepfceeaaedepemepfcaaklepfdeheogiaaaaaaadaaaaaa
aiaaaaaafaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadamaaaafmaaaaaaacaaaaaaaaaaaaaa
adaaaaaaacaaaaaaahaiaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfcee
aaklklklfdeieefceiafaaaaeaaaabaafcabaaaafjaaaaaeegiocaaaaaaaaaaa
aeaaaaaafjaaaaaeegiocaaaabaaaaaaabaaaaaafjaaaaaeegiocaaaacaaaaaa
bdaaaaaafpaaaaadpcbabaaaaaaaaaaafpaaaaadfcbabaaaacaaaaaafpaaaaad
dcbabaaaadaaaaaafpaaaaadpcbabaaaafaaaaaaghaaaaaepccabaaaaaaaaaaa
abaaaaaagfaaaaaddccabaaaabaaaaaagfaaaaadhccabaaaacaaaaaagiaaaaac
aeaaaaaadgaaaaagbcaabaaaaaaaaaaadkiacaaaacaaaaaaamaaaaaadgaaaaag
ccaabaaaaaaaaaaadkiacaaaacaaaaaaanaaaaaadgaaaaagecaabaaaaaaaaaaa
dkiacaaaacaaaaaaaoaaaaaabaaaaaakccaabaaaaaaaaaaaegacbaaaaaaaaaaa
aceaaaaaaaaaiadpaaaaiadpaaaaiadpaaaaaaaaaaaaaaaiecaabaaaaaaaaaaa
bkaabaaaaaaaaaaaakiacaaaaaaaaaaaadaaaaaabaaaaaahbcaabaaaaaaaaaaa
egbcbaaaaaaaaaaakgakbaaaaaaaaaaadcaaaaalpcaabaaaaaaaaaaafgifcaaa
abaaaaaaaaaaaaaafgifcaaaaaaaaaaaadaaaaaaagafbaaaaaaaaaaadiaaaaak
pcaabaaaaaaaaaaaegaobaaaaaaaaaaaaceaaaaamnmmpmdpamaceldpaaaamado
mlkbefdobkaaaaafpcaabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaappcaabaaa
aaaaaaaaegaobaaaaaaaaaaaaceaaaaaaaaaaaeaaaaaaaeaaaaaaaeaaaaaaaea
aceaaaaaaaaaaalpaaaaaalpaaaaaalpaaaaaalpbkaaaaafpcaabaaaaaaaaaaa
egaobaaaaaaaaaaadcaaaaappcaabaaaaaaaaaaaegaobaaaaaaaaaaaaceaaaaa
aaaaaaeaaaaaaaeaaaaaaaeaaaaaaaeaaceaaaaaaaaaialpaaaaialpaaaaialp
aaaaialpdiaaaaajpcaabaaaabaaaaaaegaobaiaibaaaaaaaaaaaaaaegaobaia
ibaaaaaaaaaaaaaadcaaaabapcaabaaaaaaaaaaaegaobaiambaaaaaaaaaaaaaa
aceaaaaaaaaaaaeaaaaaaaeaaaaaaaeaaaaaaaeaaceaaaaaaaaaeaeaaaaaeaea
aaaaeaeaaaaaeaeadiaaaaahpcaabaaaaaaaaaaaegaobaaaaaaaaaaaegaobaaa
abaaaaaaaaaaaaahhcaabaaaaaaaaaaangafbaaaaaaaaaaaigaabaaaaaaaaaaa
diaaaaajhcaabaaaabaaaaaafgifcaaaaaaaaaaaabaaaaaaegiccaaaacaaaaaa
bbaaaaaadcaaaaalhcaabaaaabaaaaaaegiccaaaacaaaaaabaaaaaaaagiacaaa
aaaaaaaaabaaaaaaegacbaaaabaaaaaadcaaaaalhcaabaaaabaaaaaaegiccaaa
acaaaaaabcaaaaaakgikcaaaaaaaaaaaabaaaaaaegacbaaaabaaaaaadiaaaaah
hcaabaaaacaaaaaafgafbaaaaaaaaaaaegacbaaaabaaaaaadiaaaaahhcaabaaa
acaaaaaaegacbaaaacaaaaaapgbpbaaaafaaaaaadiaaaaaiicaabaaaaaaaaaaa
akiacaaaaaaaaaaaadaaaaaaabeaaaaamnmmmmdndiaaaaahfcaabaaaadaaaaaa
pgapbaaaaaaaaaaaagbcbaaaacaaaaaadiaaaaahccaabaaaadaaaaaadkbabaaa
afaaaaaaabeaaaaajkjjjjdodcaaaaajhcaabaaaaaaaaaaaegacbaaaaaaaaaaa
egacbaaaadaaaaaaegacbaaaacaaaaaadiaaaaaiicaabaaaaaaaaaaadkbabaaa
afaaaaaadkiacaaaaaaaaaaaabaaaaaadcaaaaajhcaabaaaaaaaaaaaegacbaaa
aaaaaaaapgapbaaaaaaaaaaaegbcbaaaaaaaaaaadcaaaaajhcaabaaaaaaaaaaa
pgbpbaaaafaaaaaaegacbaaaabaaaaaaegacbaaaaaaaaaaadiaaaaaipcaabaaa
abaaaaaafgafbaaaaaaaaaaaegiocaaaacaaaaaaabaaaaaadcaaaaakpcaabaaa
abaaaaaaegiocaaaacaaaaaaaaaaaaaaagaabaaaaaaaaaaaegaobaaaabaaaaaa
dcaaaaakpcaabaaaaaaaaaaaegiocaaaacaaaaaaacaaaaaakgakbaaaaaaaaaaa
egaobaaaabaaaaaadcaaaaakpccabaaaaaaaaaaaegiocaaaacaaaaaaadaaaaaa
pgbpbaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaaldccabaaaabaaaaaaegbabaaa
adaaaaaaegiacaaaaaaaaaaaacaaaaaaogikcaaaaaaaaaaaacaaaaaadgaaaaaf
hccabaaaacaaaaaaegbcbaaaafaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
Bind "vertex" Vertex
Bind "color" Color
Bind "normal" Normal
Bind "texcoord" TexCoord0
ConstBuffer "$Globals" 64
Vector 16 [_Wind]
Vector 32 [_MainTex_ST]
Float 48 [_WindEdgeFlutter]
Float 52 [_WindEdgeFlutterFreqScale]
ConstBuffer "UnityPerCamera" 128
Vector 0 [_Time]
ConstBuffer "UnityPerDraw" 336
Matrix 0 [glstate_matrix_mvp]
Matrix 192 [_Object2World]
Matrix 256 [_World2Object]
BindCB  "$Globals" 0
BindCB  "UnityPerCamera" 1
BindCB  "UnityPerDraw" 2
"vs_4_0_level_9_1
eefiecedicocbjbehcggbhgonhbfmcmipedomdhmabaaaaaaceakaaaaaeaaaaaa
daaaaaaajmadaaaaomaiaaaaleajaaaaebgpgodjgeadaaaageadaaaaaaacpopp
aaadaaaageaaaaaaafaaceaaaaaagaaaaaaagaaaaaaaceaaabaagaaaaaaaabaa
adaaabaaaaaaaaaaabaaaaaaabaaaeaaaaaaaaaaacaaaaaaaeaaafaaaaaaaaaa
acaaamaaadaaajaaaaaaaaaaacaabaaaadaaamaaaaaaaaaaaaaaaaaaaaacpopp
fbaaaaafapaaapkaaaaaiadpaaaaaaeaaaaaaalpaaaaialpfbaaaaafbaaaapka
mnmmpmdpamaceldpaaaamadomlkbefdofbaaaaafbbaaapkaaaaaaaeaaaaaeaea
mnmmmmdnjkjjjjdobpaaaaacafaaaaiaaaaaapjabpaaaaacafaaaciaacaaapja
bpaaaaacafaaadiaadaaapjabpaaaaacafaaafiaafaaapjaaeaaaaaeaaaaadoa
adaaoejaacaaoekaacaaookaabaaaaacaaaaabiaajaappkaabaaaaacaaaaacia
akaappkaabaaaaacaaaaaeiaalaappkaaiaaaaadaaaaaciaaaaaoeiaapaaaaka
acaaaaadaaaaaeiaaaaaffiaadaaaakaaiaaaaadaaaaabiaaaaaoejaaaaakkia
abaaaaacabaaadiaadaaoekaaeaaaaaeaaaaapiaaeaaffkaabaaffiaaaaafaia
afaaaaadaaaaapiaaaaaoeiabaaaoekabdaaaaacaaaaapiaaaaaoeiaaeaaaaae
aaaaapiaaaaaoeiaapaaffkaapaakkkabdaaaaacaaaaapiaaaaaoeiaaeaaaaae
aaaaapiaaaaaoeiaapaaffkaapaappkacdaaaaacaaaaapiaaaaaoeiaafaaaaad
acaaapiaaaaaoeiaaaaaoeiaaeaaaaaeaaaaapiaaaaaoeiabbaaaakbbbaaffka
afaaaaadaaaaapiaaaaaoeiaacaaoeiaacaaaaadaaaaahiaaaaanniaaaaamiia
abaaaaacacaaahiaabaaoekaafaaaaadabaaaoiaacaaffiaanaajakaaeaaaaae
abaaaoiaamaajakaacaaaaiaabaaoeiaaeaaaaaeabaaaoiaaoaajakaacaakkia
abaaoeiaafaaaaadacaaahiaaaaaffiaabaapjiaafaaaaadacaaahiaacaaoeia
afaappjaafaaaaadaaaaaiiaabaaaaiabbaakkkaafaaaaadadaaafiaaaaappia
acaaoejaafaaaaadadaaaciaafaappjabbaappkaaeaaaaaeaaaaahiaaaaaoeia
adaaoeiaacaaoeiaafaaaaadaaaaaiiaafaappjaabaappkaaeaaaaaeaaaaahia
aaaaoeiaaaaappiaaaaaoejaaeaaaaaeaaaaahiaafaappjaabaapjiaaaaaoeia
afaaaaadabaaapiaaaaaffiaagaaoekaaeaaaaaeabaaapiaafaaoekaaaaaaaia
abaaoeiaaeaaaaaeaaaaapiaahaaoekaaaaakkiaabaaoeiaaeaaaaaeaaaaapia
aiaaoekaaaaappjaaaaaoeiaaeaaaaaeaaaaadmaaaaappiaaaaaoekaaaaaoeia
abaaaaacaaaaammaaaaaoeiaabaaaaacabaaahoaafaaoejappppaaaafdeieefc
eiafaaaaeaaaabaafcabaaaafjaaaaaeegiocaaaaaaaaaaaaeaaaaaafjaaaaae
egiocaaaabaaaaaaabaaaaaafjaaaaaeegiocaaaacaaaaaabdaaaaaafpaaaaad
pcbabaaaaaaaaaaafpaaaaadfcbabaaaacaaaaaafpaaaaaddcbabaaaadaaaaaa
fpaaaaadpcbabaaaafaaaaaaghaaaaaepccabaaaaaaaaaaaabaaaaaagfaaaaad
dccabaaaabaaaaaagfaaaaadhccabaaaacaaaaaagiaaaaacaeaaaaaadgaaaaag
bcaabaaaaaaaaaaadkiacaaaacaaaaaaamaaaaaadgaaaaagccaabaaaaaaaaaaa
dkiacaaaacaaaaaaanaaaaaadgaaaaagecaabaaaaaaaaaaadkiacaaaacaaaaaa
aoaaaaaabaaaaaakccaabaaaaaaaaaaaegacbaaaaaaaaaaaaceaaaaaaaaaiadp
aaaaiadpaaaaiadpaaaaaaaaaaaaaaaiecaabaaaaaaaaaaabkaabaaaaaaaaaaa
akiacaaaaaaaaaaaadaaaaaabaaaaaahbcaabaaaaaaaaaaaegbcbaaaaaaaaaaa
kgakbaaaaaaaaaaadcaaaaalpcaabaaaaaaaaaaafgifcaaaabaaaaaaaaaaaaaa
fgifcaaaaaaaaaaaadaaaaaaagafbaaaaaaaaaaadiaaaaakpcaabaaaaaaaaaaa
egaobaaaaaaaaaaaaceaaaaamnmmpmdpamaceldpaaaamadomlkbefdobkaaaaaf
pcaabaaaaaaaaaaaegaobaaaaaaaaaaadcaaaaappcaabaaaaaaaaaaaegaobaaa
aaaaaaaaaceaaaaaaaaaaaeaaaaaaaeaaaaaaaeaaaaaaaeaaceaaaaaaaaaaalp
aaaaaalpaaaaaalpaaaaaalpbkaaaaafpcaabaaaaaaaaaaaegaobaaaaaaaaaaa
dcaaaaappcaabaaaaaaaaaaaegaobaaaaaaaaaaaaceaaaaaaaaaaaeaaaaaaaea
aaaaaaeaaaaaaaeaaceaaaaaaaaaialpaaaaialpaaaaialpaaaaialpdiaaaaaj
pcaabaaaabaaaaaaegaobaiaibaaaaaaaaaaaaaaegaobaiaibaaaaaaaaaaaaaa
dcaaaabapcaabaaaaaaaaaaaegaobaiambaaaaaaaaaaaaaaaceaaaaaaaaaaaea
aaaaaaeaaaaaaaeaaaaaaaeaaceaaaaaaaaaeaeaaaaaeaeaaaaaeaeaaaaaeaea
diaaaaahpcaabaaaaaaaaaaaegaobaaaaaaaaaaaegaobaaaabaaaaaaaaaaaaah
hcaabaaaaaaaaaaangafbaaaaaaaaaaaigaabaaaaaaaaaaadiaaaaajhcaabaaa
abaaaaaafgifcaaaaaaaaaaaabaaaaaaegiccaaaacaaaaaabbaaaaaadcaaaaal
hcaabaaaabaaaaaaegiccaaaacaaaaaabaaaaaaaagiacaaaaaaaaaaaabaaaaaa
egacbaaaabaaaaaadcaaaaalhcaabaaaabaaaaaaegiccaaaacaaaaaabcaaaaaa
kgikcaaaaaaaaaaaabaaaaaaegacbaaaabaaaaaadiaaaaahhcaabaaaacaaaaaa
fgafbaaaaaaaaaaaegacbaaaabaaaaaadiaaaaahhcaabaaaacaaaaaaegacbaaa
acaaaaaapgbpbaaaafaaaaaadiaaaaaiicaabaaaaaaaaaaaakiacaaaaaaaaaaa
adaaaaaaabeaaaaamnmmmmdndiaaaaahfcaabaaaadaaaaaapgapbaaaaaaaaaaa
agbcbaaaacaaaaaadiaaaaahccaabaaaadaaaaaadkbabaaaafaaaaaaabeaaaaa
jkjjjjdodcaaaaajhcaabaaaaaaaaaaaegacbaaaaaaaaaaaegacbaaaadaaaaaa
egacbaaaacaaaaaadiaaaaaiicaabaaaaaaaaaaadkbabaaaafaaaaaadkiacaaa
aaaaaaaaabaaaaaadcaaaaajhcaabaaaaaaaaaaaegacbaaaaaaaaaaapgapbaaa
aaaaaaaaegbcbaaaaaaaaaaadcaaaaajhcaabaaaaaaaaaaapgbpbaaaafaaaaaa
egacbaaaabaaaaaaegacbaaaaaaaaaaadiaaaaaipcaabaaaabaaaaaafgafbaaa
aaaaaaaaegiocaaaacaaaaaaabaaaaaadcaaaaakpcaabaaaabaaaaaaegiocaaa
acaaaaaaaaaaaaaaagaabaaaaaaaaaaaegaobaaaabaaaaaadcaaaaakpcaabaaa
aaaaaaaaegiocaaaacaaaaaaacaaaaaakgakbaaaaaaaaaaaegaobaaaabaaaaaa
dcaaaaakpccabaaaaaaaaaaaegiocaaaacaaaaaaadaaaaaapgbpbaaaaaaaaaaa
egaobaaaaaaaaaaadcaaaaaldccabaaaabaaaaaaegbabaaaadaaaaaaegiacaaa
aaaaaaaaacaaaaaaogikcaaaaaaaaaaaacaaaaaadgaaaaafhccabaaaacaaaaaa
egbcbaaaafaaaaaadoaaaaabejfdeheomaaaaaaaagaaaaaaaiaaaaaajiaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapapaaaakbaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaapaaaaaakjaaaaaaaaaaaaaaaaaaaaaaadaaaaaaacaaaaaa
ahafaaaalaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaadaaaaaaapadaaaalaaaaaaa
abaaaaaaaaaaaaaaadaaaaaaaeaaaaaaapaaaaaaljaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaafaaaaaaapapaaaafaepfdejfeejepeoaafeebeoehefeofeaaeoepfc
enebemaafeeffiedepepfceeaaedepemepfcaaklepfdeheogiaaaaaaadaaaaaa
aiaaaaaafaaaaaaaaaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaa
aaaaaaaaaaaaaaaaadaaaaaaabaaaaaaadamaaaafmaaaaaaacaaaaaaaaaaaaaa
adaaaaaaacaaaaaaahaiaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfcee
aaklklkl"
}
}
Program "fp" {
SubProgram "opengl " {
SetTexture 0 [_MainTex] 2D 0
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
TEX result.color, fragment.texcoord[0], texture[0], 2D;
END
# 1 instructions, 0 R-regs
"
}
SubProgram "d3d9 " {
SetTexture 0 [_MainTex] 2D 0
"ps_2_0
dcl_2d s0
dcl t0.xy
texld r0, t0, s0
mov_pp oC0, r0
"
}
SubProgram "d3d11 " {
SetTexture 0 [_MainTex] 2D 0
"ps_4_0
eefiecedkngggoceknopbiflolehcklodmjfepckabaaaaaadmabaaaaadaaaaaa
cmaaaaaajmaaaaaanaaaaaaaejfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaa
aaaaaaaaabaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaa
adaaaaaaabaaaaaaadadaaaafmaaaaaaacaaaaaaaaaaaaaaadaaaaaaacaaaaaa
ahaaaaaafdfgfpfaepfdejfeejepeoaafeeffiedepepfceeaaklklklepfdeheo
cmaaaaaaabaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaa
apaaaaaafdfgfpfegbhcghgfheaaklklfdeieefcgeaaaaaaeaaaaaaabjaaaaaa
fkaaaaadaagabaaaaaaaaaaafibiaaaeaahabaaaaaaaaaaaffffaaaagcbaaaad
dcbabaaaabaaaaaagfaaaaadpccabaaaaaaaaaaaefaaaaajpccabaaaaaaaaaaa
egbabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaaaaaaaaaadoaaaaab"
}
SubProgram "d3d11_9x " {
SetTexture 0 [_MainTex] 2D 0
"ps_4_0_level_9_1
eefiecedochfimlcbbiepimpbgdkknpkhompiaieabaaaaaakmabaaaaaeaaaaaa
daaaaaaajmaaaaaaaiabaaaahiabaaaaebgpgodjgeaaaaaageaaaaaaaaacpppp
dmaaaaaaciaaaaaaaaaaciaaaaaaciaaaaaaciaaabaaceaaaaaaciaaaaaaaaaa
aaacppppbpaaaaacaaaaaaiaaaaaadlabpaaaaacaaaaaajaaaaiapkaecaaaaad
aaaacpiaaaaaoelaaaaioekaabaaaaacaaaicpiaaaaaoeiappppaaaafdeieefc
geaaaaaaeaaaaaaabjaaaaaafkaaaaadaagabaaaaaaaaaaafibiaaaeaahabaaa
aaaaaaaaffffaaaagcbaaaaddcbabaaaabaaaaaagfaaaaadpccabaaaaaaaaaaa
efaaaaajpccabaaaaaaaaaaaegbabaaaabaaaaaaeghobaaaaaaaaaaaaagabaaa
aaaaaaaadoaaaaabejfdeheogiaaaaaaadaaaaaaaiaaaaaafaaaaaaaaaaaaaaa
abaaaaaaadaaaaaaaaaaaaaaapaaaaaafmaaaaaaaaaaaaaaaaaaaaaaadaaaaaa
abaaaaaaadadaaaafmaaaaaaacaaaaaaaaaaaaaaadaaaaaaacaaaaaaahaaaaaa
fdfgfpfaepfdejfeejepeoaafeeffiedepepfceeaaklklklepfdeheocmaaaaaa
abaaaaaaaiaaaaaacaaaaaaaaaaaaaaaaaaaaaaaadaaaaaaaaaaaaaaapaaaaaa
fdfgfpfegbhcghgfheaaklkl"
}
}
 }
}
}