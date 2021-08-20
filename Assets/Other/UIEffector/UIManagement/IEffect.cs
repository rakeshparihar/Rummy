using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrolicRummy.UIManagement
{
    public interface IEffecter
    {
        void Play(Interactable.InteractionState interactionState, bool instant, bool resetEarlier);
    }
}
