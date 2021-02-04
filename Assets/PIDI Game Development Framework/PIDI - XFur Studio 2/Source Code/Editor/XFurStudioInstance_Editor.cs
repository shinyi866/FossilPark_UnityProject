#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace XFurStudio2 {

    [CanEditMultipleObjects]
    [CustomEditor( typeof( XFurStudioInstance ) )]
    public class XFurStudioInstance_Editor : Editor {


        public GUISkin pidiSkin2;
        public Texture2D xfurStudioLogo;

        private XFurStudioInstance xfur;


        XFurStudioInstance.XFurRenderingMode renderingMode;
        XFurStudioDatabase databaseAsset;

        XFurStudio2_Random randomModule;
        XFurStudio2_LOD lodModule;
        XFurStudio2_Physics physicsModule;
        XFurStudio2_VFX vfxModule;
        XFurStudio2_Decals decalsModule;

        private void OnEnable() {

            xfur = (XFurStudioInstance)target;

            randomModule = xfur.RandomizationModule;
            lodModule = xfur.LODModule;
            physicsModule = xfur.PhysicsModule;
            vfxModule = xfur.VFXModule;
            decalsModule = xfur.DecalsModule;

            if ( Application.isPlaying ) {
                return;
            }

            xfur.InitialSetup();

            //if ( !Application.isPlaying )
                //EditorApplication.update += xfur.RenderFur;

        }

        public void OnDisable() {
            //EditorApplication.update -= xfur.RenderFur;

            decalsModule.Unload();

        }


        public override void OnInspectorGUI() {

            SceneView.RepaintAll();

            if ( !xfurStudioLogo ) {
                if ( AssetDatabase.FindAssets( "l: XFurStudio2Logo" ).Length > 0 ) {
                    xfurStudioLogo = (Texture2D)AssetDatabase.LoadAssetAtPath( AssetDatabase.GUIDToAssetPath( AssetDatabase.FindAssets( "l: XFurStudio2Logo" )[0] ), typeof( Texture2D ) );
                }
            }

            if ( !pidiSkin2 ) {
                if ( AssetDatabase.FindAssets( "l: XFurStudio2UI" ).Length > 0 ) {
                    pidiSkin2 = (GUISkin)AssetDatabase.LoadAssetAtPath( AssetDatabase.GUIDToAssetPath( AssetDatabase.FindAssets( "l: XFurStudio2UI" )[0] ), typeof( GUISkin ) );
                }
            }


            if ( !pidiSkin2 ) {
                GUILayout.Space( 12 );
                EditorGUILayout.HelpBox( "The needed GUISkin for this asset has not been found or is corrupted. Please re-download the asset to try to fix this issue or contact support if it persists", MessageType.Error );
                GUILayout.Space( 12 );
                return;
            }

            var lStyle = new GUIStyle();

            GUILayout.BeginVertical( pidiSkin2.box );
            GUILayout.BeginHorizontal(); GUILayout.Space( 12 );
            GUILayout.BeginVertical();

            AssetLogoAndVersion();

            GUILayout.Space( 16 );

            if ( serializedObject.isEditingMultipleObjects ) {

                HelpBox( "XFur Studio depends on per-instance behavior and data sets. Editing multiple instances is not allowed. If you need to share properties across multiple instances, use XFur Templates or Unity Prefabs instead", MessageType.Warning );

                GUILayout.Space( 16 );

                GUILayout.Space( 16 );

                GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();


                lStyle.fontStyle = FontStyle.Italic;
                lStyle.normal.textColor = Color.white;
                lStyle.fontSize = 8;

                GUILayout.Label( "Copyright© 2017-2020,   Jorge Pinal N.", lStyle );

                GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();

                GUILayout.Space( 24 );
                GUILayout.EndVertical();
                GUILayout.Space( 12 ); GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                return;
            }


            Undo.RecordObject( xfur, "XFurInstance_" + GetInstanceID() );

            xfur.FurDatabase = ObjectField<XFurStudioDatabase>( new GUIContent( "XFur Database Asset", "The Xfur Studio Database Asset containing the references to all the shaders, models and templates required by this project" ), (XFurStudioDatabase)xfur.FurDatabase, false );


            if ( !xfur.MainRenderer.renderer ) {
                if ( xfur.GetComponent<Renderer>() ) {
                    xfur.MainRenderer.AssignRenderer( xfur.GetComponent<Renderer>() );
                }
                else {
                    var rends = new List<Renderer>();
                    foreach ( Transform t in xfur.transform ) {
                        if ( t.GetComponent<Renderer>() ) {
                            rends.Add( t.GetComponent<Renderer>() );
                        }
                    }

                    if ( rends.Count == 1 ) {
                        xfur.MainRenderer.AssignRenderer( rends[0] );
                    }
                }
            }



            var FurRenderCopy = xfur.MainRenderer;
            var tempRender = FurRenderCopy.renderer;
            tempRender = ObjectField<Renderer>( new GUIContent( "Main Renderer", "The main renderer component that displays the mesh for this XFur instance or the highest LOD (LOD0) in mesh with multiple levels of detail" ), tempRender, true );

            if ( tempRender != xfur.MainRenderer.renderer ) {
                if ( xfur.MainRenderer.renderer == null || EditorUtility.DisplayDialog( "WARNING", "Changing the main renderer of this XFur Studio Instance may destroy some settings and profiles assigned to it if the new renderer does not have the same amount of materials and a similar configuration. Do you want to continue?", "Continue", "Cancel" ) ) {
                    FurRenderCopy.AssignRenderer( tempRender );
                }
            }

            xfur.MainRenderer = FurRenderCopy;


            GUILayout.Space( 20 );

            if ( xfur.FurDatabase != null && xfur.MainRenderer.renderer != null ) {

                var actualDatabase = (XFurStudioDatabase)xfur.FurDatabase;

                if ( BeginCenteredGroup( "GENERAL SETTINGS", ref xfur.folds[0] ) ) {
                    GUILayout.Space( 16 );

                    xfur.BetaMode = EnableDisableToggle( new GUIContent( "Beta Features", "Enables or disables the support for beta features. WARNING : Beta features are not intended for use in finished projects as they are not yet production ready. They are available for testing purposes and are subject to change between versions" ), xfur.BetaMode );
                    GUILayout.Space( 4 );
                    xfur.ShowAdvancedProperties = EnableDisableToggle( new GUIContent( "Advanced Properties", "Exposes advanced fur properties for editing." ), xfur.ShowAdvancedProperties );

                    GUILayout.Space( 16 );

                    if ( xfur.FurDatabase.RenderingMode == XFurStudioDatabase.XFurRenderingMode.Standard ) {
                        xfur.RenderingMode = (XFurStudioInstance.XFurRenderingMode)UpperCaseEnumField( new GUIContent( "Rendering Mode", "The rendering mode to be used for this XFur Instance, either XShells or Basuc Shells.\n\nXShells : Allows for extremely high sample counts, full shadow control, per-vertex animation driven physics and much more. They work on all platforms and most devices, including mid to high-end mobile phones. It uses CPU skinning so it may be slow when used in many instances or with very high polygon counts.\n\nBasic Shells have some limited features and only allow either 4 or 8 samples. However, they work mostly on the GPU allowing many instances to be rendered with a smaller performance impact. They do not support animation driven physics, shadowing is limited in Forward Mode, among other restrictions" ), xfur.RenderingMode );
                    }
                    else {
                        xfur.RenderingMode = XFurStudioInstance.XFurRenderingMode.XFShells;
                        xfur.FullForwardMode = false;
                    }

                    if ( xfur.FurDatabase.RenderingMode == XFurStudioDatabase.XFurRenderingMode.Standard && xfur.RenderingMode == XFurStudioInstance.XFurRenderingMode.XFShells ) {

                        xfur.FullForwardMode = EnableDisableToggle( new GUIContent( "Forward Add Mode", "Enables the Forward Add setup (additional pixel lights, point lights, etc ) to the XFShells method when using Forward Rendering, but adds some performance impact" ), xfur.FullForwardMode );

                    }
                    else if ( xfur.FurDatabase.RenderingMode != XFurStudioDatabase.XFurRenderingMode.Standard ){
                        xfur.FullForwardMode = EnableDisableToggle( new GUIContent( "Compatibility Mode", "Enables the High compatibility mode (necessary for some older devices) to the XFShells method by disabling some of the advanced graphical optimizations, but adds some performance impact" ), xfur.FullForwardMode );
                    }

                    GUILayout.Space( 8 );

                    xfur.AutoUpdateMaterials = EnableDisableToggle( new GUIContent( "Auto-Update Materials", "Automatically update the fur materials every certain time, allowing runtime changes to length, thickness, textures etc. to be instantly applied" ), xfur.AutoUpdateMaterials, true );

                    if ( xfur.AutoUpdateMaterials ) {
                        xfur.AutoAdjustForScale = EnableDisableToggle( new GUIContent( "Auto-Adjust for Scale", "Automatically adjusts all fur parameters to be scale-relative" ), xfur.AutoAdjustForScale, true );
                        xfur.UpdateFrequency = FloatField( new GUIContent( "Update Frequency (s)", "The update frequency of the fur material properties (in seconds). You can disable auto-updates entirely if you do not plan to modify the fur properties at runtime" ), xfur.UpdateFrequency );
                    }

                    GUILayout.Space( 16 );
                    CenteredLabel( "BUILT-IN MODULES" );
                    GUILayout.Space( 16 );

                    randomModule.XFurModuleStatus();

                    GUILayout.Space( 4 );

                    lodModule.XFurModuleStatus();

                    GUILayout.Space( 4 );

                    physicsModule.XFurModuleStatus();

                    GUILayout.Space( 4 );

                    vfxModule.XFurModuleStatus();

                    GUILayout.Space( 4 );

                    decalsModule.XFurModuleStatus();

                    /* ON HOLD FOR VERSION 2.1
                    GUILayout.Space( 16 );

                    CenteredLabel( "CUSTOM MODULES" );

                    GUILayout.Space( 16 );
                    CenteredButton( "ADD NEW CUSTOM MODULE", 300 );
                    */
                    GUILayout.Space( 32 );
                }
                EndCenteredGroup();


                if ( BeginCenteredGroup( "FUR MATERIALS SETTINGS", ref xfur.folds[1] ) ) {
                    GUILayout.Space( 16 );

                    GUILayout.Space( 16 );

                    for ( int i = 0; i < xfur.MainRenderer.isFurMaterial.Length; ++i ) {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label( xfur.MainRenderer.renderer.sharedMaterials[i].name, pidiSkin2.label, GUILayout.Width( 140 ) ); GUILayout.Space( 64 ); GUILayout.Label( " Fur Rendering ", pidiSkin2.label ); GUILayout.FlexibleSpace(); xfur.MainRenderer.isFurMaterial[i] = EnableDisableToggle( null, xfur.MainRenderer.isFurMaterial[i], false, GUILayout.MaxWidth( EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth - 200 ) );
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.Space( 32 );

                    for ( int i = 0; i < xfur.MainRenderer.isFurMaterial.Length; ++i ) {
                        if ( xfur.MainRenderer.isFurMaterial[i] ) {

                            if ( BeginCenteredGroup( "Material : " + xfur.MainRenderer.renderer.sharedMaterials[i].name + " - Fur Profile", ref xfur.profileFolds[i] ) ) {
                                GUILayout.Space( 16 );

                                CenteredLabel( "Base Settings" );

                                GUILayout.Space( 16 );


                                if ( xfur.RenderingMode == XFurStudioInstance.XFurRenderingMode.XFShells ) {
                                    if ( xfur.FurDatabase.HasDoubleSide )
                                        xfur.FurDataProfiles[i].DoubleSided = EnableDisableToggle( new GUIContent( "Double Sided Fur" ), xfur.FurDataProfiles[i].DoubleSided );
                                    xfur.FurDataProfiles[i].ProbeUse = EnableDisableToggle( new GUIContent( "Use Light Probes" ), xfur.FurDataProfiles[i].ProbeUse, true );
                                    xfur.FurDataProfiles[i].CastShadows = EnableDisableToggle( new GUIContent( "Cast Shadows" ), xfur.FurDataProfiles[i].CastShadows, true );
                                    xfur.FurDataProfiles[i].ReceiveShadows = EnableDisableToggle( new GUIContent( "Receive Shadows" ), xfur.FurDataProfiles[i].ReceiveShadows, true );
                                }
                                else {
                                    HelpBox( "Basic Shells automatically cast and receive shadows unless these features are disabled directly on the Mesh / Skinned Mesh Renderer. Shadows do not work properly in Forward rendering mode in multiple devices. To control fur shadows casting and receiving on a per-instance basis as well as for more consistent results, use XShells instead.", MessageType.Info );
                                }

                                xfur.FurDataProfiles[i].FurSamples = IntSliderField( new GUIContent( "Fur Samples", "The amount of samples used to render the fur. More samples give better results, but may result in a reduced performance (especially when using ss" ), xfur.FurDataProfiles[i].FurSamples, 4, 128 );

                                /*
                                if ( actualDatabase.FurStrandMapNames.Length > 1 ) {
                                    xfur.FurDataProfiles[i].FurGenerationMapID = PopupField( new GUIContent( "Fur Strands Asset", "The asset used to generate the fur strands for this fur profile" ), xfur.FurDataProfiles[i].FurGenerationMapID + 1, actualDatabase.FurStrandMapNames ) - 1;
                                }
                                else {
                                    xfur.FurDataProfiles[i].FurGenerationMapID = -1;
                                }

                                if ( xfur.FurDataProfiles[i].FurGenerationMapID == -1 ) {
                                    
                                }
                                */

                                xfur.FurDataProfiles[i].FurStrandsAsset = ObjectField<XFurStudioStrandsAsset>( new GUIContent( "Fur Strands Asset", "The texture map used to generate the fur strands for this fur profile" ), xfur.FurDataProfiles[i].FurStrandsAsset );

                                if ( xfur.FurDataProfiles[i].FurStrandsAsset ) {
                                    xfur.FurDataProfiles[i].FurUVTiling = FloatField( new GUIContent( "Fur Strands Tiling", "The tiling (UV size) to be applied to the fur strands" ), xfur.FurDataProfiles[i].FurUVTiling );
                                }


                                if (xfur.RenderingMode == XFurStudioInstance.XFurRenderingMode.BasicShells ) {
                                    GUILayout.Space( 24 );

                                    CenteredLabel( "Base Skin Settings" );

                                    GUILayout.Space( 16 );

                                    xfur.FurDataProfiles[i].SkinColor = ColorField( new GUIContent( "Skin Tint" ), xfur.FurDataProfiles[i].SkinColor );
                                    xfur.FurDataProfiles[i].SkinColorMap = ObjectField<Texture>( new GUIContent( "Skin Color Map", "The texture that controls the color applied for the skin under the fur in Basic Shells rendering mode" ), xfur.FurDataProfiles[i].SkinColorMap );

                                    GUILayout.Space( 8 );
                                    xfur.FurDataProfiles[i].SkinSmoothness = SliderField( new GUIContent( "Smoothness" ), xfur.FurDataProfiles[i].SkinSmoothness );
                                    xfur.FurDataProfiles[i].SkinNormalMap = ObjectField<Texture>( new GUIContent( "Skin Normal Map", "The texture that controls the color applied for the skin under the fur in Basic Shells rendering mode" ), xfur.FurDataProfiles[i].SkinNormalMap );


                                }


                                GUILayout.Space( 24 );

                                CenteredLabel( "Fur Settings" );

                                GUILayout.Space( 16 );


                                xfur.FurDataProfiles[i].FurColorMap = ObjectField<Texture>( new GUIContent( "Fur Color Map", "The texture that controls the color / albedo applied over the whole fur surface" ), xfur.FurDataProfiles[i].FurColorMap );

                                xfur.FurDataProfiles[i].FurMainTint = ColorField( new GUIContent( "Fur Main Tint", "The main tint to be applied to the fur" ), xfur.FurDataProfiles[i].FurMainTint );

                                if ( xfur.ShowAdvancedProperties ) {
                                    GUILayout.Space( 8 );
                                    xfur.FurDataProfiles[i].FurUnderColorMod = SliderField( new GUIContent( "Fur Strands R Modifier", "Manually modifies the color boost applied to the first fur pass (fur strands texture's red channel" ), xfur.FurDataProfiles[i].FurUnderColorMod, 0, 0.65f );
                                    xfur.FurDataProfiles[i].FurOverColorMod = SliderField( new GUIContent( "Fur Strands G Modifier", "Manually modifies the color boost applied to the second fur pass (fur strands texture's green channel" ), xfur.FurDataProfiles[i].FurOverColorMod, 0.25f, 1.5f );
                                }

                                GUILayout.Space( 8 );

                                xfur.FurDataProfiles[i].FurRim = ColorField( new GUIContent( "Fur Rim Tint", "The main tint to be applied to the fur's rim lighting" ), xfur.FurDataProfiles[i].FurRim );

                                xfur.FurDataProfiles[i].FurRimPower = SliderField( new GUIContent( "Fur Rim Power" ), xfur.FurDataProfiles[i].FurRimPower, 0.1f, 10 );

                                xfur.FurDataProfiles[i].FurRimBoost = SliderField( new GUIContent( "Fur Rim Boost", "Applies an additional color boost to the fur's rim lighting effect" ), xfur.FurDataProfiles[i].FurRimBoost, 1.0f, 3.0f );
                                

                                if ( !actualDatabase.MobileMode ) {
                                    xfur.FurDataProfiles[i].FurColorVariation = ObjectField<Texture>( new GUIContent( "Fur Color Variation", "The texture that controls four additional coloring variations to be applied over the fur, either all four to the whole fur or two to the undercoat and two to the overcoat by using the four color channels." ), xfur.FurDataProfiles[i].FurColorVariation );
                                }

                                if ( xfur.FurDataProfiles[i].FurColorVariation ) {
                                    GUILayout.Space( 8 );
                                    xfur.FurDataProfiles[i].FurColorA = ColorField( new GUIContent( "Fur Color A", "The fur color to be applied on the red channel of the Color Variation map" ), xfur.FurDataProfiles[i].FurColorA );
                                    xfur.FurDataProfiles[i].FurColorB = ColorField( new GUIContent( "Fur Color B", "The fur color to be applied on the green channel of the Color Variation map" ), xfur.FurDataProfiles[i].FurColorB );
                                    xfur.FurDataProfiles[i].FurColorC = ColorField( new GUIContent( "Fur Color C", "The fur color to be applied on the blue channel of the Color Variation map" ), xfur.FurDataProfiles[i].FurColorC );
                                    xfur.FurDataProfiles[i].FurColorD = ColorField( new GUIContent( "Fur Color D", "The fur color to be applied on the alpha channel of the Color Variation map" ), xfur.FurDataProfiles[i].FurColorD );
                                    GUILayout.Space( 8 );
                                }


                                GUILayout.Space( 8 );

                                xfur.FurDataProfiles[i].FurData0 = ObjectField<Texture>( new GUIContent( "Fur Data Map", "The texture that controls the parameters of the fur :\n\n R = fur mask\n G = length\n B = occlusion\n A = thickness" ), xfur.FurDataProfiles[i].FurData0 );
                                xfur.FurDataProfiles[i].FurData1 = ObjectField<Texture>( new GUIContent( "Fur Grooming Map", "The texture that controls the direction of the fur :\n\n RGB = absolute fur direction half-normalized in tangent space" ), xfur.FurDataProfiles[i].FurData1 );

                                GUILayout.Space( 12 );

                                xfur.FurDataProfiles[i].FurLength = SliderField( new GUIContent( "Fur Length", "The maximum overall length of the fur. This will be multiplied by the actual fur profile length and the length painted in XFur Studio™ - Designer" ), xfur.FurDataProfiles[i].FurLength );

                                GUILayout.Space( 8 );
                                xfur.FurDataProfiles[i].FurThickness = SliderField( new GUIContent( "Fur Thickness", "The maximum overall thickness of the fur. This will be multiplied by the actual fur profile thickness and the thickness painted in XFur Studio™ - Designer" ), xfur.FurDataProfiles[i].FurThickness );
                                xfur.FurDataProfiles[i].FurThicknessCurve = SliderField( new GUIContent( "Thickness Curve", "How the fur strands' thickness bias will change from the root to the top of each strand" ), xfur.FurDataProfiles[i].FurThicknessCurve );
                                GUILayout.Space( 8 );

                                xfur.FurDataProfiles[i].FurShadowsTint = ColorField( new GUIContent( "Occlusion Tint" ), xfur.FurDataProfiles[i].FurShadowsTint );
                                xfur.FurDataProfiles[i].FurOcclusion = SliderField( new GUIContent( "Fur Occlusion / Shadowing", "The shadowing applied over the surface of the fur strands as a simple occlusion pass. Multiplied by the per-profile occlusion value and the one painted through XFur Studio™ - Designer" ), xfur.FurDataProfiles[i].FurOcclusion );
                                xfur.FurDataProfiles[i].FurOcclusionCurve = SliderField( new GUIContent( "Fur Occlusion Curve", "How the shadowing / occlusion of the fur will go from the root to the tip of each strand" ), xfur.FurDataProfiles[i].FurOcclusionCurve );

                                GUILayout.Space( 8 );
                                xfur.FurDataProfiles[i].FurSmoothness = SliderField( new GUIContent( "Fur Smoothness" ), xfur.FurDataProfiles[i].FurSmoothness );
                                GUILayout.Space( 8 );


                                if ( xfur.BetaMode ) {
                                    GUILayout.Space( 8 );

                                    CenteredLabel( "Simple Curly Fur*" );

                                    GUILayout.Space( 8 );

                                    xfur.FurDataProfiles[i].FurCurlAmountX = SliderField( new GUIContent( "Curl Amount X" ), xfur.FurDataProfiles[i].FurCurlAmountX );
                                    xfur.FurDataProfiles[i].FurCurlAmountY = SliderField( new GUIContent( "Curl Amount Y" ), xfur.FurDataProfiles[i].FurCurlAmountY );
                                    xfur.FurDataProfiles[i].FurCurlSizeX = SliderField( new GUIContent( "Curl Size X" ), xfur.FurDataProfiles[i].FurCurlSizeX, 0, 0.1f, 3 );
                                    xfur.FurDataProfiles[i].FurCurlSizeY = SliderField( new GUIContent( "Curl Size Y" ), xfur.FurDataProfiles[i].FurCurlSizeY, 0, 0.1f, 3 );

                                    GUILayout.Space( 16 );

                                }
                                /*
                                GUILayout.Space( 24 );

                                if ( ( (XFurStudioDatabase)xfur.FurDatabase ).MultiProfileBlending ) {
                                    CenteredLabel( "Multi-Profile Blending" );
                                    GUILayout.Space( 16 );

                                    xfur.FurDataProfiles[i].FurBlendSplatmap = ObjectField<Texture>( new GUIContent( "Fur Blending Splatmap", "A splat map that contains information about how to mix 4 different blending profiles on top of the active one" ), xfur.FurDataProfiles[i].FurBlendSplatmap );

                                    GUILayout.Space( 16 );

                                    if ( xfur.FurDataProfiles[i].FurBlendSplatmap && ( (XFurStudioDatabase)xfur.FurDatabase ).ProfileNames.Length > 0 ) {
                                        for ( int f = 0; f < 4; f++ ) {
                                            xfur.FurDataProfiles[i].BlendedProfiles[f] = PopupField( new GUIContent( "Blended Profile " + f, "The fur profile, from the referenced profiles in the database, to use for blending" ), xfur.FurDataProfiles[i].BlendedProfiles[f], ( (XFurStudioDatabase)xfur.FurDatabase ).ProfileNames );
                                            GUILayout.Space( 4 );
                                        }
                                    }
                                    GUILayout.Space( 20 );
                                }

                                */
                                CenteredLabel( "Per Instance Wind Settings" );

                                GUILayout.Space( 24 );

                                xfur.FurDataProfiles[i].SelfWindStrength = SliderField( new GUIContent( "Wind Strength Multiplier", "The value by which the global wind strength will be multiplied, useful to fine tune the overall wind strength applied over this instance" ), xfur.FurDataProfiles[i].SelfWindStrength, 0.0f, 8.0f );

                                GUILayout.Space( 16 );

                                GUILayout.BeginHorizontal();
                                GUILayout.FlexibleSpace();

                                if ( StandardButton( "Load Profile", 200 ) ) {
                                    var path = EditorUtility.OpenFilePanel( "Load Fur Profile Asset", "Assets/", "asset" );

                                    path = path.Replace( Application.dataPath, "Assets" );

                                    var asset = (XFurStudioFurProfile)AssetDatabase.LoadAssetAtPath( path, typeof( XFurStudioFurProfile ) );

                                    if ( asset && asset.GetType() == typeof( XFurStudioFurProfile ) ) {
                                        xfur.SetFurProfileAsset( i, asset );
                                        Debug.Log( "Successfully loaded XFur Profile" );
                                    }
                                    else {
#if XFURDESKTOP_LEGACY

                                        var legacyAsset = (XFurStudio.XFur_CoatingProfile)AssetDatabase.LoadAssetAtPath( path, typeof( XFurStudio.XFur_CoatingProfile ) );

                                        if ( legacyAsset && legacyAsset.GetType() == typeof( XFurStudio.XFur_CoatingProfile ) ) {
                                            xfur.LoadLegacyXFurProfile( i, legacyAsset );
                                            Debug.Log( "Successfully loaded Legacy XFur Profile" );
                                        }
#endif

#if XFurStudioMobile_LEGACY

                                        var legacyAsset = (XFurStudioMobile.XFur_CoatingProfile)AssetDatabase.LoadAssetAtPath( path, typeof( XFurStudioMobile.XFur_CoatingProfile ) );

                                        if ( legacyAsset && legacyAsset.GetType() == typeof( XFurStudioMobile.XFur_CoatingProfile ) ) {
                                            xfur.LoadLegacyXFurProfile( i, legacyAsset );
                                            Debug.Log( "Successfully loaded Legacy XFur Profile" );
                                        }
#endif

                                    }

                                }


                                GUILayout.Space( 32 );
                                if ( StandardButton( "Export Profile", 200 ) ) {

                                    var path = EditorUtility.SaveFilePanelInProject( "Save Fur Profile Asset", "New Fur Profile", "asset", "Create a new Fur profile asset" );

                                    Debug.Log( path );

                                    var asset = ScriptableObject.CreateInstance<XFurStudioFurProfile>();

                                    xfur.GetFurData( i, out asset.FurTemplate );

                                    AssetDatabase.CreateAsset( asset, path );
                                    AssetDatabase.SaveAssets();
                                    AssetDatabase.Refresh();

                                    if ( AssetDatabase.LoadAssetAtPath( path, typeof( XFurStudioFurProfile ) ) != null ) {
                                        Debug.Log( "XFur Profile asset created successfully at " + path );
                                    }

                                }

                                GUILayout.FlexibleSpace();
                                GUILayout.EndHorizontal();

                                GUILayout.Space( 24 );


                            }
                            EndCenteredGroup();

                            if ( i == xfur.MainRenderer.isFurMaterial.Length - 1 ) {
                                GUILayout.Space( 32 );
                            }
                            else {
                                GUILayout.Space( 8 );
                            }
                        }
                    }




                    GUILayout.Space( 16 );

                }
                EndCenteredGroup();

                if ( randomModule.enabled ) {
                    if ( BeginCenteredGroup( "RANDOMIZATION MODULE", ref xfur.folds[2] ) ) {
                        randomModule.ModuleUI();
                    }
                    EndCenteredGroup();
                }

                if ( lodModule.enabled ) {
                    if ( BeginCenteredGroup( "DYNAMIC LOD MODULE", ref xfur.folds[3] ) ) {
                        lodModule.ModuleUI();
                    }
                    EndCenteredGroup();
                }

                if ( physicsModule.enabled ) {
                    if ( BeginCenteredGroup( "FUR PHYSICS MODULE", ref xfur.folds[4] ) ) {
                        physicsModule.ModuleUI();
                    }
                    EndCenteredGroup();
                }

                if ( vfxModule.enabled ) {
                    if ( BeginCenteredGroup( "VFX & WEATHER MODULE", ref xfur.folds[5] ) ) {
                        vfxModule.ModuleUI();
                    }
                    EndCenteredGroup();
                }

                if ( decalsModule.enabled ) {
                    if ( BeginCenteredGroup( decalsModule.moduleName.ToUpper()+" MODULE", ref xfur.folds[6] ) ) {
                        decalsModule.ModuleUI();
                    }
                    EndCenteredGroup();
                }

            }


            if ( BeginCenteredGroup( "HELP & SUPPORT", ref xfur.folds[15] ) ) {

                GUILayout.Space( 16 );
                CenteredLabel( "Support & Assitance" );
                GUILayout.Space( 10 );

                HelpBox( "Please make sure to include the following information with your request :\n - Invoice number\n- Unity version used\n- Universal RP / HDRP version used (if any)\n- Target platform\n - Screenshots of the XFurStudioInstance component and its settings\n - Steps to reproduce the issue.\n\nOur support service usually takes 2-4 business days to reply, so please be patient. We always reply to all emails and support requests as soon as possible.", MessageType.Info );

                GUILayout.Space( 8 );
                GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
                GUILayout.Label( "For support, contact us at : support@irreverent-software.com", pidiSkin2.label );
                GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();

                GUILayout.Space( 8 );

                GUILayout.Space( 16 );
                CenteredLabel( "Online Tutorials and Documentation" );
                GUILayout.Space( 10 );
                if ( CenteredButton( "Video Tutorial Series #1", 400 ) ) {
                    Help.BrowseURL( "https://www.youtube.com/playlist?list=PLavLy1wK-FBeJ0ANYbxGQJWRx4s7hS0OG" );
                }
                if ( CenteredButton( "Quick Start Guide", 400 ) ) {
                    Help.BrowseURL( "https://pidiwiki.irreverent-software.com/wiki/doku.php?id=xfur_studio_2#quick_start_guide" );
                }
                if ( CenteredButton( "Upgrading from XFur Studio™ 1.x and XFur Mobile™", 400 ) ) {
                    Help.BrowseURL( "https://pidiwiki.irreverent-software.com/wiki/doku.php?id=xfur_studio_2#upgrading_characters_from_xfur_studio_19x_xfur_mobile" );
                }
                if ( CenteredButton( "XFur Studio Instance", 400 ) ) {
                    Help.BrowseURL( "https://pidiwiki.irreverent-software.com/wiki/doku.php?id=xfur_studio_2#xfur_studio_instance" );
                }
                if ( CenteredButton( "XFur Studio - Built in Modules", 400 ) ) {
                    Help.BrowseURL( "https://pidiwiki.irreverent-software.com/wiki/doku.php?id=xfur_studio_2#xfur_studio_built-in_modules" );
                }
                if ( CenteredButton( "XFur Studio - Fur Profiles", 400 ) ) {
                    Help.BrowseURL( "https://pidiwiki.irreverent-software.com/wiki/doku.php?id=xfur_studio_2#xfur_studio_profiles" );
                }
                if ( CenteredButton( "XFur Studio - Designer", 400 ) ) {
                    Help.BrowseURL( "https://pidiwiki.irreverent-software.com/wiki/doku.php?id=xfur_studio_2#xfur_studio_designer" );
                }
                if ( CenteredButton( "XFur Studio - API Reference", 400 ) ) {
                    Help.BrowseURL( "https://pidiwiki.irreverent-software.com/wiki/doku.php?id=xfur_studio_2#xfur_studio_-_api_reference" );
                }
                GUILayout.Space( 16 );

            }
            EndCenteredGroup();

            GUILayout.Space( 16 );

            GUILayout.Space( 16 );

            GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();

            lStyle = new GUIStyle();
            lStyle.fontStyle = FontStyle.Italic;
            lStyle.normal.textColor = Color.white;
            lStyle.fontSize = 8;

            GUILayout.Label( "Copyright© 2017-2020,   Jorge Pinal N.", lStyle );

            GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();

            GUILayout.Space( 24 );
            GUILayout.EndVertical();
            GUILayout.Space( 12 ); GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }








        #region PIDI 2020 EDITOR

        public void XFurModuleStatus( XFurStudioModule module ) {
            GUILayout.BeginHorizontal();
            GUILayout.Label( module.moduleName + ", v" + module.version, pidiSkin2.label, GUILayout.Width( 140 ) );
            GUILayout.Space( 64 );
            GUILayout.Label( " Status : " + module.ModuleStatus, pidiSkin2.label );
            GUILayout.FlexibleSpace();
            var t = EnableDisableToggle( null, module.enabled, false, GUILayout.MaxWidth( EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth - 200 ) ) && !module.criticalError;
            if ( t ) {
                module.Enable();
            }
            else {
                module.Disable();
            }
            GUILayout.EndHorizontal();
        }


        public void HelpBox( string message, MessageType messageType ) {
            GUILayout.Space( 8 );
            GUILayout.BeginHorizontal(); GUILayout.Space( 8 );
            GUILayout.BeginVertical( pidiSkin2.customStyles[5] );

            GUILayout.Space( 4 );
            GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();

            var mType = "INFO";

            switch ( messageType ) {
                case MessageType.Error:
                    mType = "ERROR";
                    break;

                case MessageType.Warning:
                    mType = "WARNING";
                    break;
            }

            var tStyle = new GUIStyle();
            tStyle.fontSize = 11;
            tStyle.fontStyle = FontStyle.Bold;
            tStyle.normal.textColor = Color.black;

            GUILayout.Label( mType, tStyle );

            GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
            GUILayout.Space( 4 );

            GUILayout.BeginHorizontal(); GUILayout.Space( 8 ); GUILayout.BeginVertical();
            tStyle.fontSize = 10;
            tStyle.fontStyle = FontStyle.Bold;
            tStyle.wordWrap = true;
            GUILayout.TextArea( message, tStyle );

            GUILayout.Space( 8 );
            GUILayout.EndVertical(); GUILayout.Space( 8 ); GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.Space( 8 ); GUILayout.EndHorizontal();
            GUILayout.Space( 8 );
        }


        public Color ColorField( GUIContent label, Color currentValue ) {

            GUILayout.Space( 2 );
            GUILayout.BeginHorizontal();
            GUILayout.Label( label, pidiSkin2.label, GUILayout.Width( EditorGUIUtility.labelWidth ) );
            currentValue = EditorGUILayout.ColorField( currentValue );
            GUILayout.EndHorizontal();
            GUILayout.Space( 2 );

            return currentValue;

        }



        /// <summary>
        /// Draws a standard object field in the PIDI 2020 style
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="label"></param>
        /// <param name="inputObject"></param>
        /// <param name="allowSceneObjects"></param>
        /// <returns></returns>
        public T ObjectField<T>( GUIContent label, T inputObject, bool allowSceneObjects = true ) where T : UnityEngine.Object {

            GUILayout.Space( 4 );
            GUILayout.BeginHorizontal();
            GUILayout.Label( label, pidiSkin2.label, GUILayout.Width( EditorGUIUtility.labelWidth ) );
            inputObject = (T)EditorGUILayout.ObjectField( inputObject, typeof( T ), allowSceneObjects );
            GUILayout.EndHorizontal();
            GUILayout.Space( 4 );
            return inputObject;
        }


        /// <summary>
        /// Draws a centered button in the standard PIDI 2020 editor style
        /// </summary>
        /// <param name="label"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public bool CenteredButton( string label, float width ) {
            GUILayout.Space( 2 );
            GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
            var tempBool = GUILayout.Button( label, pidiSkin2.button, GUILayout.MaxWidth( width ) );
            GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();
            GUILayout.Space( 2 );
            return tempBool;
        }

        /// <summary>
        /// Draws a button in the standard PIDI 2020 editor style
        /// </summary>
        /// <param name="label"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public bool StandardButton( string label, float width ) {
            var tempBool = GUILayout.Button( label, pidiSkin2.button, GUILayout.MaxWidth( width ) );
            return tempBool;
        }


        /// <summary>
        /// Draws the asset's logo and its current version
        /// </summary>
        public void AssetLogoAndVersion() {

            GUILayout.BeginVertical( xfurStudioLogo, pidiSkin2 ? pidiSkin2.customStyles[1] : null );
            GUILayout.Space( 45 );
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label( xfur.Version, pidiSkin2.customStyles[2] );
            GUILayout.Space( 6 );
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Draws a label centered in the Editor window
        /// </summary>
        /// <param name="label"></param>
        public void CenteredLabel( string label ) {

            GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
            GUILayout.Label( label, pidiSkin2.label );
            GUILayout.FlexibleSpace(); GUILayout.EndHorizontal();

        }

        /// <summary>
        /// Begins a custom centered group similar to a foldout that can be expanded with a button
        /// </summary>
        /// <param name="label"></param>
        /// <param name="groupFoldState"></param>
        /// <returns></returns>
        public bool BeginCenteredGroup( string label, ref bool groupFoldState ) {

            if ( GUILayout.Button( label, pidiSkin2.button ) ) {
                groupFoldState = !groupFoldState;
            }
            GUILayout.BeginHorizontal(); GUILayout.Space( 12 );
            GUILayout.BeginVertical();
            return groupFoldState;
        }


        /// <summary>
        /// Finishes a centered group
        /// </summary>
        public void EndCenteredGroup() {
            GUILayout.EndVertical();
            GUILayout.Space( 12 );
            GUILayout.EndHorizontal();
        }



        /// <summary>
        /// Custom integer field following the PIDI 2020 editor skin
        /// </summary>
        /// <param name="label"></param>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        public int IntField( GUIContent label, int currentValue ) {

            GUILayout.Space( 2 );
            GUILayout.BeginHorizontal();
            GUILayout.Label( label, pidiSkin2.label, GUILayout.Width( EditorGUIUtility.labelWidth ) );
            currentValue = EditorGUILayout.IntField( currentValue, pidiSkin2.customStyles[4] );
            GUILayout.EndHorizontal();
            GUILayout.Space( 2 );

            return currentValue;
        }

        /// <summary>
        /// Custom float field following the PIDI 2020 editor skin
        /// </summary>
        /// <param name="label"></param>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        public float FloatField( GUIContent label, float currentValue ) {

            GUILayout.Space( 2 );
            GUILayout.BeginHorizontal();
            GUILayout.Label( label, pidiSkin2.label, GUILayout.Width( EditorGUIUtility.labelWidth ) );
            currentValue = EditorGUILayout.FloatField( currentValue, pidiSkin2.customStyles[4] );
            GUILayout.EndHorizontal();
            GUILayout.Space( 2 );

            return currentValue;
        }


        /// <summary>
        /// Custom text field following the PIDI 2020 editor skin
        /// </summary>
        /// <param name="label"></param>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        public string TextField( GUIContent label, string currentValue ) {

            GUILayout.Space( 2 );
            GUILayout.BeginHorizontal();
            GUILayout.Label( label, pidiSkin2.label, GUILayout.Width( EditorGUIUtility.labelWidth ) );
            currentValue = EditorGUILayout.TextField( currentValue, pidiSkin2.customStyles[4] );
            GUILayout.EndHorizontal();
            GUILayout.Space( 2 );

            return currentValue;
        }


        public Vector2 Vector2Field( GUIContent label, Vector2 currentValue ) {

            GUILayout.Space( 2 );
            GUILayout.BeginHorizontal();
            GUILayout.Label( label, pidiSkin2.label, GUILayout.Width( EditorGUIUtility.labelWidth ) );
            currentValue.x = EditorGUILayout.FloatField( currentValue.x, pidiSkin2.customStyles[4] );
            GUILayout.Space( 8 );
            currentValue.y = EditorGUILayout.FloatField( currentValue.y, pidiSkin2.customStyles[4] );
            GUILayout.EndHorizontal();
            GUILayout.Space( 2 );

            return currentValue;

        }

        public Vector3 Vector3Field( GUIContent label, Vector3 currentValue ) {

            GUILayout.Space( 2 );
            GUILayout.BeginHorizontal();
            GUILayout.Label( label, pidiSkin2.label, GUILayout.Width( EditorGUIUtility.labelWidth ) );
            currentValue.x = EditorGUILayout.FloatField( currentValue.x, pidiSkin2.customStyles[4] );
            GUILayout.Space( 8 );
            currentValue.y = EditorGUILayout.FloatField( currentValue.y, pidiSkin2.customStyles[4] );
            GUILayout.Space( 8 );
            currentValue.z = EditorGUILayout.FloatField( currentValue.z, pidiSkin2.customStyles[4] );
            GUILayout.EndHorizontal();
            GUILayout.Space( 2 );

            return currentValue;

        }


        public Vector4 Vector4Field( GUIContent label, Vector4 currentValue ) {

            GUILayout.Space( 2 );
            GUILayout.BeginHorizontal();
            GUILayout.Label( label, pidiSkin2.label, GUILayout.Width( EditorGUIUtility.labelWidth ) );
            currentValue.x = EditorGUILayout.FloatField( currentValue.x, pidiSkin2.customStyles[4] );
            GUILayout.Space( 8 );
            currentValue.y = EditorGUILayout.FloatField( currentValue.y, pidiSkin2.customStyles[4] );
            GUILayout.Space( 8 );
            currentValue.z = EditorGUILayout.FloatField( currentValue.z, pidiSkin2.customStyles[4] );
            GUILayout.Space( 8 );
            currentValue.w = EditorGUILayout.FloatField( currentValue.w, pidiSkin2.customStyles[4] );
            GUILayout.EndHorizontal();
            GUILayout.Space( 2 );

            return currentValue;

        }


        /// <summary>
        /// Custom slider using the PIDI 2020 editor skin and adding a custom suffix to the float display
        /// </summary>
        /// <param name="label"></param>
        /// <param name="currentValue"></param>
        /// <param name="minSlider"></param>
        /// <param name="maxSlider"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public float SliderField( GUIContent label, float currentValue, float minSlider = 0.0f, float maxSlider = 1.0f, int decimals = 2 ) {

            GUILayout.Space( 4 );
            GUILayout.BeginHorizontal();
            GUILayout.Label( label, pidiSkin2.label, GUILayout.Width( EditorGUIUtility.labelWidth ) );
            GUI.color = Color.gray;
            currentValue = GUILayout.HorizontalSlider( currentValue, minSlider, maxSlider, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb );
            GUI.color = Color.white;
            GUILayout.Space( 12 );
            currentValue = Mathf.Clamp( EditorGUILayout.FloatField( float.Parse( currentValue.ToString( "n"+decimals ) ), pidiSkin2.customStyles[4], GUILayout.MaxWidth( 40 ) ), minSlider, maxSlider );
            GUILayout.EndHorizontal();
            GUILayout.Space( 4 );

            return currentValue;
        }


        /// <summary>
        /// Custom slider using the PIDI 2020 editor skin and adding a custom suffix to the float display
        /// </summary>
        /// <param name="label"></param>
        /// <param name="currentValue"></param>
        /// <param name="minSlider"></param>
        /// <param name="maxSlider"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public int IntSliderField( GUIContent label, int currentValue, int minSlider = 0, int maxSlider = 1 ) {

            GUILayout.Space( 4 );
            GUILayout.BeginHorizontal();
            GUILayout.Label( label, pidiSkin2.label, GUILayout.Width( EditorGUIUtility.labelWidth ) );
            GUI.color = Color.gray;
            currentValue = (int)GUILayout.HorizontalSlider( currentValue, minSlider, maxSlider, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb );
            GUI.color = Color.white;
            GUILayout.Space( 12 );
            currentValue = (int)Mathf.Clamp( EditorGUILayout.IntField( int.Parse( currentValue.ToString() ), pidiSkin2.customStyles[4], GUILayout.MaxWidth( 40 ) ), minSlider, maxSlider );
            GUILayout.EndHorizontal();
            GUILayout.Space( 4 );

            return currentValue;
        }


        /// <summary>
        /// Draw a custom popup field in the PIDI 2020 style
        /// </summary>
        /// <param name="label"></param>
        /// <param name="toggleValue"></param>
        /// <returns></returns>
        public int PopupField( GUIContent label, int selected, string[] options ) {


            GUILayout.Space( 2 );
            GUILayout.BeginHorizontal();
            GUILayout.Label( label, pidiSkin2.label, GUILayout.Width( EditorGUIUtility.labelWidth ) );
            selected = EditorGUILayout.Popup( selected, options, pidiSkin2.button );
            GUILayout.EndHorizontal();
            GUILayout.Space( 2 );
            return selected;
        }



        /// <summary>
        /// Draw a custom toggle that instead of using a check box uses an Enable/Disable drop down menu
        /// </summary>
        /// <param name="label"></param>
        /// <param name="toggleValue"></param>
        /// <returns></returns>
        public bool EnableDisableToggle( GUIContent label, bool toggleValue, bool trueFalseToggle = false, params GUILayoutOption[] options ) {

            int option = toggleValue ? 1 : 0;

            GUILayout.Space( 2 );
            GUILayout.BeginHorizontal();
            if ( label != null ) {
                GUILayout.Label( label, pidiSkin2.label, GUILayout.Width( EditorGUIUtility.labelWidth ) );

                if ( trueFalseToggle ) {
                    option = EditorGUILayout.Popup( option, new string[] { "FALSE", "TRUE" }, pidiSkin2.button );
                }
                else {
                    option = EditorGUILayout.Popup( option, new string[] { "DISABLED", "ENABLED" }, pidiSkin2.button );
                }
            }
            else {
                if ( trueFalseToggle ) {
                    option = EditorGUILayout.Popup( option, new string[] { "FALSE", "TRUE" }, pidiSkin2.button, options );
                }
                else {
                    option = EditorGUILayout.Popup( option, new string[] { "DISABLED", "ENABLED" }, pidiSkin2.button, options );
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space( 2 );
            return option == 1;
        }


        /// <summary>
        /// Draw an enum field but changing the labels and names of the enum to Upper Case fields
        /// </summary>
        /// <param name="label"></param>
        /// <param name="userEnum"></param>
        /// <returns></returns>
        public int UpperCaseEnumField( GUIContent label, System.Enum userEnum ) {

            var names = System.Enum.GetNames( userEnum.GetType() );

            for ( int i = 0; i < names.Length; i++ ) {
                names[i] = System.Text.RegularExpressions.Regex.Replace( names[i], "(\\B[A-Z])", " $1" ).ToUpper();
            }

            GUILayout.Space( 2 );
            GUILayout.BeginHorizontal();
            GUILayout.Label( label, pidiSkin2.label, GUILayout.Width( EditorGUIUtility.labelWidth ) );
            var result = EditorGUILayout.Popup( System.Convert.ToInt32( userEnum ), names, pidiSkin2.button );
            GUILayout.EndHorizontal();
            GUILayout.Space( 2 );
            return result;
        }


        /// <summary>
        /// Draw an enum field but changing the labels and names of the enum to Upper Case fields
        /// </summary>
        /// <param name="label"></param>
        /// <param name="userEnum"></param>
        /// <returns></returns>
        public int StandardEnumField( GUIContent label, System.Enum userEnum ) {

            var names = System.Enum.GetNames( userEnum.GetType() );

            for ( int i = 0; i < names.Length; i++ ) {
                names[i] = names[i].ToUpper();
            }

            GUILayout.Space( 2 );
            GUILayout.BeginHorizontal();
            GUILayout.Label( label, pidiSkin2.label, GUILayout.Width( EditorGUIUtility.labelWidth ) );
            var result = EditorGUILayout.Popup( System.Convert.ToInt32( userEnum ), names, pidiSkin2.button );
            GUILayout.EndHorizontal();
            GUILayout.Space( 2 );
            return result;
        }


        /// <summary>
        /// Draw a layer mask field in the PIDI 2020 style
        /// </summary>
        /// <param name="label"></param>
        /// <param name="selected"></param>
        public LayerMask LayerMaskField( GUIContent label, LayerMask selected ) {

            List<string> layers = null;
            string[] layerNames = null;

            if ( layers == null ) {
                layers = new List<string>();
                layerNames = new string[4];
            }
            else {
                layers.Clear();
            }

            int emptyLayers = 0;
            for ( int i = 0; i < 32; i++ ) {
                string layerName = LayerMask.LayerToName( i );

                if ( layerName != "" ) {

                    for ( ; emptyLayers > 0; emptyLayers-- ) layers.Add( "Layer " + ( i - emptyLayers ) );
                    layers.Add( layerName );
                }
                else {
                    emptyLayers++;
                }
            }

            if ( layerNames.Length != layers.Count ) {
                layerNames = new string[layers.Count];
            }
            for ( int i = 0; i < layerNames.Length; i++ ) layerNames[i] = layers[i];


            GUILayout.Space( 2 );
            GUILayout.BeginHorizontal();
            GUILayout.Label( label, pidiSkin2.label, GUILayout.Width( EditorGUIUtility.labelWidth ) );

            selected.value = EditorGUILayout.MaskField( selected.value, layerNames, pidiSkin2.button );

            GUILayout.EndHorizontal();
            GUILayout.Space( 2 );
            return selected;
        }



        #endregion




    }

}

#endif