using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public Card[] hand;
    [SerializeField] public int money;
    [SerializeField] public int color;

    public int CallAmount { get; set; }
    public int MinimumRaiseAmount { get; set; }

    public int[] Discards { get; set; } = null;
    public int BetAmount { get; set; } = -1;

    public int GetBet() {
        DateTime start = DateTime.Now;
        while (BetAmount < 0) {
            if ((start - DateTime.Now).TotalSeconds > 60) {
                Debug.LogWarning("Bet timed out");
                return 0;
            }
        }

        int bet = BetAmount;
        BetAmount = -1;
        return bet;
	}

    public int[] GetDiscards() {
        DateTime start = DateTime.Now;
        while (Discards == null) {
            if ((start - DateTime.Now).TotalSeconds > 60) {
                Debug.LogWarning("Discard timed out");
                return new int[] { };
			}
		}

        int[] discards = Discards;
        Discards = null;
        return discards;
	}
}
