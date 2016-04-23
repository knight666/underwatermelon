using UnityEngine;
using System.Collections;
#if UNITY_EDITOR_WIN && UNITY_STANDALONE_WIN
using XInputDotNetPure;
#endif

public class InputController : MonoBehaviour {

	// Who controls this object.
	public int index;
	public string control;

	// Variables for the gamepads
	protected float gamepadXAxis;
	protected float gamepadYAxis;
	protected float gamepadAngle;
	protected float gamepadLBumper;

	protected Fish fishScript;
	protected Rigidbody2D rb;

	public float speed;
	public float maxSpeed;

	#if UNITY_EDITOR_WIN && UNITY_STANDALONE_WIN
	GamePadState state;
	GamePadState prevState;
	#endif

	// Use this for initialization
	void Start () {
		fishScript = this.gameObject.GetComponent<Fish> ();
		rb = this.gameObject.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Gamepad variables
		gamepadXAxis = Input.GetAxis("Gamepad" + index + control + "_X");
		gamepadYAxis = Input.GetAxis("Gamepad" + index + control + "_Y");
		gamepadLBumper = Input.GetAxis ("Gamepad" + index + control + "_Suck");

		#if UNITY_EDITOR_WIN && UNITY_STANDALONE_WIN
		// Xinput States
		prevState = state;
		state = GamePad.GetState((PlayerIndex)index-1);
		#endif


		// Rotation
		if(gamepadXAxis != 0 || gamepadYAxis != 0 ){
			// calculate the angle from the gamepad axis and rotate it yeah 8-) 
			gamepadAngle = Mathf.Atan2(gamepadXAxis, gamepadYAxis) * Mathf.Rad2Deg;
			transform.eulerAngles = new Vector3(0, 0, gamepadAngle - 180);
			rb.velocity += new Vector2(gamepadXAxis*speed,-gamepadYAxis*speed);
			rb.velocity = Vector2.ClampMagnitude (rb.velocity, maxSpeed);
		}


		#if UNITY_EDITOR_WIN && UNITY_STANDALONE_WIN
		// Actions
		// Left player sucking
		if (prevState.Triggers.Left < 1 && state.Triggers.Left == 1 && control == "L")
		{
			Debug.Log ("Player " + index + " left sucks.");
			fishScript.Suck();
		}
		if (prevState.Triggers.Left == 1 && state.Triggers.Left < 1 && control == "L")
		{
			Debug.Log ("Player " + index + " left sucks.");
			fishScript.Suck(); //TODO: use the stopfunction when the function is there
		}


		// Right player sucking
		if (prevState.Triggers.Right < 1 && state.Triggers.Right == 1 && control == "R")
		{
			Debug.Log ("Player " + index + " right sucks.");
			fishScript.Suck();
		}
		if (prevState.Triggers.Right == 1 && state.Triggers.Right < 1 && control == "R")
		{
			Debug.Log ("Player " + index + " right sucks.");
			fishScript.Suck(); //TODO: use the stopfunction when the function is there
		}
		#endif

		// Puff
		if (Input.GetButtonDown ("Gamepad" + index + control + "_Boom")) {
			Debug.Log ("Player " + index + control + " booms.");
			fishScript.Puff ();
		}


	}
}
