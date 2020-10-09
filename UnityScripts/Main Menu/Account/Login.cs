using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{

    [HideInInspector]
    public bool success = false;
    [HideInInspector]
    public bool done = false;
    private string baseURL = "http://fth-media-031/aeondemo/Login.php/?";

    [ContextMenu("Test")]
    public void LoginButton(string username, string password, AlertController ac) {
        StartCoroutine(LoginRoutine(username, password, ac));
    }

    private IEnumerator LoginRoutine(string username, string password, AlertController ac){

        string escapedUsername = UnityWebRequest.EscapeURL(username);
        string escapedpassword = UnityWebRequest.EscapeURL(password);
        string phpURL = baseURL + "username=" + escapedUsername + "&password=" + escapedpassword;

        UnityWebRequest uwrLogin = UnityWebRequest.Get(phpURL);
        yield return uwrLogin.SendWebRequest();


        if(uwrLogin.error != null){
            Debug.Log("There was an error with UnityWebRequest");
            ac.CreateAlert("Login Failed", "Could not connect to server");
        }
        else if (uwrLogin.downloadHandler.text == "MySQL failed to execute"){
            ac.CreateAlert("Account Creation Failed", "Could not connect to server");
        }
        else if(uwrLogin.downloadHandler.text == "Email or password incorrect"){
            ac.CreateAlert("Login Failed", "Email or password incorrect");
        }

        // Assign the information to the player after a succesful Debug.LogWarning
        string[] items = uwrLogin.downloadHandler.text.Split(' ');  // Format: "username email ID"

        // Assign to the PlayerInfo script
        PlayerInfo playerInfo = GameObject.Find("Player Information").GetComponent<PlayerInfo>();
        playerInfo.userName = items[0];
        playerInfo.ID = int.Parse(items[2]);
        playerInfo.currency = int.Parse(items[5]);
        playerInfo.level = int.Parse(items[6]);
        

        while(true){

            if(uwrLogin.isDone){
                
                // check if the login was successful
                if(items[3] == "Successful"){
                    playerInfo.loggedIn = true;
                    success = true;
                }
                else{
                    success = false;
                    playerInfo.loggedIn = false;
                }

                done = true;
                Destroy(this);
                break;

            }
        }

    }
}
