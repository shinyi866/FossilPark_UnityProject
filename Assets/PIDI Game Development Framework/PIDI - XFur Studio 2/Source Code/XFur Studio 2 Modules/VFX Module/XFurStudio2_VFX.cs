namespace XFurStudio2 {

    using UnityEngine;

    [System.Serializable]
    public class XFurStudio2_VFX : XFurStudioModule {

        [System.Serializable]
        public class VFXEffect {

            public string name = "New Effect";

            public Color color;
            public Color specular;
            public float penetration;
            public float smoothness;
            public float intensity = 2.0f;
            public float fadeoutTime = 5f;
            public float normalFalloff = 0.15f;
            public Texture vfxMask;
            public float vfxTiling = 2;
            public bool updated;

        }


        [SerializeField] protected Shader VFXProgressiveShader;

        static Material vfxMat;

        private RenderTexture[] vfxTexture;

        private int internalRes;

        public bool disableOnLOD;

        protected float updateTime = 0.05f;

        private float timer;

        public RenderTexture[] VFXTexture { get { return vfxTexture; } }


        [SerializeField] protected ModuleQuality quality = ModuleQuality.Normal;

        public VFXEffect Snow = new VFXEffect { name = "SnowFX", color = new Color( 0.85f, 0.95f, 1f, 1f ), specular = new Color( 0.45f, 0.5f, 0.6f ), smoothness = 0.5f, intensity = 1.25f, fadeoutTime = 4.0f, vfxTiling = 2, normalFalloff = 0.75f, penetration = 0.65f };
        
        public VFXEffect Rain = new VFXEffect { name = "RainFX", color = Color.white, specular = new Color( 0.45f, 0.5f, 0.6f ), smoothness = 0.75f, intensity = 1.25f, fadeoutTime = 4.0f, vfxTiling = 2, normalFalloff = 0.75f };
        
        public VFXEffect Blood = new VFXEffect { name = "BloodFX", color = new Color(0.75f,0.05f,0.05f), specular = new Color( 0.15f, 0.15f, 0.15f ), smoothness = 0.2f, intensity = 1.25f, fadeoutTime = 10.0f, vfxTiling = 2, normalFalloff = 0.75f };

        public override void Setup( XFurStudioInstance xfurOwner, bool update = false ) {
            if ( !update ) {
                moduleName = "VFX & Weather";
                version = "3.1";
                moduleStatus = 3;
                experimentalFeatures = false;
                isEnabled = true;
                hasMobileMode = true;
                hasSRPMode = true;
            }
            xfurInstance = xfurOwner;

        }


#if UNITY_2019_3_OR_NEWER
        [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.SubsystemRegistration )]
        static void DestroyMaterial() {
            if (vfxMat)
                Object.DestroyImmediate( vfxMat );
        }
#endif


        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetQuality"></param>
        public virtual void SetQuality( ModuleQuality targetQuality ) {

            quality = targetQuality;

              switch ( quality ) {
                case ModuleQuality.VeryLow:
                    internalRes = Owner.FurDatabase.MobileMode ? 16 : 32;
                    break;
                case ModuleQuality.Low:
                    internalRes = Owner.FurDatabase.MobileMode ? 32 : 64;
                    break;
                case ModuleQuality.Normal:
                    internalRes = Owner.FurDatabase.MobileMode ? 64 : 128;
                    break;
                case ModuleQuality.High:
                    internalRes = Owner.FurDatabase.MobileMode ? 128 : 256;
                    break;
            }

        }





        public override void Load() {

            updateTime = Random.Range( updateTime * 0.9f, updateTime * 1.1f );

            SetQuality(quality);

#if UNITY_2019_3_OR_NEWER
            var rd = new RenderTextureDescriptor( internalRes, internalRes, RenderTextureFormat.ARGB32, 0, 2 );
#else
            var rd = new RenderTextureDescriptor( internalRes, internalRes, RenderTextureFormat.ARGB32, 0 );
#endif

            vfxTexture = new RenderTexture[Owner.MainRenderer.furProfiles.Length];

            for ( int i = 0; i < vfxTexture.Length; i++ ) {

                if ( Owner.MainRenderer.isFurMaterial[i] ) {
                    vfxTexture[i] = RenderTexture.GetTemporary( rd );
                    vfxTexture[i].filterMode = FilterMode.Bilinear;
                    XFurStudioAPI.LoadPaintResources();
                    XFurStudioAPI.FillTexture( Owner, Color.clear, vfxTexture[i], Color.clear );
                    vfxTexture[i].name = "XFUR2_VFX"+i+"_"+ Owner.name;
                }
            }


            if ( !vfxMat ) {
                vfxMat = new Material( VFXProgressiveShader );
            }

        }


        public override void MainLoop() {

            if ( criticalError ) {
                return;
            }

            if ( Application.isPlaying ) {
                if ( Time.timeSinceLevelLoad > timer ) {
                    VFXPass();
                    timer = Time.timeSinceLevelLoad + updateTime;
                }
            }

        }


        public override void MainRenderLoop( MaterialPropertyBlock block, int furProfileIndex ) {

            if ( criticalError ) {
                return;
            }

            block.SetTexture( "_XFurVFXMask", vfxTexture[furProfileIndex] );
            block.SetColor( "_XFurVFX1Color", Blood.color );
            block.SetColor( "_XFurVFX2Color", Snow.color );
            block.SetFloat( "_XFurVFX1Penetration", 1-Blood.penetration );
            block.SetFloat( "_XFurVFX2Penetration", 1-Snow.penetration );
            block.SetFloat( "_XFurVFX3Penetration", 1-Rain.penetration );
            block.SetFloat( "_XFurVFX1Smoothness", Blood.smoothness );
            block.SetFloat( "_XFurVFX2Smoothness", Snow.smoothness );
            block.SetFloat( "_XFurVFX3Smoothness", Rain.smoothness );
        }


        public override void Unload() {
            if ( vfxTexture != null ) {
                for ( int i = 0; i < vfxTexture.Length; i++ ) {
                    if ( vfxTexture[i] )
                        RenderTexture.ReleaseTemporary( vfxTexture[i] );
                }
            }
        }


        protected void VFXPass() {

            if (internalRes < 16 ) {
                Unload();
                Load();
            }

            if ( !vfxMat ) {
                vfxMat = new Material( VFXProgressiveShader );
            }

            var targetMatrix = Owner.CurrentFurRenderer.renderer.transform.localToWorldMatrix;
            var targetMesh = Owner.CurrentMesh;

#if UNITY_2019_3_OR_NEWER
            var rd = new RenderTextureDescriptor( internalRes, internalRes, RenderTextureFormat.ARGB32, 0, 2 );
#else
            var rd = new RenderTextureDescriptor( internalRes, internalRes, RenderTextureFormat.ARGB32, 0 );
#endif

            for ( int i = 0; i < vfxTexture.Length; i++ ) {

                if ( vfxTexture[i] && Owner.MainRenderer.isFurMaterial[i] ) {
                    var tempRT1 = RenderTexture.GetTemporary( rd );

                    vfxMat.SetFloat( "_XFurBasicMode", Owner.RenderingMode == XFurStudioInstance.XFurRenderingMode.BasicShells ? 1 : 0 );
                    vfxMat.SetMatrix( "_XFurObjectMatrix", Owner.transform.localToWorldMatrix );
                    vfxMat.SetTexture( "_InputMap", vfxTexture[i] );
                    vfxMat.SetTexture( "_VFXMask0", Blood.vfxMask ? Blood.vfxMask : Texture2D.whiteTexture );
                    vfxMat.SetFloat( "_VFXTiling0", Blood.vfxTiling );
                    vfxMat.SetFloat( "_FXAdd0", 0 );
                    vfxMat.SetFloat( "_FXMelt0", ( 1.0f / Blood.fadeoutTime ) * updateTime );
                    vfxMat.SetTexture( "_VFXMask1", Snow.vfxMask ? Snow.vfxMask : Texture2D.whiteTexture );
                    vfxMat.SetFloat( "_VFXTiling1", Snow.vfxTiling );
                    vfxMat.SetFloat( "_FXAdd1", Snow.intensity * updateTime );
                    vfxMat.SetFloat( "_FXMelt1", ( 1.0f / Snow.fadeoutTime ) * updateTime );
                    vfxMat.SetFloat( "_FXFalloff1", Snow.normalFalloff );
                    vfxMat.SetTexture( "_VFXMask2", Rain.vfxMask ? Rain.vfxMask : Texture2D.whiteTexture );
                    vfxMat.SetFloat( "_VFXTiling2", Rain.vfxTiling );
                    vfxMat.SetFloat( "_FXAdd2", Rain.intensity * updateTime );
                    vfxMat.SetFloat( "_FXMelt2", ( 1.0f / Rain.fadeoutTime ) * updateTime );
                    vfxMat.SetFloat( "_FXFalloff2", Rain.normalFalloff );

                    if ( XFurStudioInstance.WindZone ) {
                        vfxMat.SetVector( "_XFurVFX1Direction", XFurStudioInstance.WindZone.SnowDirection );
                        vfxMat.SetVector( "_XFurVFX2Direction", XFurStudioInstance.WindZone.RainDirection );
                        vfxMat.SetFloat( "_XFurVFX1GlobalAdd", XFurStudioInstance.WindZone.SnowIntensity );
                        vfxMat.SetFloat( "_XFurVFX2GlobalAdd", XFurStudioInstance.WindZone.RainIntensity );
                    }


                    var currentActive = RenderTexture.active;
                    RenderTexture.active = tempRT1;
                    GL.Clear( true, true, new Color( 0, 0, 0, 0 ) );
                    vfxMat.SetPass( 0 );
                    Graphics.DrawMeshNow( targetMesh, targetMatrix, i );
                    RenderTexture.active = currentActive;

                    Graphics.Blit( tempRT1, vfxTexture[i] );

                    RenderTexture.ReleaseTemporary( tempRT1 );
                }

            }
        
        }




#if UNITY_EDITOR


        private bool[] fxFolds = new bool[8];

        public override void UpdateModule() {

            if (Snow.penetration < 0.1f ) {
                Snow.penetration = 0.65f;
            }

            if (Blood.penetration < 0.1f ) {
                Blood.penetration = 0.75f;
            }
            
            if (Rain.penetration < 0.1f ) {
                Rain.penetration = 0.75f;
            }

            moduleName = "VFX & Weather";
            version = "3.0";
            moduleStatus = 3;
            experimentalFeatures = false;
            hasMobileMode = true;
            hasSRPMode = true;


            if ( !VFXProgressiveShader ) {
                VFXProgressiveShader = Shader.Find( "Hidden/XFur Studio 2/VFX/VFXProgressive" );

                if ( !VFXProgressiveShader ) {
                    criticalError = true;
                    Debug.LogError( "Critical Error on the VFX Module : The GPU accelerated VFX shader has not been found. Please re-import the asset in order to restore the missing files" );
                }
            }

            if ( VFXProgressiveShader ) {
                criticalError = false;
            }

        }


        public override void ModuleUI() {

            base.ModuleUI();

            GUILayout.Space( 16 );

            quality = (ModuleQuality)StandardEnumField( new GUIContent( "FX Mask Quality", "The overall quality of the mask used to paint different FX over the fur" ), quality );

            if (!Application.isPlaying)
                updateTime = FloatField( new GUIContent( "Update Frequency (s)", "The update frequency (in seconds) of this module" ), Mathf.Clamp( updateTime, 0.01f, 1 ) );

            GUILayout.Space( 16 );

            if ( Owner.LODModule.enabled ) {
                disableOnLOD = EnableDisableToggle( new GUIContent( "Disable with LOD", "Disables this module when the character is far from the camera" ), disableOnLOD );
            }
            else {
                disableOnLOD = false;
            }

            GUILayout.Space( 16 );

            if (BeginCenteredGroup("Snow FX", ref fxFolds[0] ) ) {
                GUILayout.Space( 8 );
                Snow.color = ColorField( new GUIContent( "Snow Color" ), Snow.color );
                Snow.intensity = SliderField( new GUIContent( "Intensity" ), Snow.intensity, 0, 24 );
                Snow.penetration = SliderField( new GUIContent( "Penetration" ), Snow.penetration, 0.25f, 1.0f );
                Snow.fadeoutTime = FloatField( new GUIContent( "Fade Time" ), Snow.fadeoutTime );
                Snow.normalFalloff = SliderField( new GUIContent( "Falloff" ), Snow.normalFalloff, 0.05f, 2.0f );

                GUILayout.Space( 8 );

                Snow.vfxMask = ObjectField<Texture>( new GUIContent( "VFX Mask Texture" ), Snow.vfxMask );
                Snow.vfxTiling = SliderField( new GUIContent( "VFX Tiling" ), Snow.vfxTiling, 0, 32 );

                GUILayout.Space( 8 );
            }
            EndCenteredGroup();
            
            if (BeginCenteredGroup("Rain FX", ref fxFolds[1] ) ) {
                GUILayout.Space( 8 );

                Rain.intensity = SliderField( new GUIContent( "Intensity" ), Rain.intensity, 0, 24 );
                Rain.penetration = SliderField( new GUIContent( "Penetration" ), Rain.penetration, 0.25f, 1.0f );
                Rain.smoothness = SliderField( new GUIContent( "Smoothness" ), Rain.smoothness, 0.25f, 1.0f );
                Rain.fadeoutTime = FloatField( new GUIContent( "Fade Time" ), Rain.fadeoutTime );
                Rain.normalFalloff = SliderField( new GUIContent( "Falloff" ), Rain.normalFalloff, 0.05f, 2.0f );

                GUILayout.Space( 8 );

                Rain.vfxMask = ObjectField<Texture>( new GUIContent( "VFX Mask Texture" ), Rain.vfxMask );
                Rain.vfxTiling = SliderField( new GUIContent( "VFX Tiling" ), Rain.vfxTiling, 0, 32 );

                GUILayout.Space( 8 );
            }
            EndCenteredGroup();
            
            if (BeginCenteredGroup("Blood FX", ref fxFolds[2] ) ) {
                GUILayout.Space( 8 );

                Blood.color = ColorField( new GUIContent( "Blood Color" ), Blood.color );
                Blood.fadeoutTime = FloatField( new GUIContent( "Fade Time" ), Blood.fadeoutTime );
                Blood.penetration = SliderField( new GUIContent( "Penetration" ), Blood.penetration, 0.25f, 1.0f );
                Blood.smoothness = SliderField( new GUIContent( "Smoothness" ), Blood.smoothness, 0.25f, 1.0f );
                GUILayout.Space( 8 );

                Blood.vfxMask = ObjectField<Texture>( new GUIContent( "VFX Mask Texture" ), Blood.vfxMask );
                Blood.vfxTiling = SliderField( new GUIContent( "VFX Tiling" ), Blood.vfxTiling, 0, 32 );

                GUILayout.Space( 8 );
            }
            EndCenteredGroup();

            GUILayout.Space( 16 );


        }

#endif

    }
}