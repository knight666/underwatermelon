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
		if(Input.GetKeyDown(KeyCode.Space)){
			rb.AddForce(Vector2.left,ForceMode2D.Impulse);
		}


	}
}
