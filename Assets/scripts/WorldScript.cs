using UnityEngine;
using System.Collections;

public class WorldScript : MonoBehaviour {

	public bool heaven;
	public float longMin;
	public float longMax;
	public float wideRange;
	public float heightMin;
	public float heightMax;
	public float spawnInterval;

	public GameObject cube;
	public GameObject player;

	public Color heavenSky1;
	public Color heavenSky2;
	public Color hellSky1;
	public Color hellSky2;
	public float skyDuration;

	// Use this for initialization
	void Start () {
		InvokeRepeating("spawnCube", 1f, spawnInterval);
		InvokeRepeating("spawnCube", 2f, spawnInterval);
		InvokeRepeating("flipWorld", 10f, 10f);
	}
	
	// Update is called once per frame
	void Update () {
		float t = Mathf.PingPong(Time.time, skyDuration) / skyDuration;
		Camera.main.backgroundColor = heaven ? Color.Lerp(heavenSky1, heavenSky2, t) : Color.Lerp(hellSky1, hellSky2, t);
	}

	void spawnCube(){
		Vector3 pos = player.transform.position;
		pos.z += Random.Range(longMin, longMax);
		pos.x += Random.Range(-wideRange, wideRange);
		pos.y += !heaven ? Random.Range(-heightMin, -heightMax) : Random.Range(heightMin, heightMax);
		Instantiate(cube, pos, Quaternion.identity);
	}

	void flipWorld(){
	
		heaven = !heaven;
	}

}
