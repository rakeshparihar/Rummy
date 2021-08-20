using UnityEngine;

namespace FrolicRummy.TweenManagment
{
    public struct Scale : ITween
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

        public Scale(GameObject tweenObject, Vector3 from, Vector3 to) : this()
        {
            mTweenObject = tweenObject;
            mFrom = from;
            mTo = to;
        }

        void ITween.DoAnim(float val)
        {
            mTweenObject.transform.SetLocalScale(CustomLerp.Action(mFrom, mTo, val));
        }
    }

    /*public class Scale2D : ITweener
    {
        private GameObject mTweenObject;
        protected Vector2 mFrom;
        protected Vector2 mTo;
        public Scale2D(GameObject tweenObject, Vector2 from, Vector2 to)
        {
            mTweenObject = tweenObject;
            mFrom = from;
            mTo = to;
        }

        void ITweener.DoAnim(float val)
        {
            mTweenObject.transform.SetLocalScale(CustomLerp.Action(mFrom, mTo, val));
        }

        void ITweener.DoAnimReverse(float val)
        {
            mTweenObject.transform.SetLocalScale(CustomLerp.Action(mTo, mFrom, val));
        }
    }*/

}
