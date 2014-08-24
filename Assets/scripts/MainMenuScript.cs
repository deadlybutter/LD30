using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	float x;
	float y;

	public float w;
	public float h;

	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		y = Screen.height - (Screen.height / 5);
		x = (Screen.width / 2) - (w / 2);
		if(GUI.Button(new Rect(x, y, w, h), "OKAY!")){
			audio.Play();
			Invoke("loadGame", 1f);
		}
	}

	void loadGame(){
		Application.LoadLevel("level");
	}
}
