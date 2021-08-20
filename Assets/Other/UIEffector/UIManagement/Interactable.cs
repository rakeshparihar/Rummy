using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FrolicRummy.UIManagement
{
    [RequireComponent(typeof(Graphic))]
    public class Interactable : UIBehaviour, IMoveHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        public enum InteractionState
        {
            Normal,
            Highlighted,
            Pressed,
            Selected,
            Disabled,
        }

        [SerializeField] private bool m_Interactable = true;
       
        private bool mIsHovering;
        private bool mIsPointerDown;
        private bool mHasSelection;
        private Widget mWidget;
        protected IEffecter[] mEffecters; 

        protected Widget pWidget
        {
            get
            {
                if (mWidget == null)
                    mWidget = GetComponentInParent<Widget>();
                if (mWidget == null)
                    Debug.Log("Widget Component is missing:  " + gameObject.name);
                return mWidget;
            }
        }

        public InteractionState pCurrentInteractionState
        {
            get
            {
                if (!IsInteractable())
                    return InteractionState.Disabled;
                if (mIsPointerDown)
                    return InteractionState.Pressed;
                if (mHasSelection)
                    return InteractionState.Selected;
                if (mIsHovering)
                    return InteractionState.Highlighted;
                return InteractionState.Normal;
            }
        }

       
        void ISelectHandler.OnSelect(BaseEventData eventData)
        {
            mHasSelection = true;
            EvaluateAndTransitionToSelectionState();
        }

        void IDeselectHandler.OnDeselect(BaseEventData eventData)
        {
            mHasSelection = false;
            EvaluateAndTransitionToSelectionState();
        }

        void IMoveHandler.OnMove(AxisEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (!eventData.IsPointerValid())
                return;

            mIsPointerDown = true;
            EvaluateAndTransitionToSelectionState();
            if (pWidget.pEventTarget != null)
                pWidget.pEventTarget.TriggerOnPress(pWidget, mIsPointerDown, eventData);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (!eventData.IsPointerValid())
                return;

            mIsPointerDown = false;
            EvaluateAndTransitionToSelectionState();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            mIsHovering = true;
            EvaluateAndTransitionToSelectionState();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            mIsHovering = false;
            EvaluateAndTransitionToSelectionState();
        }

        private void EvaluateAndTransitionToSelectionState()
        {
            if (!IsInteractable())
                return;
            DoStateTransition(pCurrentInteractionState, false);
        }

        protected virtual void DoStateTransition(InteractionState state, bool instant)
        {
            if (!gameObject.activeInHierarchy || pWidget.pParentUI == null)
                return;
            if (mEffecters == null)
                mEffecters = GetComponents<IEffecter>();
            if (mEffecters != null)
            {
                for (int i = 0; i < mEffecters.Length; ++i)
                    mEffecters[i].Play(state, instant, i == 0);
            }
        }

        public void UpdateEffecterAndReDoStateTransition(IEffecter[] effecters)
        {
            mEffecters = effecters;
            DoStateTransition(pCurrentInteractionState, false);
        }

        public virtual bool IsInteractable()
        {
            return m_Interactable;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (Application.isPlaying)
                DoStateTransition(pCurrentInteractionState, false);
        }
#endif
    }
}
