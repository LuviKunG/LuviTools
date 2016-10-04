using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

namespace LuviKunG
{
    [CustomPropertyDrawer(typeof(DividerAttribute))]
    public class DividerAttributeDrawer : DecoratorDrawer
    {
        public static Texture2D lineTex = null;
        DividerAttribute divider { get { return ((DividerAttribute)attribute); } }

        public override float GetHeight()
        {
            return divider.space;
        }

        public override void OnGUI(Rect position)
        {
            Color co = Color.black;
            switch (divider.color.ToLower())
            {
                case "white": co = Color.white; break;
                case "red": co = Color.red; break;
                case "blue": co = Color.blue; break;
                case "green": co = Color.green; break;
                case "gray": co = Color.gray; break;
                case "grey": co = Color.grey; break;
                case "black": co = Color.black; break;
            }

            lineTex = new Texture2D(1, 1, TextureFormat.ARGB32, true);
            lineTex.SetPixel(0, 1, co);
            lineTex.Apply();

            float lineWidth = position.width * divider.width;
            float lineX = ((position.x + position.width) - lineWidth - ((position.width - lineWidth) / 2));
            float lineY = position.y + (divider.space / 2);
            float lineHeight = divider.thickness;

            EditorGUI.DrawPreviewTexture(new Rect(lineX, lineY, lineWidth, lineHeight), lineTex);
        }
    }
}
