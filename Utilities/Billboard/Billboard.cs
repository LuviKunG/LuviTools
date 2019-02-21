using UnityEngine;

namespace LuviKunG
{
    public class Billboard : CacheBehaviour
    {
        public new Camera camera;
        public bool runOnUpdate;

        private void OnEnable()
        {
            enabled = runOnUpdate;
            UpdateBillboard();
        }

        private void Update()
        {
            UpdateBillboard();
        }

        public void UpdateBillboard()
        {
            if (camera == null)
                camera = Camera.main;
            transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (camera == null)
                camera = Camera.main;
        }
#endif
    }
}