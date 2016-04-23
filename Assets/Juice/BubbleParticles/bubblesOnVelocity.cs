using UnityEngine;
using System.Collections;

public class bubblesOnVelocity : MonoBehaviour {

	public float defaultAmount;
	private GameObject ball;
	private Rigidbody2D ballRb;
	private ParticleSystem particles;

	void OnEnable(){
		if (ball == null) {
			ball = transform.parent.gameObject;
		}
		if (GetComponent<ParticleSystem> () != null) {
			particles = GetComponent<ParticleSystem> ();
		} 

		if (ball != null) {
			ballRb = ball.GetComponent<Rigidbody2D> ();
		}
	}

	// Update is called once per frame
	void Update () {
		//change position to ball position
		transform.position = ball.transform.position;

		//change emission based on velocity of ball
		float rbVel = ballRb.velocity.magnitude*5;
		ParticleSystem.EmissionModule em = particles.emission;
		ParticleSystem.MinMaxCurve mm = new ParticleSystem.MinMaxCurve ();
		mm.constantMax = defaultAmount + rbVel;
		em.rate = mm;


	}
}
