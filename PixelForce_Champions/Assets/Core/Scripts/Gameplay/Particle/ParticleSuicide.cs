using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSuicide : MonoBehaviour {



    public GameObject particleself;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {




        Destroy(particleself, 2);

		
	}
}
