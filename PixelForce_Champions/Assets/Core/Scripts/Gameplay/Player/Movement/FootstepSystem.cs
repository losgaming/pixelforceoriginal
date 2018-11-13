using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSystem : MonoBehaviour {




    public CPMPlayer cPMPlayer;





    //Footstep rate.
    public float FSRate = 5f;
    public float nextTimeToFS = 0f;
    public float RateOfAction = 2f;


    //Walking on dirt.
    public AudioSource dirt1;
    public AudioSource dirt2;
    public AudioSource dirt3;
    public AudioSource dirt4;
    public AudioSource dirt5;
    public int dirtplayrand = 0;

    //Walking weapon rattle
    public AudioSource rattle1;
    public AudioSource rattle2;
    public AudioSource rattle3;
    public AudioSource rattle4;
    public AudioSource rattel5;
    public AudioSource rattle6;
    public AudioSource rattle7;
    public AudioSource rattle8;
    public int rattleplayrand = 0;



    //Reference scripts.
    public RunDetection runDetection;




    // Use this for initialization
    void Start () {
		
	}



    public void WeaponRattle ()
    {


        rattleplayrand = Random.Range(1, 8);



        if (rattleplayrand == 1)
        {

            rattle1.volume = Random.Range(0.1f, 0.2f);
            rattle1.pitch = Random.Range(0.8f, 1);
            rattle1.Play();

        }


        if (rattleplayrand == 2)
        {

            rattle2.volume = Random.Range(0.1f, 0.2f);
            rattle2.pitch = Random.Range(0.8f, 1);
            rattle2.Play();

        }

        if (rattleplayrand == 3)
        {

            rattle3.volume = Random.Range(0.2f, 0.3f);
            rattle3.pitch = Random.Range(0.8f, 1);
            rattle3.Play();

        }

        if (rattleplayrand == 4)
        {

            rattle4.volume = Random.Range(0.2f, 0.3f);
            rattle4.pitch = Random.Range(0.8f, 1);
            rattle4.Play();

        }

        if (rattleplayrand == 5)
        {

            rattel5.volume = Random.Range(0.2f, 0.3f);
            rattel5.pitch = Random.Range(0.8f, 1);
            rattel5.Play();

        }

        if (rattleplayrand == 6)
        {

            rattle6.volume = Random.Range(0.2f, 0.3f);
            rattle6.pitch = Random.Range(0.8f, 1);
            rattle6.Play();

        }

        if (rattleplayrand == 7)
        {

            rattle7.volume = Random.Range(0.2f, 0.3f);
            rattle7.pitch = Random.Range(0.8f, 1);
            rattle7.Play();

        }

        if (rattleplayrand == 8)
        {

            rattle8.volume = Random.Range(0.2f, 0.3f);
            rattle8.pitch = Random.Range(0.8f, 1);
            rattle8.Play();

        }


    }


    public void FootStep ()
    {



        dirtplayrand = Random.Range(1, 5);



        if (dirtplayrand == 1)
        {

            dirt1.volume = Random.Range(0.1f, 0.2f);
            dirt1.pitch = Random.Range(0.8f, 1);
            dirt1.Play();

        }

        if (dirtplayrand == 2)
        {
            dirt2.volume = Random.Range(0.1f, 0.2f);
            dirt2.pitch = Random.Range(0.8f, 1);
            dirt2.Play();

        }


        if (dirtplayrand == 3)
        {
            dirt3.volume = Random.Range(0.1f, 0.2f);
            dirt3.pitch = Random.Range(0.8f, 1);
            dirt3.Play();

        }

        if (dirtplayrand == 4)
        {
            dirt4.volume = Random.Range(0.1f, 0.2f);
            dirt4.pitch = Random.Range(0.8f, 1);
            dirt4.Play();

        }

        if (dirtplayrand == 5)
        {
            dirt5.volume = Random.Range(0.1f, 0.2f);
            dirt5.pitch = Random.Range(0.8f, 1);
            dirt5.Play();

        }

    }




	
	// Update is called once per frame
	void Update () {



        //If players runs footsteps get played at a faster rate.
        if (runDetection.isRunning == true)
        {


            RateOfAction = 1.65f;


        }

        //If players runs footsteps get played at a slower rate.
        if (runDetection.isRunning == false)
        {


            RateOfAction = 2.1f;


        }



        if (cPMPlayer.isMoving && Time.time >= nextTimeToFS)
        {



            nextTimeToFS = Time.time + RateOfAction / FSRate;
            FootStep();
            WeaponRattle();

        }



		
	}
}
