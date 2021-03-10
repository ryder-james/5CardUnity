using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentServerComponent : MonoBehaviour
{
    [SerializeField] int port = 7899;
    SimpleHttpServer server;
    GameStateSerializable readIn;
    string gamestate;

    void Start()
    {
        server = new SimpleHttpServer(port);
        server.RegisterPostAction("changegamestate", ChangeGameState);
        server.Start();
    }

    private void Update()
    {
        if(gamestate != null)
        {
            int x = 0;
            readIn = JsonUtility.FromJson<GameStateSerializable>(gamestate);

        }
    }


    private string ChangeGameState(string gamestate)
    {
        //change gamestate
        //translate gamestate somehow
        //get the UI ready for the players to start playing
        Debug.Log("GAMESTATE MY DUDE: " + gamestate);
        this.gamestate = gamestate;
        return "{}";
    }
    
}
