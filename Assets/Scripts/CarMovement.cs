using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CarMovement : MonoBehaviour {

	private float thrustTorque;
	//podatkovni clanovi klase za kretanje autica
	public float speed = 150.0f;
	/*	Kola i tijelo auta moraju biti sinkronizirana
	 *  inace će se kola vrtiti krivo
	 * */
	private Quaternion wheelRotationToBody;
	private Rigidbody carBody;
	private float speedModifier = 1.0f;
	
	//svi kotaci u string zapisu
	private string[] tireNames = new string[4] {
		"simpleCar/SteerLeftTire/LeftFrontTirePivot/LeftFrontTire",
		"simpleCar/SteerRightTire/RightFrontTirePivot/RightFrontTire",
		"simpleCar/LeftBackTirePivot/LeftBackTire",
		"simpleCar/RightBackTirePivot/RightBackTire",
	};

	//svi pivoti u string zapisu
	private string[] tirePivotNames = new string[4]{
		"simpleCar/SteerLeftTire/LeftFrontTirePivot",
		"simpleCar/SteerRightTire/RightFrontTirePivot",
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
	private GameObject[] steerWheels;
	private float AntiRoll = 10000f;

	//niz kotaca i pivota
	private GameObject[] tires;
	private GameObject[] tirePivots;


	//ova se metoda pokrece na pocetku 
	//NIJE KONSTRUKTOR i ne smiju se koristiti
	public void Start() {
		this.tires = new GameObject[4];
		this.tirePivots = new GameObject[4];
		this.wheelColliders = new WheelCollider[4];
		this.steerWheels = new GameObject[2];

		this.steerWheels [0] = GameObject.Find ("simpleCar/SteerLeftTire");
		this.steerWheels [1] = GameObject.Find ("simpleCar/SteerRightTire");

		for (int i = 0; i < 4; i++) {
			this.tires[i] = GameObject.Find(this.tireNames[i]);
			this.tirePivots[i] = GameObject.Find(this.tirePivotNames[i]);
			this.wheelColliders[i] = GameObject.Find(this.wheelColiderNames[i]).GetComponent<WheelCollider>();
		}

		//spustimo centar mase
		this.carBody = GameObject.Find ("PlayerHolder").GetComponent<Rigidbody> ();
	}


	private void MoveCar() {
		for(int i = 2; i < 4; i++) {
			this.wheelColliders[i].motorTorque = thrustTorque;
		}

	}

	private void RotateTheTires(float move) {
		move = move * 25f;
		for (int i = 0; i < 4; i++) 
			this.tirePivots[i].transform.Rotate(Vector3.forward, this.speed * move * Time.deltaTime);			
	}

	//kocnica
	private void ApplyBrake() {
		for (int i = 2; i < 4; i++)
			this.wheelColliders [i].brakeTorque = 2f;
	}

	//imitacija "centripetalne" sile 
	private void centripetalForce(float steer) {
		WheelHit hit;

		bool groundedL = this.wheelColliders [0].GetGroundHit (out hit);
		bool groundedR = this.wheelColliders [1].GetGroundHit (out hit);

		if ((!groundedL && !groundedR) || (groundedL && groundedR))
			return;

		if (!groundedL)
			this.carBody.AddForceAtPosition (this.wheelColliders [0].transform.up * -this.AntiRoll, this.wheelColliders [0].transform.position);
			
		if (!groundedR)
			this.carBody.AddForceAtPosition (this.wheelColliders [1].transform.up * -this.AntiRoll, this.wheelColliders [1].transform.position);
	}

	//skretanje
	private void SteerCar(float steer) {
		steer = 45f * steer;
		for (int i = 0; i < 2; i++) {
			Quaternion quat;
			Vector3 pos;
			this.wheelColliders[i].steerAngle = steer;
			this.steerWheels[i].transform.localRotation = Quaternion.Euler(0f, 90f + this.wheelColliders[i].steerAngle, 0f);
		}

		this.centripetalForce (steer);
	}

	//dodat cemo za skupljanje kutija
	void OnTriggerEnter(Collider col) {
		if (col.gameObject.layer == LayerMask.NameToLayer ("RoadRightWayMarker"))
			Debug.Log ("Your going the right way!");
	}

	//ova se metoda poziva svaki frame
	public void FixedUpdate() {
		float move = Input.GetAxis ("Vertical");

		if (Input.GetKey(KeyCode.Space))
			ApplyBrake ();

		SteerCar (Input.GetAxis ("Horizontal"));	

		this.thrustTorque = move * this.speed;
		MoveCar ();
		this.RotateTheTires (-move);	
	}


}

