using UnityEngine;
using System.Collections;

public class CameraPixelPerUnit : MonoBehaviour
{
    [SerializeField]
    Camera mCamera;

    public int VerticalResolution = 768;
    public float PixelPerUnit = 32f;
    public float PPUScale = 1f;

    public float OrthographicSize { get { return (VerticalResolution / (PixelPerUnit * PPUScale)) / 2f; } }

    void Reset()
    {
        mCamera.GetComponent<Camera>();
        if (mCamera != null) Execute();
    }

    [ContextMenu("Execute")]
    void Execute()
    {
        if (mCamera.orthographic)
        {
            mCamera.orthographicSize = OrthographicSize;
        }        
    }
}
