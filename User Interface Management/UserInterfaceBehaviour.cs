using UnityEngine;

namespace LuviKunG.UI
{
    public abstract class UserInterfaceBehaviour : MonoBehaviour
    {
        [SerializeField]
        private RectTransform m_rectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (m_rectTransform == null)
                    if (transform is RectTransform)
                        m_rectTransform = transform as RectTransform;
                return m_rectTransform;
            }
            protected set => m_rectTransform = value;
        }

        [SerializeField]
        private GameObject m_gameObject;
        public new GameObject gameObject
        {
            get
            {
                if (m_gameObject == null)
                    m_gameObject = base.gameObject;
                return m_gameObject;
            }
            protected set => m_gameObject = value;
        }

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            if (m_rectTransform == null)
                m_rectTransform = GetComponent<RectTransform>();
            if (m_gameObject == null)
                m_gameObject = base.gameObject;
        }
#endif
    }
}