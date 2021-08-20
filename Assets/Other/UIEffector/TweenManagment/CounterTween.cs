
using UnityEngine;
using UnityEngine.UI;

namespace FrolicRummy.TweenManagment
{
   /* public struct Count : ITweener
    {
        private GameObject mTweenObject;
        private Text mUIText;
        private TextMesh mTextMesh;
        private float mFrom;
        private float mTo;

        public Count(GameObject tweenObject, Text uiText, float from, float to)
        {
            mUIText = uiText;
            mTweenObject = tweenObject;
            mTextMesh = null;
            mFrom = from;
            mTo = to;
        }

        public Count(GameObject tweenObject, TextMesh textMesh, float from, float to)
        {
            mTextMesh = textMesh;
            mTweenObject = tweenObject;
            mUIText = null;
            mFrom = from;
            mTo = to;
        }

        void ITweener.DoAnim(float val)
        {
            int textValue = (int)CustomLerp.Action(mFrom, mTo, val);
            if (mUIText != null)
                mUIText.SetText(textValue.ToString());
            else if (mTextMesh != null)
                mTextMesh.SetText(textValue.ToString());
        }

        void ITweener.DoAnimReverse(float val)
        {
            int textValue = (int)CustomLerp.Action(mFrom, mTo, val);
            if (mUIText != null)
                mUIText.SetText(textValue.ToString());
            else if (mTextMesh != null)
                mTextMesh.SetText(textValue.ToString());
        }
    }

    public struct Timer : ITweener
    {
        private GameObject mTweenObject;
        private float mFrom;
        private float mTo;
        private Timer()
         {
             GameObject timerObj = new GameObject("Timer");
             mTweenObject = timerObj;
         }

        void ITweener.DoAnim(float val)
        {
            if (val >= 1)
                GameObject.Destroy(mTweenObject);
        }

        void ITweener.DoAnimReverse(float val)
        {
           
        }
    }*/
}