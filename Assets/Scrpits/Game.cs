﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] public Player[] players;
    [SerializeField] public Card[] mainHand;
    [SerializeField] public Card[] otherHands;
    [SerializeField] public Text pot;
    [SerializeField] public Text bigBlind;
    [SerializeField] public Text smallBlind;
    [SerializeField] public Text minBet;
    [SerializeField] public Button flipButton;
    [SerializeField] public Image blindIncrease;
    [SerializeField] public Button discard;
    [SerializeField] public Button fold;
    [SerializeField] public Button callCheck;
    [SerializeField] public Button raiseBet;
    [SerializeField] public Text mainMoney;
    [SerializeField] public Text rightMoney;
    [SerializeField] public Text leftMoney;
    [SerializeField] public Text topMoney;

    private int[] storeCards = new int[5];
    private bool flipped = true;

    public void Flip()
    {
        if(flipped)
        {
            for(int i = 0; i < 5; i++)
            {
                storeCards[i] = mainHand[i].rank + ((int)mainHand[i].suit * 13 - 1);

                mainHand[i].suit = Suit.CLUB;
                mainHand[i].rank = 15;
            }
            flipped = false; ;
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                mainHand[i].rank = (storeCards[i] + 1) % 13;
                int suit = (storeCards[i] + 1) - mainHand[i].rank;
                int counter = 0;
                while (suit != 0)
                {
                    suit /= 13;
                    counter++;
                }
                switch(counter)
                {
                    case 0:
                        mainHand[i].suit = Suit.HEART;
                        break;
                    case 1:
                        mainHand[i].suit = Suit.SPADE;
                        break;
                    case 2:
                        mainHand[i].suit = Suit.DIAMOND;
                        break;
                    case 3:
                        mainHand[i].suit = Suit.CLUB;
                        break;
                }
            }
            flipped = true;
        }
    }

    public void Discard()
    {
        List<int> cards = new List<int>();
        int counter = 0;

        foreach(Card card in mainHand)
        {
            if (card.Selected)
            {
                cards.Add(counter);
            }
            counter++;
        }
    }

}
