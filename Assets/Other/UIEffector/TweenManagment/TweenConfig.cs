using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FrolicRummy.TweenManagment
{
    [System.Serializable]
    public struct TweenConfig
    {
        [SerializeField] private bool m_IsTimeBased;
        [SerializeField] private float m_DurationOrSpeed;
        [SerializeField] private float m_Delay;
        [SerializeField] private int m_LoopCount;
        [SerializeField] private bool m_PingPong;
        [SerializeField] private EaseType m_Type;
        [SerializeField] private bool m_UseAnimationCurve;
        [SerializeField] private AnimationCurve m_AnimationCurve;

        public UnityEvent OnTweeningComplete;
        private CustomCurve mCustomCurve;
        public OnAnimationCompleteCallback pOnTweenCompleteCallback { get; set; }
        public CustomCurve pCustomAnimationCurve
        {
            get
            {
                if (mCustomCurve == null)
                    mCustomCurve = Tween.GetCustomCurve(m_Type);
                return mCustomCurve;
            }
        }

        public float pDurationOrSpeed { get => m_DurationOrSpeed; set => m_DurationOrSpeed = value; }
        public float pDelay { get => m_Delay; set => m_Delay = value; }
        public int pLoopCount { get => m_LoopCount == 0 ? 1 : m_LoopCount; set => m_LoopCount = (value == 0) ? 1 : value; }
        public bool pPingPong { get => m_PingPong; set => m_PingPong = value; }
        public EaseType pEaseType { get => m_Type; set => m_Type = value; }
        public bool pUseAnimationCurve { get => m_UseAnimationCurve; set => m_UseAnimationCurve = value; }
        public AnimationCurve pAnimationCurve { get => m_AnimationCurve; set => m_AnimationCurve = value; }
        public bool pIsTimeBased { get => m_IsTimeBased; set => m_IsTimeBased = value; }
        public float pDelta { get; set; }

        public TweenConfig(float durationOrSpeed = 1, bool timeBased = true, float delay = 0, int loopCount = 1, bool pingPong = false, EaseType easeType = EaseType.Linear, bool useAnimationCurve = false, AnimationCurve animationCurve = null, CustomCurve customCurve = null, OnAnimationCompleteCallback tweenCompleteCallback = null)
        {
            m_IsTimeBased = timeBased;
            m_DurationOrSpeed = durationOrSpeed;
            m_Delay = delay;
            m_LoopCount = loopCount;
            m_PingPong = pingPong;
            m_Type = easeType;
            m_UseAnimationCurve = useAnimationCurve;
            m_AnimationCurve = animationCurve;
            OnTweeningComplete = null;
            mCustomCurve = customCurve;
            pOnTweenCompleteCallback = tweenCompleteCallback;
            pDelta = 0;
        }
    }
}