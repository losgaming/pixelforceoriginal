using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_health : MonoBehaviour {



    public float plhealth  = 90f;




	// Use this for initialization
	void Start () {
		
	}



    //pun rpc 
    [PunRPC]
    private void TakeDamage()
    {
        plhealth -= 5f;
        Debug.Log(plhealth);


       



    }

    // Update is called once per frame
    void Update () {


		
	}
}
