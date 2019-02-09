using UnityEditor;
using System.Text;

namespace LuviKunG.LocaleCore.Editor
{
    [CustomEditor(typeof(LocaleData))]
    public class EditorLocaleData : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            LocaleData localeData = target as LocaleData;
            StringBuilder str = new StringBuilder();
            EditorGUILayout.LabelField("List of Key");
            for (int i = 0; i < localeData.list.Length - 1; i++) { str.AppendLine(localeData.list[i].Key); }
            str.Append(localeData.list[localeData.list.Length - 1].Key);
            EditorGUILayout.HelpBox(str.ToString(), MessageType.None);
        }
    }
}