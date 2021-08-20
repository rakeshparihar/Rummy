using UnityEngine;
using UnityEngine.UI;
using System;

namespace FrolicRummy.TweenManagment
{
	public delegate float CustomCurve(float start, float end, float val);
	public delegate void OnAnimationCompleteCallback(object o);

	public enum TweenState
	{
		NONE,
		RUNNING,
		PAUSE,
		DONE
	}


    public interface ITween
    {
        TweenState pState { get; set; }
        float pDelayCounter { get; set; }
        bool pDoInReverese { get; set; }
        short pCompletedLoopCount { get; set; }
        float pValue { get; set; }
        int pId { get; }

        void DoAnim(float val);
    }

    public struct CustomLerp
    {
        public static float Action(float start, float end, float value)
        {
            return ((1 - value) * start + value * end);
        }

        public static Vector2 Action(Vector2 start, Vector2 end, float value)
        {
            return new Vector2(Action(start.x, end.x, value), Action(start.y, end.y, value));
        }

        public static Vector3 Action(Vector3 start, Vector3 end, float value)
        {
            return new Vector3(Action(start.x, end.x, value), Action(start.y, end.y, value), Action(start.z, end.z, value));
        }

        public static Vector4 Action(Vector4 start, Vector4 end, float value)
        {
            return new Vector4(Action(start.x, end.x, value), Action(start.y, end.y, value), Action(start.z, end.z, value), Action(start.w, end.w, value));
        }

    }
}

