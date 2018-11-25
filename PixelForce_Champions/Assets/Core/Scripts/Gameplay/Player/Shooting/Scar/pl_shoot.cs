using UnityEngine;
using UnityEngine.UI;

public class pl_shoot : MonoBehaviour
{




    //Reference to ammo text in canvas (UI)
    public Text currentammotext;
    public Text maxammotext;


    //Reference to shake transform sript.
    public ShakeTransform st;
    public ShakeTransformEventData data;

    //pl_health reference script
    public pl_health pl_Health;

    //Scar anims
    public scar_animatorone scar_Animatorone;
    public scar_animatortwo scar_Animatortwo;


    //Where the raycast come from.
    public Camera cam;





    //Aim sway scipt reference
    public AimSway aimSway;


    //When player dies.
    public GameObject BloodScreen;




    //Scar muzzle flash spawn point.
    public Transform Scar_MFSpawnPoint;



    //Scar FireRate.
    public float ScarfireRate = 5f;
    private float nextTimeToFire = 0f;


    //Access the movement script.
    public CPMPlayer cPMPlayer;


    //Scar ammo system.
    public float ScarClip = 30;
    public float ScarMags = 270;
    public float ScarClipRecord = 0;
    public bool canReload = true;


    //Scar Shoot Sounds with different pitches
    public AudioSource ScaraudioSourceShoot;
    public AudioSource ScaraudioSourceShoot1;
    public AudioSource ScaraudioSourceShoot2;

    //Scar animators for shoot and walk etc...
    public Animator Scaranimone;
    public Animator Scaranimtwo;

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


    }





    public void AnimSetFalse()
    {

        Scaranimone.SetBool("IsShoot", false);
        Scaranimtwo.SetBool("IsShoot", false);


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





        st.AddShakeEvent(data);


        ScarClipRecord += 1;
        ScarClip -= 1;



        //Everytime we shoot scar spread value goes up. The crosshair expands in the update function.
        scarSpread += Mathf.Lerp(scarSpread, scarSpreadCollection, 32);




        Scaranimone.SetBool("IsShoot", true);
        Scaranimtwo.SetBool("IsShoot", true);



        //Dont set too high or animation will stay at the end for a long time. This needs to happen quick. Recommended amount (0.1f) on any fire animation.
        Invoke("AnimSetFalse", 0.1f);


        Scaraudioran = Random.Range(1, 3);


        //Random shoot noice with pitch (volume (0.7f, 0.8f)
        if (Scaraudioran == 1)
        {

            ScaraudioSourceShoot.volume = (Random.Range(0.35f, 0.45f));
            ScaraudioSourceShoot.pitch = (Random.Range(0.7f, 1.0f));
            ScaraudioSourceShoot.Play();

        }

        if (Scaraudioran == 2)
        {
            ScaraudioSourceShoot1.volume = (Random.Range(0.35f, 0.45f));
            ScaraudioSourceShoot1.pitch = (Random.Range(0.7f, 1.0f));
            ScaraudioSourceShoot1.Play();

        }


        if (Scaraudioran == 3)
        {
            ScaraudioSourceShoot2.volume = (Random.Range(0.35f, 0.45f));
            ScaraudioSourceShoot2.pitch = (Random.Range(0.7f, 1.0f));
            ScaraudioSourceShoot2.Play();


        }

        //recoil


        if (ScarisADS == false)
        {

            cPMPlayer.rotX += Random.Range(-1.15f, -1.35f);
            cPMPlayer.rotY += Random.Range(-1.15f, 1.30f);

        }



        if (ScarisADS == true)
        {


            cPMPlayer.rotX += Random.Range(-0.9f, -0.75f);
            cPMPlayer.rotY += Random.Range(-0.9f, 0.55f);

        }



        //increase spread of scar every shot

        if (ScarisADS == false)
        {

            scarSpreadCollection += 0.00035f;

        }



        if (ScarisADS == true)
        {


            scarSpreadCollection += 0.00020f;

        }

        //PhotonNetwork.Instantiate("MuzzleFlash", Scar_MFSpawnPoint.position, Scar_MFSpawnPoint.rotation, 0);
        Ray ray = cam.ViewportPointToRay(new Vector3(0, 0, 0));
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward + Random.insideUnitSphere * scarSpread, out hit))

        {


            PhotonNetwork.Instantiate("ScanLocation", hit.point, Quaternion.identity, 0);
        }





        //If we hit a test object with a tag "Test" do something.
        if (hit.collider.tag == "Test")
        {


            hitmarkerrand = Random.Range(1, 2);


            if (hitmarkerrand == 1 )
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

        //If we hit a player with photonview do something.
        if (hit.collider.tag == "Enemy")
        {

            hit.collider.GetComponent<PhotonView>().RPC("ScarTakeDamage", PhotonTargets.AllBuffered);
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
        currentammotext.text = "" + ScarClip;
        maxammotext.text = "" + ScarMags;



        //Checks if clip is empty.
        if (ScarClip == 0)
        {

            ScarCanShoot = false;
            scarSpread = 0.01f;
            scarSpreadCollection = 0.01f;


        }

        //Ensure the scar clip record cant exceed 30. 
        if (ScarClipRecord == 30)
        {

            ScarClipRecord = 30;


        }

        //Reload your gun.
        if (Input.GetKeyDown (KeyCode.R) && canReload == true)
        {

            ScarMags -= ScarClipRecord;
            ScarCanShoot = true;
            ScarClip = 30;
            ScarClipRecord = 0;
            scarSpread = 0.01f;
            scarSpreadCollection = 0.01f;


        }

        //Can't shoot if no mags. 
        if (ScarMags <= 0)
        {

            //ScarCanShoot = false; (deprecated)
            scarSpread = 0.01f;
            scarSpreadCollection = 0.01f;
            ScarMags = 0;
            canReload = false;

        }

        //replenish ammo test
        if (Input.GetKeyDown(KeyCode.P))
        {


            ScarMags = 270;
            ScarClip = 30;
            ScarClipRecord = 0;
            canReload = true;
            ScarCanShoot = true;

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
            Scaranimtwo.SetBool("IsADS", true);
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

            Scaranimtwo.SetBool("IsADS", false);
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
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {







            nextTimeToFire = Time.time + 0.65f / ScarfireRate; //Scar default (0.65f)




            if (ScarCanShoot == true)
            {

                ScarShoot();

            }

        }
    }
}
