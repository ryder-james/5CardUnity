using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public Card[] hand;
    [SerializeField] public int money;
    [SerializeField] public int color;

    public int[] Discards { get; set; } = null;

    public IEnumerable GetDiscards() {
        while (Discards == null) {
            yield return null;
		}

        yield return Discards;
	}
}
