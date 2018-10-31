using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_shoot : MonoBehaviour {

    public Camera cam;
    public float Health = 90f;
    public AudioSource DeathSound;
    public GameObject BloodScreen;
    public Transform Scar_MFSpawnPoint;
    public float fireRate = 5f;
    private float nextTimeToFire = 0f;
    public CPMPlayer cPMPlayer;
    public AudioSource audioSourceShoot;
   



    void Start()
    {

    }




    [PunRPC]
    void TakeDamage()
    {
        Health -= 10f;

    }



    public void Shoot ()
    {


        audioSourceShoot.Play();
        cPMPlayer.rotX += Random.Range(-2, -3);
        cPMPlayer.rotY += Random.Range(-2f, 2);
        PhotonNetwork.Instantiate("MuzzleFlash1 (1)", Scar_MFSpawnPoint.position, Scar_MFSpawnPoint.rotation, 0);
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            if (hit.collider.tag == "Enemy")
            {


                hit.collider.GetComponent<PhotonView>().RPC("TakeDamage", PhotonTargets.AllBuffered);
                Debug.Log("You have hit an enemy");
                print("I'm looking at " + hit.transform.name);

            }


            else
            {

                return;
            }


    }


    void Update()
    {


        //Do something after death.

        if (Health <0)
        {

            Debug.Log("You have died");
            DeathSound.enabled = true;
            BloodScreen.SetActive(true);
        }




        //Shoot Weapon
        if (Input.GetMouseButton (0) && Time.time >= nextTimeToFire)
        {


            nextTimeToFire = Time.time + 0.04f / fireRate;
            Shoot();

        }
    }
}
