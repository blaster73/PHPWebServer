using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavigation : MonoBehaviour
{

    public GameObject[] pages;
    
    public void SelectPage(GameObject page){

        foreach(GameObject p in pages){
            if(p != page){
                p.SetActive(false);
            }
            else{
                p.SetActive(true);
            }
        }

    }

}
