using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lean : MonoBehaviour
{
	public Transform weaponCamera;
	public float speed = 100f; //the speed at wich the weapon rotates
	public float maxAngle = 20f; //the maximum rotation angle
	public KeyCode leanLeft = KeyCode.Q;
	public KeyCode leanRight = KeyCode.E;
	private float curAngle = 0f;

	// Use this for initialization
	void Awake()
	{
		if (weaponCamera == null && transform.parent != null)
			weaponCamera = transform.parent;
	}

	// Update is called once per frame
	void Update()
	{

		// lean right
		if (Input.GetKey(leanRight))
			curAngle = Mathf.MoveTowardsAngle(curAngle, maxAngle, speed * Time.deltaTime);
		// lean left
		else if (Input.GetKey(leanLeft))
			curAngle = Mathf.MoveTowardsAngle(curAngle, -maxAngle, speed * Time.deltaTime);
		// reset lean
		else
			curAngle = Mathf.MoveTowardsAngle(curAngle, 0f, speed * Time.deltaTime);
		weaponCamera.transform.localRotation = Quaternion.AngleAxis(curAngle, Vector3.forward);
	}

}
