using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializableCard
{
    public SerializableCard(string rank, string type)
    {
        this.rank = rank;
        this.type = type;
    }
    [SerializeField] string rank;
    [SerializeField] string type;
}
