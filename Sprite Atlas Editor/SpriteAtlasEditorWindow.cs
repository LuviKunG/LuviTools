using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.U2D;

namespace LuviKunG.Editor
{
    public sealed class SpriteAtlasEditorWindow : EditorWindow
    {
        private const string WINDOW_TITLE = "Sprite Atlas Editor";

        private SpriteAtlas m_spriteAtlas;
        private Vector2 m_scrollPosition;

        public static SpriteAtlasEditorWindow OpenEditorWindow()
        {
            SpriteAtlasEditorWindow window = GetWindow<SpriteAtlasEditorWindow>();
            window.titleContent = new GUIContent(WINDOW_TITLE, EditorGUIUtility.Load("SpriteAtlas On Icon") as Texture2D);
            window.Show();
            window.Focus();
            return window;
        }

        public static SpriteAtlasEditorWindow OpenEditorWindow(SpriteAtlas spriteAtlas)
        {
            SpriteAtlasEditorWindow window = OpenEditorWindow();
            window.Load(spriteAtlas);
            return window;
        }

        public void Load(SpriteAtlas spriteAtlas)
        {
            m_spriteAtlas = spriteAtlas;
            SerializedObject soSpriteAtlas = new SerializedObject(m_spriteAtlas);
            foreach (SerializedProperty a in soSpriteAtlas.GetIterator())
                Debug.Log(a.name);
        }

        private void OnGUI()
        {
            if (m_spriteAtlas == null)
            {
                m_spriteAtlas = EditorGUILayout.ObjectField(new GUIContent("Sprite Atlas"), m_spriteAtlas, typeof(SpriteAtlas), false) as SpriteAtlas;
            }   
            else
            {
                using (var scrollViewScope = new GUILayout.ScrollViewScope(m_scrollPosition))
                {
                    m_scrollPosition = scrollViewScope.scrollPosition;
                    SerializedObject soSpriteAtlas = new SerializedObject(m_spriteAtlas);
                    var spPackedSprites = soSpriteAtlas.FindProperty("m_PackedSprites");
                    EditorGUILayout.PropertyField(spPackedSprites);
                    var spPackedSpriteNameToIndex = soSpriteAtlas.FindProperty("m_PackedSpriteNamesToIndex");
                    EditorGUILayout.PropertyField(spPackedSpriteNameToIndex);
                    GUILayout.Space(0.0f);
                }
            }
        }
    }
}