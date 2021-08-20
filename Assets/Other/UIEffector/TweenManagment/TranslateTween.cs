
using UnityEngine;

namespace FrolicRummy.TweenManagment
{
    public struct Move : ITween
    {
        private GameObject mTweenObject;
        private Vector3 mFrom;
        private Vector3 mTo;

        float ITween.pDelayCounter { get; set;}
        bool ITween.pDoInReverese { get; set; }
        short ITween.pCompletedLoopCount { get; set; }
        float ITween.pValue { get; set; }
        TweenState ITween.pState { get; set; }
        int ITween.pId { get => mTweenObject != null ? mTweenObject.GetInstanceID() : -1; }

        public Move(GameObject tweenObject, Vector3 from, Vector3 to) : this()
        {
            mTweenObject = tweenObject;
            mFrom = from;
            mTo = to;
        }

        void ITween.DoAnim(float val)
        {
            mTweenObject.transform.SetPosition(CustomLerp.Action(mFrom, mTo, val));
        }
    }

    public struct MoveLocal : ITween
    {
        private GameObject mTweenObject;
        private Vector3 mFrom;
        private Vector3 mTo;

        float ITween.pDelayCounter { get; set; }
        bool ITween.pDoInReverese { get; set; }
        short ITween.pCompletedLoopCount { get; set; }
        float ITween.pValue { get; set; }
        TweenState ITween.pState { get; set; }
        int ITween.pId { get => mTweenObject != null ? mTweenObject.GetInstanceID() : -1; }

        public MoveLocal(GameObject tweenObject, Vector3 from, Vector3 to) : this()
        {
            mTweenObject = tweenObject;
            mFrom = from;
            mTo = to;
        }

        void ITween.DoAnim(float val)
        {
            mTweenObject.transform.SetLocalPosition(CustomLerp.Action(mFrom, mTo, val));
        }
    }

    /*public class MoveLocal : ITweener
    {
        private GameObject mTweenObject;
        private Vector3 mFrom;
        private Vector3 mTo;

        public MoveLocal(GameObject tweenObject, Vector3 from, Vector3 to)
        {
            mTweenObject = tweenObject;
            mFrom = from;
            mTo = to;
        }

        void ITweener.DoAnim(float val)
        {
            mTweenObject.transform.SetLocalPosition(CustomLerp.Action(mFrom, mTo, val));
        }

        void ITweener.DoAnimReverse(float val)
        {
            mTweenObject.transform.SetLocalPosition(CustomLerp.Action(mTo, mFrom, val));
        }
    }

    public struct Move2D : ITweener
    {
        private GameObject mTweenObject;
        private RectTransform mRectTransform;
        private Vector2 mFrom;
        private Vector2 mTo;

        public Move2D(GameObject tweenObject, Vector2 from, Vector2 to)
        {
            mTweenObject = tweenObject;
            mRectTransform = null;
            mFrom = from;
            mTo = to;
        }

        public Move2D(RectTransform rectTransform, Vector2 from, Vector2 to)
        {
            mRectTransform = rectTransform;
            mTweenObject = mRectTransform.gameObject;
            this.mFrom = from;
            this.mTo = to;
        }

        void ITweener.DoAnim(float val)
        {
            if (mRectTransform != null)
            {
                mRectTransform.SetPosition(CustomLerp.Action(mFrom, mTo, val));
                return;
            }

            mTweenObject.transform.SetPosition(CustomLerp.Action(mFrom, mTo, val));
        }

        void ITweener.DoAnimReverse(float val)
        {
            if (mRectTransform != null)
            {
                mRectTransform.SetPosition(CustomLerp.Action(mTo, mFrom, val));
                return;
            }

            mTweenObject.transform.SetPosition(CustomLerp.Action(mTo, mFrom, val));
        }
    }

    public struct MoveLocal2D : ITweener
    {
        private GameObject mTweenObject;
        private Vector2 mFrom;
        private Vector2 mTo;
        public MoveLocal2D(GameObject tweenObject, Vector2 from, Vector2 to)
        {
            mTweenObject = tweenObject;
            mFrom = from;
            mTo = to;
        }

        void ITweener.DoAnim(float val)
        {
            mTweenObject.transform.SetLocalPosition(CustomLerp.Action(mFrom, mTo, val));
        }

        void ITweener.DoAnimReverse(float val)
        {
            mTweenObject.transform.SetLocalPosition(CustomLerp.Action(mTo, mFrom, val));
        }
    }*/
}

