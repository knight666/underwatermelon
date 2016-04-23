using UnityEngine;
using System.Collections;

public class bubblesOnVelocity : MonoBehaviour {

	public static float defaultAmount;
	private GameObject ball;
	private Rigidbody2D ballRb;
	private ParticleSystem particles;
	private 

	void OnEnable(){
		if (ball == null) {
			ball = gameObject;
		}
		if (particles == null) {
			particles = GetComponent<ParticleSystem> ();
		}
		if (ball != null) {
			ballRb = ball.GetComponent<Rigidbody2D> ();
		}
	}

	// Update is called once per frame
	void Update () {
		float rbVel = ballRb.velocity.magnitude;
		ParticleSystem.EmissionModule em = particles.emission;
		ParticleSystem.MinMaxCurve mm = new ParticleSystem.MinMaxCurve ();
		mm.constantMax = defaultAmount + rbVel;
		em.rate = mm;
	}
}
