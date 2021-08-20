using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FrolicRummy.UIManagement
{
    public class Droppable : UIBehaviour, IDropHandler
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

        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            if (!eventData.IsPointerValid())
                return;

            if (pInteractable == null || pInteractable.IsInteractable())
            {
                if (pWidget.pEventTarget != null)
                    pWidget.pEventTarget.TriggerOnDrop(pWidget, eventData.pointerDrag.GetComponent<Widget>(), eventData);
            }
        }
    }
}
