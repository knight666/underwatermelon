using UnityEngine;
using System.Collections;

public class SpawnParticlesOnWin : MonoBehaviour {
	public GameObject particles;

	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			SpawnConfetti ();	
		}
	}

	void SpawnConfetti(){
		Instantiate (particles, Vector3.zero, Quaternion.identity);
	}
}
