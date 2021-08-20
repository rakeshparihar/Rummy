using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace FrolicRummy.TweenManagment
{
	internal sealed class Tweener : AutoInstantiateSingleton<Tweener>
	{
        struct TweenArray
        {
            public ITween _Tween;
            public TweenConfig _TweenConfig;
            public TweenArray(ref ITween tween, ref TweenConfig tweenConfig)
            {
                _Tween = tween;
                _TweenConfig = tweenConfig;
            }
        }

        private float mValue = 0;
        private ushort mSize = 0;
        private ushort mCapacity = 4;

        private TweenArray[] mTweens = new TweenArray[4];

        public static void Add(ITween tween, ref TweenConfig tweenConfig)
        {
            pInstance.EnsureCapacity();
            pInstance.mTweens[pInstance.mSize++] = new TweenArray(ref tween, ref tweenConfig);
        }

        private void EnsureCapacity()
        {
            if (mSize >= mCapacity)
            {
                TweenArray[] newItems = new TweenArray[mCapacity *= 2];
                if (mSize > 0)
                    Array.Copy(mTweens, 0, newItems, 0, mSize);
                mTweens = newItems;
            }
        }


        private void Remove(ref ITween tween)
        {
            ushort index = IndexOf(ref mTweens, tween.pId);
            if (index >= 0)
            {
                --mSize;
                if (index < mSize)
                    Array.Copy(mTweens, index + 1, mTweens, index, mSize - index);
                mTweens[mSize] = default(TweenArray);
            }
        }

        private ushort IndexOf(ref TweenArray[] array, int tweenId)
        {
            for (ushort i = 0; i < ushort.MaxValue; i++)
            {
                if (tweenId == array[i]._Tween.pId)
                    return i;
            }
            return ushort.MaxValue;
        }


        float deltaTime = 0;
        private void Update()
        {
            deltaTime = Time.deltaTime;
            for (int i = 0; i < mSize; ++i)
                DoUpdate(ref mTweens[i]._Tween, ref mTweens[i]._TweenConfig);
        }
       
        private void DoUpdate(ref ITween tween, ref TweenConfig param)
        {
            if (tween.pState == TweenState.PAUSE || tween.pState == TweenState.DONE)
                return;
            if (tween.pId == -1 || tween.pState == TweenState.DONE)
            {
                Remove(ref tween);
                return;
            }

            if (tween.pDelayCounter < param.pDelay)
            {
                tween.pDelayCounter += deltaTime;
                return;
            }

            tween.pValue = Mathf.MoveTowards(tween.pValue, 1, deltaTime * param.pDelta);

            if (param.pUseAnimationCurve)
                mValue = param.pAnimationCurve.Evaluate(tween.pValue);
            else
                mValue = param.pCustomAnimationCurve(0, 1, tween.pValue);

            tween.DoAnim(!tween.pDoInReverese ? mValue : 1 - mValue);

            if (tween.pValue >= 1)
            {
                tween.pValue = 0;
                tween.pDoInReverese = param.pPingPong ? !tween.pDoInReverese : tween.pDoInReverese;
                ++tween.pCompletedLoopCount;

                if (!(tween.pCompletedLoopCount != (param.pPingPong ? param.pLoopCount != 1 ? 2 * param.pLoopCount : 2 : param.pLoopCount)))
                {
                    tween.pState = TweenState.DONE;
                    param.pOnTweenCompleteCallback?.Invoke(null);
                    param.OnTweeningComplete?.Invoke();
                    Remove(ref tween);
                }
            }
        }

        //TODO:
        internal void Pause(int tweenID, bool state)
        {
            ushort index = IndexOf(ref mTweens, tweenID);
            if (index >= 0)
                mTweens[index]._Tween.pState = state ? TweenState.PAUSE : TweenState.RUNNING;
        }

        internal void Stop(int tweenID)
        {
            // need to revisit it
            for (int i = 0; i < mTweens.Length; ++i)
            {
                if (mTweens[i]._Tween != null && mTweens[i]._Tween.pId == tweenID)
                    mTweens[i]._Tween.pState = TweenState.DONE;
            }
        }
    }
}
