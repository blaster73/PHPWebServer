using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    public ItemIndex itemIndex;
    public InvHelper invHelper;
    public ReadInventory readInventory;

    public int slot;
    public int itemID;
    public string itemName;
    public string type;

    public Image iconImage;
    public Text iconText;

    [Header("Deleting UI")]
    public GameObject deleteBarBorder;
    public Image deleteBar;
    public GameObject deletingText;

    public GameObject equipButton;
    public GameObject removeButton;

    private bool pointerDown = false;
    private float pointerDownTimer;
    private float temp;

    private float startFillTime;
    private bool startFillTimeSet = false;

    [SerializeField]
	private float requiredHoldTime;

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
            iconText.text ="Slot: " + (slot) + "\nItem: " + itemName;
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
    
    public void Equip(){
        if(type == "mod"){
            type = "mod1";
        }
        invHelper.Equip(readInventory.readPlayerID, slot, type);
        equipButton.SetActive(false);
        removeButton.SetActive(false);
    }

    public void OnPress(){
        pointerDown = true;
    }

    public void OnRelease(){
        pointerDown = false;
    }

    private void Update() {

        if(itemID != 0){
            if(pointerDown){
                pointerDownTimer += Time.deltaTime;
                // Only show delete status if it's further than a quarter of the way done
                if(pointerDownTimer > (requiredHoldTime / 5)){
                    if(startFillTimeSet == false){
                        startFillTime = Time.time;
                        startFillTimeSet = true;
                    }
                    deleteBarBorder.SetActive(true);
                    deleteBar.fillAmount = Mathf.Lerp(0, 1 + (requiredHoldTime / 5), Time.time - startFillTime);
                }
            }
            else{
                pointerDownTimer = 0;
                deleteBarBorder.SetActive(false);
                deleteBar.fillAmount = 0;
                startFillTimeSet = false;
            }

            if(pointerDownTimer >= requiredHoldTime){
                invHelper.RemoveItem(readInventory.readPlayerID, slot);
            }

            if(pointerDownTimer < temp){
                if(temp < (requiredHoldTime / 5)){
                    if(type == "mod"){
                        type = "mod1";
                    }
                    invHelper.Equip(readInventory.readPlayerID, slot, type);
                }
            }

            temp = pointerDownTimer;
        }
        else{
            deleteBar.fillAmount = 0;
            deleteBarBorder.SetActive(false);
        }

    }


}
