using System.Collections.Generic;
using FrolicRummy;
using FrolicRummy.Utility;
using UnityEngine;

namespace FrolicRummy.Game
{

    public class HandCardSpot
    {
        public List<Card> Objects { get; protected set; } = new List<Card>();
        public bool HasCards => Objects.Count > 0;

        [Tooltip("The factor by which cards will be scaled when added to the spot. When removed, the scaling is undone")]
        public float CardScale = 1.0f;
        [SerializeField] private UICards m_UICards; //UICard elemenet [SerializeField] private UICards m_UICards; //UICard elemenet

        protected void InitValues()
        {
            zIncrement = 0.01f;
        }

        public void AddCard(Card card)
        {
            if (Objects.Contains(card))
                throw new RummyException("CardSpot " + gameObject.name + " already contains " + card);
            AddCard(card, Objects.Count);
            m_UICards.CreateCards(card.CardName, card);
        }

        protected void AddCard(Card card, int index)
        {
            Objects.Insert(index, card);
            card.transform.SetParent(transform, true);
            card.transform.localScale = card.transform.localScale * CardScale;
            //UpdatePositions();
        }

        public void RemoveCard(Card card)
        {
            Objects.Remove(card);
            card.transform.localScale = card.transform.localScale / CardScale;
            card.transform.SetParent(null, true);
           // UpdatePositions();
        }

        public virtual List<Card> ResetSpot()
        {
            var cards = new List<Card>(Objects);
            while (Objects.Count > 0)
                RemoveCard(Objects[0]);
            return cards;
        }
    }

}