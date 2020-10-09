using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedSlot : MonoBehaviour
{

    public ItemIndex itemIndex;
    public InvHelper invHelper;
    public ReadInventory readInventory;

    public string slotType;
    public int itemID;
    public string itemName;
    public string type;

    public Image iconImage;
    public Text iconText;

    public GameObject unEquipButton;

    public void AssignSlot(int readItemID){

        // Check if the item is empty
        if(readItemID == 0){
            iconImage.enabled = false;
            iconText.enabled = false;
            itemID = 0;
            itemName = "";
            type = "";
        }
        else{
            // Find which item this is
            foreach(ItemData item in itemIndex.itemDataPrefabs){
                if(item.itemID == readItemID){
                    itemID = item.itemID;
                    itemName = item.itemName;
                    type = item.itemType.ToString();
                }
            }
            iconText.text =slotType + ":\n" + itemName;
            iconImage.enabled = true;
            iconText.enabled = true;
        }

        // Assign sprites to guitars
        if(itemName.Contains("guitar") || itemName.Contains("male") || itemName.Contains("female")){
            iconImage.sprite =  Resources.Load<Sprite>("Thumbnails/" + itemName);
            iconImage.color = Color.white;
            iconText.enabled = false;
        }
        
        
    }

    public void SlotButton(){
        if(itemID != 0){
            unEquipButton.SetActive(!unEquipButton.activeSelf);
        }
        else{
            unEquipButton.SetActive(false);
        }
    }
    
    public void UnEquip(){
        invHelper.UnEquip(readInventory.readPlayerID, slotType);
        unEquipButton.SetActive(false);
    }
}
