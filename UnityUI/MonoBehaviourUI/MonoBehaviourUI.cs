using UnityEngine;

public class MonoBehaviourUI : MonoBehaviour
{
    public RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
                _rectTransform = transform as RectTransform;
            return _rectTransform;
        }
        protected set
        {
            _rectTransform = value;
        }
    }
    private RectTransform _rectTransform;
}
