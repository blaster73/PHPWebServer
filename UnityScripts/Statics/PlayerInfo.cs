using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

    public static PlayerInfo instance = null;

    public int ID;
    public string userName;
    public int currency;
    public int level;
    public bool loggedIn;

    void Awake(){

        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);


        
    }

    public void Logout(){
        ID = 0;
        userName = "";
        currency = 0;
        level = 0;
        loggedIn = false;
    }

}
