using UnityEngine;

[DisallowMultipleComponent, ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class CameraSorting : MonoBehaviour
{
    [SerializeField]
    public Camera targetCamera;
    public bool runOnlyOnce;
    public TransparencySortMode sortingMode;
    public Vector3 sortingAxis = new Vector3(0.0f, 0.0f, 1.0f);

    private void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    private void Update()
    {
        UpdateCameraSorting();
#if !UNITY_EDITOR
        if (runOnlyOnce) enabled = false;
#endif
    }

    [ContextMenu("Update Camera Sorting")]
    public void UpdateCameraSorting()
    {
        targetCamera.transparencySortMode = sortingMode;
        targetCamera.transparencySortAxis = sortingAxis;
    }

    [ContextMenu("Reset Camera Sorting")]
    public void ResetCameraSorting()
    {
        targetCamera.ResetTransparencySortSettings();
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

#if UNITY_EDITOR
    private void Reset()
    {
        if (targetCamera != null)
            targetCamera = GetComponent<Camera>();
    }
#endif
}
