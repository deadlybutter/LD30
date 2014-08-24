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

	public Light light;
	public Color heavenLightColor;
	public Color heavenFogColor;
	public float heavenFogStart;
	public Color hellLightColor;
	
	public float minSunRot;
	public float maxSunRot;

	public Material lightMaterial1;
	public Material lightMaterial2;
	public Material darkMaterial1;
	public Material darkMaterial2;

	public ParticleSystem particles;

	public bool gameOver = false;

	// Use this for initialization
	void Start () {
		InvokeRepeating("spawnCube", 1f, spawnInterval);//stop
		InvokeRepeating("spawnCube", 2f, spawnInterval);//stop
		InvokeRepeating("flipWorld", 10f, 10f);
		Invoke("startEmit", 9f);
		fixColors();
		particles.enableEmission = false;
	}
	
	// Update is called once per frame
	void Update () {
		float t = Mathf.PingPong(Time.time, skyDuration) / skyDuration;
		Camera.main.backgroundColor = heaven ? Color.Lerp(heavenSky1, heavenSky2, t) : Color.Lerp(hellSky1, hellSky2, t);

		Vector3 lightAngle = light.transform.eulerAngles;
		lightAngle.y = Mathf.Lerp(minSunRot, maxSunRot, t);
		light.transform.eulerAngles = lightAngle;
	}

	void spawnCube(){
		Vector3 pos = player.transform.position;
		pos.z += Random.Range(longMin, longMax);
		pos.x += Random.Range(-wideRange, wideRange);
		pos.y += !heaven ? Random.Range(-heightMin, -heightMax) : Random.Range(heightMin, heightMax);
		applyRandomCubeColor(cube);
		Instantiate(cube, pos, Quaternion.identity);
	}

	void flipWorld(){
		heaven = !heaven;
		fixColors();
	}

	void startEmit(){
		particles.enableEmission = true;
		Invoke("stopEmit", 1f);
	}

	void stopEmit(){
		particles.enableEmission = false;
		Invoke("startEmit", 9f);
	}

	void fixColors(){
		if(heaven){
			RenderSettings.fog = true;
			light.color = heavenLightColor;
			RenderSettings.fogColor = heavenFogColor;
			RenderSettings.fogStartDistance = heavenFogStart;
		}
		else{
			RenderSettings.fog = false;
		}
		GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
		for(int index = 0; index < cubes.Length; index++){
			applyRandomCubeColor(cubes[index]);
		}
	}

	void applyRandomCubeColor(GameObject cube){
		if(heaven){
			if(Random.value > 0.5){
				cube.renderer.material = lightMaterial1;
			}
			else{
				cube.renderer.material = lightMaterial2;
			}
		}
		else{
			if(Random.value > 0.5){
				cube.renderer.material = darkMaterial1;
			}	
			else{
				cube.renderer.material = darkMaterial2;
			}
		}
	}

	public void endGame(){
		gameOver = true;
		CancelInvoke("spawnCube");
		Invoke("endGameSequence", 1f);
	}

	void endGameSequence(){
		Camera.main.transform.Translate (Vector3.up * 100);
		Camera.main.gameObject.AddComponent("CameraOrbit");
	}

}
