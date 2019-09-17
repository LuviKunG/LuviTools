using UnityEngine;
using UnityEngine.UI;

namespace LuviKunG.UI
{
    [AddComponentMenu("LuviKunG/Unity UI/Touchable")]
    public class Touchable : Graphic
    {
        public override bool Raycast(Vector2 sp, Camera eventCamera)
        {
            return true;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}