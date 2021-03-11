using System.Collections;
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
    [SerializeField] public CardMommy cardMommy;
    [SerializeField] public InputField betInputField;
    public RoundState currentRoundState;
    public bool gamestateChanged = false;
    public GameStateSerializable gamestate;

    private int currentPlayer = 0;
    private int[] storeCards = new int[5];
    private bool flipped = true;

    public void Start()
    {
        betInputField.enabled = false;

    }
    public void Update()
    {
        if(gamestateChanged)
        {
            gamestateChanged = false;
            UpdateGame();
        }
    }

    public void UpdateGame()
    {
        if(currentRoundState == RoundState.AssignPot)
        {
            Player player = players[currentPlayer];
            SerializablePlayer inPlayer = gamestate.players[currentPlayer];

            //update player's money
            player.money = inPlayer.chips;
            mainMoney.text = players[currentPlayer].money.ToString();

        }
        else if(currentRoundState == RoundState.Draw)
        {
            //convert main player's hand
            Player player = players[currentPlayer];
            SerializablePlayer inPlayer = gamestate.players[currentPlayer]; 

            for (int j = 0; j < mainHand.Length; j++)
            {
                mainHand[j].rank = PokerUtility.ConvertRankFromSerialized(inPlayer.cards[j].rank);
                mainHand[j].suit = PokerUtility.ConvertSuitFromSerialized(inPlayer.cards[j].type);
            }
        }
        


    }

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

        players[currentPlayer].Discards = cards.ToArray();
    }

    public void ShowBet()
    {
        betInputField.enabled = true;
        
    }

    public void SubmitBet()
    {
        int betAmount;
        if(int.TryParse(betInputField.text, out betAmount))
        {
            if(betAmount <= players[currentPlayer].money)
            {
                betInputField.text = "";
                betInputField.enabled = false;
            }
        }
    }

}
