using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace FrolicRummy.TweenManagment
{
	public struct Tween
	{
        #region Move

        public static void Move(GameObject tweenObject, Vector3 to, ref TweenConfig tweenConfig)
        {
            tweenConfig.pDelta = GetDeltaMove(tweenObject.transform.position, to, tweenConfig.pDurationOrSpeed, tweenConfig.pIsTimeBased);
            ITween iTween = new Move(tweenObject, tweenObject.transform.position, to);
            Tweener.Add(iTween, ref tweenConfig);
        }

        public static void MoveLocal(GameObject tweenObject, Vector3 to, ref TweenConfig tweenConfig)
        {
            tweenConfig.pDelta = GetDeltaMove(tweenObject.transform.localPosition, to, tweenConfig.pDurationOrSpeed, tweenConfig.pIsTimeBased);
            ITween iTween = new MoveLocal(tweenObject, tweenObject.transform.localPosition, to);
            Tweener.Add(iTween, ref tweenConfig);
        }

        public static void Color(Renderer renderer, Color to, ref TweenConfig tweenConfig)
        {
            Color color = renderer.material.color;
            tweenConfig.pDelta = GetDeltaMove(color, to, tweenConfig.pDurationOrSpeed, tweenConfig.pIsTimeBased);
            ITween iTween = new MaterialColor(renderer.gameObject, renderer, color, to);
            Tweener.Add(iTween, ref tweenConfig);
        }

        public static void Color(Graphic graphic, Color to, ref TweenConfig tweenConfig)
        {
            Color color = graphic.color;
            tweenConfig.pDelta = GetDeltaMove(color, to, tweenConfig.pDurationOrSpeed, tweenConfig.pIsTimeBased);
            ITween iTween = new GraphicColor(graphic.gameObject, graphic, color, to);
            Tweener.Add(iTween, ref tweenConfig);
        }

        #endregion
        public static void Scale(GameObject tweenObject, Vector3 to, ref TweenConfig tweenConfig)
        {
            tweenConfig.pDelta = GetDeltaMove(tweenObject.transform.localScale, to, tweenConfig.pDurationOrSpeed, tweenConfig.pIsTimeBased);
            ITween iTween = new Scale(tweenObject, tweenObject.transform.localScale, to);
            Tweener.Add(iTween, ref tweenConfig);
        }
       
        #region Helper

        public static CustomCurve GetCustomCurve(EaseType easeType)
		{
			switch (easeType)
			{
			case EaseType.Linear: return EaseCurve.Linear;
			case EaseType.Spring: return EaseCurve.Spring;
			case EaseType.EaseInQuad: return EaseCurve.EaseInQuad;
			case EaseType.EaseOutQuad: return EaseCurve.EaseOutQuad;
			case EaseType.EaseInOutQuad: return EaseCurve.EaseInOutQuad;
			case EaseType.EaseInCubic: return EaseCurve.EaseInCubic;
			case EaseType.EaseOutCubic: return EaseCurve.EaseOutCubic;
			case EaseType.EaseInOutCubic: return EaseCurve.EaseInOutCubic;
			case EaseType.EaseInQuart: return EaseCurve.EaseInQuart;
			case EaseType.EaseOutQuart: return EaseCurve.EaseOutQuart;
			case EaseType.EaseInOutQuart: return EaseCurve.EaseInOutQuart;
			case EaseType.EaseInQuint: return EaseCurve.EaseInQuint;
			case EaseType.EaseOutQuint: return EaseCurve.EaseOutQuint;
			case EaseType.EaseInOutQuint: return EaseCurve.EaseInOutQuint;
			case EaseType.EaseInSine: return EaseCurve.EaseInSine;
			case EaseType.EaseOutSine: return EaseCurve.EaseOutSine;
			case EaseType.EaseInOutSine: return EaseCurve.EaseInOutSine;
			case EaseType.EaseInExpo: return EaseCurve.EaseInExpo;
			case EaseType.EaseOutExpo: return EaseCurve.EaseOutExpo;
			case EaseType.EaseInOutExpo: return EaseCurve.EaseInOutExpo;
			case EaseType.EaseInCirc: return EaseCurve.EaseInCirc;
			case EaseType.EaseOutCirc: return EaseCurve.EaseOutCirc;
			case EaseType.EaseInOutCirc: return EaseCurve.EaseInOutCirc;
			case EaseType.EaseInBounce: return EaseCurve.EaseInBounce;
			case EaseType.EaseOutBounce: return EaseCurve.EaseOutBounce;
			case EaseType.EaseInOutBounce: return EaseCurve.EaseInOutBounce;
			case EaseType.EaseInBack: return EaseCurve.EaseInBack;
			case EaseType.EaseOutBack: return EaseCurve.EaseOutBack;
			case EaseType.EaseInOutBack: return EaseCurve.EaseInOutBack;
			case EaseType.EaseInElastic: return EaseCurve.EaseInElastic;
			case EaseType.EaseOutElastic: return EaseCurve.EaseOutElastic;
			case EaseType.EaseInOutElastic: return EaseCurve.EaseInOutElastic;
			default: return EaseCurve.Linear;

			}
		}

		private static float GetDeltaMove(Vector2 from, Vector2 to, float factor ,bool isTimeDepndent)
		{
			return factor = (isTimeDepndent ? 1 / factor : factor / Vector2.Distance(from, to));
		}

		private static float GetDeltaMove(Vector3 from, Vector3 to, float factor ,bool isTimeDepndent)
		{
			return factor = (isTimeDepndent ? 1 / factor : factor / Vector3.Distance(from, to));
		}

		private static float GetDeltaMove(Vector4 from, Vector4 to, float factor ,bool isTimeDepndent)
		{
			return factor = (isTimeDepndent ? 1 / factor : factor / Vector4.Distance(from, to));
		}

		private static float GetDeltaMove(float from, float to, float factor ,bool isTimeDepndent)
		{
			return factor = (isTimeDepndent ? 1 / factor : factor / Mathf.Abs(from - to));
		}

	#endregion

		public static void Pause(GameObject tweenObject, bool state)
		{
            Tweener.pInstance.Pause(tweenObject.GetInstanceID(), state);
			
		}

		public static bool IsRunning(GameObject tweenObject)
		{
			return false;
		}

		public static void Stop(GameObject tweenObject)
		{
           Tweener.pInstance.Stop(tweenObject.GetInstanceID());
        }

		public static void PauseAll(bool state)
		{
			Tweener.pInstance.enabled = !state;
		}
	}
}
