namespace FrolicRummy.Game
{

    public class Single
    {
        public Card Card { get; private set; }
        public override string ToString() => Card.ToString();

        ///<summary>
        /// The joker which will be replaced by this single card
        ///</summary>
        public Card Joker { get; private set; }

        public Single(Card card) : this(card, null) { }
        public Single(Card card, Card joker)
        {
            Card = card;
            Joker = joker;
        }
    }

}