using UnityEngine;
using System.Collections;

public class impulsetest : MonoBehaviour {

	public Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.A)){
			rb.AddForce(Vector2.left*40,ForceMode2D.Impulse);
		}
		if(Input.GetKey(KeyCode.D)){
			rb.AddForce(Vector2.right*40,ForceMode2D.Impulse);
		}
		if(Input.GetKey(KeyCode.W)){
			rb.AddForce(Vector2.up*40,ForceMode2D.Impulse);
		}
		if(Input.GetKey(KeyCode.S)){
			rb.AddForce(Vector2.down*40,ForceMode2D.Impulse);
		}

	}
}
