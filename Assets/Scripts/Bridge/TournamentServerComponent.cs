using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentServerComponent : MonoBehaviour
{
    [SerializeField] int port = 7899;
    [SerializeField] Game game;
    SimpleHttpServer server;
    GameStateSerializable gamestate;
    string gamestateJson;

    void Start()
    {
        server = new SimpleHttpServer(port);
        server.RegisterPostAction("changegamestate", ChangeGameState);
        server.Start();
    }



    private string ChangeGameState(string json)
    {
        gamestateJson = json;
        gamestate = JsonUtility.FromJson<GameStateSerializable>(json);
        //get the UI ready for the players to start playing
        game.currentRoundState = (RoundState)gamestate.newState;
        game.gamestate = gamestate;
        game.gamestateChanged = true;

        return "{}";
    }
    
}
