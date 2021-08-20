using UnityEngine.EventSystems;

namespace FrolicRummy.UIManagement
{
    interface IUIInitialized
    {
        void OnInitialize();
    }

    interface IWidgetClickHandler
    {
        void OnClick(Widget widget, PointerEventData eventData);

    }

    interface IWidgetPressHandler
    {
        void OnPress(Widget widget, bool isPressed, PointerEventData eventData);
    }

    interface IWidgetPressRepeatEventHandler
    {
        void OnPressRepeat(Widget widget, PointerEventData eventData);
    }

    interface IWidgetHoverHandler
    {
        void OnHover(Widget widget, bool isHovering, PointerEventData eventData);
    }

    interface IWidgetBeginDragHandler
    {
        void OnBeginDrag(Widget widget, PointerEventData eventData);
    }

    interface IWidgetDragHandler
    {
        void OnDrag(Widget widget, PointerEventData eventData);
    }

    interface IWidgetDragEndHandler
    {
        void OnDragEnd(Widget widget, PointerEventData eventData);
    }

    interface IWidgetDropHandler
    {
        void OnDrop(Widget dropWidget, Widget dragWidget, PointerEventData eventData);
    }

    public class UIEvents
    {
        public event System.Action<Widget, PointerEventData> OnClick;
        public event System.Action<Widget, bool, PointerEventData> OnPress;
        public event System.Action<Widget, PointerEventData> OnPressRepeated;
        public event System.Action<Widget, bool, PointerEventData> OnHover;
        public event System.Action<Widget, PointerEventData> OnDragStart;
        public event System.Action<Widget, PointerEventData> OnDrag;
        public event System.Action<Widget, PointerEventData> OnDragEnd;
        public event System.Action<Widget, Widget, PointerEventData> OnDrop;


        public void TriggerOnClick(Widget widget, PointerEventData eventData)
        {
            OnClick?.Invoke(widget, eventData);
        }

        public void TriggerOnPress(Widget widget, bool isPressed, PointerEventData eventData)
        {
            OnPress?.Invoke(widget, isPressed, eventData);
        }

        public void TriggerOnPressRepeated(Widget widget, PointerEventData eventData)
        {
            OnPressRepeated?.Invoke(widget, eventData);
        }

        public void TriggerOnHover(Widget widget, bool isHovering, PointerEventData eventData)
        {
            OnHover?.Invoke(widget, isHovering, eventData);
        }

        public void TriggerOnDragStart(Widget widget, PointerEventData eventData)
        {
            OnDragStart?.Invoke(widget, eventData);
        }

        public void TriggerOnDrag(Widget widget, PointerEventData eventData)
        {
            OnDrag?.Invoke(widget, eventData);
        }

        public void TriggerOnDragEnd(Widget widget, PointerEventData eventData)
        {
            OnDragEnd?.Invoke(widget, eventData);
        }

        public void TriggerOnDrop(Widget dropWidget, Widget dragWidget, PointerEventData eventData)
        {
            OnDrop?.Invoke(dropWidget, dragWidget, eventData);
        }
    }
}