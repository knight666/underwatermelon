using UnityEngine;
using System.Collections;

public class GoalManager : MonoBehaviour {

	public int scoreValue = 1; //Score added to the player after goal
	public GameStateController gamestate;

	void Start () {

	}
	

	void Update () {
//		if (Input.GetKeyDown (KeyCode.Space))		just for testing the Scorecards
//			scoring ("one");
//		if (Input.GetKeyDown (KeyCode.Return))
//			scoring ("two");
	}

	void scoring () {
		
		if (this.gameObject.name == "goal1") {
			ScoreManager.score2 += scoreValue;
		}
		if (this.gameObject.name == "goal2") {
			ScoreManager.score1 += scoreValue;
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag == "Watermelon") {

			scoring ();
			gamestate.ResetGame();
			//Destroy (col.gameObject);
		}
	}

}
