using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace FrolicRummy.Game
{
    /// <summary>
    /// Responsible to get card data form json file
    /// 
    /// </summary>
    public class CardGenerator : MonoBehaviour
    {
        [SerializeField] private TextAsset m_CardsInfo; //Text asset referance for CardInfo
        [SerializeField] private UICards m_UICards; //UICard elemenet
        [SerializeField] private CardsAssetsConfig m_CardsAssetsConfig; //Card asset configuration


        void Start()
        {
            //Deserialize card data in local
            Phandcard phandcard = JsonConvert.DeserializeObject<Phandcard>(m_CardsInfo.text);
            List<string> cardsId = phandcard.pCardsData.pCardsId;

            //Create card
            foreach (string cId in cardsId)
                m_UICards.CreateCards(cId, m_CardsAssetsConfig.GetSprite(cId));
        }
    }
}