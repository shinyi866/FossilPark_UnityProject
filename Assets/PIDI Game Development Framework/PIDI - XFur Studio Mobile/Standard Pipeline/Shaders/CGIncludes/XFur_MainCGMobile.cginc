/*
XFur Studio™ Mobile v 1.0

You cannot sell, redistribute, share nor make public this code, even modified, through any means on any platform.
Modifications are allowed only for your own use and to make this product suit better your project's needs.
These modifications may not be redistributed, sold or shared in any way.

For more information, contact me at support@irreverent-software.com

Copyright© 2018, Jorge Pinal Negrete. All Rights Reserved.
*/


//=====================  VARIABLES USED BY XFUR Mobile 1.0 AND ABOVE  ========================

//Textures
sampler2D _BaseTex; //PER MATERIAL/INSTANCE. Skin diffuse color
sampler2D _Normalmap; 
sampler2D _GlossSpecular;
sampler2D _FurColorMap;
sampler2D _FurNoiseMap;
sampler2D _FurData0;
sampler2D _FurData1;
sampler2D _PhysicsData;
sampler2D _FXData;
sampler2D _FurPainter;
sampler2D _OcclusionMap; //NOT READY
sampler2D _FurFXNoise; //GLOBAL. Noise to be applied to the fur FX. RG = First noise / tone variation map, BA = Second noise / tone variation map
sampler2D _VertexNormals;
sampler2D _FurFXMap;

//Colors
fixed4 _BaseColor;
fixed4 _BaseSpecular; //PER INSTANCE. Specular color for the skin.
fixed4 _FurColorA;
fixed4 _FurColorB;
fixed4 _FurSpecular; //PER INSTANCE. Specular color for the fur. 
fixed4 _FXColor0;
fixed4 _FXColor1;
fixed4 _FXColor2;
fixed4 _FXColor3;
fixed4 _FXSpecSmooth0;
fixed4 _FXSpecSmooth1;
fixed4 _FXSpecSmooth2;
fixed4 _FXSpecSmooth3;
fixed4 _RimColor; //REVIEW MODE. might be removed. Controls the final coloration of the rim lighting applied to the fur

//Toggles
half _PerInstanceBaseSpec;
half _PerInstanceFurNoise;
half _PerInstanceFurData0;
half _PerInstanceFurData1;
half _PerInstanceFurData2;
half _HasGlossMap;
half _TriplanarMode;
half _UV2Grooming;
half _UV2Painting;
half _SelfCollision;

//Numerics
half _UV0Scale1;
half _UV0Scale2;
half _UV1Scale1;
half _UV1Scale2;
half _FurStep; //INTERNALLY DEFINED
half _FurLength; //PER MATERIAL/INSTANCE. Controls the fur length of this instance.
half _FurStiffness; //PER MATERIAL/INSTANCE. Controls the fur stiffness of this instance.
half _FurThin; //PER MATERIAL/INSTANCE. Controls the fur thickness of this instance.
half _FurOcclusion; //PER MATERIAL/INSTANCE. Controls the fur occlusion of this instance.
half _FurShadowing; //PER MATERIAL/INSTANCE. Controls the fur shadowin of this instance.
half _FurRimStrength;
half _WindSpeed; //GLOBAL. Wind Speed
half _LocalWindStrength; //PER INSTANCE. The global wind strength will be multiplied by this value.
half _FurCutoff;
half _BaseSmoothness; //PER INSTANCE. Controls smoothness of the base skin.
half _FurSmoothness; //PER INSTANCE. Controls the smoothness of the fur.
half _FurPasses;

//Numeric Data
half _FXTexSize;
half _TriplanarScale = 2;
half _AnisotropicOffset; //PER INSTANCE. Controls the offset of the anisotropic specularity effects
half _LocalPhysicsStrength; //PER INSTANCE. The final physics influence will be multiplied by this value.
half _FX0Penetration; //PER INSTANCE. Global opacity of the first fur FX.
half _FX1Penetration; //PER INSTANCE. Global opacity of the second fur FX.
half _FX2Penetration; //PER INSTANCE. Global opacity of the third fur FX.
half _FX3Penetration; //PER INSTANCE. Global opacity of the fourth fur FX.
half _FurGravity;
half _FurPhysics;

//Vectorial Data
fixed4 _FurDirection; //PER INSTANCE. Global direction applied to the fur on a per-object basis

//Vectorial forces
fixed4 _WindDirection; //GLOBAL. Direction / Strength of the wind

//Vectorial Physics
fixed4 _VectorPhysics[128]; //PER INSTANCE. Vectorial forces to be used for the new Physics module. Up to 128 bones are supported.
fixed4 _AnglePhysics[128]; //PER INSTANCE. Angular forces to be used for the new Physics module.


struct Input {
	float2 uv_BaseTex;
	float2 uv2_FurNoiseMap;
    float3 viewDir;
    float4 vertexPos;
    float4 vertexNormal;
    float samplePass;
    fixed4 color:COLOR;
};



