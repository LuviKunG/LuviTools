using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LuviKunG
{
    public class Parallax : MonoBehaviour
    {
        public ParallaxLayer[] m_layers = new ParallaxLayer[0];

        [SerializeField]
        private Vector3 m_parallaxPosition = default;
        public Vector3 parallaxPosition
        {
            get => m_parallaxPosition;
            set
            {
                m_parallaxPosition = value;
                UpdateParallaxPosition();
            }
        }

        [SerializeField]
        private Transform m_transform = default;

        public Vector3 position
        {
            get => m_transform.position;
            set => m_transform.position = value;
        }

        public void UpdateParallaxPosition()
        {
            for (int i = 0; i < m_layers.Length; ++i)
                m_layers[i].UpdatePosition(this);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (m_transform == null)
                m_transform = transform;
        }

        private void OnValidate()
        {
            parallaxPosition = m_parallaxPosition;
        }

        [ContextMenu("Create Layer")]
        private void Editor_CreateLayer()
        {
            GameObject go = new GameObject(nameof(ParallaxLayer));
            ParallaxLayer layer = go.AddComponent<ParallaxLayer>();
            go.transform.SetParent(m_transform);
            go.transform.localScale = Vector3.one;
            List<ParallaxLayer> layers = new List<ParallaxLayer>(m_layers);
            layers.Add(layer);
            m_layers = layers.ToArray();
            UpdateParallaxPosition();
        }

        [ContextMenu("Batch Layers")]
        private void Editor_BatchLayers()
        {
            m_layers = GetComponentsInChildren<ParallaxLayer>(false);
        }
#endif
    }
}