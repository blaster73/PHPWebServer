using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCosmeticsMain : MonoBehaviour
{
    // This  is the list of all items in the game
    public ItemIndex itemIndex;

    // Makre sure these are in the order of Head, Back, Shirt, Pants, Shoes
    public int[] equippedIds;

    // These are tje empty locators for prefabs to be activated at
    public GameObject[] cosmeticSlots;

    // All potential back cosmetics 
    public ItemData[] backCosmetics;

    public ItemData[] shirtSlots;

    public void UpdateCosmetics(){

        // Disable any items already equipped
        foreach(GameObject g in cosmeticSlots){

            foreach(Transform child in g.transform){
                child.gameObject.SetActive(false);
            }

        }

        shirtSlots[0].gameObject.SetActive(true);

        // For now only check the back slot
        foreach(Transform child in cosmeticSlots[1].gameObject.transform){
            // Cheack our item list for any equipped items
            foreach(ItemData item in backCosmetics){
                if(item.itemID == equippedIds[1]){
                    // Enable the prefab of the item
                    item.gameObject.SetActive(true);
                }
            }
        }

        foreach(ItemData item in shirtSlots){
            if(item.itemID == equippedIds[2]){
                item.gameObject.SetActive(true);
            }
            else{
                item.gameObject.SetActive(false);
            }
        }
          
    }
}
