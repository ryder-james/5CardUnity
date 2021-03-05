using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerServerComponent : MonoBehaviour
{
    [SerializeField] int port;
    [SerializeField] bool start;
    SimpleHttpServer server;

    void Start()
    {
        if(start)
        {
            server = new SimpleHttpServer(port);
            server.Start();
            server.RegisterGetAction("bet", Bet);
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


    public string Bet()
    {
        return "{\"Bet\": \"Do it you won't\"}";
    }
}
