using UnityEditor;

[CustomEditor(typeof(Touchable))]
public class TouchableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Touchable touchable = target as Touchable;
        EditorGUILayout.HelpBox("This UI will be touchable.", MessageType.Info);
        touchable.raycastTarget = EditorGUILayout.Toggle("Raycast Target", touchable.raycastTarget);
    }
}