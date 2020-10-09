using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertController : MonoBehaviour
{

    public GameObject alert;
    public Text alertTitle;
    public Text alertBody;

    private void Start() {
        // Make sure the alert is off at the start of the game
        alert.SetActive(false);
    }

    public void CreateAlert(string title, string body){

        alertTitle.text = title;
        alertBody.text = body;
        alert.SetActive(true);

    }

    public void CloseAlert(){

        alert.SetActive(false);
        
    }
}
