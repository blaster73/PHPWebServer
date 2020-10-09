using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InvHelper : MonoBehaviour
{
    private string inventoryBaseURL = "http://fth-media-031/aeondemo/Inventory.php/?";
    private string equippedBaseURL = "http://fth-media-031/aeondemo/Equipped.php/?";

    public ReadInventory readInventory;
    public int playerID;
    public int inventorySlot;
    public string slotType;
    public int itemID;

    private void UpdateInventory(){
        readInventory.UpdateInventory();
    }


    public void Equip(int playerID, int inventorySlot, string slotType){
        StartCoroutine(EquipRoutine(playerID, inventorySlot, slotType));
    }

    IEnumerator EquipRoutine(int playerID, int inventorySlot, string slotType){

        string phpURL = equippedBaseURL + "action=equip&playerID=" + playerID + "&invSlot=" + inventorySlot + "&slotType=" + slotType;

        UnityWebRequest uwrEquip = UnityWebRequest.Get(phpURL);
        yield return uwrEquip.SendWebRequest();

        Debug.Log(uwrEquip.downloadHandler.text);
        UpdateInventory();

    }


    public void UnEquip(int playerID, string slotType){
        StartCoroutine(UnEquipRoutine(playerID, slotType));
    }

    IEnumerator UnEquipRoutine(int playerID, string slotType){

        string phpURL = equippedBaseURL + "action=unequip&playerID=" + playerID + "&slotType=" + slotType;

        UnityWebRequest uwrUnEquip = UnityWebRequest.Get(phpURL);
        yield return uwrUnEquip.SendWebRequest();

        Debug.Log(uwrUnEquip.downloadHandler.text);
        UpdateInventory();

    }


    public void AddItem(int playerID, int itemID){
        StartCoroutine(AddItemRoutine(playerID, itemID));
    }

    IEnumerator AddItemRoutine(int playerID, int itemID){
        
        string phpURL = inventoryBaseURL + "action=add&playerID=" + playerID + "&itemID=" + itemID;

        UnityWebRequest uwrAddItem = UnityWebRequest.Get(phpURL);
        yield return uwrAddItem.SendWebRequest();

        Debug.Log(uwrAddItem.downloadHandler.text);
        UpdateInventory();

    }


    public void RemoveItem(int playerID, int inventorySlot){
        StartCoroutine(RemoveItemRoutine(playerID, inventorySlot));
    }

    IEnumerator RemoveItemRoutine(int playerID, int inventorySlot){
        
        string phpURL = inventoryBaseURL + "action=remove&playerID=" + playerID + "&invSlot=" + inventorySlot;

        UnityWebRequest uwrRemoveItem = UnityWebRequest.Get(phpURL);
        yield return uwrRemoveItem.SendWebRequest();

        Debug.Log(uwrRemoveItem.downloadHandler.text);
        UpdateInventory();

    }

}
