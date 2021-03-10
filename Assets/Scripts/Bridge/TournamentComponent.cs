using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class TournamentComponent : MonoBehaviour
{

    Process tournamentProcess;

    void Start()
    {
        string temp = Directory.GetCurrentDirectory();
        tournamentProcess = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        Directory.SetCurrentDirectory(Application.streamingAssetsPath + "/poker-holdem-engine/");
        //startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = "/K node 5Card/live.js --run";
        tournamentProcess = Process.Start(startInfo);
        Directory.SetCurrentDirectory(temp);
        //tournamentProcess.WaitForExit();
    }

    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if(tournamentProcess != null)
        {
            tournamentProcess.Kill();
        }
    }
}
