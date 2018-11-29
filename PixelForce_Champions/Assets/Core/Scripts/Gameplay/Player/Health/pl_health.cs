using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class pl_health : MonoBehaviour {


    //Main player health variable. 
    public float plhealth  = 100f;
    public Image healthbar;
    public Text healthtext;


    public GameObject currentplayer;







    void Update () {


        healthbar.fillAmount = plhealth / 100f;
        healthtext.text = "" + plhealth;


        //What happens after your player dies.
        if (plhealth < 0)
        {


            currentplayer.transform.localPosition = new Vector3(0, 0, 15);
            plhealth = 100f;
            Debug.Log("Player character has died.");



        }

        if (plhealth == 0)
        {
            currentplayer.transform.localPosition = new Vector3(0, 0, 15);
            plhealth = 100f;
            Debug.Log("Player character has died.");


        }




    }
}
