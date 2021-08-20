using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrolicRummy.UIManagement
{
	public class Widget : UIBaseBehaviour
	{
        [SerializeField] protected Graphic m_Icon;
        [SerializeField] protected Text m_Text;

        public Sprite pImage
        {
            get
            {
                if (m_Icon != null)
                    return (m_Icon as Image).sprite;
                else
                {
                    Debug.LogError("Background is not set. Default value returned");
                    return null;
                }
            }
            set
            {
                if (m_Icon != null)
                    (m_Icon as Image).sprite = value;
                else
                    Debug.LogError("Background is not set");
            }
        }

        public Texture pRawImage
        {
            get
            {
                if (m_Icon != null)
                    return (m_Icon as RawImage).texture;
                else
                {
                    Debug.LogError("Background is not set. Default value returned");
                    return null;
                }
            }
            set
            {
                if (m_Icon != null)
                    (m_Icon as RawImage).texture = value;
                else
                    Debug.LogError("Background is not set");
            }
        }

        public virtual string pText
        {
            get
            {
                if (m_Text != null)
                    return m_Text.text;
                else
                {
                    Debug.LogError("Text is not set. Default value returned");
                    return string.Empty;
                }
            }
            set
            {
                if (m_Text != null)
                    m_Text.text = value;
                else
                    Debug.LogError("Text is not set");
            }
        }

        public Color pTextColor
        {
            get
            {
                if (m_Text != null)
                    return m_Text.color;
                else
                {
                    Debug.LogError("Text is not set. Default value returned");
                    return Color.white;
                }
            }
            set
            {
                if (m_Text != null)
                    m_Text.color = value;
                else
                    Debug.LogError("Text is not set");
            }
        }

        public UI pParentUI { get; private set; }
        public RectTransform pParentGroup { get; private set; }
        public Widget pParentWidget { get; private set; }
        public UIEvents pEventTarget { get => pParentUI.pEventReceiver; }

        public List<Widget> pChildWidgets { get; private set; }

		private bool mInitialized;
        private bool mIsVisible;

        public void AddChild(Widget widget)
        {
            if (widget.pParentWidget == null && widget.pParentUI != null)
                widget.pParentUI.RemoveChildWidget(widget, false, false);
            else if (widget.pParentWidget != null)
                widget.pParentWidget.RemoveChild(widget, false, false);

            pChildWidgets.Add(widget);
            widget.transform.SetParent(pRectTransform, false);
            widget.Initialize(pParentUI, this);
        }

        public void RemoveChild(Widget widget, bool destroy = false, bool removeReferences = true)
        {
            if (widget.pParentWidget == this)
            {
                pChildWidgets.Remove(widget);
                widget.pParentWidget = null;

                if (destroy)
                    Destroy(widget.gameObject);
                else if (removeReferences)
                {
                    widget.transform.SetParent(null);
                    widget.Initialize(null, null);
                }
            }
        }

        public Widget Duplicate(string name = "")
        {
            GameObject widgetObj = Instantiate(gameObject);
            if (!string.IsNullOrEmpty(name))
                widgetObj.name = name;
            Widget widget = widgetObj.GetComponent<Widget>();
            return widget;
        }

        public virtual void Initialize(UI parentUI, Widget parentWidget)
		{
			if (!mInitialized)
			{
				mInitialized = true;
                pChildWidgets = new List<Widget>();
				CacheWidgets(pRectTransform);
			}

			pParentUI = parentUI;
			pParentWidget = parentWidget;

			foreach (Widget childWidget in pChildWidgets)
				childWidget.Initialize(pParentUI, this);
		}

        protected void CacheWidgets(Transform cacheTransform)
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
                    if (childTransform.childCount > 0)
                        CacheWidgets(childTransform);
                }
            }
        }

        public sealed override bool pVisible
        {
            get { return base.pVisible; }
            set
            {
                if (gameObject.activeSelf != value)
                {
                    gameObject.SetActive(value);
                    m_Visible = value;
                    // OnVisibilityChanged(value);
                }

            }
        }

        public override bool pVisibleInHierarchy
        {
            get
            {
                return m_Visible && (pParentWidget!= null ? pParentWidget.pVisibleInHierarchy : m_Visible) && pParentUI.pVisibleInHierarchy;
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (mInitialized)
                pVisible = m_Visible;
        }
#endif

        public void AddGroup(RectTransform group)
        {
            pParentGroup = group;
            Debug.Log("Group name   " + group.name);
            transform.SetParent(group);
        }
    }
}
