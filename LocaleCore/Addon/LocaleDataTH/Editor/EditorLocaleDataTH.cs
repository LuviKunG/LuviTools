using UnityEditor;

namespace LuviKunG.LocaleCore.Editor
{
    [CustomEditor(typeof(LocaleDataTH))]
    public class EditorLocaleDataTH : EditorLocaleData
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This object has apply an add-on 'Thai Font Adjuster'.", MessageType.Info);
            base.OnInspectorGUI();
        }
    }
}