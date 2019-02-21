using UnityEngine;

[DisallowMultipleComponent, ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class CameraAspectRatio : MonoBehaviour
{
    [NotNull]
    public Camera targetCamera;
    public bool runOnlyOnce;
    public Vector2 size = new Vector2(4, 3);
    public float zoomScale = 1.0f;

    private float orthographicSize;

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

    [ContextMenu("Update Camera Aspect Ratio")]
    public void UpdateCameraAspect()
    {
        if (zoomScale < 0.01f)
            zoomScale = 0.01f;
        orthographicSize = size.x / targetCamera.aspect;
        if (orthographicSize < 0.01f)
            orthographicSize = 0.01f;
        if (orthographicSize > size.y)
            targetCamera.orthographicSize = orthographicSize * (1 / zoomScale);
        else
            targetCamera.orthographicSize = size.y * (1 / zoomScale);
    }

#if UNITY_EDITOR
    private void Reset()
    {
        if (targetCamera == null)
            targetCamera = GetComponent<Camera>();
    }

    private readonly Color gizmosColor = new Color(0, 1, 1, 1);

    private void OnDrawGizmos()
    {
        DrawCameraAspectBox(0.5f);
    }

    private void OnDrawGizmosSelected()
    {
        DrawCameraAspectBox(1.0f);
    }

    private void DrawCameraAspectBox(float a)
    {
        Color cache = Gizmos.color;
        Color color = gizmosColor;
        color.a = a;
        Gizmos.color = color;
        Gizmos2D.DrawWireBox(transform.position, size);
        Gizmos.color = cache;
    }
#endif
}