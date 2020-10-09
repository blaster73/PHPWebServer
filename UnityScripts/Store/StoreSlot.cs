using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreSlot : MonoBehaviour
{

    public ItemIndex itemIndex;
    public InvHelper invHelper;
    public NavigationBar navigationBar;
    public AlertController alertController;
    public InventorySlot[] inventorySlots;
    public StatsHelper statsHelper;
    public int itemID;
    public string itemName;
    public int itemPrice;
    public string itemType;

    public Image iconImage;
    public Text iconText;

    public void AssignSlot(int readItemID, int price){

        itemPrice = price;

        // Find which item this is
        foreach(ItemData item in itemIndex.itemDataPrefabs){
            if(item.itemID == readItemID){
                itemID = item.itemID;
                itemName = item.itemName;
                itemType = item.itemType.ToString();
            }
        }

        // Assign the price
        iconText.text = price.ToString();


        // Assign sprites to item (if sprite exists)
        if(itemName.Contains("guitar") || itemName.Contains("male") || itemName.Contains("female")){
            iconImage.sprite =  Resources.Load<Sprite>("Thumbnails/" + itemName);
            iconImage.color = Color.white;
        }

    }

    public void BuyItem(){

        if(PlayerInfo.instance.currency > itemPrice){

            bool freeslot = false;
            foreach(InventorySlot slot in inventorySlots){
                if(slot.itemID == 0){
                    freeslot = true;
                }
            }
            if(freeslot == true){
                statsHelper.Stats("remove", PlayerInfo.instance.ID, itemPrice);
                invHelper.AddItem(PlayerInfo.instance.ID, itemID);
                PlayerInfo.instance.gameObject.GetComponent<InfoUpdater>().UpdateInfo(PlayerInfo.instance.ID);
                navigationBar.UpdateNavBar();
                string message = itemType + " item purchased for " + itemPrice + " vinyl!";
                alertController.CreateAlert("Item Received!", message);
            }
            else{
                Debug.Log("Inventory full");
                alertController.CreateAlert("Inventory Full!", "Please removed something and try again.");
            }
            
        }
        else{
            Debug.Log("Not enough money");
            alertController.CreateAlert("Not enough currency", "Try collecting some Vinyl!");
        }
    
    }


}
