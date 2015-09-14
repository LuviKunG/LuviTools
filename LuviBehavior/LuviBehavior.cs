using UnityEngine;

public class LuviBehavior : MonoBehaviour
{
    /*
     * LuviBehavior 1.1.0
     * By LuviKunG
     * - Remove LuviDebug Log
     * - Add Rigidbody & Rigidbody2D cache
     */
    private Transform _transform = null;
    public new Transform transform
    {
        get
        {
            if (_transform == null)
            {
                _transform = base.transform;
            }
            return _transform;
        }
    }

    private GameObject _gameObject = null;
    public new GameObject gameObject
    {
        get
        {
            if (_gameObject == null)
            {
                _gameObject = base.gameObject;
            }
            return _gameObject;
        }
    }

    private RectTransform _rectTransform;
    public RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }

    private Rigidbody _rigidbody;
    public Rigidbody rigidbody
    {
        get
        {
            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }
            return _rigidbody;
        }
    }

    private Rigidbody2D _rigidbody2D;
    public Rigidbody2D rigidbody2D
    {
        get
        {
            if (_rigidbody2D == null)
            {
                _rigidbody2D = GetComponent<Rigidbody2D>();
            }
            return _rigidbody2D;
        }
    }
}