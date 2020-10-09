using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StatsHelperMotion : MonoBehaviour
{

    private string statsBaseURL = "http://fth-media-031/aeondemo/Stats.php/?action=";
    public Text score;

    public void Stats(string action, int playerID, int amount){

        StartCoroutine(SRoutine(action, playerID, amount));
        
    }

    IEnumerator SRoutine(string action, int playerID, int amount){

        string phpURL = statsBaseURL + action + "&playerID=" + playerID + "&amount=" + amount;

        UnityWebRequest uwrStats = UnityWebRequest.Get(phpURL);
        yield return uwrStats.SendWebRequest();

        score.text = uwrStats.downloadHandler.text;

    }


}
