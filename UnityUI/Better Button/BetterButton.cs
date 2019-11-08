using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LuviKunG
{
    [AddComponentMenu("UI/Better Button", 30)]
    public class BetterButton : Selectable, IEventSystemHandler, IPointerClickHandler, ISubmitHandler
    {
        [Serializable]
        public class ButtonClickedEvent : UnityEvent { }

        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickedEvent m_OnDown = new ButtonClickedEvent();

        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickedEvent m_OnUp = new ButtonClickedEvent();

        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

        public ButtonClickedEvent onDown => m_OnDown;
        public ButtonClickedEvent onUp => m_OnUp;
        public ButtonClickedEvent onClick => m_OnClick;

        protected BetterButton() { }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (IsActive() && IsInteractable())
                if (eventData.button == PointerEventData.InputButton.Left)
                    m_OnDown?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (IsActive() && IsInteractable())
                if (eventData.button == PointerEventData.InputButton.Left)
                    m_OnUp?.Invoke();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (IsActive() && IsInteractable())
                if (eventData.button == PointerEventData.InputButton.Left)
                    m_OnClick.Invoke();
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            if (IsActive() && IsInteractable())
            {
                m_OnClick.Invoke();
                DoStateTransition(SelectionState.Pressed, instant: false);
                StartCoroutine(OnFinishSubmit());
            }
        }

        private IEnumerator OnFinishSubmit()
        {
            float fadeTime = colors.fadeDuration;
            float elapsedTime = 0f;
            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }
            DoStateTransition(currentSelectionState, instant: false);
        }
    }
}