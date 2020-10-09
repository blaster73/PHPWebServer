using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StoreHelper : MonoBehaviour
{

    private string storeURL = "http://fth-media-031/aeondemo/Store.php/?action=";
    public string store = "storea";
    public StoreSlot[] storeSlots;


    public void ShowStore()
    {
        StartCoroutine(SSRoutine(store));
    }

    IEnumerator SSRoutine(string store){

        string phpURL = storeURL + "read&storeName=" + store;

        UnityWebRequest uwrStore = UnityWebRequest.Get(phpURL);
        yield return uwrStore.SendWebRequest();

        string[] items = uwrStore.downloadHandler.text.Split(' ');

        for(int i = 0; i < storeSlots.Length; i++){
            string[] temp = items[i].Split('.');
            int itemID = int.Parse(temp[0]);
            int price = int.Parse(temp[1]);
            storeSlots[i].AssignSlot(itemID, price);
        }
        
    }

}
