using UnityEngine;
using System.Collections;

public class SpawnParticlesOnWin : MonoBehaviour {
	public GameObject particles;

	void Update () {
		if (Input.GetKeyDown (KeyCode.P)) {
			Instantiate (particles, transform.position, Quaternion.identity);
		}
	}
}
