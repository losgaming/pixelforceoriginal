using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Switches your weapons in the game.
//Make sure you place this in the root of your player prefab. It is universal.


public class SwitchWeapons : MonoBehaviour {





    //Visual weapon.
    //FN SCAR
    public Renderer scarrend;
    public Renderer scarrend2;
    public Renderer scarrend3;

    //SPAS 12
    public Renderer spasrend;
    public Renderer spasrend2;
    public Renderer spasrend3;


    //Mauler
    public Renderer maulerrend;
    public Renderer maulerrend2;
    public Renderer maulerrend3;


    //Actual weapon. (Script)
    public GameObject ScarShoot;
    public GameObject SpasShoot;
    public GameObject MaulerShoot;


	// Use this for initialization
	void Start () {



        //FN SCAR
        scarrend.enabled = false;
        scarrend2.enabled = false;
        scarrend3.enabled = false;

        //SPAS12
        spasrend.enabled = false;
        spasrend2.enabled = false;
        spasrend3.enabled = false;

        //Mauler
        maulerrend.enabled = false;
        maulerrend2.enabled = false;
        maulerrend3.enabled = false;



	}
	
	// Update is called once per frame
	void Update () {




        //Sets scar active. Make sure everything else gets set to false.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            //Visual
            //FNSCAR
            scarrend.enabled = true;
            scarrend2.enabled = true;
            scarrend3.enabled = true;
            //OTHERS
            spasrend.enabled = false;
            spasrend2.enabled = false;
            spasrend3.enabled = false;
            maulerrend.enabled = false;
            maulerrend2.enabled = false;
            maulerrend3.enabled = false;

            //Script
            ScarShoot.SetActive(true);
            //False
            SpasShoot.SetActive(false);
            MaulerShoot.SetActive(false);
        }


        //Sets spas-12 active. Make sure everything else gets set to false.
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            //Visual
            //SPAS12
            spasrend.enabled = true;
            spasrend2.enabled = true;
            spasrend3.enabled = true;
            //OTHERS
            scarrend.enabled = false;
            scarrend2.enabled = false;
            scarrend3.enabled = false;
            maulerrend.enabled = false;
            maulerrend2.enabled = false;
            maulerrend3.enabled = false;

            //Script
            SpasShoot.SetActive(true);
            //False
            ScarShoot.SetActive(false);
            MaulerShoot.SetActive(false);



        }


        //Sets mauler active. Make sure everything else gets set to false.
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {


            //Visual
            //Mauler
            maulerrend.enabled = true;
            maulerrend2.enabled = true;
            maulerrend3.enabled = true;
            //OTHERS
            scarrend.enabled = false;
            scarrend2.enabled = false;
            scarrend3.enabled = false;
            spasrend.enabled = false;
            spasrend2.enabled = false;
            spasrend3.enabled = false;

            //Script
            MaulerShoot.SetActive(true);
            //False
            ScarShoot.SetActive(false);
            SpasShoot.SetActive(false);


        }






    }
}
