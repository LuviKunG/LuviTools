using UnityEngine;
using System.Collections;
using LuviKunG;

[ExecuteInEditMode]
public class CameraPixelPerUnit : LuviBehaviour
{
    [SerializeField]
    Camera mCamera;

    public int VerticalResolution = 768;
    public float PixelPerUnit = 32f;
    public float PPUScale = 1f;

    public float GetOrthographicSize()
    {
        return (VerticalResolution / (PixelPerUnit * PPUScale)) / 2f;
    }

    #if UNITY_EDITOR
    void Reset()
    {
        mCamera = GetComponent<Camera>();
        if (mCamera != null) Execute();
    }
    #endif

    void Execute()
    {
        if (mCamera.orthographic)
            mCamera.orthographicSize = GetOrthographicSize();
    }

    #if UNITY_EDITOR
    void Update()
    {
        Execute();
    }
    #endif
}
