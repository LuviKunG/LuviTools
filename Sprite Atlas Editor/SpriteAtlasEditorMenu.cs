using UnityEditor;
using UnityEngine.U2D;

namespace LuviKunG.Editor
{
    public static class SpriteAtlasEditorMenu
    {
        [MenuItem("Assets/Sprite Atlas Editor", validate = true)]
        public static bool ValidateAssetOpenEditorWindow()
        {
            return Selection.activeObject is SpriteAtlas;
        }

        [MenuItem("Assets/Sprite Atlas Editor", validate = false)]
        public static void AssetOpenEditorWindow()
        {
            SpriteAtlas spriteAtlas = Selection.activeObject as SpriteAtlas;
            SpriteAtlasEditorWindow.OpenEditorWindow(spriteAtlas);
        }

        [MenuItem("Windows/Sprite Atlas Editor")]
        public static void OpenEditorWindow()
        {
            SpriteAtlasEditorWindow.OpenEditorWindow();
        }
    }
}