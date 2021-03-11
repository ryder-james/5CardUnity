using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerServerComponent : MonoBehaviour
{
    [SerializeField] int port;
    [SerializeField] bool start = true;
    [SerializeField] private Player player = null;
    SimpleHttpServer server;
    int data = 0;
    private string randomPlayerData = "This is player data that is accessible from the server thread";
    //private Card card;
    void Start()
    {
        if(start)
        {
            server = new SimpleHttpServer(port);
            server.RegisterPostAction("discard", Discard);
            server.RegisterPostAction("bet", Bet);
            server.RegisterGetAction("", Root);
            server.RegisterGetAction("version", Version);
            server.Start();
            //card = GetComponent<Card>();
        }
        
    }
    private void OnDestroy()
    {
        if(server != null)
        {
            server.Stop();
        }
    }

    private string Root()
    {
        //call player method here
        return "";
    }

    private string Version()
    {
        return "";
    }

    private string Bet(string jsonIn)
    {
        //certain functions can not be called from this callback (particularly unity functions like GetComponent<>(), since it gets called from the server's thread
        
        Debug.Log("Data Received from POST: " + jsonIn + "\nMoreData: " + data);
        data++;
        Debug.Log("Data has changed" + data);
        return "90";
    }

    private string Discard(string jsonIn)
    {
        IEnumerable result = player.GetDiscards();
        while (result == null) {
            Debug.Log("null!");
            result = player.GetDiscards();
		}
        Debug.Log(result);

        return "[1, 2]";
    }
}
