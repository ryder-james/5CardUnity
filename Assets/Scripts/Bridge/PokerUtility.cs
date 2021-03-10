using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;


public class PokerUtility : MonoBehaviour
{


    //take in the card type that Carson makes, convert it into a serializable card, then serialize.
    public static string ToJson(Card card)
    {
        SerializableCard serializedCard = new SerializableCard(card);
        string json = JsonUtility.ToJson(serializedCard);

        return json;
    }

    //return the card type that Carson makes
    public static Card FromJSON(string json, GameObject objectToAddCardTo)
    {
        var serializableCard = JsonUtility.FromJson<SerializableCard>(json);

        //conversion 'magic' goes here
        Card card = objectToAddCardTo.AddComponent<Card>();


        card.rank = ConvertRankFromSerialized(serializableCard.rank);
        card.suit = ConvertSuitFromSerialized(serializableCard.type);


        return card;

    }

    public static Card ConvertSerializedCard(SerializableCard inCard)
    {
        Card newCard = new Card();

        newCard.suit = ConvertSuitFromSerialized(inCard.type);
        newCard.rank = ConvertRankFromSerialized(inCard.rank);

        return newCard;
    }

    private static Suit ConvertSuitFromSerialized(string suit)
    {
        Suit convertedSuit = Suit.CLUB;
        switch(suit)
        {
            case "H":
                convertedSuit = Suit.HEART;
                break;
            case "D":
                convertedSuit = Suit.DIAMOND;
                break;
            case "S":
                convertedSuit = Suit.SPADE;
                break;
        }

        return convertedSuit;
    }

    private static int ConvertRankFromSerialized(string rank)
    {
        int newRank;
        //if our parse fails, we need to convert royalty
        if(!int.TryParse(rank, out newRank))
        {
            switch(rank)
            {
                case "J":
                    newRank = 11;
                    break;
                case "Q":
                    newRank = 12;
                    break;
                case "K":
                    newRank = 13;
                    break;
            }
        }

        return newRank;
    }


}
