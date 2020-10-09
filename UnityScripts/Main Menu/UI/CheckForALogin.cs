using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForALogin : MonoBehaviour
{

    public GameObject loggingIn;
    public AlertController ac;
    public CosmeticHelper cosmeticHelper;

    [HideInInspector]
    public bool activated;

    private MenuNavigation mN;


    // Start is called before the first frame update
    void Start()
    {
        mN = GetComponent<MenuNavigation>();
    }

    // Update is called once per frame
    void Update()
    {

        if(PlayerInfo.instance.loggedIn == true && activated == false){

            activated = true;

            // Let the user know the login was successful 
                string message = "Welcome, " + PlayerInfo.instance.userName + "!";
                ac.CreateAlert("Login Succeeded", message);

                // Move them to the logged in page
                mN.SelectPage(mN.pages[3]);

                cosmeticHelper.ShowCosmeticsLoginScreen(PlayerInfo.instance.ID);

        }
        
    }
}
