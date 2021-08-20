using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace FrolicRummy
{
    /// <summary>
    /// Contain card sprite data 
    /// </summary>
    [System.Serializable]
    public class CardAsset
    {
        [SerializeField] private Sprite m_Sprite;
        public Sprite pSprites { get => m_Sprite; }
    }

    /// <summary>
    /// Create scriptable Data for cards
    /// Get card sprite
    /// </summary>
    [CreateAssetMenu(fileName = "CardsAssetsConfig")]
    public class CardsAssetsConfig : ScriptableObject
    {
        //Referance to card asset data
        [SerializeField] private CardAsset[] m_CardsAssets;

        //Return card asset sprite with compare wiht name
        public Sprite GetSprite(string cardId)
        {
            var cardSp = Array.Find(m_CardsAssets, ele => ele.pSprites.name.Equals(cardId));
            if (cardSp != null)
                return cardSp.pSprites;
            else
                return null;
        }
    }
}
