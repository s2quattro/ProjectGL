using UnityEngine;

namespace CityStage
{
    public class Sprite2DOutlineHighLighter : MonoBehaviour
    {
        [Header("Shader Option")]
        [Range(0, 16)]
        public int outlineSize = 1;

        private SpriteRenderer spriteRenderer;
        private Color hightLightingColor;

        //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
        //public Sprite2DOutlineHighLighter()
        //{
        //    initShader(GetComponent<SpriteRenderer>(), LinkContainer.Instance.localProperties.hightLightColor);
        //}

        public void initShader(SpriteRenderer renderer, Color setColor)
        {
            spriteRenderer = renderer;
            hightLightingColor = setColor;
        }

        public void exeToggleOutline(bool flag)
        {
            UpdateOutline(flag);
        }

        void UpdateOutline(bool outline)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat("_Outline", outline ? 1f : 0);
            mpb.SetColor("_OutlineColor", hightLightingColor);
            mpb.SetFloat("_OutlineSize", outlineSize);
            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}