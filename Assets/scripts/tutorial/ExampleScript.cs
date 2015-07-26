using UnityEngine;
using System.Collections;

// This adds a rigidbody component
[RequireComponent (typeof (Rigidbody))] 

public class ExampleScript : MonoBehaviour {

	public GameObject myGameObject;
	private Rigidbody myRigidBody;

	// Use this for initialization
	void Start () {
		// pressumes rigid body exists
		myRigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		// myGameObject.transform.position = Vector3.up * Time.deltaTime;
		myRigidBody.AddForce(Vector3.up * 50);
	}
}
