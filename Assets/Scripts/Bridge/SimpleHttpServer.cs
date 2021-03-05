using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

public class SimpleHttpServer
{
    public delegate string GetAction();
    public delegate void PostAction(string jsonIn);
    public int _port { get; private set; }
    private string _rootDirectory;
    private HttpListener _listener;
    private Thread _server;

    private Dictionary<string, GetAction> getActions;
    private Dictionary<string, PostAction> postActions;

    public void RegisterGetAction(string path, GetAction action)
    {
        getActions.Add(path, action);
    }

    public void RegisterPostAction(string path, PostAction action)
    {
        postActions.Add(path, action);
    }

    public SimpleHttpServer(int port)
    {
        _port = port;
        getActions = new Dictionary<string, GetAction>();
        postActions = new Dictionary<string, PostAction>();
    }

    public void Start()
    {
        if(_server == null || !_server.IsAlive)
        {
            Debug.Log("Server Started, Listening on " + _port + ".");
            _server = new Thread(Listen);
            _server.Start();
        }
        
    }

    public void Stop()
    {
        if (_server != null && _server.IsAlive)
        {
            _server.Abort();
            _listener.Stop();
        }
         
    }

    private void Listen()
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add("http://127.0.0.1:" + _port.ToString() + "/");
        _listener.Start();


        while (true)
        {
            HttpListenerContext context = _listener.GetContext();
            ProcessContext(context);
        }


    }

    private void ProcessContext(HttpListenerContext context)
    {
        string fullPath = context.Request.Url.AbsolutePath;
        Debug.Log(fullPath);
        string path = fullPath.Substring(1).ToLower();

        if(getActions.ContainsKey(path))
        {
            //read the OutputStream, which will be the body of the response

            GetAction action;
            if(getActions.TryGetValue(path, out action))
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;

                string result = action();
                OutputStringToStreamAndClose(context.Response.OutputStream, result);
            }
        }
        else if(postActions.ContainsKey(path) && context.Request.HttpMethod.ToUpper() == "GET")
        {
            PostAction action;
            
            if (postActions.TryGetValue(path, out action))
            {
                //read the InputStream, which should be the body of the POST request
                long streamLength = context.Request.InputStream.Length;
                byte[] data = new byte[streamLength];
                context.Request.InputStream.Read(data, 0, data.Length);

                //translate the bytes into a string
                string json = data.ToString();

                //call the method that the player has added.
                    //This may need to be adjusted to send data back to the engine
                action(json);

            }

        }
        else
        {
            //send 200 OK
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            OutputStringToStreamAndClose(context.Response.OutputStream, "OK DUDE");
        }


    }



    //helpers

    private void OutputStringToStreamAndClose(Stream stream, string output)
    {
        byte[] bytes = StringToByteArray(output);

        stream.Write(bytes, 0, bytes.Length);

        stream.Flush();
        stream.Close();
    }

    private byte[] StringToByteArray(string input)
    {
        byte[] output = new byte[input.Length];

        for(int i = 0; i < input.Length; i++)
        {
            output[i] = (byte)input[i];
        }

        return output;
    }

}
