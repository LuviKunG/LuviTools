using UnityEngine;

[DisallowMultipleComponent, ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class CameraAspect : MonoBehaviour
{
    [NotNull]
    public Camera targetCamera;
    public bool runOnlyOnce;
    public float size = 5.0f;

    private void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
        UpdateCameraAspect();
    }

    private void Update()
    {
        UpdateCameraAspect();
#if !UNITY_EDITOR
        if (runOnlyOnce) enabled = false;
#endif
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