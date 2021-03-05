using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class Card : MonoBehaviour
{
    [SerializeField] public int rank = 1;

    [SerializeField] public Suit suit;

    [SerializeField] public Sprite[] Cards;

    [SerializeField] public SpriteRenderer sprite;

    private void Update()
    {
        sprite.sprite = Cards[rank + ((int)suit * 13) - 1];
    }
}
