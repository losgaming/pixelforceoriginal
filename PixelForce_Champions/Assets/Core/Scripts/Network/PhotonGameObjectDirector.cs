using UnityEngine;

public class PhotonGameObjectDirector : Photon.MonoBehaviour
{




    public GameObject PlayerCamera;
    public MonoBehaviour[] PlayerScripts;
    public GameObject[] gameObjects;


    private void Update()
    {


        if (photonView.isMine)
        {



            PlayerCamera.SetActive(true);



            foreach (GameObject m in gameObjects)
            {

                m.SetActive(false);

            }

        }

        else
        {



            foreach (GameObject m in gameObjects)
            {

                m.SetActive(true);

            }


            foreach (MonoBehaviour m in PlayerScripts)
            {


                m.enabled = false;

            }


            PlayerCamera.SetActive(false);



        }
    }
}
