using UnityEngine;
using System.Collections;

public class ASRTrigger : MonoBehaviour {

	public GameObject callTo;

	public void informCallTo (int status) {
		callTo.GetComponent<dspASR> ().active = status;
	}

	void OnDisable () {
		informCallTo(0);
	}
}
