using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StatsHelper : MonoBehaviour
{

    private string statsBaseURL = "http://fth-media-031/aeondemo/Stats.php/?action=";

    public bool useScore;
    private Score score;

    private void Start() {
        score = GetComponent<Score>();   
    }

    public void Stats(string action, int playerID, int amount){

        StartCoroutine(SRoutine(action, playerID, amount));
        
    }

    IEnumerator SRoutine(string action, int playerID, int amount){

        string phpURL = statsBaseURL + action + "&playerID=" + playerID + "&amount=" + amount;

        UnityWebRequest uwrStats = UnityWebRequest.Get(phpURL);
        yield return uwrStats.SendWebRequest();

        //Debug.Log(uwrStats.downloadHandler.text);
        score.UpdateScore(int.Parse(uwrStats.downloadHandler.text));

    }


}
