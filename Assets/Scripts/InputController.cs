using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	// Who controls this object.
	public string control;

	// Variables for the gamepads
	protected float gamepadXAxis;
	protected float gamepadYAxis;
	protected float gamepadAngle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Gamepad variables
		gamepadXAxis = Input.GetAxis(control + "_X");
		gamepadYAxis = Input.GetAxis(control + "_Y");

		// rotate the Balloon (only if one of the gamepad axis is actually moved)
		if(gamepadXAxis != 0 || gamepadYAxis != 0 ){
			// calculate the angle from the gamepad axis and rotate it yeah 8-) 
			gamepadAngle = Mathf.Atan2(gamepadXAxis, gamepadYAxis) * Mathf.Rad2Deg;
			transform.eulerAngles = new Vector3(0, 0, gamepadAngle - 180);
		}

	}
}
