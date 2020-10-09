using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InfoUpdater : MonoBehaviour
{

    string baseURL = "http://fth-media-031/aeondemo/PlayerInfo.php/?playerID=";

    public void UpdateInfo(int playerID){

        StartCoroutine(UIRoutine(playerID));
        
    }

    IEnumerator UIRoutine(int playerID){

        string phpURL = baseURL + playerID;

        UnityWebRequest uwrUPdateInfo = UnityWebRequest.Get(phpURL);
        yield return uwrUPdateInfo.SendWebRequest();

        string[] items = uwrUPdateInfo.downloadHandler.text.Split(' ');

        PlayerInfo.instance.currency = int.Parse(items[0]);
        PlayerInfo.instance.level = int.Parse(items[1]);
        PlayerInfo.instance.userName = items[2];
        Debug.Log("Updated Player Information");

    }
}
