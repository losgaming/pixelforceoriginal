using ExitGames.Client.Photon;
using UnityEngine;

public class PhotonSceneDirector : Photon.MonoBehaviour
{

    //Spawn Points.
    public Transform spawnPoint;


    //Access to any gamemodes.
    public GamemodeBase gamemodeBase;

    //Bool for UI to start a match.
    public bool FFAButton = false;
    public bool TDMButton = false;

    //Bools to set gamemodes on and off.
    public bool FreeForAll = false;
    public bool TeamDeathMatch = false;

    //Randomize server mathcmaking.
    int RandomNumb = 0;


    //Tells GamemodeBase to activate "hasStarted("gamemode"). The authorization must be checked within the update function or else (gamemodebase) wont kick in when you connect to the room.
    public bool FFASceneTransfer = false;
    public bool FFASceneTransferConfirmation = false;









    // Use this for initialization

    private void Start()
    {

        PhotonNetwork.ConnectUsingSettings("0.1");
        PhotonNetwork.automaticallySyncScene = true;

    }


    private void Update()
    {


        //This piece of code actually enables FFA. Derives from the gamemodebase system.
        if (FFASceneTransfer == true)
        {



            gamemodeBase.hasStartedFFA = true;

        }



        //Join or create a photon room when FFA UI button is pressed.
        if (FFASceneTransferConfirmation == true)
        {
            PhotonNetwork.LoadLevel("SampleScene");
            RoomOptions FFA = new RoomOptions
            {
                CustomRoomProperties = new Hashtable() { { "ffa", 1 } },
                MaxPlayers = 10
            };
            PhotonNetwork.JoinOrCreateRoom("freeforall", FFA, null);

        }







        if (Input.GetMouseButtonDown(3))
        {



            RandomNumb = Random.Range(1, 3);


            if (RandomNumb == 1)
            {
                RoomOptions TDM1 = new RoomOptions
                {
                    CustomRoomProperties = new Hashtable() { { "tdm1", 1 } },
                    MaxPlayers = 12
                };
                PhotonNetwork.JoinOrCreateRoom("tdm1", TDM1, null);
                TeamDeathMatch = true;


            }

            if (RandomNumb == 2)
            {

                RoomOptions TDM2 = new RoomOptions
                {
                    CustomRoomProperties = new Hashtable() { { "tdm2", 1 } },
                    MaxPlayers = 12
                };
                PhotonNetwork.JoinOrCreateRoom("tdm2", TDM2, null);
                TeamDeathMatch = true;


            }

            if (RandomNumb == 3)
            {

                RoomOptions TDM3 = new RoomOptions
                {
                    CustomRoomProperties = new Hashtable() { { "tdm3", 1 } },
                    MaxPlayers = 12
                };
                PhotonNetwork.JoinOrCreateRoom("tdm3", TDM3, null);
                TeamDeathMatch = true;


            }

        }

    }



    private void OnConnectedToMaster()
    {


        Debug.Log("You have connected to photon API successfully");

    }


    private void OnJoinedRoom()
    {

        PhotonNetwork.Instantiate("First-Person Character", spawnPoint.position, spawnPoint.rotation, 0);


    }


    private void OnLeftRoom()
    {

        Debug.Log("Someone has left the room");

    }



    private void OnCreatedRoom()
    {

        Debug.Log("You have created a room and entered it");

    }


    private void OnJoinedLobby()
    {

        Debug.Log("You have created a lobby on the master server");
    }


    private void OnLeftLobby()
    {
        Debug.Log("You have left a lobby");


    }


    private void OnPhotonMaxCccuReached()
    {

        Debug.Log("The concurrent player limit was reached. Try connecting at a later time.");

    }
}
