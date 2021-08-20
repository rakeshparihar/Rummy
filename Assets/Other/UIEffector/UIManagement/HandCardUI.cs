using System.Collections.Generic;
using FrolicRummy.UIManagement;
using UnityEngine;

namespace FrolicRummy.Game
{
    //WIP: Add/Remove.

    public class HandCardUI : UIBaseBehaviour
    {       
        [SerializeField] protected RectTransform m_Viewport;
        [SerializeField] protected RectTransform m_Content;
        [SerializeField] private UIBaseBehaviour m_Template;
        [SerializeField] protected RectTransform m_ContentTemplate;
        [SerializeField] protected List<CardGroup> cardGroups = new List<CardGroup>();
        private UIManagement.UI m_ParentUI;
        public RectTransform pContent { get => m_Content; }


        public UIManagement.UI pParentUI
        {
            get
            {
                if (m_ParentUI == null)
                    m_ParentUI = GetComponentInParent<UIManagement.UI>();
                return m_ParentUI;
            }
        }

        public UIBaseBehaviour Add(string name)
        {
            GameObject newWidgetObj = Instantiate(m_Template.gameObject, m_Content);
            newWidgetObj.name = name;
            newWidgetObj.transform.localScale = m_Template.transform.localScale;

            if (m_Template is Widget)
            {
                Widget widget = newWidgetObj.GetComponent<Widget>();
                widget.Initialize(pParentUI, null);
                pParentUI.pChildWidgets.Add(widget);
                widget.pVisible = true;
            }
            else if (m_Template is UIManagement.UI)
            {
                UIManagement.UI ui = newWidgetObj.GetComponent<UIManagement.UI>();
                ui.Initialize(pParentUI);
                pParentUI.pChildUIs.Add(ui);
                ui.pVisible = true;
            }
            return newWidgetObj.GetComponent<UIBaseBehaviour>();
        }

        internal void CreateGroup(HashSet<Card> mSelectedCard)
        {
            CardGroup activeCardGroup = GetAvliableGroup();
            if (activeCardGroup != null)
            {
                activeCardGroup.gameObject.SetActive(true);

                int count = -1;
               
                foreach (Card card in mSelectedCard)
                {
                    card.pPlaceHolder.SetParent(activeCardGroup.transform);
                    card.pPlaceHolder.GetComponent<Widget>().AddGroup(activeCardGroup.GetComponent<RectTransform>());
                    card.pPlaceHolder.SetSiblingIndex(++count);
                }
            }
        }

       
        internal CardGroup GetAvliableGroup()
        {
            CardGroup cardGroup = default;
            for (int i = 0; i < cardGroups.Count; i++)
            {
                if (!cardGroups[i].gameObject.activeSelf)
                {
                    cardGroup = cardGroups[i];
                    break;
                }
            }
            return cardGroup;
        }

        internal void AddGroup(CardGroup cardGroup)
        {
            if(cardGroups.Find((x)=> x.id != cardGroups.Count))
            {
                cardGroup.id = cardGroups.Count;
                cardGroups.Add(cardGroup);
            }
            CheckEmptyGroup();
        }

        public void CheckEmptyGroup()
        {
            for (int i = 0; i < cardGroups.Count; i++)
            {
                if (cardGroups[i].transform.childCount == 0)
                {
                    cardGroups[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
