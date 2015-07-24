using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CarMovement : MonoBehaviour {


	//podatkovni clanovi klase za kretanje autica
	public float speed = 15.0f;
	/*	Kola i tijelo auta moraju biti sinkronizirana
	 *  inace će se kola vrtiti krivo
	 * */
	private Quaternion wheelRotationToBody;

	//svi kotaci u string zapisu
	private string[] tireNames = new string[4] {
		"simpleCar/LeftFrontTirePivot/LeftFrontTire",
		"simpleCar/RightFrontTirePivot/RightFrontTire",
		"simpleCar/LeftBackTirePivot/LeftBackTire",
		"simpleCar/RightBackTirePivot/RightBackTire",
	};

	//svi pivoti u string zapisu
	private string[] tirePivotNames = new string[4]{
		"simpleCar/LeftFrontTirePivot",
		"simpleCar/RightFrontTirePivot",
		"simpleCar/LeftBackTirePivot",
		"simpleCar/RightBackTirePivot"
	};
	//niz kotaca i pivota
	private GameObject[] tires;
	private GameObject[] tirePivots;


	//ova se metoda pokrece na pocetku 
	//NIJE KONSTRUKTOR i ne smiju se koristiti
	public void Start() {
		this.tires = new GameObject[4];
		this.tirePivots = new GameObject[4];

		for (int i = 0; i < 4; i++) {
			this.tires[i] = GameObject.Find(this.tireNames[i]);
			this.tirePivots[i] = GameObject.Find(this.tirePivotNames[i]);
		}
	}


	private void RotateTheTires(float move) {
		move = move * 25f;
		for(int i = 0; i < 4; i++)
			this.tirePivots[i].transform.Rotate (0f, 0f, this.speed * move * Time.deltaTime);
	}

	//ova se metoda poziva svaki frame
	public void FixedUpdate()
	{
		float move = Input.GetAxis ("Vertical");
		//float steer = Input.GetAxis ("Horizontal");

		Vector3 movement = Vector3.forward * move * this.speed * Time.deltaTime;

		this.transform.Translate (movement);
		this.RotateTheTires (-move);
	}


}

