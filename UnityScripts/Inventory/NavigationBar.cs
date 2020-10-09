using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationBar : MonoBehaviour
{

    public InfoUpdater infoUpdater;
    public Score score;
    public Text username;
    public Text level;
    public Text currency;
    private Text tempCurrency;

    // Start is called before the first frame update
    void Start()
    {
        // Get the player's ID if one exists
        if(PlayerInfo.instance.ID == 0){
            PlayerInfo.instance.ID = 7;
            PlayerInfo.instance.loggedIn = true;
        }

        PlayerInfo.instance.gameObject.GetComponent<InfoUpdater>().UpdateInfo(PlayerInfo.instance.ID);

        UpdateNavBar();
    }

    public void UpdateNavBar(){
        Debug.Log("Updating Navigation Bar");
        username.text = PlayerInfo.instance.userName;
        level.text = "Lvl " + PlayerInfo.instance.level.ToString();
        currency.text = "Vinyl: " + PlayerInfo.instance.currency.ToString();

        StartCoroutine(CheckForUpdate());
    }

    IEnumerator CheckForUpdate(){

        yield return new WaitForSeconds(0.25f);
        Debug.Log("Updating Navigation Bar");
        username.text = PlayerInfo.instance.userName;
        level.text = "Lvl " + PlayerInfo.instance.level.ToString();
        currency.text = "Vinyl: " + PlayerInfo.instance.currency.ToString();

    }

}
