using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rzr.Core.Game;
using Rzr.Core.Controls;

namespace Rzr.Core.Editors.Board
{
    public class BoardModel : CardSelectionModel
    {
        public int ActiveCard { get; set; }

        public CardModel[] BoardCards { get; set; }

        public CardModel[] Cards { get; set; }

        public BoardModel()
        {
            Cards = new CardModel[52];
            for (int i = 0; i < 52; i++)
            {
                CardModel model = new CardModel(i);
                Cards[i] = model;
            }

            BoardCards = new CardModel[5];
            for (int i = 0; i < 5; i++)
            {
                CardModel model = new CardModel();
                BoardCards[i] = model;
            }            
        }

        public void SelectCard(CardModel model)
        {
            if (BoardCards[ActiveCard].ModelCard != null)
                Cards[BoardCards[ActiveCard].ModelCard.RawInt].Available = true;

            Cards[model.ModelCard.RawInt].Available = false;            
            BoardCards[ActiveCard].ModelCard = model.ModelCard;
            if (ActiveCard < 4) ActiveCard++;
        }

        public void UnselectCard(CardModel model)
        {
            SetActiveCard(model);
            if (model.ModelCard == null) return;

            Cards[model.ModelCard.RawInt].Available = true;
            BoardCards[ActiveCard].ModelCard = null;
        }

        public void SetActiveCard(CardModel model)
        {
            for (int i = 0; i < 5; i++)
                if (BoardCards[i] == model)
                    ActiveCard = i;
        }
    }
}
