using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {
	private Canvas Menu;

	public void Start() {
		Time.timeScale = 1.2f;
		this.Menu = GameObject.Find ("Menu").GetComponent<Canvas>();
		this.Menu.enabled = false;
	}

	//zaustavi i pokreni igru 
	private void HandlePauseGame() {
		if (this.Menu.enabled) {
			this.Menu.enabled = false;
			Time.timeScale = 1.2f;
		} else {
			this.Menu.enabled = true;
			Time.timeScale = 0;
		}
	}

	void Update() {
		if (Input.GetKeyUp (KeyCode.Escape))
			HandlePauseGame ();
	}


	//metoda za ponovno pokretanje igre
	public void StartNewGame(int scene) {
		Application.LoadLevel (scene);
	}

	//izlazak iz igre
	public void QuitGame() {
		Application.Quit ();
	}
}
