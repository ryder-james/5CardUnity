using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GameStateSerializable
{
    public int game;
    public int hand;
    public int spinCount;
    public int sb;
    public int pot;
    public SerializableSidePot[] sidepots;
    public SerializableCard[] commonCards;

    public int newState;

    public SerializablePlayer[] players;

    public int GetDealer()
    {
        return 0;//players[];//implement dealer logic
    }
}
