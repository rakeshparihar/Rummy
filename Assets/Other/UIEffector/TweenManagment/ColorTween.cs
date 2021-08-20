using UnityEngine;
using UnityEngine.UI;

namespace FrolicRummy.TweenManagment
{
    public struct MaterialColor : ITween
    {
        private GameObject mTweenObject;
        private Renderer mRenderer;
        private Vector4 mFrom;
        private Vector4 mTo;

        float ITween.pDelayCounter { get; set; }
        bool ITween.pDoInReverese { get; set; }
        short ITween.pCompletedLoopCount { get; set; }
        float ITween.pValue { get; set; }
        TweenState ITween.pState { get; set; }
        int ITween.pId { get => mTweenObject != null ? mTweenObject.GetInstanceID() : -1; }

        public MaterialColor(GameObject tweenObject, Renderer renderer, Vector4 from, Vector4 to) : this()
        {
            mRenderer = renderer;
            mTweenObject = tweenObject;
            mFrom = from;
            mTo = to;
        }

        void ITween.DoAnim(float val)
        {
            mRenderer.SetColor(CustomLerp.Action(mFrom, mTo, val));
        }
    }

    public struct GraphicColor : ITween
    {
        private GameObject mTweenObject;
        private Graphic mGraphic;
        private Vector4 mFrom;
        private Vector4 mTo;

        float ITween.pDelayCounter { get; set; }
        bool ITween.pDoInReverese { get; set; }
        short ITween.pCompletedLoopCount { get; set; }
        float ITween.pValue { get; set; }
        TweenState ITween.pState { get; set; }
        int ITween.pId { get => mTweenObject != null ? mTweenObject.GetInstanceID() : -1; }

        public GraphicColor(GameObject tweenObject, Graphic graphic, Vector4 from, Vector4 to) : this()
        {
            mGraphic = graphic;
            mTweenObject = tweenObject;
            mFrom = from;
            mTo = to;
        }

        void ITween.DoAnim(float val)
        {
            mGraphic.SetColor(CustomLerp.Action(mFrom, mTo, val));
        }
    }

    /*public struct Alpha : ITweener
    {
        private GameObject mTweenObject;
        private Renderer mRenderer;
        private float mFrom;
        private float mTo;

        public Alpha(GameObject tweenObject, Renderer renderer, float from, float to)
        {
            mRenderer = renderer;
            mTweenObject = tweenObject;
            mFrom = from;
            mTo = to;
        }

        void ITweener.DoAnim(float val)
        {
            mRenderer.SetAlpha(CustomLerp.Action(mFrom, mTo, val));
        }

        void ITweener.DoAnimReverse(float val)
        {
            mRenderer.SetAlpha(CustomLerp.Action(mTo, mFrom, val));
        }
    }

    public struct TextMeshColor : ITweener
    {
        private GameObject mTweenObject;
        private TextMesh mTextMesh;
        private Vector4 mFrom;
        private Vector4 mTo;

        public TextMeshColor(GameObject tweenObject, TextMesh textMesh, Vector4 from, Vector4 to)
        {
            mTweenObject = tweenObject;
            mTextMesh = textMesh;
            mFrom = from;
            mTo = to;
        }

        void ITweener.DoAnim(float val)
        {
            mTextMesh.SetColor(CustomLerp.Action(mFrom, mTo, val));
        }

        void ITweener.DoAnimReverse(float val)
        {
            mTextMesh.SetColor(CustomLerp.Action(mTo, mFrom, val));
        }
    }

    public class GraphicColor : ITweener
    {
        private GameObject mTweenObject;
        private Graphic mUIGraphic;
        private Vector4 mFrom;
        private Vector4 mTo;

        public GraphicColor(GameObject tweenObject, Graphic uiGraphic, Vector4 from, Vector4 to) 
        {
            mUIGraphic = uiGraphic;
            mTweenObject = tweenObject;
            mFrom = from;
            mTo = to;
        }

        void ITweener.DoAnim(float val)
        {
            mUIGraphic.SetColor(CustomLerp.Action(mFrom, mTo, val));
        }

        void ITweener.DoAnimReverse(float val)
        {
            mUIGraphic.SetColor(CustomLerp.Action(mTo, mFrom, val));
        }
    }
    */
}