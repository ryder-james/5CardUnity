using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMommy : MonoBehaviour
{

    private GameObject save;

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject card = hit.collider.gameObject;
            if (card.tag == "Card")
            {
                if (save)
                {
                    if(save != card)
                    {
                        LetGo();
                    }
                }
                if (!card.GetComponent<Card>().Locked && !card.GetComponent<Card>().LockedForReal)
                {
                    Vector3 newPos = card.GetComponent<Transform>().position;
                    newPos.y += 2;
                    card.GetComponent<Transform>().position = newPos;
                    card.GetComponent<Card>().Locked = true;
                    save = card;
                }
                save = card;
                if (Input.GetMouseButtonDown(0))
                {
                    card.GetComponent<Card>().LockedForReal = !card.GetComponent<Card>().LockedForReal;
                    card.GetComponent<Card>().Selected = !card.GetComponent<Card>().Selected;
                }
            }
            else
            {
                LetGo();
            }
        }
        else
        {
            LetGo();
        }
    }

    private void LetGo()
    {
        if (save)
        {
            Card card = save.GetComponent<Card>();
            Transform trans = save.GetComponent<Transform>();

            if (!card.LockedForReal)
            {
                Vector3 newPos = trans.position;
                newPos.y -= 2;
                trans.position = newPos;
                card.Locked = false;
            }

            save = null;
        }
    }

}
