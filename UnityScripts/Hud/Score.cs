using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    private StatsHelper statsHelper;
    public Text scroreText;


    void Start()
    {

        statsHelper = GetComponent<StatsHelper>();
        
        int playerID;
        if(PlayerInfo.instance.ID != 0){
            playerID = (PlayerInfo.instance.ID);
        }
        else{
            playerID = 7;
        }

        // 0 in the third variable means nothing
        statsHelper.Stats("read", playerID, 0);

    }

    public void UpdateScore(int playerScore)
    {
        scroreText.text = "Vinyl Collected: " + playerScore;
    }
}
