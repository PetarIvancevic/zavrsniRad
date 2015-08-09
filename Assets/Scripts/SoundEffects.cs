using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundEffects : MonoBehaviour {
	private AudioSource reverse;
	private AudioSource[] music;
	private string[] songNames;
	private int playingIndex;
	private Text nowPlaying;
	private float timeToHidePlayingText;
	private bool changedSong;

	void Awake () {
		this.nowPlaying = GameObject.Find ("nowPlayingText").GetComponent<Text> ();
		this.reverse = this.GetComponent<AudioSource>();
		this.music = new AudioSource[3];
		this.songNames = new string[3];
		this.songNames [0] = "Now playing\n The Mexican Hat Dance";
		this.songNames [1] = "Now playing\n Pat and Patachon";
		this.songNames [2] = "Now playing\n Benny Hill theme song";

		this.playingIndex = Mathf.RoundToInt(Random.Range (0f, 2));
		this.music = GameObject.Find ("MusicHolder").GetComponents<AudioSource> ();
		this.music[this.playingIndex].Play();
		this.nowPlaying.text = this.songNames [this.playingIndex];
		this.timeToHidePlayingText = Time.fixedTime + 10f;
		this.changedSong = true;
	}

	//promijeni pismu
	private void playNextSong() {
		this.music [this.playingIndex].Stop ();
		this.playingIndex = ((this.playingIndex+1) >= 3) ? 0 : (this.playingIndex + 1);
		this.music [this.playingIndex].Play ();
		this.nowPlaying.text = this.songNames [this.playingIndex];
		this.changedSong = true;
		this.timeToHidePlayingText = Time.fixedTime + 5f;
		this.nowPlaying.text = this.songNames[this.playingIndex];
	}

	//sakrij tekst za pjesmu koja se svira
	private void hideNowPlayingText() {
		this.changedSong = false;
		this.nowPlaying.text = "";
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

		if ((Time.fixedTime > this.timeToHidePlayingText) && this.changedSong) {
			this.hideNowPlayingText ();
		}

	}
}
