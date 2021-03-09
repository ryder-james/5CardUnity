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
        server.RegisterPostAction("changegamestate", ChangeGameState);
        server.Start();
    }


    private string ChangeGameState(string gamestate)
    {
        //change gamestate
        //translate gamestate somehow
        //get the UI ready for the players to start playing

        Debug.Log(gamestate);

        return "{}";
    }
    
}
