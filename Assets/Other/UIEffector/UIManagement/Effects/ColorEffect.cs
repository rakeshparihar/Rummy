using UnityEngine;
using FrolicRummy.TweenManagment;
using UnityEngine.UI;

namespace FrolicRummy.UIManagement
{
    public class ColorEffect : MonoBehaviour, IEffecter
    {
        [System.Serializable]
        public class Data
        {
            [System.Serializable]
            class GraphicColorMap
            {
                [SerializeField] private Graphic m_TargetGraphic;
                [SerializeField] private Color m_Color = Color.grey;

                public Graphic pTargetGraphic { get => m_TargetGraphic; set => m_TargetGraphic = value; }
                public Color pColor { get => m_Color; set => m_Color = value; }
            }

            [EnumFlagsAttribute]
            [SerializeField] private Interactable.InteractionState m_EffectState;
            [SerializeField] GraphicColorMap[] m_GraphicColor;

            [SerializeField] private float m_Time = 0.1f;
            [SerializeField] private EaseType m_EaseType = EaseType.Linear;

            private TweenConfig mTweenConfig = new TweenConfig();
            public Interactable.InteractionState pEffectState { get => m_EffectState; set => m_EffectState = value; }

            public void Play(bool instant, bool resetEarlier)
            {
                mTweenConfig.pDurationOrSpeed = !instant ? m_Time : 0;
                mTweenConfig.pEaseType = m_EaseType;
                mTweenConfig.pIsTimeBased = true;
                for (int i = 0; i < m_GraphicColor.Length; ++i)
                {
                    if (resetEarlier)
                        Tween.Stop(m_GraphicColor[i].pTargetGraphic.gameObject);
                    Tween.Color(m_GraphicColor[i].pTargetGraphic, m_GraphicColor[i].pColor, ref mTweenConfig);
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
