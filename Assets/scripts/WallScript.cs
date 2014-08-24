using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {

	private WorldScript world;

	// Use this for initialization
	void Start () {
		world = GameObject.Find("World").GetComponent<WorldScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if(world.gameOver){
			return;
		}
		transform.Translate(transform.forward * 6 * Time.deltaTime, Space.World);
	}
}
