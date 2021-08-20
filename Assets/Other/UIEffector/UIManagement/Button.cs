using FrolicRummy.TweenManagment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace FrolicRummy.UIManagement
{
    [RequireComponent(typeof(Interactable))]
    public class Button : Widget, IPointerClickHandler
    {
        private Interactable mInteractable;
        [SerializeField] private UnityEvent m_OnClick;

        protected Interactable pInteractable { get { if (mInteractable == null) mInteractable = GetComponent<Interactable>(); return mInteractable; } }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!pInteractable.IsInteractable() && !eventData.IsPointerValid())
                return;
            if (m_OnClick != null)
                m_OnClick.Invoke();
            if (pEventTarget != null)
                pEventTarget.TriggerOnClick(this, eventData);
        }
    }
}
