using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PokerJSON : MonoBehaviour
{

    void Start()
    {
        SerializableCard test = new SerializableCard("Ten", "Hearts");
        string json = ToJson(test);

        SerializableCard fromTest = FromJSON(json);
    }

    //take in the card type that Carson makes, convert it into a serializable card, then serialize.
    public static string ToJson(SerializableCard card)
    {
        //this will probably need to get modified before being sent over to the poker engine
        string json = JsonUtility.ToJson(card);

        return json;
    }

    //return the card type that Carson makes
    public static SerializableCard FromJSON(string json)
    {
        var serializableCard = JsonUtility.FromJson<SerializableCard>(json);

        //conversion 'magic' goes here


        return serializableCard;

    }
}
