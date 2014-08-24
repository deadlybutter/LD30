using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour {

	public Transform target;
	public float xSpeed = 250;
	public float ySpeed = 120;
	public float yMinLimit = -20;
	public float yMaxLimit = 80;
	public float distance = 10;

	private float x = 0;
	private float y = 0;

	private float xA = 1;
	private float yA = 1;
	private bool forward;

	// Use this for initialization
	void Start () {
		target = GameObject.Find ("Player").transform;
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		// Make the rigid body not change rotation
		if(GetComponent<Rigidbody>()){
			GetComponent<Rigidbody>().freezeRotation = true;
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(target) {
			if(xA > 260){
				forward = false;
			}
			else if(xA < 0){
				forward = true;
			}

			if(forward){
				xA++; 
				yA++;
			}

			x += Time.deltaTime * xA * xSpeed * 0.02f;
			y -=  Time.deltaTime * xA * ySpeed * 0.02f;
			
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			
			Quaternion rotation = Quaternion.Euler(y, x, 0);
			Vector3 position = rotation * new Vector3(0, 0, -distance) + target.position;
			
			transform.rotation = rotation;
			transform.position = position;
		}
	}
	
	static float ClampAngle(float angle, float min, float max) {
		if (angle < -360) {
			angle += 360;
		}
		if (angle > 360) {
			angle -= 360;
		}
		return Mathf.Clamp(angle, min, max);
	}

}
