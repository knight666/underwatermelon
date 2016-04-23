using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{

	Transform camTransform;
	
	public float seconds = 1f;
	
	public float shakeAmount = 0.3f;

	Vector3 originalPos;
	
	void Awake(){
		camTransform = Camera.main.transform;
	}
	
	void OnEnable(){
		//originalPos = camTransform.localPosition;
		StartCoroutine ("Shake", seconds);
	}

	IEnumerator Shake(float s){
		//if(Camera.main.GetComponent<MotionBlur>())
		Vector3 orgPosition = transform.position;
		while (s>0) {
			camTransform.transform.position += Random.insideUnitSphere * shakeAmount*(100/seconds*s)/100;
			s-=Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
		transform.position = orgPosition;
		this.enabled = false;
	}
}