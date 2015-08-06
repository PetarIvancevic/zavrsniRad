using UnityEngine;
using System.Collections;

public class RandomizeBirds : MonoBehaviour {
	private AudioSource[] birds;
	private int numBirds;

	void Awake () {
		this.numBirds = this.GetComponentsInChildren<AudioSource> ().Length;
		this.birds = new AudioSource[this.numBirds];
		this.birds = this.GetComponentsInChildren<AudioSource> ();
	}

	//dodat da se na random ptice glasaju
	void Update() {

	}
}
