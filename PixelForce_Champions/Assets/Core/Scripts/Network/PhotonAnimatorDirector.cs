using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonAnimatorDirector : Photon.MonoBehaviour {


    public Animator[] animators;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        //Enables animator on our own photon view.
        if(photonView.isMine)
        {

            foreach (Animator m in animators)
            {

                m.enabled = true;

            }

        }


        //Disables aniamtor on other clients that do not have our own photon view.
        else
        {
            foreach (Animator m in animators)
            {

                m.enabled = false;

            }


        }
		
	}
}
