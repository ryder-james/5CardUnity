using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerServerComponent : MonoBehaviour
{
    [SerializeField] int port;
    [SerializeField] bool start = true;
    SimpleHttpServer server;
    int data = 0;
    private string randomPlayerData = "This is player data that is accessible from the server thread";
    //private Card card;
    void Start()
    {
        if(start)
        {
            server = new SimpleHttpServer(port);
            server.RegisterPostAction("bet", Bet);
            server.Start();
            //card = GetComponent<Card>();
        }
        
    }

    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if(server != null)
        {
            server.Stop();
        }
    }


    public string Bet(string jsonIn)
    {
        //certain functions can not be called from this callback (particularly unity functions like GetComponent<>(), since it gets called from the server's thread
        
        Debug.Log("Data Received from POST: " + jsonIn + "\nMoreData: " + data);
        data++;
        Debug.Log("Data has changed" + data);
        return "{\"Bet\": \"Do it you won't\"}\n" + randomPlayerData;
            //+ '\n' + card.rank.ToString();
    }
}
