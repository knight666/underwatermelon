using UnityEngine;
using System.Collections;

public class WaterMelonController : MonoBehaviour {

	public GameObject m_melonPrefab;
	public int m_level = 0;
	public float hitpoints = 10f;
	public static int MaxLevel = 1;
	public float splitVelocity = 5f;
	GameStateController gamestate;
	bool canBeDestroyed = false;

	// Use this for initialization
	void Start () {
		gamestate = GameObject.Find("GameState").GetComponent<GameStateController>();
		StartCoroutine(WaitToBeDestroyable());
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Return)) {
			Split(m_level);
		}
	}

	IEnumerator WaitToBeDestroyable(){
		yield return new WaitForSeconds(1f);
		canBeDestroyed = true;
	}


	void Split(int currentLevel) {
		if (currentLevel < MaxLevel){
			GameObject splitMelonLeft = (GameObject)Instantiate(m_melonPrefab);
			splitMelonLeft.GetComponent<WaterMelonController>().m_level = currentLevel + 1;
			splitMelonLeft.transform.localScale = transform.localScale / 2;
			splitMelonLeft.transform.localPosition = splitMelonLeft.transform.position + new Vector3(transform.localScale.x,0,0);
			splitMelonLeft.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;
			gamestate.watermelons.Add(splitMelonLeft);

			GameObject splitMelonRight = (GameObject)Instantiate(m_melonPrefab);
			splitMelonRight.GetComponent<WaterMelonController>().m_level = currentLevel + 1;
			splitMelonRight.transform.localScale = transform.localScale / 2;
			splitMelonRight.transform.position = splitMelonRight.transform.position + new Vector3(-transform.localScale.x,0,0);
			splitMelonRight.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;
			gamestate.watermelons.Add(splitMelonRight);

		}


		gamestate.OnWatermelonDestroyed(gameObject);

		Destroy(gameObject);
	}


	void OnCollisionEnter2D(Collision2D c){
		if(c.gameObject.tag == "Watermelon" || !canBeDestroyed){
			return;
		}

		Rigidbody2D rbOther = c.gameObject.GetComponent<Rigidbody2D>();
		if(rbOther != null){
			float force = (rbOther.mass+rbOther.velocity.sqrMagnitude);
			HitMe(force*0.01f);
		}
	}


	public void HitMe(float val){
		hitpoints -= val;
		print(hitpoints);
		if(hitpoints <= 0){
			Split(m_level);
		}
	}

}
