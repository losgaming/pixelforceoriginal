using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunDetection : MonoBehaviour {



    //Reference this script to know if your character (player) is running or not.
    public bool isRunning = false;




    private void Update()
    {
        

        if (Input.GetKey(KeyCode.LeftShift))
        {


            isRunning = true;


        }



        if (Input.GetKeyUp(KeyCode.LeftShift))
        {


            isRunning = false;


        }


    }

}
