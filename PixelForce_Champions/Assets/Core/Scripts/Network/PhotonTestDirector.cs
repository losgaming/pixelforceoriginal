using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonTestDirector : MonoBehaviour {



    public Transform SpawnPointTest;


	// Use this for initialization
	void Start () {



        PhotonNetwork.ConnectUsingSettings("0.1");
		
	}
	


    void OnConnectedToMaster ()
    {

        Debug.Log("You have connected to test master");
        PhotonNetwork.JoinOrCreateRoom("TestRoom", null, null);

    }



    void OnJoinedRoom ()
    {

        Debug.Log("You have joined the test room");
        PhotonNetwork.Instantiate("PlayerObject", SpawnPointTest.position, SpawnPointTest.rotation, 0);

    }



	// Update is called once per frame
	void Update () {
		
	}




}
