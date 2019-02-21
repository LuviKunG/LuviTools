using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public sealed class CombineAsset : EditorWindow
{
    private Object parent;
    private Object assetChild;
    private Object assetDelete;

    private List<GUIContent> listContentAssetDelete;
    private List<Object> listObjectAssetDelete;
    private int selectedAssetDeleteIndex;
    private string rename;

    [MenuItem("Window/Combine Asset")]
    public static CombineAsset OpenWindow()
    {
        CombineAsset window = GetWindow<CombineAsset>(true, "Combine Asset", true);
        window.Show();
        return window;
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        parent = EditorGUILayout.ObjectField("Parent", parent, typeof(Object), false);
        if (GUILayout.Button("Refresh", GUILayout.MaxWidth(64.0f)))
        {
            RefreshDeletion();
        }
        if (EditorGUI.EndChangeCheck())
        {
            RefreshDeletion();
        }
        GUILayout.EndHorizontal();
        if (parent != null)
        {
            GUILayout.Space(16.0f);
            GUILayout.BeginHorizontal();
            rename = EditorGUILayout.TextField(rename);
            if (GUILayout.Button("Rename", GUILayout.MaxWidth(64.0f)))
            {
                parent.name = rename;
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(parent));
                EditorUtility.SetDirty(parent);
                AssetDatabase.SaveAssets();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            assetChild = EditorGUILayout.ObjectField("Asset to Combine", assetChild, typeof(Object), false);
            if (GUILayout.Button("Combine", GUILayout.MaxWidth(64.0f)))
            {
                if (parent == null || assetChild == null)
                {
                    EditorUtility.DisplayDialog("Combine Asset Error", "Please select parent and child to conbine.", "Okay");
                }
                else
                {
                    Combine(parent, assetChild);
                }
            }
            GUILayout.EndHorizontal();
            if (listContentAssetDelete.Count > 0 && listObjectAssetDelete.Count > 0)
            {
                GUILayout.BeginHorizontal();
                selectedAssetDeleteIndex = EditorGUILayout.Popup(selectedAssetDeleteIndex, listContentAssetDelete.ToArray());
                if (GUILayout.Button("Delete", GUILayout.MaxWidth(64.0f)))
                {
                    assetDelete = listObjectAssetDelete[selectedAssetDeleteIndex];
                    if (assetDelete == null)
                    {
                        EditorUtility.DisplayDialog("Delete Asset Error", "Please input an asset to delete.", "Okay");
                    }
                    else
                    {
                        Delete(parent, assetDelete);
                        RefreshDeletion();
                    }
                }
                GUILayout.EndHorizontal();
            }

        }
    }

    private void RefreshDeletion()
    {
        rename = parent.name;
        selectedAssetDeleteIndex = 0;
        Object[] objs = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(parent));
        listContentAssetDelete = listContentAssetDelete == null ? new List<GUIContent>() : listContentAssetDelete;
        listContentAssetDelete.Clear();
        listObjectAssetDelete = listObjectAssetDelete == null ? new List<Object>() : listObjectAssetDelete;
        listObjectAssetDelete.Clear();
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i] != parent)
            {
                listContentAssetDelete.Add(string.IsNullOrEmpty(objs[i].name.Trim()) ? new GUIContent("(No Name)") : new GUIContent(objs[i].name));
                listObjectAssetDelete.Add(objs[i]);
            }
        }
    }

    private void ShowHideSubAsset(Object parent, HideFlags flag)
    {
        Object[] os = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(parent));
        foreach (Object o in os) { if (o != parent) { o.hideFlags = flag; EditorUtility.SetDirty(o); } }
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(parent));
        EditorUtility.SetDirty(parent);
        AssetDatabase.SaveAssets();
    }

    private void Combine(Object parent, Object child)
    {
        Object newAssetObj = Instantiate(child);
        newAssetObj.name = child.name;
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(child));
        AssetDatabase.AddObjectToAsset(newAssetObj, parent);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(parent));
        EditorUtility.SetDirty(parent);
        AssetDatabase.SaveAssets();
    }

    private void Delete(Object parent, Object delete)
    {
        DestroyImmediate(delete, true);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(parent));
        EditorUtility.SetDirty(parent);
        AssetDatabase.SaveAssets();
    }
}