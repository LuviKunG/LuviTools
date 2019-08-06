using UnityEngine;
using UnityEditor;

namespace LuviKunG
{
    public static class MipMapBiasMenu
    {
        private static Object[] selection;

        [MenuItem("Assets/LuviKunG/Set Mipmap Bias", true)]
        static bool ValidateSetBias()
        {
            selection = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
            return (selection.Length > 0);
        }

        [MenuItem("Assets/LuviKunG/Set Mipmap Bias", false)]
        static void SetBias()
        {
            MipMapBiasEditor.OpenWindowWithSelection(selection);
        }
    }
}