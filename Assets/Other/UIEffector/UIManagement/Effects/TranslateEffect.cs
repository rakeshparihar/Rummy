using UnityEngine;
using FrolicRummy.TweenManagment;
using UnityEngine.UI;

namespace FrolicRummy.UIManagement
{
    public class TranslateEffect : MonoBehaviour, IEffecter
    {
        [System.Serializable]
        public class Data
        {
            [System.Serializable]
            class RectTransformPositionMap
            {
                [SerializeField] private RectTransform m_TargetRectTransform;
                [SerializeField] private Vector3 m_PositionOffset = Vector3.zero;

                public RectTransform pTargetRectTransform { get => m_TargetRectTransform; set => m_TargetRectTransform = value; }
                public Vector3 pPositionOffset { get => m_PositionOffset; set => m_PositionOffset = value; }
            }

            [EnumFlagsAttribute]
            [SerializeField] private Interactable.InteractionState m_EffectState;
            [SerializeField] RectTransformPositionMap[] m_RectTransformsPosition;

            [SerializeField] private float m_Time = 0.1f;
            [SerializeField] private EaseType m_EaseType = EaseType.Linear;

            private TweenConfig mTweenConfig = new TweenConfig();
            public Interactable.InteractionState pEffectState { get => m_EffectState; set => m_EffectState = value; }

            public void Play(bool instant, bool resetEarlier)
            {
                mTweenConfig.pDurationOrSpeed = !instant ? m_Time : 0;
                mTweenConfig.pEaseType = m_EaseType;
                mTweenConfig.pIsTimeBased = true;
                for (int i = 0; i < m_RectTransformsPosition.Length; ++i)
                {
                    if (resetEarlier)
                        Tween.Stop(m_RectTransformsPosition[i].pTargetRectTransform.gameObject);
                    Tween.MoveLocal(m_RectTransformsPosition[i].pTargetRectTransform.gameObject, m_RectTransformsPosition[i].pPositionOffset, ref mTweenConfig); //Need to revisit
                }
            }
        }

        [SerializeField] private Data[] m_EffectData;

        void IEffecter.Play(Interactable.InteractionState interactionState, bool instant, bool resetEarlier)
        {
            for (int i = 0; i < m_EffectData.Length; ++i)
            {
                if (Utilities.IsBitSet((byte)m_EffectData[i].pEffectState, (int)interactionState))
                    m_EffectData[i].Play(instant, resetEarlier);
            }
        }
    }
}
