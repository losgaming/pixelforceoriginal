using UnityEngine;
using UnityEngine.UI;

public class GamemodeBase : Photon.MonoBehaviour
{



    //FFA Time
    private float currentTimeFFA = 0f;
    private float startingTimeFFA = 120f;

    //TDM Time
    private float currentTimeTDM = 0f;
    private float startingTimeTDM = 120f;

    //Has started gamemodes
    public bool hasStartedFFA = false;
    public bool hasStartedTDM = false;

    //Gamemode timers.
    public Text countdownTextFFA;
    public Text countdownTextTDM;


    // Use this for initialization
    private void Start()
    {

        currentTimeFFA = startingTimeFFA;
        currentTimeTDM = startingTimeTDM;
    }





    private void OnPhotonSerializeView(PhotonStream photonStream, PhotonMessageInfo photonMessageInfo)
    {

        if (photonStream.isWriting)
        {

            photonStream.SendNext(currentTimeFFA);
            photonStream.SendNext(currentTimeTDM);

        }

        else if (photonStream.isReading)
        {

            currentTimeFFA = (float)photonStream.ReceiveNext();
            currentTimeTDM = (float)photonStream.ReceiveNext();

        }

    }


    // Update is called once per frame
    private void Update()
    {


        //If FFA started.
        if (hasStartedFFA == true)
        {



            currentTimeFFA -= 1 * Time.deltaTime;
            countdownTextFFA.text = currentTimeFFA.ToString("0");

        }

        //If TDM started
        if (hasStartedTDM == true)
        {


            currentTimeFFA -= 1 * Time.deltaTime;
            countdownTextTDM.text = currentTimeFFA.ToString("0");

        }




        if (currentTimeFFA < 0f)

        //If FFA time is up do something below.

        {

            countdownTextFFA.text = ("Times up!");

        }

        if (currentTimeTDM < 0f)


        //If TDM time is up do something below.
        {

            countdownTextTDM.text = ("Times up!");


        }




    }
}
