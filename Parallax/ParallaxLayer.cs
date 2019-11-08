using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LuviKunG
{
    public class ParallaxLayer : MonoBehaviour
    {
        public float depth = 1.0f;

        [SerializeField]
        private Transform m_transform = default;

        public Vector3 position
        {
            get => m_transform.position;
            set => m_transform.position = value;
        }

        public void UpdatePosition(Parallax parallax)
        {
            position = parallax.position + (parallax.parallaxPosition * depth);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (m_transform == null)
                m_transform = transform;
        }
#endif
    }
}