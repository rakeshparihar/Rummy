using Newtonsoft.Json;
using System.Collections.Generic;

namespace FrolicRummy
{
    /// <summary>
    /// Card data structure
    /// Referance to card data
    /// </summary>
    [System.Serializable]
    public struct CardsData
    {
        //Card Deck data
        [JsonProperty("deck")]
        public List<string> pCardsId { get; set; }
    }

    /// <summary>
    /// Contain cardData and use to parse card desk data
    /// </summary>
    [System.Serializable]
    public class Phandcard
    {
        //Card data
        [JsonProperty("data")]
        public CardsData pCardsData { get; set; }
    }
}
