using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CarMovement : MonoBehaviour {

	//podatkovni clanovi klase za kretanje autica
	//kola i pivoti, pivoti se koriste za okretanje kola
	private GameObject LeftBackTire;
	private GameObject LeftBackTirePivot;
	private GameObject RightBackTire;
	private GameObject RightBackTirePivot;
	private GameObject LeftFrontTire;
	private GameObject LeftFrontTirePivot;
	private GameObject RightFrontTire;
	private GameObject RightFrontTirePivot;

	//ova se metoda pokrece na pocetku 
	//NIJE KONSTRUKTOR i ne smiju se koristiti
	public void Start() {
		this.LeftBackTire = GameObject.Find("simpleCar/LeftBackTirePivot/LeftBackTire");
		this.LeftBackTirePivot = GameObject.Find ("simpleCar/LeftBackTirePivot");
		this.RightBackTire = GameObject.Find("simpleCar/RightBackTirePivot/RightBackTire");
		this.RightBackTirePivot = GameObject.Find("simpleCar/RightBackTirePivot");
		this.LeftFrontTire = GameObject.Find("simpleCar/LeftFrontTirePivot/LeftFrontTire");
		this.LeftFrontTirePivot = GameObject.Find("simpleCar/LeftFrontTirePivot");
		this.RightFrontTire = GameObject.Find("simpleCar/RightFrontTirePivot/RightFrontTire");
		this.RightFrontTirePivot = GameObject.Find("simpleCar/RightFrontTirePivot");
	}

	//ova se metoda poziva svaki frame
	public void FixedUpdate()
	{
		float move = Input.GetAxis ("Horizontal");

		this.LeftBackTire.transform.RotateAround (this.LeftBackTirePivot.transform.position, Vector3.right, 50*Time.deltaTime); 
	}
}

