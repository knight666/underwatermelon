﻿using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {

	public Vector3 startPosition;
	public Rigidbody2D rb;
	public float suckingSpeed = 2f;
	public float puffForce = 100f;
	public GameObject PuffFish;
	public AudioClip audioSuck;
	public AudioClip audioPuff;
	AudioSource audio;
	public GameObject suckEffect;
	GameObject curSuckEffect;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.A)){
			rb.AddForce(Vector2.left*40,ForceMode2D.Impulse);
		}
		if(Input.GetKey(KeyCode.D)){
			rb.AddForce(Vector2.right*40,ForceMode2D.Impulse);
		}
		if(Input.GetKey(KeyCode.W)){
			rb.AddForce(Vector2.up*40,ForceMode2D.Impulse);
		}
		if(Input.GetKey(KeyCode.S)){
			rb.AddForce(Vector2.down*40,ForceMode2D.Impulse);
		}



		if(Input.GetKey(KeyCode.F)) {
			Suck();
		}
		if(Input.GetKeyDown(KeyCode.G)) {
			Puff();
		}
	}


	public void Suck(){
		if (!audio.isPlaying)
			audio.PlayOneShot(audioSuck, 1.0f);

		RaycastHit2D[] hits = Physics2D.CircleCastAll(new Vector2(transform.position.x,transform.position.y),10,(new Vector2(rb.velocity.x,rb.velocity.y)).normalized);
		foreach(RaycastHit2D h in hits){
			if(h.rigidbody != null){
				h.rigidbody.AddForce((transform.position-h.transform.position).normalized*suckingSpeed);
			}
		}
		if(curSuckEffect == null){
			curSuckEffect = Instantiate(suckEffect);
			curSuckEffect.transform.SetParent(transform);
			curSuckEffect.transform.position = transform.position + (new Vector3(rb.velocity.x,rb.velocity.y,0)).normalized;
		}

	}

	public void Puff(){
		audio.PlayOneShot(audioPuff, 1.0f);

		RaycastHit2D[] hits = Physics2D.CircleCastAll(new Vector2(transform.position.x,transform.position.y),2,Vector2.zero);
		foreach(RaycastHit2D h in hits){
			if(h.rigidbody != null){
				h.rigidbody.AddForce((h.transform.position-transform.position).normalized*puffForce,ForceMode2D.Impulse);

				if(h.transform.gameObject.tag == "Watermelon"){
					h.transform.gameObject.GetComponent<WaterMelonController>().HitMe(3f);
				}
			}
		}
		StartCoroutine(PuffScale());
	}


	IEnumerator PuffScale(){
		GameObject pfish = (GameObject)Instantiate(PuffFish,transform.position,transform.rotation);
		pfish.transform.SetParent(transform);
		yield return new WaitForSeconds(0.2f);
		Destroy(pfish);
	}



	public void Reset(){
		transform.position = startPosition;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = 0.0f;
		transform.rotation = Quaternion.identity;
	}

}
