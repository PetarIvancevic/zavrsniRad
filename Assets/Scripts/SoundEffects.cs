using UnityEngine;
using System.Collections;

public class SoundEffects : MonoBehaviour {
	private AudioSource reverse;
	private AudioSource[] music;
	private int playingIndex;
	private int numSongs;

	void Awake () {
		this.reverse = this.GetComponent<AudioSource>();
		this.numSongs = GameObject.Find("MusicHolder").GetComponents<AudioSource>().Length;
		this.music = new AudioSource[this.numSongs];
		this.playingIndex = Mathf.RoundToInt(Random.Range (0f, this.numSongs-1));
		this.music = GameObject.Find ("MusicHolder").GetComponents<AudioSource> ();
		this.music[this.playingIndex].Play();
	}

	//promijeni pismu
	private void playNextSong() {
		this.music [this.playingIndex].Stop ();
		this.playingIndex = ((this.playingIndex+1) >= this.numSongs) ? 0 : (this.playingIndex + 1);
		this.music [this.playingIndex].Play ();
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.DownArrow))
			this.reverse.Play ();
		
		if (Input.GetKeyUp (KeyCode.DownArrow))
			this.reverse.Stop ();

		if (Input.GetKeyUp (KeyCode.X))
			this.playNextSong ();

		if (!this.music [this.playingIndex].isPlaying)
			this.playNextSong ();
	}
}
