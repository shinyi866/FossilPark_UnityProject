﻿namespace XFurStudio2 {

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class XFurStudio2_LOD : XFurStudioModule {

        public XFurRendererData[] lodRenderers = new XFurRendererData[0];

        [SerializeField] protected int[] furSamples = new int[1];

        public float MaxFurDistance = 150;

        public float MinFurDistance = 2;

        public Vector2Int FurSamplesRange = new Vector2Int( 4, 32 );

        public bool SwitchToBasicShells = false;

        public int BasicShellsSamples = 8;

        public Vector2 BasicShellsRegion = new Vector2( 25, 40 );


        private XFurStudioInstance.XFurRenderingMode prevRenderMode;

        private Camera mainCam;

        public bool IsFarLOD { get; private set; }

        public override void Setup( XFurStudioInstance xfurOwner, bool update = false ) {

            if ( !update ) {
                moduleName = "Dynamic LOD";
                moduleStatus = 3;
                isEnabled = true;
                hasMobileMode = true;
                hasSRPMode = true;
            }

            xfurInstance = xfurOwner;

            if ( lodRenderers.Length > 0 ) {
                SynchLodGroup();
            }
        }



        public void SynchLodGroup() {
            var lodGroup = Owner.GetComponent<LODGroup>();
            lodRenderers = new XFurRendererData[lodGroup.lodCount];
            for ( int i = 0; i < lodRenderers.Length; i++ ) {
                lodRenderers[i].AssignRenderer( lodGroup.GetLODs()[i].renderers[0] );
            }
        }


        public override void Load() {
            if ( furSamples.Length != xfurInstance.FurDataProfiles.Length ) {
                furSamples = new int[xfurInstance.FurDataProfiles.Length];
            }

            for ( int i = 0; i < furSamples.Length; i++ ) {
                furSamples[i] = xfurInstance.FurDataProfiles[i].FurSamples;
            }
        }


        public override void MainLoop() {

            var distance = 0.0f;

            if ( furSamples.Length != xfurInstance.FurDataProfiles.Length ) {
                furSamples = new int[xfurInstance.FurDataProfiles.Length];
            }

            for ( int i = 0; i < furSamples.Length; i++ ) {
                furSamples[i] = xfurInstance.FurDataProfiles[i].FurSamples;
            }


            if ( Application.isPlaying && mainCam ) {
                distance = Vector3.Distance( xfurInstance.transform.position, mainCam.transform.position );

                if ( SwitchToBasicShells && Application.isPlaying ) {
                    xfurInstance.RenderingMode = distance > BasicShellsRegion.x ? XFurStudioInstance.XFurRenderingMode.BasicShells : XFurStudioInstance.XFurRenderingMode.XFShells;
                }

                for ( int i = 0; i < xfurInstance.FurDataProfiles.Length; i++ ) {
                    xfurInstance.FurDataProfiles[i].FurSamples = (int)Mathf.Lerp( xfurInstance.RenderingMode == XFurStudioInstance.XFurRenderingMode.BasicShells ? 4 : FurSamplesRange.x, xfurInstance.RenderingMode == XFurStudioInstance.XFurRenderingMode.BasicShells ? 8 : FurSamplesRange.y, 1 - Mathf.Clamp01( (distance - MinFurDistance) / MaxFurDistance ) );
                }
            }
            else {
                mainCam = Camera.main;
                if ( mainCam == null ) {
                    mainCam = Camera.current;
                }


                for ( int i = 0; i < xfurInstance.FurDataProfiles.Length; i++ ) {
                    if ( Application.isPlaying ) {
                        xfurInstance.FurDataProfiles[i].FurSamples = furSamples[i];
                    }
                    else {
                        furSamples[i] = xfurInstance.FurDataProfiles[i].FurSamples;
                    }
                }

            }

            IsFarLOD = distance - MinFurDistance > ( MaxFurDistance - MinFurDistance ) * 0.65f;

            for ( int i = 0; i < lodRenderers.Length; i++ ) {
                if ( lodRenderers[i].renderer && lodRenderers[i].renderer.isVisible ) {
                    xfurInstance.CurrentFurRenderer = lodRenderers[i];
                    break;
                }
            }


            Owner.skipRenderFrame = distance > MaxFurDistance * 2 && lodRenderers.Length < 1;

            if ( xfurInstance.RenderingMode != prevRenderMode && xfurInstance.RenderingMode != XFurStudioInstance.XFurRenderingMode.BasicShells ) {
                foreach ( XFurRendererData rend in lodRenderers ) {
                    if ( rend.renderer ) {
                        var tMaterials = rend.materials;
                        for ( int m = 0; m < rend.materials.Length; m++ ) {
                            tMaterials[m] = xfurInstance.MainRenderer.materials[m];
                        }
                        rend.renderer.sharedMaterials = tMaterials;
                    }
                }
                prevRenderMode = xfurInstance.RenderingMode;
            }
            else if ( xfurInstance.RenderingMode != XFurStudioInstance.XFurRenderingMode.BasicShells ) {
                for ( int i = 0; i < lodRenderers.Length; i++ ) {
                    if ( lodRenderers[i].renderer ) {
                        lodRenderers[i].materials = lodRenderers[i].renderer.sharedMaterials;
                    }
                }
            }

        }



#if UNITY_EDITOR

        public override void UpdateModule() {

            moduleName = "Dynamic LOD";
            moduleStatus = 3;
            hasMobileMode = true;
            hasSRPMode = true;

            if ( lodRenderers.Length > 0 && !lodRenderers[0].renderer ) {
                SynchLodGroup();
            }
        }

        public override void ModuleUI() {

            //UnityEditor.Undo.RecordObject( this, xfurInstance.name + xfurInstance.GetInstanceID() + this.name );

            base.ModuleUI();

            GUILayout.Space( 16 );

            if ( experimentalFeatures ) {
                HelpBox( "Experimental features are enabled. This may cause unexpected errors and unstability within this module or the whole XFur Studio Instance component. Use with caution.", UnityEditor.MessageType.Warning );
            }
            else {
                if ( Owner.GetComponent<LODGroup>() ) {
                    if ( CenteredButton( "Synch with LOD Group", 200 ) ) {
                        SynchLodGroup();
                    }
                    GUILayout.Space( 16 );
                }
            }

            CenteredLabel( "FUR RENDERING DISTANCE" );

            GUILayout.Space( 16 );

            MinFurDistance = FloatField( new GUIContent( "Min. Distance", "At any distance closer than this to the camera, fur and effects will be rendered at the full quality defined by the user" ), MinFurDistance );
            MaxFurDistance = FloatField( new GUIContent( "Max. Distance", "The maximum distance at which fur will still be rendered. After this distance, all fur and its effects will be disabled, while the minimum amount of fur samples will be used as the object approaches this distance from the camera" ), MaxFurDistance );

            GUILayout.Space( 16 );

            CenteredLabel( "FUR SAMPLES SETTINGS" );

            GUILayout.Space( 16 );

            FurSamplesRange.x = IntSliderField( new GUIContent( "Min. Fur Samples", "The minimum amount of fur samples to be used with this instance when it is furthest away from the camera" ), FurSamplesRange.x, 4, FurSamplesRange.y );
            FurSamplesRange.y = IntSliderField( new GUIContent( "Max. Fur Samples", "The maximum amount of fur samples to be used with this instance when it is closest to the camera" ), FurSamplesRange.y, FurSamplesRange.x + 1, 128 );

            GUILayout.Space( 16 );


            if ( !Application.isPlaying ) {

                SwitchToBasicShells = EnableDisableToggle( new GUIContent( "BasicShells Fallback", "Uses GShell shaders, which are faster to render, when the object is far from the camera. Requires hardware comptible with Geometry Shaders and is not compatible with mobile devices." ), SwitchToBasicShells );

            }
            else {
                EnableDisableToggle( new GUIContent( "Basic Shells Fallback", "Uses GShell shaders, which are faster to render, when the object is far from the camera. Requires hardware comptible with Geometry Shaders and is not compatible with mobile devices." ), SwitchToBasicShells );
            }

            if ( SwitchToBasicShells ) {
                GUILayout.Space( 16 );

                CenteredLabel( "BASIC SHELLS SWITCH REGION" );

                GUILayout.Space( 16 );

                BasicShellsRegion.x = SliderField( new GUIContent( "Switch At Distance" ), BasicShellsRegion.x, MinFurDistance, MaxFurDistance );
            }

            GUILayout.Space( 16 );

            experimentalFeatures = EnableDisableToggle( new GUIContent( "Experimental Features" ), experimentalFeatures );

            if ( experimentalFeatures ) {
                GUILayout.Space( 16 );

                CenteredLabel( "Manually Defined LOD Renderers" );

                for (int i = 0; i < lodRenderers.Length; i++ ) {
                    GUILayout.BeginHorizontal();
                    var tempRenderDataCopy = lodRenderers[i];
                    var tempRenderer = tempRenderDataCopy.renderer;
                    tempRenderer = ObjectField<Renderer>( new GUIContent( "LOD" + i + " Renderer" ), tempRenderer );
                    if ( tempRenderer != tempRenderDataCopy.renderer ) {
                        tempRenderDataCopy.AssignRenderer( tempRenderer );
                    }
                    lodRenderers[i] = tempRenderDataCopy;

                    if ( StandardButton( "X", 24 ) ) {
                        UnityEditor.ArrayUtility.RemoveAt<XFurRendererData>( ref lodRenderers, i );
                        GUILayout.EndHorizontal();
                        break;
                    }
                    GUILayout.EndHorizontal();
                }

                GUILayout.Space( 16 );

                if ( CenteredButton("Add new LOD Renderer", 200) ) {
                    UnityEditor.ArrayUtility.Add<XFurRendererData>( ref lodRenderers, new XFurRendererData() );
                }

                GUILayout.Space( 16 );
            }

            GUILayout.Space( 24 );

        }

#endif

    }

}