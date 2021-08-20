using System.Collections.Generic;
using FrolicRummy.UIManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace FrolicRummy.Game
{
    /// <summary>
    /// Have refernce of GroupButton, CardTamplete and menu
    /// Get Selected card data
    /// </summary>
    public class UICards : UIManagement.UI, IWidgetBeginDragHandler, IWidgetDragHandler, IWidgetDropHandler, IWidgetDragEndHandler, IWidgetClickHandler
    {
        [SerializeField] private HandCardUI m_Menu; //Menu refernce
        [SerializeField] private Button m_GroupButton; //Group button refernce
        [SerializeField] private Card m_CardTemplate; //Card tampelet rerence
        [SerializeField] private Button m_BackButton; //Back button

        public UnityEvent onCardDrop;
        public UnityEvent onCardDragEnd;
        public UnityEvent onCardDrag;

        private HashSet<Card> mSelectedCard = new HashSet<Card>(); //Getter  for Seledcted card

        //Set placeholder and careate Cards
        public Card CreateCards(string name, Sprite sprite)
        {
            Widget placeHolder = m_Menu.Add("PlaceHolder") as Widget;
            Card card = m_CardTemplate.Duplicate(name) as Card;
            card.pImage = sprite;
            placeHolder.AddChild(card);
            card.SetPlaceholder(placeHolder.transform);
            placeHolder.AddGroup(m_Menu.pContent);
            return card;
        }

        //On Begin Drag called

        void IWidgetBeginDragHandler.OnBeginDrag(Widget widget, PointerEventData eventData)
        {
            ClearSelection();
            (widget as Card).pCanvasGroup.blocksRaycasts = false;
           // widget.transform.SetParent(transform);
        }

        //On Drag called
        void IWidgetDragHandler.OnDrag(Widget widget, PointerEventData eventData)
        {
            widget.pPosition = eventData.position;
            onCardDrag?.Invoke();
        }

        
        public RectTransform dropParentGroup;
        //On Drop called
        void IWidgetDropHandler.OnDrop(Widget dropWidget, Widget dragWidget, PointerEventData eventData)
        {
            Transform placeHolder = (dropWidget as Card).pPlaceHolder;
            Transform dragPlaceHolder  = (dragWidget as Card).pPlaceHolder;
            dragPlaceHolder.SetSiblingIndex(placeHolder.GetSiblingIndex());
            //To do Get the drop and the location of dummy card
            dropParentGroup = placeHolder.GetComponent<Widget>().pParentGroup;

            Debug.Log("Index " + placeHolder.GetComponent<Widget>().pParentGroup.name);
           // dragWidget.AddGroup(placeHolder.GetComponent<Widget>().pParentGroup);
            //(dropWidget as Card).SetPlaceholder((dragWidget as Card).pPlaceHolder);
            //(dragWidget as Card).SetPlaceholder(placeHolder);
            onCardDrop?.Invoke();
        }

        //On Drag End called
        void IWidgetDragEndHandler.OnDragEnd(Widget widget, PointerEventData eventData)
        {
            Card card = widget as Card;
            card.pCanvasGroup.blocksRaycasts = true;
            card.pIsChecked = false;
            card.SetPlaceholder(card.pPlaceHolder);
            card.pPlaceHolder.GetComponent<Widget>().AddGroup(dropParentGroup);
            Debug.Log("On Drag end group " + card.pPlaceHolder.GetComponent<Widget>().pParentGroup.name);
            onCardDragEnd?.Invoke();
        }

        //OnClic called
        void IWidgetClickHandler.OnClick(Widget widget, PointerEventData eventData)
        {
            if (widget is Card)
            {
                if ((widget as Card).pIsChecked)
                    mSelectedCard.Add(widget as Card);
                else
                    mSelectedCard.Remove(widget as Card);
                m_GroupButton.pVisible = mSelectedCard.Count > 1;
            }
            else if (widget == m_GroupButton)
            {
                ReArranageSelectedCards();
            }
        }

        //Rearrage Selected cards
        private void ReArranageSelectedCards()
        {
            m_Menu.CreateGroup(mSelectedCard);
            ClearSelection();
            onCardDragEnd?.Invoke();
        }

        //Clear Selected cards group
        private void ClearSelection()
        {
            foreach (Card card in mSelectedCard)
                card.pIsChecked = false;
            mSelectedCard.Clear();
            m_GroupButton.pVisible = false;
        }


        //Load main menu
        public void OnBackButton()
        {
            //SceneManager.LoadScene("MainScene");
        }
    }
}
