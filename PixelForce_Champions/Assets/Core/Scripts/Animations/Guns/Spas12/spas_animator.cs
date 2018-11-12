using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spas_animator : MonoBehaviour
{




    public Animator animone;
    public CPMPlayer cPMPlayer;
    public float ShootInterrupt = 0;
    public bool canShift = true;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {



        //Makes sure you can only sprint when moving
        if (cPMPlayer.isMoving == false)
        {


            canShift = false;

        }

        else
        {

            canShift = true;

        }




        //ADS interrupts sprint
        if (Input.GetMouseButton(1))
        {


            animone.SetBool("IsSprint", false);
            animone.SetBool("IsADS", true);
            canShift = false;

        }


        //Is no longer ADS
        if (Input.GetMouseButtonUp(1))
        {

            animone.SetBool("IsADS", false);
            canShift = true;

        }



        //Shoot interrupts sprint
        if (Input.GetMouseButton(0))
        {


            animone.SetBool("IsSprint", false);
            canShift = false;
            cPMPlayer.moveSpeed = 7;

        }



        //Not shooting.
        if (Input.GetMouseButtonUp(0))
        {


            animone.SetBool("IsSprint", false);
            canShift = true;


        }



        if (Input.GetKey(KeyCode.LeftShift) && canShift == true)
        {


            cPMPlayer.moveSpeed = 8.4f;
            ShootInterrupt = 1;
            animone.SetBool("IsSprint", true);


        }

        else
        {



            cPMPlayer.moveSpeed = 7;
            ShootInterrupt = 0;
            animone.SetBool("IsSprint", false);


        }






        //Check if we are moving to update animator.

        if (cPMPlayer.isMoving == false)
        {


            animone.SetBool("IsWalking", false);

        }




        if (cPMPlayer.isMoving == true)
        {


            animone.SetBool("IsWalking", true);

        }

    }
}
