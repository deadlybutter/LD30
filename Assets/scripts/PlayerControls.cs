﻿using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	private WorldScript world;

	public float speed;
	public float autoVertSpeed;
	public float autoForwardSpeed;

	public float mouseSensitivity;
	public float verticalRotation = 0;
	public float upDownRange;

	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
		world = GameObject.Find("World").GetComponent<WorldScript>();
	}
	
	void Update () {
		//Mouse rotation
		float yaw = Input.GetAxis("Mouse X") * mouseSensitivity;
		transform.Rotate(0, yaw, 0);
		
		verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
		Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);	

		transform.Translate(transform.forward * autoForwardSpeed * Time.deltaTime, Space.World);
	}

}
