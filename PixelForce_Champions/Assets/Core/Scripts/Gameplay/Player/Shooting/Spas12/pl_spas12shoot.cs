using UnityEngine;
using UnityEngine.UI;

public class pl_spas12shoot : MonoBehaviour
{


    //Reference to ammo text in canvas (UI)
    public Text spascurrentammotext;
    public Text spasmaxammotext;

    //pl_health reference script
    public pl_health pl_Health;


    //Scar anims
    public spas_animator scar_Animatorone;


    //Where the raycast come from.
    public Camera cam;



    //Aim sway scipt reference
    public AimSway aimSway;


    //When player dies.
    public GameObject BloodScreen;




    //Scar muzzle flash spawn point.
    public Transform Scar_MFSpawnPoint;



    //Amount of shots (shotguns mostly)
    int amountOfProjectiles = 10;

    //Scar FireRate.
    public float ScarfireRate = 5f;
    private float nextTimeToFire = 0f;


    //Access the movement script.
    public CPMPlayer cPMPlayer;


    //Scar ammo system.
    public float ScarClip = 8;
    public float ScarMags = 24;
    public float ScarClipRecord = 0;
    public bool canReload = true;


    //Scar Shoot Sounds with different pitches
    public AudioSource ScaraudioSourceShoot;
    public AudioSource ScaraudioSourceShoot1;
    public AudioSource ScaraudioSourceShoot2;

    //Scar animators for shoot and walk etc...
    public Animator Scaranimone;

    //This set of crosshair is for staying still spread representation.
    public GameObject Scarch1;
    public GameObject Scarch2;
    public GameObject Scarch3;
    public GameObject Scarch4;


    //Scar spread system.
    public float scarSpread = 0;
    public float scarSpreadCollection = 0;


    //Check if scar is ADS.
    public bool ScarisADS = false;


    //ADS play sounds.
    public AudioSource Scarm1;
    public AudioSource Scarm2;
    public AudioSource Scarm3;


    //Randomize sounds played.
    public int Scaraudioran = 0;
    public int Scaraudioranm = 0;

    //Check if Scar can shoot.
    public bool ScarCanShoot = true;


    //Hit indicator (marker)
    public AudioSource hitmarker;
    public AudioSource hitmarker2;
    public int hitmarkerrand = 0;




    private void Start()
    {

        //sets crosshair to normal position when started
        //Scarch1.GetComponent<RectTransform>().localPosition = new Vector3(0, 30, 0);
        //Scarch2.GetComponent<RectTransform>().localPosition = new Vector3(0, -30, 0);
        //Scarch3.GetComponent<RectTransform>().localPosition = new Vector3(30, 0, 0);
        //Scarch4.GetComponent<RectTransform>().localPosition = new Vector3(-30, 0, 0);

        scarSpread = 0.01f;
        scarSpreadCollection = 0.01f;




        aimSway.maxSwayAngle = 14;
        aimSway.swayAngle = 7;
        aimSway.swaySmooth = 8;


    }




    public void AnimSetFalse()
    {

        Scaranimone.SetBool("IsShootSpas", false);



    }


    //Cross hair goes back to normal position after I stop shooting. (STOP SHOOTING FUNCTION)
    public void CHNormal()
    {




        //sets the spread back to normal when i stop shooting
        scarSpread = 0.01f;
        scarSpreadCollection = 0.01f;


    }








    public void ScarShoot()
    {








        //Everytime we shoot scar spread value goes up. The crosshair expands in the update function.
        scarSpread += Mathf.Lerp(scarSpread, scarSpreadCollection, 32);




        Scaranimone.SetBool("IsShootSpas", true);



        



        //Dont set too high or animation will stay at the end for a long time. This needs to happen quick. Recommended amount (0.1f) on any fire animation.
        Invoke("AnimSetFalse", 0.35f);


        Scaraudioran = Random.Range(1, 3);


        //Random shoot noice with pitch (volume (0.7f, 0.8f)
        if (Scaraudioran == 1)
        {

            ScaraudioSourceShoot.volume = (Random.Range(0.5f, 0.6f));
            ScaraudioSourceShoot.pitch = (Random.Range(0.7f, 1.0f));
            ScaraudioSourceShoot.Play();

        }

        if (Scaraudioran == 2)
        {
            ScaraudioSourceShoot1.volume = (Random.Range(0.5f, 0.6f));
            ScaraudioSourceShoot1.pitch = (Random.Range(0.7f, 1.0f));
            ScaraudioSourceShoot1.Play();

        }


        if (Scaraudioran == 3)
        {
            ScaraudioSourceShoot2.volume = (Random.Range(0.5f, 0.6f));
            ScaraudioSourceShoot2.pitch = (Random.Range(0.7f, 1.0f));
            ScaraudioSourceShoot2.Play();


        }

        //recoil


        if (ScarisADS == false)
        {

            cPMPlayer.rotX += Random.Range(-2, -4f);
            cPMPlayer.rotY += Random.Range(-1.15f, 1.30f);

        }



        if (ScarisADS == true)
        {


            cPMPlayer.rotX += Random.Range(-1f, -2f);
            cPMPlayer.rotY += Random.Range(-0.9f, 0.55f);

        }



        //increase spread of scar every shot

        if (ScarisADS == false)
        {

            scarSpreadCollection += 0.008f;

        }



        if (ScarisADS == true)
        {


            scarSpreadCollection += 0.005f;

        }

        //PhotonNetwork.Instantiate("MuzzleFlash", Scar_MFSpawnPoint.position, Scar_MFSpawnPoint.rotation, 0); If shotgun multiple of these will be used.
        Ray ray = cam.ViewportPointToRay(new Vector3(0, 0, 0));
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward + Random.insideUnitSphere * scarSpread, out hit))

        {


            PhotonNetwork.Instantiate("ScanLocation", hit.point, Quaternion.identity, 0);
        }

        else
        {



            return;
        }


        //If we hit a test object with a tag "Test" do something.
        if (hit.collider.tag == "Debug")
        {


            //to actually have this register you must use an actual collider component rather than just a "Character Controller" so make sure that you don't forget if something is not working. This can be the reason most of the time.


            hit.collider.GetComponent<PhotonView>().RPC("SPAS12TakeDamage", PhotonTargets.AllBuffered);
            //PhotonNetwork.Instantiate("ScanLocation", hit.normal, Quaternion.identity, 0);




            hitmarkerrand = Random.Range(1, 2);


            if (hitmarkerrand == 1)
            {


                hitmarker.pitch = Random.Range(0.8f, 1f);
                hitmarker.Play();


            }




            if (hitmarkerrand == 2)
            {

                hitmarker2.pitch = Random.Range(0.8f, 1f);
                hitmarker2.Play();


            }




        }


        else
        {

            return;

        }


        //If we hit a player with photonview do something. (Enemy player).
        if (hit.collider.tag == "Enemy")
        {

            hit.collider.GetComponent<PhotonView>().RPC("SPAS12TakeDamage", PhotonTargets.AllBuffered);
            PhotonNetwork.Instantiate("ScanLocation", hit.normal, Quaternion.identity, 0);





            hitmarkerrand = Random.Range(1, 2);


            if (hitmarkerrand == 1)
            {


                hitmarker.pitch = Random.Range(0.8f, 1f);
                hitmarker.Play();


            }




            if (hitmarkerrand == 2)
            {

                hitmarker2.pitch = Random.Range(0.8f, 1f);
                hitmarker2.Play();


            }





        }

        else
        {

            return;

        }







    }







    private void Update()
    {




        //Updates the ammo text in (Canvas) for the UI.
        spascurrentammotext.text = "" + ScarClip;
        spasmaxammotext.text = "" + ScarMags;




        //Checks if clip is empty.
        if (ScarClip == 0)
        {

            ScarCanShoot = false;
            scarSpread = 0.01f;
            scarSpreadCollection = 0.01f;


        }

        //Ensure the scar clip record cant exceed 30. 
        if (ScarClipRecord == 8)
        {

            ScarClipRecord = 8;


        }

        //Reload your gun.
        if (Input.GetKeyDown(KeyCode.R) && canReload == true)
        {

            ScarMags -= ScarClipRecord;
            ScarCanShoot = true;
            ScarClip = 8;
            ScarClipRecord = 0;
            scarSpread = 0.01f;
            scarSpreadCollection = 0.01f;


        }

        //Can't shoot if no mags.
        if (ScarMags <= 0)
        {

            scarSpread = 0.01f;
            scarSpreadCollection = 0.01f;
            ScarMags = 0;
            canReload = false;


        }

        //replenish ammo test
        if (Input.GetKeyDown(KeyCode.P))
        {


            ScarMags = 24;
            ScarClip = 8;
            ScarClipRecord = 0;
            ScarCanShoot = true;
            canReload = true;

        }


        //Debug.Log(scarSpread);
        //Debug.Log(ScarClip);
        //Debug.Log(ScarMags);


        //Expands crosshair depending on current scar spread. (UPDATE FUNTION)
        Scarch1.GetComponent<RectTransform>().localPosition = new Vector3(0, scarSpread, 0) * 512;
        Scarch2.GetComponent<RectTransform>().localPosition = new Vector3(0, -scarSpread, 0) * 512;
        Scarch3.GetComponent<RectTransform>().localPosition = new Vector3(scarSpread, 0, 0) * 512;
        Scarch4.GetComponent<RectTransform>().localPosition = new Vector3(-scarSpread, 0, 0) * 512;


        //If you are holding down aim
        if (Input.GetMouseButtonDown(1))
        {



            //hides crosshair when aim
            Scarch1.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            Scarch2.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            Scarch3.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
            Scarch4.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);

            Scaranimone.SetBool("IsADS", true);
            scarSpread = 0;
            scarSpreadCollection = 0;
            ScarisADS = true;
            cPMPlayer.moveSpeed = 2.8f;
            cPMPlayer.runDeacceleration = 2.3f;
            cPMPlayer.runAcceleration = 2.8f;
            aimSway.maxSwayAngle = 2;
            aimSway.swayAngle = 2;
            aimSway.swaySmooth = 20;



            Scaraudioranm = Random.Range(1, 3);


            if (Scaraudioranm == 1)
            {


                Scarm1.pitch = Random.Range(0.8f, 1);
                Scarm1.Play();
            }


            if (Scaraudioranm == 2)
            {


                Scarm2.pitch = Random.Range(0.8f, 1);
                Scarm2.Play();
            }



            if (Scaraudioranm == 3)
            {


                Scarm3.pitch = Random.Range(0.8f, 1);
                Scarm3.Play();


            }


        }


        //If you let go of aim
        if (Input.GetMouseButtonUp(1))
        {

            Scarch1.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Scarch2.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Scarch3.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Scarch4.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);


            Scaranimone.SetBool("IsADS", false);

            scarSpread = 0.01f;
            scarSpreadCollection = 0.01f;
            ScarisADS = false;
            cPMPlayer.moveSpeed = 7;
            cPMPlayer.runDeacceleration = 5;
            cPMPlayer.runAcceleration = 6;
            aimSway.maxSwayAngle = 14;
            aimSway.swayAngle = 7;
            aimSway.swaySmooth = 8;


            if (Scaraudioranm == 1)
            {


                Scarm1.Stop();
            }


            if (Scaraudioranm == 2)
            {


                Scarm2.Stop();
            }



            if (Scaraudioranm == 3)
            {


                Scarm3.Stop();


            }
        }








        //Shoot weapon let go
        if (Input.GetMouseButtonUp(0))
        {




            //sets crosshair back to normal
            CHNormal();

        }


        //Shoot Weapon (this calls the shoot void)
        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire)
        {


            if (ScarCanShoot == true)
            {
                ScarClipRecord += 1;
                ScarClip -= 1;


            }





            nextTimeToFire = Time.time + 2f / ScarfireRate; //Scar default (0.65f)




            if (ScarCanShoot == true)
            {


                for(int i = 0; i < amountOfProjectiles; i++) //Makes it shoot multiple times. (amountofprojectiles) int. For shotguns mostly.
                {


                    ScarShoot();

                }
                

            }

        }
    }
}
