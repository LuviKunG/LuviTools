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
                    m_rectTransform = transform as RectTransform;
                return m_rectTransform;
            }
            protected set
            {
                m_rectTransform = value;
            }
        }

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            if (m_rectTransform == null)
                m_rectTransform = GetComponent<RectTransform>();
        }
#endif
    }
}