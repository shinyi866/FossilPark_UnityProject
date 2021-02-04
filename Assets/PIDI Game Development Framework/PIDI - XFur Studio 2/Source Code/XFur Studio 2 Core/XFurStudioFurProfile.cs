﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace XFurStudio2 {

    [System.Serializable]
    public struct XFurTemplate {

        public bool DoubleSided;

        public bool ProbeUse;
        
        /// <summary> The maximum length of the fur for this material </summary>
        public float FurLength;

        /// <summary> The general thickness of the fur </summary>
        public float FurThickness;

        /// <summary> The curve that controls the thickness variation of each strand as a curve </summary>
        public float FurThicknessCurve;

        /// <summary> The shadowing to be applied on the fur, applied to the strands from root to top </summary>
        public float FurOcclusion;
       
        /// <summary> The shadowing to be applied on the fur, applied to the strands from root to top </summary>
        public float FurOcclusionCurve;

        /// <summary>The thickness of the back lighting (rim lighting) effect</summary>
        public float FurRimPower;

        public float FurSmoothness;

        /// <summary> The fur generation map ID (used by default) to read a fur map from the database </summary>
        //public int FurGenerationMapID;

        /// <summary> The fur generation map (procedural or Texture) stored in an asset </summary>
        public XFurStudioStrandsAsset FurStrandsAsset;

        /// <summary> The way the fur strands should be projected over the surface : 0 = UV2 Simple, 1 = UV2 Schotastic, 2 = Triplanar Schotastic  </summary>
        //public int FurProjectionMode;

        /// <summary>The texture that controls how the additional 4 color tints are blended over the fur's surface</summary>
        public Texture FurColorVariation;

        /// <summary>The overall tint to be applied to the fur</summary>
        public Color FurMainTint;

        /// <summary>The color to be blended by the red channel of the Color Variation Map</summary>
        public Color FurColorA;

        /// <summary>The color to be blended by the green channel of the Color Variation Map</summary>
        public Color FurColorB;

        /// <summary>The color to be blended by the blue channel of the Color Variation Map</summary>
        public Color FurColorC;

        /// <summary>The color to be blended by the alpha channel of the Color Variation Map</summary>
        public Color FurColorD;

        /// <summary>The color of the occlusion / shadows to be applied to the fur</summary>
        public Color FurShadowsTint;

        /// <summary>The color applied to the rim lighting effect</summary>
        public Color FurRim;

        public float FurRimBoost;

        public float FurUnderColorMod;

        public float FurOverColorMod;

        public float FurCurlAmountX, FurCurlAmountY, FurCurlSizeX, FurCurlSizeY;

        //public bool Virtual3DStrands;

        //public Texture FurBlendSplatmap;

        //public int[] BlendedProfiles;

        private bool SetupProperties;

        #region PER-INSTANCE PARAMETERS

        /// <summary> Whether the fur will receive shadows or not</summary>
        public bool ReceiveShadows;

        /// <summary> Whether the fur will cast shadows or not</summary>
        public bool CastShadows;

        /// <summary> The amount of samples that the fur will use (1-64/128 for XFShells, 4 or 8 for BasicShells)</summary>
        public int FurSamples;

        /// <summary>The tiling to be applied to the Fur Strands Map</summary>
        public float FurUVTiling;

        /// <summary>The texture that gives color to the fur</summary>
        public Texture FurColorMap;

        /// <summary>The texture that controls fur coverage, length, occlusion and thickness</summary>
        public Texture FurData0;

        /// <summary>The texture that controls fur grooming direction (RGB) and stiffness (A)</summary>
        public Texture FurData1;

        /// <summary>The local multiplier for the overall wind simulation strength</summary>
        public float SelfWindStrength;

        /// <summary>The texture to be applied to the simple "skin" pass on the Basic Shell shaders</summary>
        public Texture SkinColorMap;

        /// <summary>The normal map to be applied to the simple "skin" pass on the Basic Shell shaders</summary>
        public Texture SkinNormalMap;

        /// <summary>The final tint to be applied to the simple "skin" pass on Basic Shell shaders</summary>
        public Color SkinColor;


        /// <summary>The final smoothness to be applied to the simple "skin" pass on Basic Shell shaders</summary>
        public float SkinSmoothness;

#endregion

        public XFurTemplate( bool withDefaults ) {
            DoubleSided = false;
            ProbeUse = true;
            ReceiveShadows = CastShadows = true;
            FurLength = 0.1f;
            FurThickness = 1.0f;
            FurSmoothness = 0.2f;
            FurThicknessCurve = 0.5f;
            FurShadowsTint = Color.black;
            FurOcclusion = 1.0f;
            FurOcclusionCurve = 0.5f;
            
            FurUnderColorMod = 0.25f;
            FurOverColorMod = 0.5f;

            FurStrandsAsset = null;
            FurSamples = 30;
            FurColorMap = null;
            FurColorVariation = null;
            FurData0 = null;
            FurData1 = null;
            FurUVTiling = 2;
            SelfWindStrength = 1.0f;
            SkinSmoothness = 0.3f;
            SkinColorMap = null;
            SkinNormalMap = null;
            FurRimPower = 2.5f;
            FurRim = SkinColor = FurMainTint = FurColorA = FurColorB = FurColorC = FurColorD = Color.white;
            FurRimBoost = 1.0f;

            FurCurlAmountX = FurCurlAmountY = FurCurlSizeX = FurCurlSizeY = 0;

            SetupProperties = true;
        }

    }

    [CreateAssetMenu(fileName = "New Fur Profile Asset", menuName = "XFur Studio 2/New Fur Profile Asset")]
    public class XFurStudioFurProfile:ScriptableObject {

#if UNITY_EDITOR
        public string Version { get { return "v2.0"; } }
#endif

        public XFurTemplate FurTemplate = new XFurTemplate( true );

    }

}