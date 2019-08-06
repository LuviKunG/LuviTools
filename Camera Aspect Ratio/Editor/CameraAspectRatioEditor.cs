using UnityEditor;

namespace LuviKunG
{
    [CustomEditor(typeof(CameraAspectRatio))]
    public class CameraAspectRatioEditor : Editor
    {
        private CameraAspectRatio cameraAspect;

        private void OnEnable()
        {
            cameraAspect = target as CameraAspectRatio;
        }

        private void OnSceneGUI()
        {
            cameraAspect?.UpdateCameraAspect();
        }
    }
}
