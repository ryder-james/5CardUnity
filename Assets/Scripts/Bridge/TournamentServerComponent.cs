using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentServerComponent : MonoBehaviour
{
    [SerializeField] int port = 7899;
    SimpleHttpServer server;

    void Start()
    {
        server = new SimpleHttpServer(port);
        server.RegisterPostAction("changegamestate", StartShowdown);
        server.Start();
    }


    private string StartShowdown(string gamestate)
    {
        //change to showdown state here


        return "{}";
    }
    
}
