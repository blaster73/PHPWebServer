using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ReadInventory : MonoBehaviour
{

    public GameObject[] inventorySlots;
    public GameObject[] equippedSlots;

    [HideInInspector]
    public bool done = false;
    private string inventoryBaseURL = "http://fth-media-031/aeondemo/Inventory.php/?";
    private string equippedBaseURL = "http://fth-media-031/aeondemo/Equipped.php/?";

    [HideInInspector]
    public int readPlayerID;


    void Start()
    {
        // Get the player's ID if one exists
        /*if(GameObject.Find("Player Information") != null){
            PlayerInfo playerInfo = GameObject.Find("Player Information").GetComponent<PlayerInfo>();
            readPlayerID = playerInfo.ID;
        }
        else{
            // Set the ID to the test player if no ID is detected
            readPlayerID = 7;
        }*/

        if(PlayerInfo.instance.ID != 0){
            readPlayerID = PlayerInfo.instance.ID;
        }
        else{
            // Set the ID to the test player if no ID is detected
            readPlayerID = 7;
        }

        // Read the player's inventory to start
        StartCoroutine(RIRoutine(readPlayerID));
        StartCoroutine(RERoutine(readPlayerID));
    }


    public void UpdateInventory(){
        StartCoroutine(RIRoutine(readPlayerID));
        StartCoroutine(RERoutine(readPlayerID));
        //Debug.Log("Updating the Inventory");
    }

    private IEnumerator RIRoutine(int playerID){

        string phpURL = inventoryBaseURL + "action=read" + "&playerID=" + playerID;

        UnityWebRequest uwrReadInventory = UnityWebRequest.Get(phpURL);
        yield return uwrReadInventory.SendWebRequest();

        //Debug.Log(uwrReadInventory.downloadHandler.text);

        // Split the text into seperate items
        string[] items = uwrReadInventory.downloadHandler.text.Split(' ');
        
        // Assign the strings for testing
        for(int i = 0; i  < inventorySlots.Length; i++){
            inventorySlots[i].GetComponent<InventorySlot>().AssignSlot(int.Parse(items[i]));
        }

    }

    private IEnumerator RERoutine(int playerID){

        string phpURL = equippedBaseURL + "action=read" + "&playerID=" + playerID;

        UnityWebRequest uwrReadEquipped = UnityWebRequest.Get(phpURL);
        yield return uwrReadEquipped.SendWebRequest();

        //Debug.Log(uwrReadEquipped.downloadHandler.text);

        // Split the text into seperate items
        string[] items = uwrReadEquipped.downloadHandler.text.Split(' ');
        
        // Assign the strings for testing
        for(int i = 0; i  < equippedSlots.Length; i++){
            equippedSlots[i].GetComponent<EquippedSlot>().AssignSlot(int.Parse(items[i]));
        }

        // Find the cosmetics script and show them
        if(GameObject.Find("Cosmetics") != null){
            GameObject.Find("Cosmetics").GetComponent<ShowCosmetics>().UpdateCosmetics();
        }

    }

}
