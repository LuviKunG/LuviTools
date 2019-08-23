using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace LuviKunG
{
    public sealed class AssetsManagementEditorWindow : EditorWindow
    {
        public struct Assets
        {
            public Object obj;
            public string rename;
            public bool isShow;

            public Assets(Object obj) : this()
            {
                this.obj = obj;
                rename = obj.name;
                isShow = !GetAssetHidden();
            }

            public bool GetAssetHidden() => obj.hideFlags.HasFlag(HideFlags.HideInHierarchy);
        }

        public const float BUTTON_RENAME_SIZE = 64.0f;
        public const float TOGGLE_HIDDEN_SIZE = 20.0f;
        public static readonly GUIContent CONTENT_ADD_OBJECT = new GUIContent("Add Object");
        public static readonly GUIContent CONTENT_BUTTON_SHOW = new GUIContent("Show All Sub Assets");
        public static readonly GUIContent CONTENT_BUTTON_HIDE = new GUIContent("Hide All Sub Assets");

        public static Object selection;

        private List<Assets> listChildAssets;
        private ReorderableList list;

        [MenuItem("Assets/LuviKunG/Open Assets Management Window", true)]
        public static bool ValidateAssetsManagement()
        {
            var filter = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
            selection = filter.Length == 1 ? filter[0] : null;
            return selection != null;
        }

        [MenuItem("Assets/LuviKunG/Open Assets Management Window", false)]
        public static AssetsManagementEditorWindow OpenWindow()
        {
            AssetsManagementEditorWindow window = GetWindow<AssetsManagementEditorWindow>(true, "Assets Management", true);
            window.Show();
            window.UpdateContext();
            return window;
        }

        private void OnGUI()
        {
            if (selection == null)
            {
                EditorGUILayout.HelpBox("Please select some asset using menu 'Assets > LuviKunG > Combine Asset'.", MessageType.Warning, true);
            }
            else
            {
                list.DoLayoutList();
                using (var changeScope = new EditorGUI.ChangeCheckScope())
                {
                    var addObject = EditorGUILayout.ObjectField(CONTENT_ADD_OBJECT, null, typeof(Object), false);
                    if (changeScope.changed && addObject != null)
                    {
                        Combine(selection, addObject);
                        UpdateContext();
                    }
                }
            }
        }

        private void UpdateContext()
        {
            Object[] objs = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(selection));
            listChildAssets = new List<Assets>();
            for (int i = 0; i < objs.Length; i++)
                if (objs[i] != selection)
                    listChildAssets.Add(new Assets(objs[i]));
            list = new ReorderableList(listChildAssets, typeof(Assets), false, true, false, true);
            list.drawHeaderCallback = DrawHeader;
            list.drawElementCallback = DrawElement;
            list.elementHeightCallback = ElementHeight;
            list.onRemoveCallback = OnRemove;
            //local function.
            void DrawHeader(Rect rect)
            {
                EditorGUI.LabelField(rect, selection.name);
            }
            void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
            {
                rect.height = EditorGUIUtility.singleLineHeight;
                var asset = listChildAssets[index];
                var contentIcon = EditorGUIUtility.ObjectContent(asset.obj, asset.obj.GetType()).image;
                var rectIcon = new Rect(rect.x, rect.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
                var rectContent = new Rect(rectIcon.x + rectIcon.width + 4.0f, rect.y, rect.width - rectIcon.width, rect.height);
                var rectToggle = new Rect(rectContent.x, rect.y, TOGGLE_HIDDEN_SIZE, rect.height);
                var rectText = new Rect(rectToggle.x + rectToggle.width, rect.y, rectContent.width - BUTTON_RENAME_SIZE - TOGGLE_HIDDEN_SIZE - 6.0f, rect.height);
                var rectButton = new Rect(rectText.x + rectText.width + 2.0f, rect.y, BUTTON_RENAME_SIZE, rect.height);
                var cacheAssetShow = !asset.GetAssetHidden();
                GUI.DrawTexture(rectIcon, contentIcon);
                asset.isShow = EditorGUI.Toggle(rectToggle, asset.isShow);
                asset.rename = EditorGUI.TextField(rectText, GUIContent.none, asset.rename);
                if (asset.isShow != cacheAssetShow)
                    ShowHideAsset(selection, asset.obj, !asset.isShow);
                using (var disableScope = new EditorGUI.DisabledGroupScope(asset.rename == asset.obj.name))
                {
                    if (GUI.Button(rectButton, "Rename"))
                    {
                        asset.obj.name = asset.rename;
                        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(selection));
                        EditorUtility.SetDirty(selection);
                        AssetDatabase.SaveAssets();
                    }
                }
                listChildAssets[index] = asset;
            }
            float ElementHeight(int index)
            {
                return EditorGUIUtility.singleLineHeight + 2.0f;
            }
            void OnRemove(ReorderableList list)
            {
                if (list.index < 0)
                    return;
                Delete(selection, listChildAssets[list.index].obj);
                UpdateContext();
            }
        }

        private void ShowHideAsset(Object parent, Object asset, bool isHidden)
        {
            asset.hideFlags = isHidden ? HideFlags.HideInHierarchy : HideFlags.None;
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
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
}