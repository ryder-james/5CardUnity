using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SerializableCard
{
    public string rank;
    public string type;


    public SerializableCard(Card card)
    {
        if(card.rank < 11)
        {
            rank = card.rank.ToString();
        }
        else
        {
            string royalLetter;
            switch(card.rank)
            {
                case 11:
                    royalLetter = "J";
                    break;
                case 12:
                    royalLetter = "Q";
                    break;
                case 13:
                    royalLetter = "K";
                    break;
                default:
                    royalLetter = "Error Converting Rank from Card to SerializableCard";
                    break;
            }
            rank = royalLetter;
        }
        type = card.suit.ToString()[0].ToString();
    }

   

}
