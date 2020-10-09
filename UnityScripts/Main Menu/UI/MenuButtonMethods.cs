using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonMethods : MonoBehaviour
{

    [HideInInspector]
    public AlertController ac;
    private QRCreator qR;

    [Header("Create Account Variables")]
    public InputField emailInput;
    public InputField usernameInput;
    public InputField passwordInput;

    [Header("Login Variables")]
    public InputField loginUsernameInput;
    public InputField loginPasswordInput;

    private void Start() {
        ac = GameObject.Find("Alert Holder").GetComponent<AlertController>();
    }

    public void CreateAccountButton(){

        Debug.Log("Attempting to create an account");
        CreateAccount createAccount = gameObject.AddComponent<CreateAccount>();
        createAccount.CreateAccountButton(emailInput.text, usernameInput.text, passwordInput.text, ac);

    }

    public void Login(){

        Debug.Log("Attempting to log in");
        Login login = gameObject.AddComponent<Login>();
        login.LoginButton(loginUsernameInput.text, loginPasswordInput.text, ac);

    }

    public void DebugLogin(){
        Debug.Log("Attempting to login to debug account");
        Login login = gameObject.AddComponent<Login>();
        login.LoginButton("test", "test", ac);
    }

    public void Play(){
        SceneChanger sceneChanger = GameObject.Find("Scene Changer").GetComponent<SceneChanger>();
        sceneChanger.ChangeSceneTo("CityHub Integration");
    }

    public void SignOut(){
        PlayerInfo playerInfo = GameObject.Find("Player Information").GetComponent<PlayerInfo>();
        playerInfo.Logout();
        GetComponent<CheckForALogin>().activated = false;
    }

}
