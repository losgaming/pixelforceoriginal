using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_shoot : MonoBehaviour {

    public Camera cam;
    public float Health = 90f;
    public GameObject BloodScreen;
    public Transform Scar_MFSpawnPoint;
    public float fireRate = 5f;
    private float nextTimeToFire = 0f;
    public CPMPlayer cPMPlayer;
    public AudioSource audioSourceShoot;
    public AudioSource audioSourceShoot1;
    public AudioSource audioSourceShoot2;
    public Animator animone;
    public Animator animtwo;
    public GameObject ch1;
    public GameObject ch2;
    public GameObject ch3;
    public GameObject ch4;
    public float scarSpread = 0.01f;
    public bool isADS = false;
    public AudioSource m1;
    public AudioSource m2;
    public AudioSource m3;
    public int audioran = 0;
    public int audioranm = 0;






    void Start()
    {


        //sets crosshair to normal position when started
        ch1.GetComponent<RectTransform>().localPosition = new Vector3(0, 30, 0);
        ch2.GetComponent<RectTransform>().localPosition = new Vector3(0, -30, 0);
        ch3.GetComponent<RectTransform>().localPosition = new Vector3(30, 0, 0);
        ch4.GetComponent<RectTransform>().localPosition = new Vector3(-30, 0, 0);
        scarSpread = 0.01f;

    }




    [PunRPC]
    void TakeDamage()
    {
        Health -= 10f;

    }




    public void AnimSetFalse ()
    {

        animone.SetBool("IsShoot", false);
        animtwo.SetBool("IsShoot", false);


    }


    //Cross hair goes back to normal position after I stop shooting.
    public void CHNormal ()
    {


        ch1.GetComponent<RectTransform>().localPosition = new Vector3(0, 30, 0);
        ch2.GetComponent<RectTransform>().localPosition = new Vector3(0, -30, 0);
        ch3.GetComponent<RectTransform>().localPosition = new Vector3(30, 0, 0);
        ch4.GetComponent<RectTransform>().localPosition = new Vector3(-30, 0, 0);

        scarSpread = 0.01f;

    }








    public void Shoot ()
    {

        //Crosshair spread the more I keep shooting.
        ch1.GetComponent<RectTransform>().localPosition += new Vector3(0, scarSpread) * 256;
        ch2.GetComponent<RectTransform>().localPosition += new Vector3(0, -scarSpread) * 256;
        ch3.GetComponent<RectTransform>().localPosition += new Vector3(scarSpread, 0) * 256;
        ch4.GetComponent<RectTransform>().localPosition += new Vector3(-scarSpread, 0) * 256;

        animone.SetBool("IsShoot", true);
        animtwo.SetBool("IsShoot", true);


        Invoke("AnimSetFalse", 0.05f);


        audioran = Random.Range(1, 3);



        if (audioran == 1)
        {
            audioSourceShoot.pitch = (Random.Range(0.8f, 1.0f));
            audioSourceShoot.Play();

        }

        if (audioran == 2 )
        {

            audioSourceShoot1.pitch = (Random.Range(0.8f, 1.0f));
            audioSourceShoot1.Play();

        }


        if (audioran == 3 )
        {

            audioSourceShoot2.pitch = (Random.Range(0.8f, 1.0f));
            audioSourceShoot2.Play();


        }

        //recoil


        if (isADS == false)
        {

            cPMPlayer.rotX += Random.Range(-2.6f, -3);
            cPMPlayer.rotY += Random.Range(-2f, 1.7f);

        }



        if (isADS == true)
        {


            cPMPlayer.rotX += Random.Range(-1, -2.4f);
            cPMPlayer.rotY += Random.Range(-1f, 1);

        }



        //increase spread of scar every shot

        if (isADS == false)
        {

            scarSpread += 0.00083f;

        }


        if (isADS == true)
        {


            scarSpread += 0.00041f;

        }

        PhotonNetwork.Instantiate("MuzzleFlash", Scar_MFSpawnPoint.position, Scar_MFSpawnPoint.rotation, 0);
        Ray ray = cam.ViewportPointToRay(new Vector3(0, 0, 0));
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward + Random.insideUnitSphere * scarSpread, out hit))

        {

            Debug.Log(hit.transform.name);
            PhotonNetwork.Instantiate("ScanLocation", hit.point, Quaternion.identity, 0);
        }







        if (hit.collider.tag == "Enemy")
        {

            hit.collider.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllBuffered);
            Debug.Log("You have hit an enemy");
            print("I'm looking at " + hit.transform.name);
            PhotonNetwork.Instantiate("ScanLocation", hit.normal, Quaternion.identity, 0);

        }

        else
        {

            return;

        }





    }


    void Update()
    {



        //If you are holding down aim
        if (Input.GetMouseButtonDown(1))
        {



            animtwo.SetBool("IsADS", true);
            animone.SetBool("IsADS", true);
            scarSpread = 0.01f;
            isADS = true;
            cPMPlayer.moveSpeed = 5;
            cPMPlayer.runDeacceleration = 2.5f;
            cPMPlayer.runAcceleration = 4;


            audioranm = Random.Range(1, 3);


            if (audioranm == 1)
            {


                m1.pitch = Random.Range(0.8f, 1);
                m1.Play();
            }


            if (audioranm == 2)
            {


                m2.pitch = Random.Range(0.8f, 1);
                m2.Play();
            }



            if (audioranm == 3)
            {


                m3.pitch = Random.Range(0.8f, 1);
                m3.Play();
                

            }


        }


        //If you let go of aim
        if (Input.GetMouseButtonUp(1))
        {



            animtwo.SetBool("IsADS", false);
            animone.SetBool("IsADS", false);
            scarSpread = 0.01f;
            isADS = false;
            cPMPlayer.moveSpeed = 7;
            cPMPlayer.runDeacceleration = 5;
            cPMPlayer.runAcceleration = 6;


            if (audioranm == 1)
            {


                m1.Stop();
            }


            if (audioranm == 2)
            {


                m2.Stop();
            }



            if (audioranm == 3)
            {


                m3.Stop();


            }
        }





        //Do something after death.

        if (Health <0)
        {

            Debug.Log("You have died");
            BloodScreen.SetActive(true);
        }





        //Shoot weapon let go
        if (Input.GetMouseButtonUp(0))
        {



            CHNormal();

        }


        //Shoot Weapon
        if (Input.GetMouseButton (0) && Time.time >= nextTimeToFire)
        {


            nextTimeToFire = Time.time + 0.035f / fireRate;
            Shoot();

        }
    }
}
