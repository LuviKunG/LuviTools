using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("UI/Button Toggle")]
public class ButtonToggle : UIBehaviour, IPointerClickHandler
{
    public GameObject objectOn;
    public GameObject objectOff;
    [SerializeField]
    private bool m_state;
    public bool state
    {
        get => m_state;
        set
        {
            m_state = value;
            UpdateButtonState();
        }
    }
    public Button.ButtonClickedEvent onClick;

    private void UpdateButtonState()
    {
        objectOn?.SetActive(m_state);
        objectOff?.SetActive(!m_state);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        if (Application.isPlaying)
            return;
        UpdateButtonState();
    }
#endif
}
