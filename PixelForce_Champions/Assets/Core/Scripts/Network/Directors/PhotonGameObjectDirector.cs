using UnityEngine;

public class PhotonGameObjectDirector : Photon.MonoBehaviour
{




    public GameObject PlayerCamera;
    public MonoBehaviour[] PlayerScripts;
    public GameObject[] gameObjects;
    public GameObject[] gameObjects2;



    private void Update()
    {


        if (photonView.isMine)
        {



            PlayerCamera.SetActive(true);



            foreach (GameObject m in gameObjects)
            {

                m.SetActive(false);

            }



            foreach (GameObject m in gameObjects2)
            {

                m.SetActive(true);

            }

        }

        else
        {



            foreach (GameObject m in gameObjects)
            {

                m.SetActive(true);

            }

            foreach (GameObject m in gameObjects2)
            {

                m.SetActive(false);

            }



            foreach (MonoBehaviour m in PlayerScripts)
            {


                m.enabled = false;

            }


            PlayerCamera.SetActive(false);



        }
    }
}
