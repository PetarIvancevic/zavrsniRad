using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CarMovement : MonoBehaviour {

	private float thrustTorque;
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

	//wheel Collider imena
	private string[] wheelColiderNames = new string[4] {
		"WheelColliders/LeftFrontTireCL",
		"WheelColliders/RightFrontTireCL",
		"WheelColliders/LeftBackTireCL",
		"WheelColliders/RightBackTireCL",
	};
	private WheelCollider[] wheelColliders;

	//niz kotaca i pivota
	private GameObject[] tires;
	private GameObject[] tirePivots;


	//ova se metoda pokrece na pocetku 
	//NIJE KONSTRUKTOR i ne smiju se koristiti
	public void Start() {
		this.tires = new GameObject[4];
		this.tirePivots = new GameObject[4];
		this.wheelColliders = new WheelCollider[4];

		for (int i = 0; i < 4; i++) {
			this.tires[i] = GameObject.Find(this.tireNames[i]);
			this.tirePivots[i] = GameObject.Find(this.tirePivotNames[i]);
			this.wheelColliders[i] = GameObject.Find(this.wheelColiderNames[i]).GetComponent<WheelCollider>();
		}
	}


	private void MoveCar() {
		WheelHit wheelHit;
		for(int i = 0; i < 4; i++) {
			this.wheelColliders[i].motorTorque = thrustTorque;
			//this.wheelColliders[i].GetGroundHit(out wheelHit);
			//Debug.Log(wheelHit.forwardSlip);
		}
	
	}

	private void RotateTheTires(float move) {
		move = move * 25f;
		for(int i = 0; i < 4; i++)
			this.tirePivots[i].transform.Rotate (0f, 0f, this.speed * move * Time.deltaTime);
	}

	//kocnica
	private void ApplyBrake() {
		for (int i = 2; i < 4; i++)
			this.wheelColliders [i].brakeTorque = 2f;

		Debug.Log ("Called!");
	}

	//skretanje
	private void SteerCar(float steer) {
		for (int i = 0; i < 2; i++) {
			this.wheelColliders[i].steerAngle = 15f * steer;
		}
	}


	//ova se metoda poziva svaki frame
	public void FixedUpdate()
	{
		float move = Input.GetAxis ("Vertical");

		if (Input.GetKey(KeyCode.Space))
			ApplyBrake ();

		SteerCar (Input.GetAxis ("Horizontal"));	
		

		Vector3 movement = Vector3.forward * move * this.speed * Time.deltaTime;
		this.thrustTorque = move * this.speed * 159f;
		MoveCar ();
		//this.transform.Translate (movement);
		this.RotateTheTires (-move);
	}


}

