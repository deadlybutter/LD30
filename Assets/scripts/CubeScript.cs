using UnityEngine;
using System.Collections;

public class CubeScript : MonoBehaviour {

	private WorldScript world;
	private GameObject player;

	public float speed;

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

		Vector3 toTarget = (player.transform.position - transform.position).normalized;
		if(Vector3.Dot(toTarget, transform.forward) > 0){
			Destroy(gameObject);
		}

		Vector3 force = Vector3.zero;
		if(!world.heaven){
			force = Vector3.up;
		}
		else{
			force = Vector3.down;
		}
		transform.Translate(force * speed * Time.deltaTime, Space.World);
	}
}
