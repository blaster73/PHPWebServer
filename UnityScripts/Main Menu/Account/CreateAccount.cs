using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CreateAccount : MonoBehaviour
{

    [HideInInspector]
    public bool done = false;
    private string baseURL = "http://fth-media-031/aeondemo/CreateAccount.php/?";

    [ContextMenu("Test")]
    public void CreateAccountButton(string email, string username, string password, AlertController ac) {
        StartCoroutine(CARoutine(email, username, password, ac));
    }

    private IEnumerator CARoutine(string email, string username, string password, AlertController ac){

        string escapedEmail = UnityWebRequest.EscapeURL(email);
        string escapedUsername = UnityWebRequest.EscapeURL(username);
        string escapedpassword = UnityWebRequest.EscapeURL(password);
        string phpURL = baseURL + "email=" + escapedEmail + "&username=" + escapedUsername + "&password=" + escapedpassword;

        UnityWebRequest uwrCreateAccount = UnityWebRequest.Get(phpURL);
        yield return uwrCreateAccount.SendWebRequest();


        if(uwrCreateAccount.error != null){
            Debug.Log("There was an error with UnityWebRequest");
            ac.CreateAlert("Account Creation Failed", "Could not connect to server");
        }
        else if(uwrCreateAccount.downloadHandler.text == "Invalid email address"){
            ac.CreateAlert("Account Creation Failed", "Email address is invalid!");
        }
        else if(uwrCreateAccount.downloadHandler.text == "Email already exists"){
            ac.CreateAlert("Account Creation Failed", "Email in use already!");
        }
        else if(uwrCreateAccount.downloadHandler.text == "Username already exists"){
            ac.CreateAlert("Account Creation Failed", "Username in use already!");
        }
        else if (uwrCreateAccount.downloadHandler.text == "MySQL failed to execute"){
            ac.CreateAlert("Account Creation Failed", "Could not connect to server");
        }

        while(true){
            if(uwrCreateAccount.isDone){
                done = true;
                //Debug.Log("Done");
                Destroy(this);
                break;
            }
        }

    }

}