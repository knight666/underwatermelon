﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterMelonController : MonoBehaviour {

	public GameObject m_melonPrefab;
	public int m_level = 0;
	public float hitpoints = 10f;
	public static int MaxLevel = 1;
	public float splitModifier = 0.005f;
	GameStateController gamestate;
	bool canBeDestroyed = false;
	public List<AudioClip> audioHits = new List<AudioClip>();
	public AudioClip audioSplat;
	AudioSource audio;
	public GameObject splatEffect;

	// Use this for initialization
	void Start () {
		gamestate = GameObject.Find("GameState").GetComponent<GameStateController>();
		StartCoroutine(WaitToBeDestroyable());
		audio = GetComponent<AudioSource>();
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
			
		Camera.main.gameObject.GetComponent<CameraShake>().seconds = 0.5f;
		Camera.main.gameObject.GetComponent<CameraShake>().shakeAmount = 1f;
		Camera.main.gameObject.GetComponent<CameraShake>().enabled = true;
		DestroyWatermelon();
	}

	public void DestroyWatermelon(){
		gamestate.OnWatermelonDestroyed(gameObject);

		Instantiate(splatEffect,transform.position,Quaternion.identity);
		AudioSource.PlayClipAtPoint(audioSplat, Camera.main.transform.position, 1.0f);

		Destroy(gameObject);
	}


	void OnCollisionEnter2D(Collision2D c){
		if(c.gameObject.tag == "Goal"){
			//DestroyWatermelon();
		}

		if(c.gameObject.tag == "Watermelon" || !canBeDestroyed){
			return;
		}



		Rigidbody2D rbOther = c.gameObject.GetComponent<Rigidbody2D>();
		if(rbOther != null){
			float force = (rbOther.mass+rbOther.velocity.sqrMagnitude);
			HitMe(force*splitModifier);
		}
	}


	public void HitMe(float val){
		int r = (int)Random.Range(0, audioHits.Count);
		audio.PlayOneShot(audioHits[r], 1.0f);

		hitpoints -= val;
		if(hitpoints <= 0){
			Split(m_level);
		}
	}

}
