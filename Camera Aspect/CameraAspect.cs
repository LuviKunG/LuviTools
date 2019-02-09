using UnityEngine;

[DisallowMultipleComponent, ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class CameraAspect : MonoBehaviour
{
    [NotNull]
    public Camera targetCamera;
    public float size;

    private void Awake()
    {
        UpdateCameraAspect();
    }

    private void Update()
    {
        UpdateCameraAspect();
    }

    [ContextMenu("Update Camera Aspect")]
    public void UpdateCameraAspect()
    {
        targetCamera.orthographicSize = size / targetCamera.aspect;
    }

#if UNITY_EDITOR
    private void Reset()
    {
        if (targetCamera == null)
            targetCamera = GetComponent<Camera>();
    }
#endif
}