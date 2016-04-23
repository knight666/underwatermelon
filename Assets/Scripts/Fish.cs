using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {

	public Vector3 startPosition;
	public Rigidbody2D rb;
	public float suckingSpeed = 2f;
	public float puffForce = 100f;
	public GameObject PuffFish;

	// Use this for initialization
	void Start () {
	
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.F)) {
			Suck();
		}
		if(Input.GetKeyDown(KeyCode.G)) {
			Puff();
		}
	}


	public void Suck(){
		RaycastHit2D[] hits = Physics2D.CircleCastAll(new Vector2(transform.position.x,transform.position.y),10,(new Vector2(rb.velocity.x,rb.velocity.y)).normalized);
		foreach(RaycastHit2D h in hits){
			if(h.rigidbody != null){
				h.rigidbody.AddForce((transform.position-h.transform.position).normalized*suckingSpeed);
			}
		}
	}

	public void Puff(){
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
