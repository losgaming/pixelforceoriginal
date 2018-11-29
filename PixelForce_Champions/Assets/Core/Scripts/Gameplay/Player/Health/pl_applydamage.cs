using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pl_applydamage : MonoBehaviour {


    //Main player health variable. 
    public float plhealth = 100f;
    public pl_health pl_Health;



    // Use this for initialization
    void Start () {
		
	}





    //Scar damage
    [PunRPC]
    private void ScarTakeDamage()
    {

        pl_Health.plhealth -= 12f;
        Debug.Log(pl_Health.plhealth);


        Debug.Log("You have hit");

    }

    //Spas12 damage
    [PunRPC]
    private void SPAS12TakeDamage()
    {

        pl_Health.plhealth -= 12f;
        Debug.Log(pl_Health.plhealth);

        Debug.Log("You have hit");

    }


    // Update is called once per frame
    void Update () {






        




		
	}
}
