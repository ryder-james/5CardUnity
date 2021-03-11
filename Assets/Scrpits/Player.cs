using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public Card[] hand;
    [SerializeField] public int money;
    [SerializeField] public int color;

    public int[] Discards { get; set; } = null;

    public int[] GetDiscards() {
        while (Discards == null) {
            Debug.Log("null");
		}

        int[] discards = Discards;
        Discards = null;
        return discards;
	}
}
