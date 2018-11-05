using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimSway : MonoBehaviour
{
	public Vector3 aimPosition;
	public Vector3 aimRotation;
	public Vector3 hipFirePosition;
	public Vector3 hipFireRotation;
	public bool canADS = true;
	private float currentTime = 0f;
	public float aimSpeed = 0.2f;			// the lower the value, the faster the aim speed
	public Camera mainCamera;				// the camera that sees the world
	public bool cameraZoom = true;			// should the camera zoom in
	public float zoomFOV = 50;
	private float hipfireFOV;
	public float swayAngle = 5.0f;			// Average angle that the gun can sway.
	public float maxSwayAngle = 15;			// Maximum angle that the gun can sway.
	public float swaySmooth = 4.0f;			// Speed to stabilize the movement.
	public Transform swayTarget;			// the Transform that is swayed
	public Rigidbody controllerRigidbody;	// the Rigidbody from this character
	private bool moving = false;			// is moving to aimposition or to original position
	private Quaternion normalRotation;
    public FixedTouchField touchfieldsway;
	// Use this for initialization
	void Start()
	{
		hipfireFOV = mainCamera.fieldOfView;
		normalRotation = swayTarget.transform.rotation;
	}

	// Update is called once per frame
	void Update()
	{

		// If the mouse is moving
		if (touchfieldsway.TouchDist.x != 0 || touchfieldsway.TouchDist.y != 0)
		{
			// Tilt in Y.
			float TiltY = Mathf.Clamp(touchfieldsway.TouchDist.x * -swayAngle, -maxSwayAngle, maxSwayAngle);

			// Tilt in X.
			float TiltX = Mathf.Clamp(touchfieldsway.TouchDist.y * swayAngle, -maxSwayAngle, maxSwayAngle);
			float TiltZ;
			// Tilt in Z.
			if (controllerRigidbody != null)
			{
				TiltZ = controllerRigidbody.velocity.magnitude >= 1.5f ?
					Mathf.Clamp(touchfieldsway.TouchDist.x * -swayAngle, -maxSwayAngle, maxSwayAngle) : 0;
			}
			else
				TiltZ = 0;
			// Sets the end rotation based on the sway on all axis
			Quaternion newRotation = Quaternion.Euler(-TiltX, -TiltY, -TiltZ);

			// Moves the weapon from the current rotation to the end rotation.
			if(!moving)
				swayTarget.localRotation = Quaternion.Lerp(swayTarget.localRotation, newRotation, Time.deltaTime * swaySmooth);
		}
		else
		{
			// If the mouse input is zero (Vector2.zero), reset it to its original position.
			swayTarget.localRotation = Quaternion.Lerp(swayTarget.localRotation, normalRotation, Time.deltaTime * swaySmooth);
		}
	}
}