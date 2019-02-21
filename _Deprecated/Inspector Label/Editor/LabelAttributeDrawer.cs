using UnityEngine;
using UnityEditor;
using System.Text;

namespace LuviKunG
{
    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class LabelAttributeDrawer : DecoratorDrawer
    {
        GUIStyle font = new GUIStyle();
        LabelAttribute label { get { return ((LabelAttribute)attribute); } }

        public override float GetHeight()
        {
            return label.size + 8f;
        }
        
        public override void OnGUI(Rect position)
        {
            font.fontSize = label.size;
            font.fontStyle = label.style;
            EditorGUI.LabelField(position, label.label, font);
        }
    }
}