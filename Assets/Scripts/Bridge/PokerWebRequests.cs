using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PokerWebRequests : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(PlayerGet("27017/store"));

        StartCoroutine(PlayerPost("8080", "posting data!"));
    }

    void Update()
    {
        
    }

    static IEnumerator PlayerGet(string playerPort)
    {
        string uri = "localhost:" + playerPort;// + "/version";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if(webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }


    }

    static IEnumerator PlayerPost(string playerPort, string jsonDataToPass)
    {
        string uri = "localhost:" + playerPort + "/bet";

        using(UnityWebRequest webRequest = UnityWebRequest.Post(uri, jsonDataToPass))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if(webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }





}
