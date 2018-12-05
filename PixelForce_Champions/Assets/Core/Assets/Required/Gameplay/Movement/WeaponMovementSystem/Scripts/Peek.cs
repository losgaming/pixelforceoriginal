using UnityEngine;
using System.Collections;

public class Peek : MonoBehaviour
{
	public Transform defaultCamera;
	public float camToLeftMax = 0.25f;
	public float camToRightMax = 0.25f;
	private float nextPos;
	private float nextPos2;
	private float dampVelocity;
	private float dampVelocity2;
	public KeyCode peekLeft = KeyCode.Q;
	public KeyCode peekRight = KeyCode.E;
	public bool checkColliding = true;

	private void Update()
	{
		float newPos = Mathf.SmoothDamp(defaultCamera.transform.localPosition.x, nextPos, ref dampVelocity, 0.2f);
		float newPos2 = Mathf.SmoothDamp(defaultCamera.transform.localPosition.y, nextPos2, ref dampVelocity2, 0.2f);
		defaultCamera.transform.localPosition = new Vector3(newPos, newPos2, 0);
		if (Input.GetKey(peekRight))
		{
			if (checkColliding)
			{
				Vector3 direction = defaultCamera.transform.TransformDirection(Vector3.right); // The direction that the player is looking.
				Vector3 origin = defaultCamera.transform.position; // Camera position.

				Ray ray = new Ray(origin, direction); // Creates a ray starting at origin along direction.
				RaycastHit hitInfo; // Structure used to get information back from a raycast.

				if (Physics.Raycast(ray, out hitInfo, ((camToLeftMax + camToRightMax) / 2))) // Checks whether the ray intersects something.
				{
					nextPos = hitInfo.distance;
				}
				else
				{
					nextPos = camToRightMax;
					nextPos2 = 0.0f;
				}
			}
			else
			{
				//this value is the distance your camere goes right when you peek
				nextPos = camToRightMax;
				nextPos2 = 0.0f;
			}
		}
		else if (Input.GetKey(peekLeft))
		{
			if (checkColliding)
			{
				Vector3 direction = defaultCamera.transform.TransformDirection(Vector3.left); // The direction that the player is looking.
				Vector3 origin = defaultCamera.transform.position; // Camera position.

				Ray ray = new Ray(origin, direction); // Creates a ray starting at origin along direction.
				RaycastHit hitInfo; // Structure used to get information back from a raycast.

				if (Physics.Raycast(ray, out hitInfo, ((camToLeftMax + camToRightMax) / 2))) // Checks whether the ray intersects something.
				{
					nextPos = -hitInfo.distance;
				}
				else
				{
					nextPos = -camToLeftMax;
					nextPos2 = 0.0f;
				}
			}
			else
			{
				//this value is the distance your camere goes left when you peek
				nextPos = -camToLeftMax;
				nextPos2 = 0.0f;
			}
		}
		else
		{
			//this value is the distance your camere goes left when you peek
			nextPos = 0.0f;
			nextPos2 = 0.0f;
		}
	}
}