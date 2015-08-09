using UnityEngine;
using System.Collections;

public class RotateBoxes : MonoBehaviour {
	private int numBoxes;
	private GameObject[] boxes;

	void Start() {
		this.numBoxes = GameObject.FindGameObjectsWithTag ("CollectableBox").Length;
		this.boxes = GameObject.FindGameObjectsWithTag ("CollectableBox");
	}

	void Update() {
		for(int i = 0; i < this.numBoxes; i++)
			this.boxes[i].transform.Rotate(Vector3.up * 2f);
	}

}
