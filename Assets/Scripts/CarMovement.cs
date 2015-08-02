using UnityEngine;
using System.Collections;
using UnityEngine.UI;
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
	private bool[] markersPassed;
	private int lapsPassed = 0;
	private int gear = 1;
	private Vector3 lastPos; 


	//posebne skripte za tekst
	public Text victoryText;
	public Text lapText;

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
		this.victoryText.text = "";
		this.tires = new GameObject[4];
		this.tirePivots = new GameObject[4];
		this.wheelColliders = new WheelCollider[4];
		this.steerWheels = new GameObject[2];
		this.markersPassed = new bool[7];
		this.steerWheels [0] = GameObject.Find ("simpleCar/SteerLeftTire");
		this.steerWheels [1] = GameObject.Find ("simpleCar/SteerRightTire");

		for (int i = 0; i < 4; i++) {
			this.tires[i] = GameObject.Find(this.tireNames[i]);
			this.tirePivots[i] = GameObject.Find(this.tirePivotNames[i]);
			this.wheelColliders[i] = GameObject.Find(this.wheelColiderNames[i]).GetComponent<WheelCollider>();
			this.markersPassed[i] = false;
			this.markersPassed[i+3] = false;
		}
		this.markersPassed [0] = true;

		//spustimo centar mase
		this.carBody = GameObject.Find ("PlayerHolder").GetComponent<Rigidbody> ();
		this.lastPos = this.carBody.transform.position;
	}

	//pomicemo cijelo auto preko wheel Collidera
	private void MoveCar(float steer) {
		for (int i = 2; i < 4; i++) {
			if(steer != 0f)
				this.wheelColliders[i].motorTorque = this.thrustTorque * 0.7f;
			else
				this.wheelColliders[i].motorTorque = this.thrustTorque;
		}
			
	}

	//samo okrecemo kotace ako se krece autic
	private void RotateTheTires(float movement) {
		movement = (movement == 0f) ? -1f : movement;
		if (this.carBody.transform.position != this.lastPos) {
			movement = Mathf.Sqrt(Mathf.Pow(this.carBody.transform.position.x - this.lastPos.x, 2) + Mathf.Pow (this.carBody.transform.position.z - this.lastPos.z, 2)) * 30000f * movement;
			for (int i = 0; i < 4; i++) 
				this.tirePivots [i].transform.Rotate (Vector3.forward, movement * Time.deltaTime);			
			this.lastPos = this.transform.position;
		}
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

	//napravi kada se upre esc da se otvori menu i stavi pauza
	private void gamePaused() {

	}

	//napravi UI za novu igru
	private void displayNewGameScreen() {

	}

	//promijenimo tekst na UI
	private void updateTexts() {
		this.lapText.text = "Lap " + this.lapsPassed + "/2";
		if (this.lapsPassed == 2) {
			this.victoryText.text = "You have won!";
			this.gameObject.SetActive = false;
		}
			
	}

	//skretanje
	private void SteerCar(float steer) {
		steer = steer * 45f;
		for (int i = 0; i < 2; i++) {
			this.wheelColliders[i].steerAngle = steer;
			this.steerWheels[i].transform.localRotation = Quaternion.Euler(0f, 90f + this.wheelColliders[i].steerAngle, 0f);
		}

		this.centripetalForce (steer);
	}

	//korisnik je napravio krug i resetiramo sve markere
	private void resetMarkers() {
		for (int i = 1; i < 7; i++) {
			this.markersPassed[i] = false;
		}

		this.lapsPassed++;
		this.updateTexts ();
	}

	//provjeri da li je korisnik napravio cijeli krug
	private bool checkIfLap() {
		for (int i = 0; i < 7; i++) {
			if(this.markersPassed[i] == false)
				return false;
		}
		this.resetMarkers ();
		return true;
	}

	//oznaci pravi marker i provjeri ako je krug
	private void checkTheRightMarker(string marker) {
		int ind = int.Parse(marker.Substring (6)) - 1;
		if (ind == 0) {
			this.checkIfLap ();
		} else {
			//provjerimo da ne bi slucajno isao u suprotnom smjeru
			for(int i = 0; i < ind; i++)
				if(this.markersPassed[i] == false)
					return;

			this.markersPassed[ind] = true;
		}
	}

	//dodat cemo za skupljanje kutija
	void OnTriggerEnter(Collider col) {
		if (col.gameObject.layer == LayerMask.NameToLayer ("RoadMarkers"))
			this.checkTheRightMarker (col.ToString ().Split (' ') [0]);
	}

	//ova se metoda poziva svaki frame
	public void FixedUpdate() {
		float move = Input.GetAxis ("Vertical");
		float steer = Input.GetAxis ("Horizontal");

		if (Input.GetKey(KeyCode.Space))
			ApplyBrake ();

		SteerCar (steer);	

		this.thrustTorque = move * this.speed * 1.66f;
		MoveCar (steer);
		this.RotateTheTires (-move);	
	}
}

