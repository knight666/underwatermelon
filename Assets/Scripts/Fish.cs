using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {

	public Vector3 startPosition;
	public Rigidbody2D rb;

	// Use this for initialization
	void Start () {
	
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public void Reset(){
		transform.position = startPosition;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = 0.0f;
		transform.rotation = Quaternion.identity;
	}

}
