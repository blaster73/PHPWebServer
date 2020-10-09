using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoutButton : MonoBehaviour
{
    public void SignOut(){
        PlayerInfo.instance.Logout();
        GetComponent<SceneChanger>().ChangeSceneTo("Main Menu");
    }
}
