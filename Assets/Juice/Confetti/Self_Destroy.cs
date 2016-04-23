using UnityEngine;
using System.Collections;

public class Self_Destroy : MonoBehaviour {

	public float DestroyAfter = 1f;
	private ParticleSystem p;
	// Use this for initialization
	void Start () {
		p = GetComponent<ParticleSystem> ();
		StartCoroutine(WaitAndDestroy());
	}


	IEnumerator WaitAndDestroy()
	{
		yield return new WaitForSeconds(DestroyAfter);
		if (p != null) {
//			ParticleSystem.EmissionModule em = p.emission;
//			ParticleSystem.MinMaxCurve mm = new ParticleSystem.MinMaxCurve ();
//			mm.constantMax = 0;
//			mm.constantMin = 0;
//			mm.mode = ParticleSystemCurveMode.Constant
//			em.rate = mm;

			
		}
		StartCoroutine (WaitAndDestroy2 ());
	}
	IEnumerator WaitAndDestroy2(){
		yield return new WaitForSeconds (DestroyAfter);
		Destroy(this.gameObject);
	}

}
