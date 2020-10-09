using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryActivation : MonoBehaviour
{

    public GameObject inventoryHolder;
    public GameObject navigationBar;
    public NavButtonVisuals navButtonVisuals;
    public NavigationBar navBarScript;
    public ShowCosmetics showCosmetics;
    public StoreHelper storeHelper;

    public void ActivateInventory(){

        // Update the PlayerInfo gameobject
        PlayerInfo.instance.gameObject.GetComponent<InfoUpdater>().UpdateInfo(PlayerInfo.instance.ID);

        // Check if menu is already open
        bool needsOpening = true;
        foreach(GameObject page in navBarScript.gameObject.GetComponent<MenuNavigation>().pages){
            if(page.activeSelf == true){
                page.SetActive(false);
                needsOpening = false;
            }
        }
        if(needsOpening){
            inventoryHolder.SetActive(true);
        }
        else{
            inventoryHolder.SetActive(false);
        }
        
        // Set the inventory button to inventory
        navButtonVisuals.SelectInventoryButton();

        navigationBar.SetActive(!navigationBar.activeSelf);
        navBarScript.UpdateNavBar();
        GetComponent<ReadInventory>().UpdateInventory();
        showCosmetics.UpdateCosmetics();
        storeHelper.ShowStore();
        
    }
}
