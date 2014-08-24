using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {

	private WorldScript world;
	private GameObject player;

	public float gap;

	// Use this for initialization
	void Start () {
		world = GameObject.Find("World").GetComponent<WorldScript>();
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(world.gameOver){
			return;
		}

		//abs of diff between player Z and wall Z
		Vector3 newPos = transform.position;
		newPos.z = player.transform.position.z - gap;
		transform.position = newPos;

//		transform.Translate(transform.forward * 6 * Time.deltaTime, Space.World);
	}
}
