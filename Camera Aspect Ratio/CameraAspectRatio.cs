using UnityEngine;

namespace LuviKunG
{
    [DisallowMultipleComponent, RequireComponent(typeof(Camera))]
    public class CameraAspectRatio : MonoBehaviour
    {
        public enum Mode
        {
            Width,
            Expand,
            Shrink
        }

        public Camera targetCamera;
        public bool runOnlyOnce;
        public Vector2 size = new Vector2(4, 3);
        public Mode mode;

        [SerializeField]
        private float m_zoomScale = 1.0f;
        public float zoomScale
        {
            get => m_zoomScale;
            set
            {
                m_zoomScale = value;
                if (m_zoomScale < 0.01f)
                    m_zoomScale = 0.01f;
            }
        }

        private float m_orthographicSize;
        public float orthographicSize
        {
            get => m_orthographicSize;
            set
            {
                m_orthographicSize = value;
                if (m_orthographicSize < 0.01f)
                    m_orthographicSize = 0.01f;
            }
        }

        private void Awake()
        {
            if (targetCamera == null)
                targetCamera = Camera.main;
        }

        private void Start()
        {
            UpdateCameraAspect();
        }

        private void Update()
        {
            UpdateCameraAspect();
            if (runOnlyOnce)
                enabled = false;
        }

        public void UpdateCameraAspect()
        {
            orthographicSize = size.x / targetCamera.aspect;
            if (mode == Mode.Width)
            {
                targetCamera.orthographicSize = size.x / targetCamera.aspect / m_zoomScale;
            }
            else if (mode == Mode.Expand)
            {
                if (orthographicSize > size.y)
                    targetCamera.orthographicSize = orthographicSize * (1 / m_zoomScale);
                else
                    targetCamera.orthographicSize = size.y * (1 / m_zoomScale);
            }
            else if (mode == Mode.Shrink)
            {
                if (orthographicSize > size.y)
                    targetCamera.orthographicSize = size.y * (1 / m_zoomScale);
                else
                    targetCamera.orthographicSize = orthographicSize * (1 / m_zoomScale);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (targetCamera == null)
                return;
            zoomScale = m_zoomScale;
            UpdateCameraAspect();
        }

        private void Reset()
        {
            if (targetCamera == null)
                targetCamera = GetComponent<Camera>();
        }

        private readonly Color gizmosColor = new Color(0, 1, 1, 1);

        private void OnDrawGizmos()
        {
            if (targetCamera == null || !enabled)
                return;
            DrawCameraAspectBox(0.5f);
        }

        private void OnDrawGizmosSelected()
        {
            if (targetCamera == null || !enabled)
                return;
            DrawCameraAspectBox(1.0f);
        }

        private void DrawCameraAspectBox(float a)
        {
            Color cache = Gizmos.color;
            Color color = gizmosColor;
            color.a = a;
            Gizmos.color = color;
            if (mode == Mode.Width)
            {
                Vector2 sizeWidth = new Vector2(size.x, size.x / targetCamera.aspect);
                Gizmos.DrawWireCube(transform.position, sizeWidth * 2);
            }
            else
            {
                Gizmos.DrawWireCube(transform.position, size * 2);
            }
            Gizmos.color = cache;
        }
#endif
    }
}