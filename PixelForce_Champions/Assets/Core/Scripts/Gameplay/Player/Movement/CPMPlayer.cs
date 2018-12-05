/*
 * - Edited by PrzemyslawNowaczyk (11.10.17)
 *   -----------------------------
 *   Deleting unused variables
 *   Changing obsolete methods
 *   Changing used input methods for consistency
 *   -----------------------------
 *
 * - Edited by NovaSurfer (31.01.17).
 *   -----------------------------
 *   Rewriting from JS to C#
 *   Deleting "Spawn" and "Explode" methods, deleting unused varibles
 *   -----------------------------
 * Just some side notes here.
 *
 * - Should keep in mind that idTech's cartisian plane is different to Unity's:
 *    Z axis in idTech is "up/down" but in Unity Z is the local equivalent to
 *    "forward/backward" and Y in Unity is considered "up/down".
 *
 * - Code's mostly ported on a 1 to 1 basis, so some naming convensions are a
 *   bit fucked up right now.
 *
 * - UPS is measured in Unity units, the idTech units DO NOT scale right now.
 *
 * - Default values are accurate and emulates Quake 3's feel with CPM(A) physics.
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains the command the user wishes upon the character
struct Cmd
{
    public float forwardMove;
    public float rightMove;
    public float upMove;
}

public class CPMPlayer : MonoBehaviour
{
    public Transform playerView;     // Camera
    public float playerViewYOffset = 0.6f; // The height at which the camera is bound to
    public float xMouseSensitivity = 30.0f;
    public float yMouseSensitivity = 30.0f;
    public FixedTouchField fixedTouchField;
//
    /*Frame occuring factors*/
    public float gravity = 20.0f;

    public float friction = 6; //Ground friction

    /* Movement stuff */
    public float moveSpeed = 7.0f;                // Ground move speed
    public float runAcceleration = 14.0f;         // Ground accel
    public float runDeacceleration = 10.0f;       // Deacceleration that occurs when running on the ground
    public float airAcceleration = 2.0f;          // Air accel
    public float airDecceleration = 2.0f;         // Deacceleration experienced when ooposite strafing
    public float airControl = 0.3f;               // How precise air control is
    public float sideStrafeAcceleration = 50.0f;  // How fast acceleration occurs to get up to sideStrafeSpeed when
    public float sideStrafeSpeed = 1.0f;          // What the max speed to generate when side strafing
    public float jumpSpeed = 8.0f;                // The speed at which the character's up axis gains when hitting jump
    public bool holdJumpToBhop = false;           // When enabled allows player to just hold jump button to keep on bhopping perfectly. Beware: smells like casual.
    public bool isMovingForward = false;
    public bool isMovingSideways = false;
    public bool isMoving = false;

    /*print() style */
    public GUIStyle style;

    /*FPS Stuff */
    public float fpsDisplayRate = 4.0f; // 4 updates per sec

    private int frameCount = 0;
    private float dt = 0.0f;
    private float fps = 0.0f;

    private CharacterController _controller;

    // Camera rotations
    public float rotX = 0.0f;
    public float rotY = 0.0f;

    private Vector3 moveDirectionNorm = Vector3.zero;
    private Vector3 playerVelocity = Vector3.zero;
    private float playerTopVelocity = 0.0f;

    // Q3: players can queue the next jump just before he hits the ground
    private bool wishJump = false;

    // Used to display real time fricton values
    private float playerFriction = 0.0f;

    // Player commands, stores wish commands that the player asks for (Forward, back, jump, etc)
    private Cmd _cmd;

    private void Start()
    {
        // Hide the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (playerView == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
                playerView = mainCamera.gameObject.transform;
        }

        // Put the camera inside the capsule collider
        playerView.position = new Vector3(
            transform.position.x,
            transform.position.y + playerViewYOffset,
            transform.position.z);

        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {



        //Spacebar (Jump) prevents you from speeding in air.
        if (Input.GetKey(KeyCode.Space))
        {

            moveSpeed = 5f;

        }






        //Sprint
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {



            moveSpeed = 7.5f;


        }

        //Stop sprinting
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {



            moveSpeed = 7f;


        }



        //Checks if we are moving.
        if (_cmd.forwardMove != 0)
        {

            isMoving = true;
 



        }


        else
        {

            isMoving = false;


        }


        if (_cmd.rightMove != 0)
        {

            isMoving = true;


        }



        // Do FPS calculation
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0 / fpsDisplayRate)
        {
            fps = Mathf.Round(frameCount / dt);
            frameCount = 0;
            dt -= 1.0f / fpsDisplayRate;
                 }
        /* Ensure that the cursor is locked into the screen */
        if (Cursor.lockState != CursorLockMode.Locked) {
            if (Input.GetButtonDown("Fire1"))
                Cursor.lockState = CursorLockMode.Locked;
        }

        /* Camera rotation stuff, mouse controls this shit */
        rotX -= Input.GetAxisRaw("Mouse Y") * xMouseSensitivity * 0.02f;
        rotY += Input.GetAxisRaw("Mouse X") * yMouseSensitivity * 0.02f;

        // Clamp the X rotation
        if(rotX < -90)
            rotX = -90;
        else if(rotX > 90)
            rotX = 90;

        this.transform.rotation = Quaternion.Euler(0, rotY, 0); // Rotates the collider
        playerView.rotation     = Quaternion.Euler(rotX, rotY, 0); // Rotates the camera




        //Need to move the camera after the player has been moved because otherwise the camera will clip the player if going fast enough and will always be 1 frame behind.
        // Set the camera's position to the transform
        playerView.position = new Vector3(
            transform.position.x,
            transform.position.y + playerViewYOffset,
            transform.position.z);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 400, 100), "FPS: " + fps, style);
        var ups = _controller.velocity;
        ups.y = 0;
        GUI.Label(new Rect(0, 15, 400, 100), "Speed: " + Mathf.Round(ups.magnitude * 100) / 100 + "ups", style);
        GUI.Label(new Rect(0, 30, 400, 100), "Top Speed: " + Mathf.Round(playerTopVelocity * 100) / 100 + "ups", style);
    }
}
