namespace XFurStudio2 {

    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;

    [CustomEditor(typeof(XFurStudioFurProfile))]
    public class XFurStudioFurProfile_Editor : Editor {


        public GUISkin pidiSkin2;
        public Texture2D xfurStudioLogo;

        XFurStudioFurProfile profile;


        private void OnEnable() {
            profile = (XFurStudioFurProfile)target;
        }

        public override void OnInspectorGUI() {


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

            Undo.RecordObject( profile, "XFurStudioFurProfile_" + GetInstanceID() );

            GUILayout.BeginVertical( pidiSkin2.box );
            GUILayout.BeginHorizontal(); GUILayout.Space( 12 );
            GUILayout.BeginVertical();

            AssetLogoAndVersion();

            var lStyle = new GUIStyle();

            if ( serializedObject.isEditingMultipleObjects ) {

                HelpBox( "XFur Studio does not support multi-object editing", MessageType.Warning );

                GUILayout.Space( 16 );

                GUILayout.Space( 16 );

                GUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();


                lStyle.fontStyle = FontStyle.Italic;
                lStyle.alignment = TextAnchor.MiddleCenter;
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

            GUILayout.Space( 16 );

            CenteredLabel( "General Settings" );

            GUILayout.Space( 16 );

            profile.FurTemplate.CastShadows = EnableDisableToggle( new GUIContent( "Cast Shadows" ), profile.FurTemplate.CastShadows, true );
            profile.FurTemplate.ReceiveShadows = EnableDisableToggle( new GUIContent( "Receive Shadows" ), profile.FurTemplate.ReceiveShadows, true );
            profile.FurTemplate.FurSamples = IntSliderField( new GUIContent( "Fur Samples", "The amount of samples used to render the fur. More samples give better results, but may result in a reduced performance (especially when using ss" ), profile.FurTemplate.FurSamples, 4, 128 );

            GUILayout.Space( 8 );

            profile.FurTemplate.FurStrandsAsset = ObjectField<XFurStudioStrandsAsset>( new GUIContent( "Fur Strands Asset", "The texture map used to generate the fur strands for this fur profile" ), profile.FurTemplate.FurStrandsAsset );

            if ( profile.FurTemplate.FurStrandsAsset ) {
                profile.FurTemplate.FurUVTiling = FloatField( new GUIContent( "Fur Strands Tiling", "The tiling (UV size) to be applied to the fur strands" ), profile.FurTemplate.FurUVTiling );
            }

            GUILayout.Space( 24 );

            CenteredLabel( "Basic Shells Settings" );

            GUILayout.Space( 16 );

            profile.FurTemplate.SkinColor = ColorField( new GUIContent( "Skin Tint" ), profile.FurTemplate.SkinColor );
            profile.FurTemplate.SkinColorMap = ObjectField<Texture>( new GUIContent( "Skin Color Map", "The texture that controls the color applied for the skin under the fur in Basic Shells rendering mode" ), profile.FurTemplate.SkinColorMap );

            GUILayout.Space( 8 );
            profile.FurTemplate.SkinSmoothness = SliderField( new GUIContent( "Smoothness" ), profile.FurTemplate.SkinSmoothness );
            profile.FurTemplate.SkinNormalMap = ObjectField<Texture>( new GUIContent( "Skin Normal Map", "The texture that controls the color applied for the skin under the fur in Basic Shells rendering mode" ), profile.FurTemplate.SkinNormalMap );


            GUILayout.Space( 24 );

            CenteredLabel( "Fur Settings" );


            GUILayout.Space( 16 );

            profile.FurTemplate.FurColorMap = ObjectField<Texture>( new GUIContent( "Fur Color Map", "The texture that controls the color / albedo applied over the whole fur surface" ), profile.FurTemplate.FurColorMap );

            profile.FurTemplate.FurMainTint = ColorField( new GUIContent( "Fur Main Tint", "The main tint to be applied to the fur" ), profile.FurTemplate.FurMainTint );

            GUILayout.Space( 8 );

            profile.FurTemplate.FurRim = ColorField( new GUIContent( "Fur Rim Tint", "The main tint to be applied to the fur's rim lighting" ), profile.FurTemplate.FurRim );

            profile.FurTemplate.FurRimPower = SliderField( new GUIContent( "Fur Rim Power" ), profile.FurTemplate.FurRimPower, 0.1f, 10 );


            profile.FurTemplate.FurColorVariation = ObjectField<Texture>( new GUIContent( "Fur Color Variation", "The texture that controls four additional coloring variations to be applied over the fur, either all four to the whole fur or two to the undercoat and two to the overcoat by using the four color channels." ), profile.FurTemplate.FurColorVariation );
            

            if ( profile.FurTemplate.FurColorVariation ) {
                GUILayout.Space( 8 );
                profile.FurTemplate.FurColorA = ColorField( new GUIContent( "Fur Color A", "The fur color to be applied on the red channel of the Color Variation map" ), profile.FurTemplate.FurColorA );
                profile.FurTemplate.FurColorB = ColorField( new GUIContent( "Fur Color B", "The fur color to be applied on the green channel of the Color Variation map" ), profile.FurTemplate.FurColorB );
                profile.FurTemplate.FurColorC = ColorField( new GUIContent( "Fur Color C", "The fur color to be applied on the blue channel of the Color Variation map" ), profile.FurTemplate.FurColorC );
                profile.FurTemplate.FurColorD = ColorField( new GUIContent( "Fur Color D", "The fur color to be applied on the alpha channel of the Color Variation map" ), profile.FurTemplate.FurColorD );
                GUILayout.Space( 8 );
            }


            GUILayout.Space( 8 );

            profile.FurTemplate.FurData0 = ObjectField<Texture>( new GUIContent( "Fur Data Map", "The texture that controls the parameters of the fur :\n\n R = fur mask\n G = length\n B = occlusion\n A = thickness" ), profile.FurTemplate.FurData0 );
            profile.FurTemplate.FurData1 = ObjectField<Texture>( new GUIContent( "Fur Grooming Map", "The texture that controls the direction of the fur :\n\n RGB = fur direction\n A = stiffness" ), profile.FurTemplate.FurData1 );


            GUILayout.Space( 12 );

            profile.FurTemplate.FurLength = SliderField( new GUIContent( "Fur Length", "The maximum overall length of the fur. This will be multiplied by the actual fur profile length and the length painted in XFur Studio™ - Designer" ), profile.FurTemplate.FurLength );

            GUILayout.Space( 8 );
            profile.FurTemplate.FurThickness = SliderField( new GUIContent( "Fur Thickness", "The maximum overall thickness of the fur. This will be multiplied by the actual fur profile thickness and the thickness painted in XFur Studio™ - Designer" ), profile.FurTemplate.FurThickness );
            profile.FurTemplate.FurThicknessCurve = SliderField( new GUIContent( "Thickness Curve", "How the fur strands' thickness bias will change from the root to the top of each strand" ), profile.FurTemplate.FurThicknessCurve );
            GUILayout.Space( 8 );

            profile.FurTemplate.FurShadowsTint = ColorField( new GUIContent( "Occlusion Tint" ), profile.FurTemplate.FurShadowsTint );
            profile.FurTemplate.FurOcclusion = SliderField( new GUIContent( "Fur Occlusion / Shadowing", "The shadowing applied over the surface of the fur strands as a simple occlusion pass. Multiplied by the per-profile occlusion value and the one painted through XFur Studio™ - Designer" ), profile.FurTemplate.FurOcclusion );
            profile.FurTemplate.FurOcclusionCurve = SliderField( new GUIContent( "Fur Occlusion Curve", "How the shadowing / occlusion of the fur will go from the root to the tip of each strand" ), profile.FurTemplate.FurOcclusionCurve );



            GUILayout.Space( 32 );

            lStyle = new GUIStyle();
            lStyle.alignment = TextAnchor.MiddleCenter;
            lStyle.fontStyle = FontStyle.Italic;
            lStyle.normal.textColor = Color.white;
            lStyle.fontSize = 8;

            GUILayout.Label( "Copyright© 2017-2020,   Jorge Pinal N.", lStyle );

            GUILayout.Space( 16 );

            GUILayout.EndVertical();
            GUILayout.Space( 12 ); GUILayout.EndHorizontal();
            GUILayout.EndVertical();


            EditorUtility.SetDirty( profile );

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
            GUILayout.Label( profile.Version, pidiSkin2.customStyles[2] );
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
        public float SliderField( GUIContent label, float currentValue, float minSlider = 0.0f, float maxSlider = 1.0f, string suffix = "" ) {

            GUILayout.Space( 4 );
            GUILayout.BeginHorizontal();
            GUILayout.Label( label, pidiSkin2.label, GUILayout.Width( EditorGUIUtility.labelWidth ) );
            GUI.color = Color.gray;
            currentValue = GUILayout.HorizontalSlider( currentValue, minSlider, maxSlider, GUI.skin.horizontalSlider, GUI.skin.horizontalSliderThumb );
            GUI.color = Color.white;
            GUILayout.Space( 12 );
            currentValue = Mathf.Clamp( EditorGUILayout.FloatField( float.Parse( currentValue.ToString( "n2" ) ), pidiSkin2.customStyles[4], GUILayout.MaxWidth( 40 ) ), minSlider, maxSlider );
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