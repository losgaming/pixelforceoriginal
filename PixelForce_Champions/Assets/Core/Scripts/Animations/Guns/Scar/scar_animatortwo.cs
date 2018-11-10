using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scar_animatortwo : MonoBehaviour {


    public Animator animtwo;
    public CPMPlayer cPMPlayer;
    public float ShootInterrupt = 0;
    public bool canShift = true;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {




        //Shoot interrupts sprint
        if(Input.GetMouseButton(0))
        {


            animtwo.SetBool("IsSprint", false);
            canShift = false;
            cPMPlayer.moveSpeed = 7;

        }


        //Not shooting
        else
        {



            animtwo.SetBool("IsSprint", false);
            canShift = true;




        }



        if (Input.GetKey(KeyCode.LeftShift) && canShift == true)
        {

            cPMPlayer.moveSpeed = 8.4f;
            ShootInterrupt = 1;
            animtwo.SetBool("IsSprint", true);


        }

        else
        {
            cPMPlayer.moveSpeed = 7;
            ShootInterrupt = 0;
            animtwo.SetBool("IsSprint", false);


        }





        //Check if we are moving to update animator.

        if (cPMPlayer.isMoving == false)
        {


            animtwo.SetBool("IsWalking", false);

        }


        if (cPMPlayer.isMoving == true)
        {


            animtwo.SetBool("IsWalking", true);

        }


    }
}
