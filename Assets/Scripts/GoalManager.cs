using UnityEngine;
using System.Collections;

public class GoalManager : MonoBehaviour {

	public int scoreValue = 1; //Score added to the player after goal
	public GameStateController gamestate;
	public GameObject confetti;
	public GameObject explosion;

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
			StartCoroutine(GoalEffect(col.contacts[0].point));
			//gamestate.ResetGame();
			//Destroy (col.gameObject);
		}
	}

	IEnumerator GoalEffect(Vector3 pos){
		gamestate.isResetting = true;
		Camera.main.gameObject.GetComponent<CameraShake>().seconds = 1.5f;
		Camera.main.gameObject.GetComponent<CameraShake>().shakeAmount = 1f;
		Camera.main.gameObject.GetComponent<CameraShake>().enabled = true;

		Time.timeScale = 0.5f;
		Instantiate(confetti,pos,Quaternion.identity);
		Instantiate(explosion,pos,Quaternion.identity);


		foreach(Fish f in gamestate.fishes){
			f.rb.AddForce((f.gameObject.transform.position-transform.position).normalized*2000f,ForceMode2D.Impulse);
			f.enabled = false;
		}

		print(gamestate.watermelons.Count);
		for (int i = 0; i < gamestate.watermelons.Count; i++) {
			gamestate.watermelons[i].GetComponent<WaterMelonController>().DestroyWatermelon();
		}

		yield return new WaitForSeconds(1.5f);

		Time.timeScale = 1f;
		foreach(Fish f in gamestate.fishes){
			f.enabled = true;
		}
		print("RESET");
		gamestate.isResetting = false;
		gamestate.ResetGame();

	}

}
