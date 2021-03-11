using Hsinpa.App;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Hsinpa.Shader {
    public class DrawToTexture : MonoBehaviour
    {
        [SerializeField, Range(0, 1)]
        private float _Power = 0.1f;

        [SerializeField, Range(0, 1)]
        private float _Range = 0.1f;

        [SerializeField]
        private UnityEngine.Shader DrawShader;

        [SerializeField]
        private UnityEngine.Shader ScoreShader;

        [SerializeField]
        private Texture maskTexture;

        private RenderTexture buffer;

        //To Check how many dirt is been clean
        private RenderTexture scoreBuffer;

        private string ShaderMainTex = "_MainTex";
        private string ShaderPowerKey = "_Power";
        private string ShaderPositionKey = "_MousePosition";
        private string ShaderColorKey = "_Color";
        private string ShaderRangeKey = "_Range";

        private string ScoreShaderPaintedTex = "_PaintedTex";
        private string ShaderShowHintKey = "_OverrideColor";
        private string ShaderShowHintAnimKey = "_ColorHintPower";

        private Material drawMaterial;
        private Material scoreMaterial;
        private Material targetMaterial;
        Texture2D _cacheTex;
        private int scoreTexSize = 24;

        public void SetUp(Material targetMaterial)
        {
            this.targetMaterial = targetMaterial;
            drawMaterial = new Material(DrawShader);
            scoreMaterial = new Material(ScoreShader);
            _cacheTex = new Texture2D(scoreTexSize, scoreTexSize, TextureFormat.RGB24, false);
            ResetBuffer();
        }

        public void EnableColorHint(PaintingManager.HintState hintState)
        {
            this.targetMaterial.SetInt(ShaderShowHintKey, 0);
            this.targetMaterial.SetFloat(ShaderShowHintAnimKey,1);

            switch (hintState) {
                case PaintingManager.HintState.None: {

                    }
                    break;

                case PaintingManager.HintState.Flash: {
                        this.targetMaterial.SetFloat(ShaderShowHintAnimKey, 0.2f);
                    }
                    break;
            }
        }

        public void SetPaintColor(Color p_color) {
            if (this.targetMaterial != null)
                this.targetMaterial.SetColor(ShaderColorKey, p_color);
        }

        public void DrawOnMesh(Vector2 textureCoord, Color paintColor) {

            drawMaterial.SetFloat(ShaderPowerKey, _Power);
            drawMaterial.SetFloat(ShaderRangeKey, _Range);
            drawMaterial.SetVector(ShaderPositionKey, textureCoord);
            drawMaterial.SetColor(ShaderColorKey, paintColor);

            RenderTexture temp = RenderTexture.GetTemporary(buffer.width, buffer.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(buffer, temp);
            Graphics.Blit(temp, buffer, drawMaterial);
            RenderTexture.ReleaseTemporary(temp);
        }

        public async Task<float> CalScoreOnDrawMat(Color paintColor)
        {
            scoreMaterial.SetColor(ShaderColorKey, paintColor);

            Graphics.Blit(null, scoreBuffer, scoreMaterial, 0);

            return await CalculateOnCPU(paintColor);
        }

        private async Task<float> CalculateOnCPU(Color paintColor) {

            Color[] colors = Utility.UtilityMethod.ToColor(Utility.UtilityMethod.toTexture2D(scoreBuffer, _cacheTex));
            Color whiteColor = Color.white;
            float allocateColor = 0;

            return await Task.Run<float>(() =>
            {
                for (int x = 0; x < scoreTexSize; x++) 
                {
                    for (int y = 0; y < scoreTexSize; y++)
                    {
                        int index = x + (y * scoreTexSize);
                        Color invertColor = whiteColor - paintColor;
                        Color targetColor = colors[index] - invertColor;

                        allocateColor += Mathf.Clamp(targetColor.r, 0, 1) + Mathf.Clamp(targetColor.g, 0, 1) + Mathf.Clamp(targetColor.b, 0, 1);
                    }
                }

                return allocateColor / (scoreTexSize * scoreTexSize);
            });


        }

#if UNITY_EDITOR
        //private void OnGUI()
        //{
        //    GUI.DrawTexture(new Rect(0, 0, 128, 128), buffer, ScaleMode.ScaleToFit, false, 1);
        //    GUI.DrawTexture(new Rect(128, 0, 128, 128), scoreBuffer, ScaleMode.ScaleToFit, false, 1);
        //}
#endif

        public void ResetBuffer() {
            buffer = new RenderTexture(128, 128, 0, RenderTextureFormat.ARGBFloat);
            scoreBuffer = new RenderTexture(scoreTexSize, scoreTexSize, 0, RenderTextureFormat.ARGBFloat);

            this.scoreMaterial.SetTexture(ShaderMainTex, maskTexture);
            this.scoreMaterial.SetTexture(ScoreShaderPaintedTex, buffer);
            this.targetMaterial.SetTexture("_EraseTex", buffer);
            EnableColorHint(PaintingManager.HintState.None);
        }
    }
}
