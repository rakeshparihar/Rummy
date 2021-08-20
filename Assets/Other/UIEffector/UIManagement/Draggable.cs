using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FrolicRummy.UIManagement
{
    public class Draggable : UIBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private bool mIsCachedInteractable = false;

        private Interactable mInteractable = null;
        private Interactable pInteractable
        {
            get
            {
                if (!mIsCachedInteractable)
                {
                    mIsCachedInteractable = true;
                    mInteractable = GetComponent<Interactable>();
                }
                return mInteractable;
            }
        }


        private Widget mWidget;
        private Widget pWidget
        {
            get
            {
                if (mWidget == null)
                    mWidget = GetComponent<Widget>();
                if (mWidget == null)
                    Debug.Log("Widget Component is missing:  " + gameObject.name);
                return mWidget;
            }
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (!eventData.IsPointerValid())
                return;

            if (pInteractable == null || pInteractable.IsInteractable())
            {
                if (pWidget.pEventTarget != null)
                    pWidget.pEventTarget.TriggerOnDragStart(pWidget, eventData);
            }
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (!eventData.IsPointerValid())
                return;

            if (pInteractable == null || pInteractable.IsInteractable())
            {
                if (pWidget.pEventTarget != null)
                    pWidget.pEventTarget.TriggerOnDrag(pWidget, eventData);
            }
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (!eventData.IsPointerValid())
                return;

            if (pInteractable == null || pInteractable.IsInteractable())
            {
                if (pWidget.pEventTarget != null)
                    pWidget.pEventTarget.TriggerOnDragEnd(pWidget, eventData);
            }
        }
    }
}
