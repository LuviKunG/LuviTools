using UnityEditor;

[CustomEditor(typeof(AndroidScreenSleepTimeout))]
public class AndroidScreenSleepTimeoutEditor : Editor
{
    private SerializedProperty type;
    private SerializedProperty sleepTimeout;

    private void OnEnable()
    {
        type = serializedObject.FindProperty("type");
        sleepTimeout = serializedObject.FindProperty("customSleepTimeout");
    }

    public override void OnInspectorGUI()
    {
        type.intValue = (int)(AndroidScreenSleepTimeout.Type)EditorGUILayout.EnumPopup("Type", (AndroidScreenSleepTimeout.Type)type.intValue);
        //type.enumValueIndex = EditorGUILayout.Popup("Type", type.enumValueIndex, type.enumDisplayNames);
        if ((AndroidScreenSleepTimeout.Type)type.intValue == AndroidScreenSleepTimeout.Type.Custom)
        //if (type.enumDisplayNames[type.enumValueIndex] == AndroidScreenSleepTimeout.Type.Custom.ToString())
        {
            sleepTimeout.intValue = EditorGUILayout.IntField("Second to sleep", sleepTimeout.intValue);
            if (sleepTimeout.intValue < 0)
                sleepTimeout.intValue = 0;
        }
        serializedObject.ApplyModifiedProperties();
    }
}