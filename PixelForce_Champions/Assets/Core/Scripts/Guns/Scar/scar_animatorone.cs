using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scar_animatorone : MonoBehaviour {




    public Animator animone;
    public CPMPlayer cPMPlayer;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        //Check if we are moving to update animator.

        if (cPMPlayer.testbool == false)
        {


            animone.SetBool("IsWalking", false);

        }




        if (cPMPlayer.testbool == true)
        {


            animone.SetBool("IsWalking", true);

        }

    }
}
