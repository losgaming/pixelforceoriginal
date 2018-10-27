using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPLShootDirector : Photon.MonoBehaviour {




    public MonoBehaviour PLShoot;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		



        if (photonView.isMine == true)
        {

            PLShoot.enabled = true;

        }

        else
        {

            PLShoot.enabled = false;

        }


	}
}
