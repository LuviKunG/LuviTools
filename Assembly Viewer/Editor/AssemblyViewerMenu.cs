using UnityEditor;

public static class AssemblyViewerMenu
{
    [MenuItem("Window/LuviKunG/Assembly Viewer")]
    public static void Open()
    {
        _ = AssemblyViewerWindow.Open();
    }
}
