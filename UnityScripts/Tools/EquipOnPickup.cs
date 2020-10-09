using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipOnPickup : MonoBehaviour
{

    public InventorySlot[] inventorySlots;

    public void EquipItem(int itemID){

        StartCoroutine(EquipRoutine(itemID));

    }

    IEnumerator EquipRoutine(int itemID){

        yield return new WaitForSeconds(0.1f);

        Debug.Log("Waited");

        foreach(InventorySlot slot in inventorySlots){
            if(itemID == slot.itemID){
                slot.Equip();
                break;
            }
        }

    }
}
