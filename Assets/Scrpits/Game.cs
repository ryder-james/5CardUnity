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
    public bool playerChanged = false;
    public GameStateSerializable gamestate;
    [SerializeField] public Sprite open;
    [SerializeField] public Sprite closed;
    [SerializeField] public GameObject betPopup;

    private int currentPlayer = 0;
    private int[] storeCards = new int[5];
    private bool flipped = true;
    private bool blindWarning = false;

    public void Start()
    {
        //betInputField.enabled = false;

    }
    public void Update()
    {
        if(gamestateChanged)
        {
            gamestateChanged = false;
            ChangeRound();
        }
        if(playerChanged)
        {
            playerChanged = false;
            ChangeTurn();
        }
    }

    public void ChangeTurn()
    {
        if(gamestate.players != null && gamestate.players.Length >= 4)
        {
            Player player = players[currentPlayer];
            SerializablePlayer inPlayer = gamestate.players[currentPlayer];

            //update player's money
            player.money = inPlayer.chips;
            mainMoney.text = players[currentPlayer].money.ToString();

            //convert main player's hand
            for (int j = 0; j < mainHand.Length; j++)
            {
                mainHand[j].rank = PokerUtility.ConvertRankFromSerialized(inPlayer.cards[j].rank);
                mainHand[j].suit = PokerUtility.ConvertSuitFromSerialized(inPlayer.cards[j].type);
            }

            rightMoney.text = players[(currentPlayer + 1) % 4].money.ToString();
            topMoney.text = players[(currentPlayer + 2) % 4].money.ToString();
            leftMoney.text = players[(currentPlayer + 3) % 4].money.ToString();

            pot.text = gamestate.pot.ToString();

            if (player.BetAmount == 0)
            {
                fold.gameObject.SetActive(false);
                callCheck.GetComponentInChildren<Text>().text = "Check";
                raiseBet.GetComponentInChildren<Text>().text = "Bet";
            }
            else
            {
                fold.gameObject.SetActive(true);
                callCheck.GetComponentInChildren<Text>().text = "Call";
                raiseBet.GetComponentInChildren<Text>().text = "Raise";
            }
        }

        
    }

    public void ChangeRound()
    {
        ChangeTurn();

        if (blindWarning)
        {
            blindIncrease.gameObject.SetActive(true);
        }
        else
        {
            blindIncrease.gameObject.SetActive(false);
        }

        switch (currentRoundState)
        {
            case RoundState.Deal:
                discard.gameObject.SetActive(false);
                fold.gameObject.SetActive(false);
                callCheck.gameObject.SetActive(false);
                raiseBet.gameObject.SetActive(false);
                break;
            case RoundState.AssignPot:
                discard.gameObject.SetActive(false);
                fold.gameObject.SetActive(false);
                callCheck.gameObject.SetActive(false);
                raiseBet.gameObject.SetActive(false);
                break;
            case RoundState.PreDraw:
                discard.gameObject.SetActive(false);
                fold.gameObject.SetActive(true);
                callCheck.gameObject.SetActive(true);
                raiseBet.gameObject.SetActive(true);
                break;
            case RoundState.PostDraw:
                discard.gameObject.SetActive(false);
                fold.gameObject.SetActive(true);
                callCheck.gameObject.SetActive(true);
                raiseBet.gameObject.SetActive(true);
                break;
            case RoundState.Draw:
                discard.gameObject.SetActive(true);
                fold.gameObject.SetActive(true);
                callCheck.gameObject.SetActive(false);
                raiseBet.gameObject.SetActive(false);
                break;
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

        foreach (Card card in mainHand)
        {
            if (card.Selected)
            {
                cards.Add(counter);
            }
            counter++;
        }

        players[currentPlayer].Discards = cards.ToArray();
        ChangePlayer();
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

    public void OpenBetMenu()
    {
        betPopup.SetActive(true);
    }

    public void Bet(Text amount)
    {
        betPopup.SetActive(false);
    }

    public void ChangePlayer()
    {
        currentPlayer = (currentPlayer + 1) % 4;
        playerChanged = true;
    }

}
