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

	public float cloudsPerSwitch;
	public float cloudHeightMin;
	public float cloudHeightMax;
	public float cloudDist;
	public float cloudWide;
	public GameObject cloud;

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

	public Material lightWallMaterial;
	public Material darkWallMaterial;

	public ParticleSystem particles;

	public GameObject wall;

	public GameObject worldChange;
	public GameObject brightMusic;
	public GameObject darkMusic;

	public bool gameOver = false;
	private bool displayEndGameGui = false;

	
	// Use this for initialization
	void Start () {
		InvokeRepeating("spawnCube", 1f, spawnInterval);
		InvokeRepeating("spawnCube", 2f, spawnInterval);
		InvokeRepeating("spawnCubeWall", 3f, spawnInterval);
		InvokeRepeating("flipWorld", 10f, 10f);
		Invoke("startEmit", 9f);
		fixColors();
		makeScenery();
		changeMusic();
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

	void spawnCubeWall(){
		Vector3 pos = player.transform.position;
		pos.x += wideRange;
		pos.z += Random.Range(longMin / 2, longMin);
		pos.y += !heaven ? Random.Range(-heightMin, -heightMax) : Random.Range(heightMin, heightMax);
		applyRandomCubeColor(cube);
		Instantiate(cube, pos, Quaternion.identity);

		pos = player.transform.position;
		pos.x -= wideRange;
		pos.z += Random.Range(longMin / 2, longMin);
		pos.y += !heaven ? Random.Range(-heightMin, -heightMax) : Random.Range(heightMin, heightMax);
		applyRandomCubeColor(cube);
		Instantiate(cube, pos, Quaternion.identity);
	}
	
	void flipWorld(){
		heaven = !heaven;
		fixColors();
		makeScenery();
		changeMusic();
	}

	void startEmit(){
		particles.enableEmission = true;
		worldChange.audio.Play();
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

			GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
			for(int index = 0; index < walls.Length; index++){
				walls[index].renderer.material = lightWallMaterial;
			}
		}
		else{
			RenderSettings.fog = false;

			GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
			for(int index = 0; index < walls.Length; index++){
				walls[index].renderer.material = darkWallMaterial;
			}
		}
		GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
		for(int index = 0; index < cubes.Length; index++){
			applyRandomCubeColor(cubes[index]);
		}
	}

	void makeScenery(){
		if(heaven){
			//Destroy terrain
			for(int index = 0; index < cloudsPerSwitch; index++){
				Vector3 pos = player.transform.position;
				pos.y = Random.Range(cloudHeightMin, cloudHeightMax);
				pos.x = Random.Range(-cloudWide, cloudWide);
				pos.z += cloudDist * index;
				Instantiate(cloud, pos, Quaternion.identity);
			}
		}
		else{
			GameObject[] clouds = GameObject.FindGameObjectsWithTag("Cloud");
			for(int index = 0; index < clouds.Length; index++){
				Destroy(clouds[index]);
			}
			//Make terrain
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

	void changeMusic(){
		if(heaven){
			darkMusic.audio.Stop();
			brightMusic.audio.Play();
		}
		else{
			darkMusic.audio.Play();
			brightMusic.audio.Stop();
		}
	}

	public void endGame(){
		gameOver = true;
		CancelInvoke();
		particles.enableEmission = false;
		Invoke("endGameSequence", .5f);
	}

	void endGameSequence(){
		Screen.lockCursor = false;
		Camera.main.gameObject.AddComponent("CameraOrbit");
		displayEndGameGui = true;
	}

	void OnGUI(){
		if(!displayEndGameGui){
			return;
		}
		float w = 300;
		float h = 100;
		float x = (Screen.width / 2) - (w / 2);
		float y = h;
		GUIStyle style = new GUIStyle();
		style.fontSize = 64;
		GUI.Label(new Rect(x, y, w, h), "Thanks for playing!", style);

		h = h / 2;
		w = w / 2;
		if(GUI.Button(new Rect(x + w, y + h + 50, w, h), "Play Again!")){
			Application.LoadLevel("level");
		}

		style.fontSize = 32;
		w = Screen.width;
		h = Screen.height / 8;
		x = 0;
		y = Screen.height - h;
		GUI.Label(new Rect(x, y, w, h), "@thedeadlybutter -- Made for LD30 Compo", style);
	}

}
