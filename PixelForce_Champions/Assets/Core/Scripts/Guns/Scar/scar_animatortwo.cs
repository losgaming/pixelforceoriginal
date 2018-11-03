using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scar_animatortwo : MonoBehaviour {


    public Animator animtwo;
    public CPMPlayer cPMPlayer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {



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
