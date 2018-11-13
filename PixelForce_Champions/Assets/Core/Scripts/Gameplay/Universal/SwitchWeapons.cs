using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Switches your weapons in the game.
//Make sure you place this in the root of your player prefab. It is universal.


public class SwitchWeapons : MonoBehaviour {



    //Visual weapon.
    //FN SCAR
    public GameObject ScarRender;
    public GameObject ScarArmOne;
    public GameObject ScarArmTwo;

    //SPAS 12
    public GameObject SpasRender;
    public GameObject SpasArmOne;
    public GameObject SpasArmTwo;


    //Actual weapon. (Script)
    public GameObject ScarShoot;
    public GameObject SpasShoot;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        //Sets spas12 active. Make sure everything else gets set to false.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            //Visual




            //Script


        }


        //Sets scar active. Make sure everything else gets set to false.
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {

            //Visual


            //Script


        }

		
	}
}
