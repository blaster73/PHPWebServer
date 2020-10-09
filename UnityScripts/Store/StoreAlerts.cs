using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreAlerts : MonoBehaviour
{

    public GameObject alert;
    public Text messageText;

    public void CreateAlertBox(){
        alert.SetActive(false);
    }

    public void CloseAlertBox(){
        alert.SetActive(false);
    }
}
