using UnityEngine;
using System.Collections;

public class Self_Destroy : MonoBehaviour {

	public float DestroyAfter = 1f;

	// Use this for initialization
	void Start () {
		StartCoroutine(WaitAndDestroy());
	}


	IEnumerator WaitAndDestroy()
	{
		yield return new WaitForSeconds(DestroyAfter);
		ParticleSystem.SubEmittersModule m = GetComponent<ParticleSystem> ().subEmitters;
		m.birth0 = null;
		StartCoroutine (WaitAndDestroy2 ());
	}
	IEnumerator WaitAndDestroy2(){
		yield return new WaitForSeconds (DestroyAfter);
		Destroy(this.gameObject);
	}

}
