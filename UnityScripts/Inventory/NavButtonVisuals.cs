using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavButtonVisuals : MonoBehaviour
{

    public Image[] images;

    public void ButtonSelector(Image image){
        foreach(Image i in images){
            // Highlight selected image
            if(i == image){
                Color tempColor = i.color;
                tempColor.a = 1f;
                i.color = tempColor;
            }
            // Hide all other images
            else{
                Color tempColor = i.color;
                tempColor.a = 0f;
                i.color = tempColor;
            }
        }
    }

    public void SelectInventoryButton(){
        ButtonSelector(images[0]);
    }
}
