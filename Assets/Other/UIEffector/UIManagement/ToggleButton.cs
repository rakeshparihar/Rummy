using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FrolicRummy.UIManagement
{
    //WIP: Add group, checked state effects
    public class ToggleButton : Widget, IPointerClickHandler
    {
        [SerializeField] private Graphic m_Checkmark;
        [SerializeField] private bool m_IsChecked = true;
        [SerializeField] private GameObject m_NormalStateEffectsHolder;
        [SerializeField] private GameObject m_CheckedStateEffectsHolder;

        private IEffecter[] mNormalStateEffects;
        private IEffecter[] mCheckedStateEffects;

        private Interactable mInteractable;
        protected Interactable pInteractable { get { if (mInteractable == null) mInteractable = GetComponent<Interactable>(); return mInteractable; } }

        public bool pIsChecked
        {
            get { return m_IsChecked; }
            set
            {
                if (m_IsChecked == value)
                    return;
                m_IsChecked = value;
                UpdateEffecters();
                UpdateGraphics();
            }
        }


        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!pInteractable.IsInteractable() && !eventData.IsPointerValid())
                return;

            Toggle();
            if (pEventTarget != null)
                pEventTarget.TriggerOnClick(this, eventData);
        }

        private void Toggle()
        {
            if (!IsActive() || !pInteractable.IsInteractable())
                return;

            pIsChecked = !pIsChecked;
        }

        private void UpdateGraphics()
        {
            if (m_Checkmark != null)
                m_Checkmark.enabled = pIsChecked;
        }

        private void UpdateEffecters()
        {
            if (!pIsChecked)
            {
                if (mNormalStateEffects == null && m_NormalStateEffectsHolder != null)
                    mNormalStateEffects = m_NormalStateEffectsHolder.GetComponents<IEffecter>();
                pInteractable.UpdateEffecterAndReDoStateTransition(mNormalStateEffects);
            }
            else
            {
                if (mCheckedStateEffects == null && m_CheckedStateEffectsHolder != null)
                    mCheckedStateEffects = m_CheckedStateEffectsHolder.GetComponents<IEffecter>();
                pInteractable.UpdateEffecterAndReDoStateTransition(mCheckedStateEffects);
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            UpdateGraphics();
            UpdateEffecters();
        }
#endif
    }
}
