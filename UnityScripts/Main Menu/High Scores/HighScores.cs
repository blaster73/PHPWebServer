using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HighScores : MonoBehaviour
{

    private string baseURL = "http://fth-media-031/aeondemo/Leaderboards.php/";
    public Text[] users;
    public Text[] scores;

    [ContextMenu("Show Leaderboards")]
    public void ShowLeaderboards()
    {
        StartCoroutine(SLRoutine());
    }

    IEnumerator SLRoutine()
    {
        string phpURL = baseURL;

        UnityWebRequest uwrLeaderboards = UnityWebRequest.Get(phpURL);
        yield return uwrLeaderboards.SendWebRequest();

        string[] items = uwrLeaderboards.downloadHandler.text.Split(' ');

        for(int i = 0; i < users.Length; i++){
            string[] temp = items[i].Split('.');
            string user = temp[0];
            string score = temp[1];
            foreach(string s in temp){
                Debug.Log(i + ": " + s);
            }
            users[i].text = (i + 1).ToString() + ". " + user;
            scores[i].text = score;
        }

    }
}
