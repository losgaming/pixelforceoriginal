using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pl_shoot : MonoBehaviour {

    public Camera cam;
    public float Health = 90f;
    public AudioSource DeathSound;
    public float fireRate = 0.3f;
    public GameObject BloodScreen;
    public PlayerMotorBehavior playerMotor;



    void Start()
    {

    }




    [PunRPC]
    void TakeDamage()
    {
        Health -= 10f;

    }






    void Update()
    {


        //Do something after death.

        if (Health <0)
        {

            Debug.Log("You have died");
            DeathSound.enabled = true;
            BloodScreen.SetActive(true);
            playerMotor.canMove = false;
        }






        //Shoot Weapon
        if (Input.GetMouseButtonDown (0))
        {


            Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
                if (hit.collider.tag == "Enemy")
                {


                    hit.collider.GetComponent<PhotonView>().RPC ("TakeDamage", PhotonTargets.AllBuffered);
                    Debug.Log("You have hit an enemy");
                    print("I'm looking at " + hit.transform.name);

                }


            else
                {

                    return;
                }

        }
    }
}
