using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public sealed class MipMapBiasEditor : EditorWindow
{
    [MenuItem("Window/Mip Map Bias")]
    public static MipMapBiasEditor OpenWindow()
    {
        MipMapBiasEditor window = GetWindow<MipMapBiasEditor>(true, "Mip Map Bias", true);
        window.Show();
        return window;
    }

    private List<Texture> list = new List<Texture>();
    private Texture texture;
    private float bias;
    private Vector2 scrollview;

    private void OnGUI()
    {
        scrollview = EditorGUILayout.BeginScrollView(scrollview, GUILayout.MaxHeight(256.0f));
        EditorGUI.BeginChangeCheck();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == null) list.RemoveAt(i--);
            else list[i] = (Texture)EditorGUILayout.ObjectField("Texture", list[i], typeof(Texture), false);
        }
        texture = (Texture)EditorGUILayout.ObjectField("Texture", texture, typeof(Texture), false);
        EditorGUILayout.EndScrollView();
        bias = EditorGUILayout.FloatField("Mip Map Bias", bias);
        if (EditorGUI.EndChangeCheck())
        {
            if (texture != null)
            {
                list.Add(texture);
                texture = null;
            }
        }
        if (GUILayout.Button("Update Bias"))
        {
            foreach (Texture item in list)
            {
                Undo.RecordObject(item, "Update Mip Map Bias on " + item.name);
                item.mipMapBias = bias;
                EditorUtility.SetDirty(item);
            }
        }
    }
}