using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_health : MonoBehaviour {


    //Main player health variable. 
    public float plhealth  = 100f;

    //Scar damage
    [PunRPC]
    private void ScarTakeDamage()
    {

        plhealth -= 5;
        Debug.Log(plhealth);

    }

    //Spas12 damage
    [PunRPC]
    private void SPAS12TakeDamage()
    {

        plhealth -= 11;
        Debug.Log(plhealth);

    }

    void Update () {




        //What happens after your player dies.
        if (plhealth < 0)
        {


            Debug.Log("Player character has died.");


        }

        if (plhealth == 0)
        {


            Debug.Log("Player character has died.");


        }




    }
}
