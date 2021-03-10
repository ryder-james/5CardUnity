using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SerializablePlayer
{
    public SerializableCard[] cards;
    public int chips;
    public int chipsBet;
    public string id;
    public string name;
    public string state;
}
