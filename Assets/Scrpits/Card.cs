using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class Card : MonoBehaviour
{
    [SerializeField] public int rank = 1;

    [SerializeField] public Suit suit;

    [SerializeField] public Sprite[] Cards;

    [SerializeField] public SpriteRenderer sprite;

    public bool Locked { get; set; } = false;
    public bool LockedForReal { get; set; } = false;

    public bool Selected { get; set; } = false;

    private void Update()
    {
        sprite.sprite = Cards[rank + ((int)suit * 13) - 1];
    }

    public void Lock(BaseEventData bed)
    {
        Locked = !Locked;
    }

    public void Hover(BaseEventData bed)
    {
        Debug.Log("enter");
        if (!Locked)
        {
            Vector3 newPos = this.GetComponent<Transform>().position;

            newPos.y += 2;

            this.GetComponent<Transform>().position = newPos;
        }
    }

    public void UnHover(BaseEventData bed)
    {
        Debug.Log("exit");
        if (!Locked)
        {
            Vector3 newPos = this.GetComponent<Transform>().position;

            newPos.y += 2;

            this.GetComponent<Transform>().position = newPos;
        }
    }
}
