using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FrolicRummy.UIManagement
{
	interface IStateObserver
	{

	}

	interface IStateObservable
	{
        void AddStateChangeObserver(IStateObserver observer);
        void RemoveStateChangeObserver();
    }

	public class UIBaseBehaviour : UIBehaviour
	{
		[SerializeField] protected bool m_Visible = true;
        private RectTransform mRectTransform;
		

		public RectTransform pRectTransform
		{
			get
			{
				if (mRectTransform == null)
					mRectTransform = GetComponent<RectTransform>();
				return mRectTransform;
			}
		}

		public Vector3 pPosition
		{
			get { return pRectTransform.position; }
			set { pRectTransform.position = value; }
		}

		public Vector3 pLocalPosition
		{
			get { return pRectTransform.localPosition; }
			set { pRectTransform.localPosition = value; }
		}

		public Vector3 pAnchoredPosition
		{
			get { return pRectTransform.anchoredPosition; }
			set { pRectTransform.anchoredPosition = value; }
		}

		public Vector3 pLocalScale
		{
			get { return pRectTransform.localScale; }
			set { pRectTransform.localScale = value; }
		}

		public Vector2 pSize
		{
			get { return pRectTransform.sizeDelta; }
			set { pRectTransform.sizeDelta = value; }
		}

		public virtual bool pVisible
		{
			get { return m_Visible; }
			set { m_Visible = value; }
		}

		public virtual bool pVisibleInHierarchy { get; }

		protected virtual void UpdateVisibleInHierarchy()
		{

		}
	}
}
