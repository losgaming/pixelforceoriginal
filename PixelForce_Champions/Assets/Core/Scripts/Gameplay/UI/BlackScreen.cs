using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreen : MonoBehaviour {


    public GameObject black;

	// Use this for initialization
	void Start () {




        Invoke("BlackTheScreen", 10);
		
	}





    void BlackTheScreen()
    {

        black.SetActive(true);

    }



	
	// Update is called once per frame
	void Update () {
		
	}
}
