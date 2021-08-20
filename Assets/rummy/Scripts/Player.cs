﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using FrolicRummy.Utility;

namespace FrolicRummy.Game
{

    public class Player : MonoBehaviour
    {
        #region Enum
        public enum PlayerState
        {
            IDLE = 0,
            DRAWING = 1,
            WAITING = 2,
            PLAYING = 3,
            LAYING = 4,
            RETURNING_JOKER = 5,
            DISCARDING = 6
        }

       

        public enum PlayerType
        {
            Player = 0,
            Opponent = 1,
            AI = 2
        }

        private enum LayStage
        {
            SETS = 0,
            RUNS = 1,
            SINGLES = 2
        }

        public PlayerState State { get; private set; } = PlayerState.IDLE;
        public PlayerType Type = PlayerType.Player;
        private LayStage layStage = LayStage.SETS;
        #endregion

        private float waitStartTime;

        #region PlayerCardSpot
        private HandCardSpot HandCardSpot;
        public int HandCardCount { get { return HandCardSpot.Objects.Count; } }

        private CardSpotsNode PlayerCardSpotsNode;
        private CardSpot currentCardSpot;
        public List<CardSpot> GetPlayerCardSpots() => PlayerCardSpotsNode.Objects;
        public int GetLaidCardsSum() => PlayerCardSpotsNode.Objects.Sum(spot => spot.GetValue());
        #endregion

        #region CardSets
        /// <summary>The card sets and runs which are going to be laid down</summary>
        private CardCombo laydownCards = new CardCombo();

        /// <summary>The single cards which are going to be laid down</summary>
        private List<Single> singleLayDownCards = new List<Single>();

        private int currentCardPackIdx = 0, currentCardIdx = 0;
        private Card returningJoker;
        private bool isCardBeingLaidDown, isJokerBeingReturned;


        /// <summary>
        /// Whether the player can lay down card packs and pick up cards from the discard stack
        /// </summary>
        public bool HasLaidDown { get; private set; }
        #endregion

        #region Events
        [HideInInspector]
        public UnityEvent TurnFinished = new UnityEvent();

        public class Event_PossibleCardCombosChanged : UnityEvent<List<CardCombo>> { }
        public Event_PossibleCardCombosChanged PossibleCardCombosChanged = new Event_PossibleCardCombosChanged();

        public class Event_PossibleSinglesChanged : UnityEvent<List<Single>> { }
        public Event_PossibleSinglesChanged PossibleSinglesChanged = new Event_PossibleSinglesChanged();

        public class Event_NewThought : UnityEvent<string> { }
        public Event_NewThought NewThought = new Event_NewThought();
        #endregion

        public List<Card> ResetPlayer()
        {
            HasLaidDown = false;
            State = PlayerState.IDLE;
            PossibleCardCombosChanged.Invoke(new List<CardCombo>());
            PossibleSinglesChanged.Invoke(new List<Single>());
            NewThought.Invoke("<CLEAR>");

            var cards = new List<Card>();
            cards.AddRange(PlayerCardSpotsNode.ResetNode());
            cards.AddRange(HandCardSpot.ResetSpot());
            return cards;
        }

        private void Start()
        {
            HandCardSpot = GetComponentInChildren<HandCardSpot>();
            PlayerCardSpotsNode = GetComponentInChildren<CardSpotsNode>();
        }

        private void Update()
        {
            if(Type == PlayerType.Player)
            {

            }
            else if (Type == PlayerType.Opponent)
            {

            }
            else if (Type == PlayerType.AI)
            {

            }

            if (State == PlayerState.WAITING && Time.time - waitStartTime > Tb.I.GameMaster.PlayWaitDuration)
            {
                State = PlayerState.PLAYING;
                if (!Tb.I.GameMaster.LayingAllowed() || !HasLaidDown)
                {
                    DiscardCard();
                }
                else
                {
                    State = PlayerState.LAYING;
                    isCardBeingLaidDown = false;
                    currentCardPackIdx = 0;
                    currentCardIdx = 0;
                    currentCardSpot = null;
                    layStage = LayStage.SETS;

                    if (laydownCards.Sets.Count == 0)
                    {
                        layStage = LayStage.RUNS;
                        if (laydownCards.Runs.Count == 0)
                            LayingCardsDone();
                    }
                }
            }

            if (State == PlayerState.LAYING)
            {
                if (isCardBeingLaidDown)
                    return;
                isCardBeingLaidDown = true;

                if (layStage == LayStage.SINGLES)
                {
                    currentCardSpot = singleLayDownCards[currentCardIdx].CardSpot;
                }
                else if (currentCardSpot == null)
                {
                    currentCardSpot = PlayerCardSpotsNode.AddCardSpot();
                    currentCardSpot.Type = (layStage == LayStage.RUNS) ? CardSpot.SpotType.RUN : CardSpot.SpotType.SET;
                }

                Card card;
                switch (layStage)
                {
                    case LayStage.SETS:
                        card = laydownCards.Sets[currentCardPackIdx].Cards[currentCardIdx];
                        break;
                    case LayStage.RUNS:
                        card = laydownCards.Runs[currentCardPackIdx].Cards[currentCardIdx];
                        break;
                    default: // LayStage.SINGLES:
                        card = singleLayDownCards[currentCardIdx].Card;
                        break;
                }

                HandCardSpot.RemoveCard(card);
                card.MoveFinished.AddListener(LayDownCardMoveFinished);
                card.MoveCard(currentCardSpot.transform.position, Tb.I.GameMaster.AnimateCardMovement);
            }

            if (State == PlayerState.RETURNING_JOKER)
            {
                if (isJokerBeingReturned)
                    return;
                isJokerBeingReturned = true;
                currentCardSpot.RemoveCard(returningJoker);
                returningJoker.MoveFinished.AddListener(ReturnJokerMoveFinished);
                returningJoker.MoveCard(HandCardSpot.transform.position, Tb.I.GameMaster.AnimateCardMovement);
            }
        }

        public void BeginTurn()
        {
            //NewThought.Invoke("<CLEAR>");
            //DrawCard(false);
        }

        public void DrawCard(bool isServingCard)
        {
            State = PlayerState.DRAWING;
            bool takeFromDiscardStack = false;
            if (!isServingCard && HasLaidDown)
            {
                Debug.Log("isServingCard--------"+ isServingCard);
                // Check if we want to draw from discard stack
                // Note that players will never discard a card which can be added to an already laid-down card pack.
                // Therefore, no need to check for that case here.
                Card discardedCard = Tb.I.DiscardStack.PeekCard();
                var hypotheticalHandCards = new List<Card>(HandCardSpot.Objects) { discardedCard };
                var hypotheticalBestCombo = GetBestCardCombo(hypotheticalHandCards, false);
                int hypotheticalValue = hypotheticalBestCombo.Value;

                int currentValue = GetBestCardCombo(HandCardSpot.Objects, false).Value;

                if (hypotheticalValue > currentValue)
                {
                    NewThought.Invoke("Take " + discardedCard + " from discard pile to finish " + hypotheticalBestCombo);
                    takeFromDiscardStack = true;
                }
            }

            Card card;
            if (takeFromDiscardStack)
                card = Tb.I.DiscardStack.DrawCard();
            else
                card = Tb.I.CardStack.DrawCard();

            Debug.Log("Card name --------" + card.gameObject.name);
            card.MoveFinished.AddListener(c => DrawCardFinished(c, isServingCard));
            card.MoveCard(transform.position, Tb.I.GameMaster.AnimateCardMovement);
        }

        private void DrawCardFinished(Card card, bool isServingCard)
        {
            card.MoveFinished.RemoveAllListeners();

            HandCardSpot.AddCard(card); //Add card here on player stack

            card.SetTurned(false);
            if (isServingCard)
            {
                State = PlayerState.IDLE;
                return;
            }

            var combos = CardUtil.GetAllPossibleCombos(HandCardSpot.Objects, Tb.I.GameMaster.GetAllCardSpotCards(), false);
            PossibleCardCombosChanged.Invoke(combos);
            laydownCards = combos.Count > 0 ? combos[0] : new CardCombo();
            singleLayDownCards = PlayerUtil.UpdateSingleLaydownCards(HandCardSpot.Objects, laydownCards);
            PossibleSinglesChanged.Invoke(singleLayDownCards);

            if (Tb.I.GameMaster.LayingAllowed())
            {
                var usedJokers = false;

                // If the player has not laid down card packs yet, check if their sum would be enough to do so
                if (!HasLaidDown)
                {
                    HasLaidDown = laydownCards.Value >= Tb.I.GameMaster.MinimumLaySum;

                    /// Try to reach <see cref="GameMaster.MinimumLaySum"/> by appending jokers to any possible cardcombo
                    var jokers = HandCardSpot.Objects.Where(c => c.IsJoker()).ToList();
                    if (!HasLaidDown && jokers.Count() > 0)
                    {
                        for (int i = 0; i < combos.Count; i++)
                        {
                            var combo = new CardCombo(combos[i]);
                            var jokersInUse = combo.GetCards().Where(c => c.IsJoker()).ToList();
                            var remainingJokers = jokers.Except(jokersInUse).ToList();
                            if (remainingJokers.Count == 0)
                                continue;
                            var canLayCombo = combo.TryAddJoker(remainingJokers);
                            if (canLayCombo && combo.CardCount < HandCardCount)
                            {
                                usedJokers = true;
                                laydownCards = combo;
                                combos.Insert(0, combo);
                                PossibleCardCombosChanged.Invoke(combos);
                                NewThought.Invoke("Use jokers to lay down");
                                HasLaidDown = true;
                                break;
                            }
                        }
                        if(!HasLaidDown)
                            NewThought.Invoke("Cannot reach " + Tb.I.GameMaster.MinimumLaySum + " using jokers");
                    }
                }

                // At least one card must remain when laying down
                if (!usedJokers && HasLaidDown && laydownCards.CardCount == HandCardCount)
                    KeepOneSingleCard();
            }

            State = PlayerState.WAITING;
            waitStartTime = Time.time;
        }

        /// <summary>
        /// Returns the card combo with the highest possible value from the given 'HandCards'.
        /// </summary>
        /// <param name="HandCards">The cards in the player's hand</param>
        /// <param name="broadcastCombos">Whether to broadcast all possible combos for UI output</param>
        /// <returns>The combo with the highest value or an empty one, if none was found</returns>
        private CardCombo GetBestCardCombo(List<Card> HandCards, bool broadcastCombos)
        {
            var possibleCombos = CardUtil.GetAllPossibleCombos(HandCards, Tb.I.GameMaster.GetAllCardSpotCards(), false);
            if (broadcastCombos)
                PossibleCardCombosChanged.Invoke(possibleCombos);
            return possibleCombos.Count > 0 ? possibleCombos[0] : new CardCombo();
        }

        /// <summary>
        /// Chooses one card in <see cref="singleLayDownCards"/> which is kept on hand.
        /// Prioritizes cards who do not replace a joker
        /// </summary>
        private void KeepOneSingleCard()
        {
            Single keptSingle = singleLayDownCards.FirstOrDefault(c => c.Joker == null);
            if (keptSingle == null)
                keptSingle = singleLayDownCards[0];
            singleLayDownCards.Remove(keptSingle);
            NewThought.Invoke("Keep single " + keptSingle);
        }

        private void LayDownCardMoveFinished(Card card)
        {
            card.MoveFinished.RemoveAllListeners();
            isCardBeingLaidDown = false;
            currentCardSpot.AddCard(card);

            int cardCount, cardPackCount;
            switch (layStage)
            {
                case LayStage.SETS:
                    cardCount = laydownCards.Sets[currentCardPackIdx].Count;
                    cardPackCount = laydownCards.Sets.Count;
                    break;
                case LayStage.RUNS:
                    cardCount = laydownCards.Runs[currentCardPackIdx].Count;
                    cardPackCount = laydownCards.Runs.Count;
                    break;
                default: // LayStage.SINGLES
                    cardCount = singleLayDownCards.Count;
                    cardPackCount = 1;

                    if (!card.IsJoker())
                    {
                        returningJoker = singleLayDownCards[currentCardIdx].Joker;
                        if (returningJoker != null)
                        {
                            State = PlayerState.RETURNING_JOKER;
                            return;
                        }
                    }
                    break;
            }

            if (currentCardIdx < cardCount - 1)
            {
                currentCardIdx++;
                return;
            }

            // All cards of the current pack have been laid down
            currentCardIdx = 0;
            currentCardPackIdx++;
            currentCardSpot = null;  // Find a new spot for the next pack

            if (currentCardPackIdx < cardPackCount)
                return;

            // All packs or singles have been laid down
            if (layStage == LayStage.RUNS || layStage == LayStage.SINGLES ||
                (layStage == LayStage.SETS && laydownCards.Runs.Count == 0))
            {
                LayingCardsDone();
            }
            else // LayStage.SETS -> Start laying runs
            {
                currentCardPackIdx = 0;
                layStage = LayStage.RUNS;
            }
        }

        private void ReturnJokerMoveFinished(Card joker)
        {
            returningJoker.MoveFinished.RemoveAllListeners();
            isJokerBeingReturned = false;
            HandCardSpot.AddCard(joker);
            returningJoker = null;

            // All possible runs/sets/singles have to be calculated again with that newly returned joker
            laydownCards = GetBestCardCombo(HandCardSpot.Objects, true);
            singleLayDownCards = PlayerUtil.UpdateSingleLaydownCards(HandCardSpot.Objects, laydownCards);
            PossibleSinglesChanged.Invoke(singleLayDownCards);

            if (laydownCards.CardCount == HandCardCount)
                KeepOneSingleCard();

            // Proceed with waiting
            State = PlayerState.WAITING;
            waitStartTime = Time.time;
        }

        private void LayingCardsDone()
        {
            // With only one card left, just end the game
            if (HandCardCount == 1)
            {
                DiscardCard();
                return;
            }

            // Check if there are any (more) single cards to lay down
            singleLayDownCards = PlayerUtil.UpdateSingleLaydownCards(HandCardSpot.Objects, laydownCards);
            PossibleSinglesChanged.Invoke(singleLayDownCards);

            if (singleLayDownCards.Count == HandCardCount)
                KeepOneSingleCard();

            if (singleLayDownCards.Count > 0)
            {
                currentCardIdx = 0;
                layStage = LayStage.SINGLES;
            }
            else
                DiscardCard();
        }

        private void DiscardCard()
        {
            var thoughts = new List<string>();
            Card card = PlayerUtil.GetCardToDiscard(HandCardSpot.Objects, singleLayDownCards, HasLaidDown, ref thoughts);
            thoughts.ForEach(t => NewThought.Invoke(t));

            State = PlayerState.DISCARDING;
            HandCardSpot.RemoveCard(card);
            card.MoveFinished.AddListener(DiscardCardMoveFinished);
            card.MoveCard(Tb.I.DiscardStack.transform.position, Tb.I.GameMaster.AnimateCardMovement);
        }

        private void DiscardCardMoveFinished(Card card)
        {
            card.MoveFinished.RemoveAllListeners();
            Tb.I.DiscardStack.AddCard(card);

            // Refresh the list of possible card combos and singles for the UI
            GetBestCardCombo(HandCardSpot.Objects, true);
            singleLayDownCards = PlayerUtil.UpdateSingleLaydownCards(HandCardSpot.Objects, laydownCards, true);
            PossibleSinglesChanged.Invoke(singleLayDownCards);

            TurnFinished.Invoke();
            State = PlayerState.IDLE;
        }
    }

}