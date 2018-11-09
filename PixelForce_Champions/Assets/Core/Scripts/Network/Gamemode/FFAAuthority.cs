using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFAAuthority : MonoBehaviour {

    public PhotonSceneDirector photonSceneDirector;

    public bool isAuthActiveAuth = false;

    public bool ButtonPressed = false;

	// Use this for initialization
	void Start () {
		
	}




    //Makes sure to change the gamemode after transfering scene. 
    private void Update()
    {
        

        if (isAuthActiveAuth == true)
        {

            photonSceneDirector.FFASceneTransfer = true;

        }
    }


    //Activates FFA with UI button (server select).
    public void OnPressed ()
    {

        photonSceneDirector.FFASceneTransferConfirmation = true;

    }


    //Calls the update function to do its thing.
    private void OnJoinedRoom()
    {

        ButtonPressed = true;

        if (ButtonPressed == true)
        {


            isAuthActiveAuth = true;

        }


    }
}
