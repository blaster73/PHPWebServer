using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CosmeticHelper : MonoBehaviour
{

    private string baseURL = "http://fth-media-031/aeondemo/Equipped.php/?action=read&playerID=";
    public ShowCosmeticsMain showCosmeticsMain;

    public void ShowCosmeticsLoginScreen(int id)
    {
        StartCoroutine(SCMRoutine(id));
    }

    IEnumerator SCMRoutine(int id){

        string phpURL = baseURL + id;

        UnityWebRequest uwrCosmetics = UnityWebRequest.Get(phpURL);
        yield return uwrCosmetics.SendWebRequest();

        string[] items = uwrCosmetics.downloadHandler.text.Split(' ');

        for(int i = 0; i < showCosmeticsMain.equippedIds.Length; i++){
            showCosmeticsMain.equippedIds[i] = int.Parse(items[i + 5]);
        }

        showCosmeticsMain.UpdateCosmetics();

        Debug.Log(uwrCosmetics.downloadHandler.text);
        

    }
}
