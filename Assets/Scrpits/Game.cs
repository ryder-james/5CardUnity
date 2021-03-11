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
    public RoundState currentRoundState;
    public bool gamestateChanged = false;
    public bool playerChanged = false;
    public GameStateSerializable gamestate;
    [SerializeField] public Sprite open;
    [SerializeField] public Sprite closed;
    [SerializeField] public GameObject betPopup;
    [SerializeField] public InputField betInputField;

    private int currentPlayer = 0;
    private int[] storeCards = new int[5];
    private bool flipped = true;
    private bool blindWarning = false;
    private bool showPopup = false;

    public void Start()
    {

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
        if(showPopup)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                Bet(betInputField.GetComponentInChildren<Text>());
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                CloseBetMenu();
            }
        }
    }

    public void ChangeTurn()
    {
        if(gamestate.players != null)
        {
            
            SerializablePlayer inPlayer = gamestate.players[currentPlayer];
            Debug.Log(JsonUtility.ToJson(inPlayer));
            //update player's money
            players[currentPlayer].money = inPlayer.chips;
            mainMoney.text = players[currentPlayer].money.ToString();

            //convert main player's hand
            Vector3 temp = new Vector3();
            for (int j = 0; j < mainHand.Length; j++)
            {
                mainHand[j].rank = PokerUtility.ConvertRankFromSerialized(inPlayer.cards[j].rank);
                mainHand[j].suit = PokerUtility.ConvertSuitFromSerialized(inPlayer.cards[j].type);
                cardMommy.LetGo();
                //temp = mainHand[j].gameObject.transform.position;
                //temp.y = -18;
                //mainHand[j].gameObject.transform.position = temp;
            }

            rightMoney.text = players[(currentPlayer + 1) % 4].money.ToString();
            topMoney.text = players[(currentPlayer + 2) % 4].money.ToString();
            leftMoney.text = players[(currentPlayer + 3) % 4].money.ToString();

            pot.text = gamestate.pot.ToString();

            if (players[currentPlayer].BetAmount == 0)
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

        if(flipped)
        {
            Flip();
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
                storeCards[i] = mainHand[i].rank + ((int)mainHand[i].suit * 13) - 1;

                mainHand[i].suit = Suit.CLUB;
                mainHand[i].rank = 15;
            }
            flipped = false;
            flipButton.GetComponent<Image>().sprite = closed;
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
            flipButton.GetComponent<Image>().sprite = open;
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


    public void OpenBetMenu()
    {
        betInputField.text = "";
        betPopup.SetActive(true);
        betInputField.enabled = true;
        showPopup = true;
    }
    public void CloseBetMenu()
    {
        betPopup.SetActive(false);
        showPopup = false;
    }

    public void Bet(Text amount)
    {
        betPopup.SetActive(false);
        int parsed = 0;
        int.TryParse(amount.text, out parsed);

        players[currentPlayer].BetAmount = parsed;
        ChangePlayer();

    }

    public void ChangePlayer()
    {
        currentPlayer = (currentPlayer + 1) % 4;
        playerChanged = true;
    }

}
