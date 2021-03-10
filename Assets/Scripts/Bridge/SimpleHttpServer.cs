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
    public delegate string PostAction(string jsonIn);
    public int Port { get; private set; }
    private string _rootDirectory;
    private HttpListener _listener;
    private Thread _server;

    private Dictionary<string, GetAction> getActions;
    private Dictionary<string, PostAction> postActions;

    /// <summary>
    /// Call this function to register a callback function to be called when the server receives a GET request with the given path
    /// </summary>
    /// <param name="path">The path of the url given after the port that should trigger the callback</param>
    /// <param name="action">The action passed in should return data as a valid json object</param>
    public void RegisterGetAction(string path, GetAction action)
    {
        getActions.Add(path, action);
    }

    /// <summary>
    /// Call this function to registar a callback function to be called when the server receives a POST request with the given path
    /// </summary>
    /// <param name="path">The path of th eurl given after the port that should trigger the callback</param>
    /// <param name="action">The action passed in should receive a string as valid json data, and return a string as valid json data</param>
    public void RegisterPostAction(string path, PostAction action)
    {
        postActions.Add(path, action);
    }

    public SimpleHttpServer(int port)
    {
        Port = port;
        getActions = new Dictionary<string, GetAction>();
        postActions = new Dictionary<string, PostAction>();
    }

    /// <summary>
    /// Starts a new thread with a server listening on the port given.
    /// </summary>
    public void Start()
    {
        if(_server == null || !_server.IsAlive)
        {
            Debug.Log("Server Started, Listening on " + Port + ".");
            _server = new Thread(Listen);
            _server.Start();
        }
        
    }

    /// <summary>
    /// Stops the server listening, and stops the thread the server runs on.
    /// </summary>
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
        _listener.Prefixes.Add("http://127.0.0.1:" + Port.ToString() + "/");
        _listener.Prefixes.Add("http://localhost:" + Port.ToString() + "/");
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

        if(context.Request.HttpMethod.ToUpper() == "GET" 
            && getActions.ContainsKey(path))
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
        else if (context.Request.HttpMethod.ToUpper() == "POST"
            && postActions.ContainsKey(path))
        {
            PostAction action;
            
            if (postActions.TryGetValue(path, out action))
            {
                //read the InputStream, which should be the body of the POST 
                string json;
                using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                {
                    json = reader.ReadToEnd();
                }

                //call the method that the player has added.
                //This may need to be adjusted to send data back to the engine
                string result = action(json);
                OutputStringToStreamAndClose(context.Response.OutputStream, result);

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

        if(stream.CanWrite)
        {
            stream.Write(bytes, 0, bytes.Length);
        }

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
