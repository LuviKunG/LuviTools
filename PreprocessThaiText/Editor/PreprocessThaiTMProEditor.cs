using UnityEditor;
using UnityEngine;

namespace LuviKunG
{
    [CustomEditor(typeof(PreprocessThaiTMPro))]
    [CanEditMultipleObjects]
    public class PreprocessThaiTMProEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Initialize"))
            {
                foreach (var target in targets)
                {
                    if (!(target is PreprocessThaiTMPro))
                        continue;
                    var component = target as PreprocessThaiTMPro;
                    component.Initialize();
                }
            }
        }
    }
}