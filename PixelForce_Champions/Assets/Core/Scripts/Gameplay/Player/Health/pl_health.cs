using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_health : MonoBehaviour {



    public float plhealth  = 90f;
    public AudioSource hurt1fem;
    public AudioSource hurt2fem;
    public AudioSource hurt3fem;
    public AudioSource hurt4fem;
    public AudioSource hurt5fem;
    public AudioSource hurt6fem;
    public int hurtfem = 0;


	// Use this for initialization
	void Start () {
		
	}



    //pun rpc 
    [PunRPC]
    private void TakeDamage()
    {
        plhealth -= 5f;
        Debug.Log(plhealth);


        hurtfem = Random.Range(1, 6);


        if (hurtfem == 1)
        {


            hurt1fem.volume = Random.Range(0.05f, 0.08f);
            hurt1fem.pitch = Random.Range(0.95f, 1.0f);
            hurt1fem.Play(); 



        }

        if (hurtfem == 2)
        {
            hurt2fem.volume = Random.Range(0.05f, 0.08f);
            hurt2fem.pitch = Random.Range(0.95f, 1.0f);
            hurt2fem.Play();



        }

        if (hurtfem == 3)
        {
            hurt3fem.volume = Random.Range(0.05f, 0.08f);
            hurt3fem.pitch = Random.Range(0.95f, 1.0f);
            hurt3fem.Play();



        }

        if (hurtfem == 4)
        {
            hurt4fem.volume = Random.Range(0.05f, 0.08f);
            hurt4fem.pitch = Random.Range(0.95f, 1.0f);
            hurt4fem.Play();



        }

        if (hurtfem == 5)
        {
            hurt5fem.volume = Random.Range(0.05f, 0.08f);
            hurt5fem.pitch = Random.Range(0.95f, 1.0f);
            hurt5fem.Play();



        }

        if (hurtfem == 6)
        {
            hurt6fem.volume = Random.Range(0.05f, 0.08f);
            hurt6fem.pitch = Random.Range(0.95f, 1.0f);
            hurt6fem.Play();



        }



    }

    // Update is called once per frame
    void Update () {


		
	}
}
