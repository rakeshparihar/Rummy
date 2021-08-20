using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrolicRummy.UIManagement
{
	public class UI : UIBaseBehaviour
	{
		private bool mInitialized = false;
		public UIEvents pEventReceiver { get; private set; }

		public UI pParentUI { get; private set; }
		public List<UI> pChildUIs { get; private set; }
		public List<Widget> pChildWidgets { get; private set; }

		private Canvas mCanvas;
		private CanvasScaler mCanvasScaler;
		private GraphicRaycaster mGetGraphicRaycaster;

		public Canvas pCanvas
		{
			get
			{
				if (mCanvas == null)
					mCanvas = GetComponent<Canvas>();
				return mCanvas;
			}
		}

		public CanvasScaler pCanvasScaler
		{
			get
			{
				if (mCanvasScaler == null)
					mCanvasScaler = GetComponent<CanvasScaler>();
				return mCanvasScaler;
			}
		}

		public GraphicRaycaster pGetGraphicRaycaster
		{
			get
			{
				if (mGetGraphicRaycaster == null)
					mGetGraphicRaycaster = GetComponent<GraphicRaycaster>();
				return mGetGraphicRaycaster;
			}
		}

		public float pScaleFactor
		{
			get { return pCanvas.scaleFactor; }
			set { pCanvas.scaleFactor = value; }
		}

		protected override void Awake()
		{
			base.Awake();
			if (transform.parent == null || transform.parent.GetComponentInParent<UI>() == null)
				Initialize(null);
		}

		public void Initialize(UI parentUI)
		{
			if (!mInitialized)
			{
				mInitialized = true;
				pParentUI = parentUI;
                pChildUIs = new List<UI>();
                pChildWidgets = new List<Widget>();
                pEventReceiver = new UIEvents();
				RegisterEvents();
				CacheWidgets(pRectTransform);

				foreach (Widget childWidget in pChildWidgets)
					childWidget.Initialize(this, null);

				foreach (UI childUI in pChildUIs)
					childUI.Initialize(this);

				if (this is IUIInitialized)
					(this as IUIInitialized).OnInitialize();
			}
		}

		private void RegisterEvents()
		{
			if (this is IWidgetClickHandler)
				pEventReceiver.OnClick += (this as IWidgetClickHandler).OnClick;
			if (this is IWidgetPressHandler)
				pEventReceiver.OnPress += (this as IWidgetPressHandler).OnPress;
			if (this is IWidgetPressRepeatEventHandler)
				pEventReceiver.OnPressRepeated += (this as IWidgetPressRepeatEventHandler).OnPressRepeat;
			if (this is IWidgetHoverHandler)
				pEventReceiver.OnHover += (this as IWidgetHoverHandler).OnHover;
            if (this is IWidgetBeginDragHandler)
                pEventReceiver.OnDragStart += (this as IWidgetBeginDragHandler).OnBeginDrag;
            if (this is IWidgetDragHandler)
                pEventReceiver.OnDrag += (this as IWidgetDragHandler).OnDrag;
            if (this is IWidgetDragEndHandler)
                pEventReceiver.OnDragEnd += (this as IWidgetDragEndHandler).OnDragEnd;
            if (this is IWidgetDropHandler)
                pEventReceiver.OnDrop += (this as IWidgetDropHandler).OnDrop;
        }

		private void CacheWidgets(Transform cacheTransform)
		{
			int childCount = cacheTransform.childCount;
			for (int i = 0; i < childCount; ++i)
			{
				Transform childTransform = cacheTransform.GetChild(i);

				Widget childWidget = childTransform.GetComponent<Widget>();
				if (childWidget != null)
					pChildWidgets.Add(childWidget);
				else
				{
					UI childUI = childTransform.GetComponent<UI>();
					if (childUI != null)
						pChildUIs.Add(childUI);
					else if(childTransform.GetComponent<HandCardUI>() == null)
					{
						if (childTransform.childCount > 0)
							CacheWidgets(childTransform);
					}
				}
			}
		}

        public sealed override bool pVisible
        {
            get { return base.pVisible; }
            set
            {
                if (pCanvas.enabled != value)
                {
                    m_Visible = pCanvas.enabled = value;
                    OnVisibilityChanged(value);
                }
            }
        }

        public virtual void RemoveChildWidget(Widget widget, bool destroy = false, bool removeReferences = true)
        {
            if (widget.pParentWidget == null && widget.pParentUI == this)
            {
                pChildWidgets.Remove(widget);

                if (destroy)
                {
                    widget.transform.SetParent(null);
                    Destroy(widget.gameObject);
                }
                else if (removeReferences)
                {
                    widget.transform.SetParent(null);
                    widget.Initialize(null, null);
                }
            }
        }

        public override bool pVisibleInHierarchy
        {
            get
            {
                if (pParentUI == null)
                    return pVisible;
                return m_Visible && pParentUI.pVisibleInHierarchy;
            }
        }

        protected virtual void OnVisibilityChanged(bool visible) { }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            pVisible = m_Visible;
        }
#endif
    }
}
