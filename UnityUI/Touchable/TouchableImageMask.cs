using UnityEngine;
using UnityEngine.UI;

namespace LuviKunG.UI
{
    [AddComponentMenu("LuviKunG/Unity UI/Touchable (Image Mask)")]
    public class TouchableImageMask : Graphic
    {
        public Image image;
        public float alphaThreshold = 0.5f;

        public override bool Raycast(Vector2 sp, Camera eventCamera)
        {
            image.alphaHitTestMinimumThreshold = alphaThreshold;
            return image.Raycast(sp, eventCamera);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            if (image == null)
                image = GetComponent<Image>();
            base.Reset();
        }
#endif
    }
}