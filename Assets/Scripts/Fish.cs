using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {

	public Vector3 startPosition;
	public Rigidbody2D rb;
	public float suckingSpeed = 2f;
	public float puffForce = 100f;
	public float puffRadius = 5f;
	public GameObject PuffFish;
	public AudioClip audioSuck;
	public AudioClip audioPuff;
	AudioSource audio;
	public GameObject suckEffect;
	GameObject curSuckEffect;
	bool isSucking = false;

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



		if(Input.GetKeyDown(KeyCode.F)) {
			Suck();
		}
		else if(Input.GetKeyUp(KeyCode.F)) {
			StopSuck();
		}
		if(Input.GetKeyDown(KeyCode.G)) {
			Puff();
		}


		if(rb.velocity != Vector2.zero){
			//transform.rotation.SetLookRotation(rb.velocity);


			//Vector2 v = rigidbody2D.velocity;
			float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
		}


	}


	public void Suck(){
		if (isSucking)
			return;

		isSucking = true;
		StartCoroutine(SuckRoutine());
	}


	public void StopSuck(){
		if (!isSucking)
			return;

		isSucking = false;
		Destroy(curSuckEffect);
	}

	IEnumerator SuckRoutine(){
		while(isSucking){
			RaycastHit2D[] hits = Physics2D.CircleCastAll(new Vector2(transform.position.x,transform.position.y),2,(new Vector2(rb.velocity.x,rb.velocity.y)).normalized);

			foreach(RaycastHit2D h in hits){
				if(h.rigidbody != null){

					if(h.transform.gameObject.tag == "Watermelon"){
						h.rigidbody.AddForce((transform.position-h.transform.position).normalized*suckingSpeed*5f);

					}
					else{
						h.rigidbody.AddForce((transform.position-h.transform.position).normalized*suckingSpeed);
					}
				}
			}
			if(curSuckEffect == null){
				curSuckEffect = Instantiate(suckEffect);
				curSuckEffect.transform.SetParent(transform);
				curSuckEffect.transform.rotation = transform.rotation;
				//curSuckEffect.transform.Rotate(0,0,90);
				curSuckEffect.transform.position = transform.position + (new Vector3(rb.velocity.x,rb.velocity.y,0)).normalized;
			}

			if (!audio.isPlaying)
				audio.PlayOneShot(audioSuck, 1.0f);
			

			yield return new WaitForEndOfFrame();
		}
	}


	public void Puff(){
		audio.PlayOneShot(audioPuff, 1.0f);

		RaycastHit2D[] hits = Physics2D.CircleCastAll(new Vector2(transform.position.x,transform.position.y),puffRadius,Vector2.zero);
		foreach(RaycastHit2D h in hits){
			if(h.rigidbody != null){
				h.rigidbody.AddForce((h.transform.position-transform.position).normalized*puffForce,ForceMode2D.Impulse);

				if(h.transform.gameObject.tag == "Watermelon"){
					h.rigidbody.AddForce((h.transform.position-transform.position).normalized*puffForce*20f,ForceMode2D.Impulse);
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
