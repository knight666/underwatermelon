using UnityEngine;
using System.Collections;
using Assets.Scripts.Audio;

public class dspASR : MonoBehaviour {

	public GameObject objectOfInterest;
	public AudioClip audioclipAttack;
	public AudioClip audioclipSustain;
	public AudioClip audioclipRelease;
	public SurroundSoundClip surroundAttack;
	public SurroundSoundClip surroundSustain;
	public SurroundSoundClip surroundRelease;
	public SoundTrigger[] soundTriggers;
	public float volume;
	private GameObject[] soundTriggerObjects;
	private AudioClip[] _clipASR;
	private SurroundSoundClip[] _surroundASR;
	private bool _playing = false;
	public int active = 0;
	public bool eventControlled = false;


	// Use this for initialization
	void Start () {
		if (objectOfInterest == null && eventControlled == false)
			Debug.LogWarning (gameObject.name + ": Kein Objekt angegeben. dspASR wird nichts machen.");
		if (audioclipAttack && audioclipSustain && audioclipRelease) {
			_clipASR = new AudioClip[3] { audioclipAttack, audioclipSustain, audioclipRelease };
			Debug.Log (gameObject + ": AudioClips geladen");
		} else {
			if (surroundAttack && surroundSustain && surroundRelease) {
				_surroundASR = new SurroundSoundClip[3] { surroundAttack, surroundSustain, surroundRelease };
				Debug.Log (gameObject + ": SurroundClips geladen");
			} else {
				Debug.Log (gameObject + ": Keine bzw. nicht genügend Clips gefunden!");
			}
		}

		soundTriggers = new SoundTrigger[3];
		soundTriggerObjects = new GameObject[3];

		for (int i = 0; i < 3; i++) {
			soundTriggerObjects[i] = new GameObject(gameObject.name + "Audio" + i);
			soundTriggerObjects[i].transform.parent = gameObject.transform;
			soundTriggerObjects[i].transform.localPosition = new Vector3 ();
			soundTriggers [i] = soundTriggerObjects[i].AddComponent<SoundTrigger>();
		}
		soundTriggers [1].enableLoop = true;
		soundTriggers [0].volume = volume;
		soundTriggers [1].volume = volume;
		soundTriggers [2].volume = volume;
	}
	
	// Update is called once per frame
	void Update () {
		// if we watch an object
		if (objectOfInterest != null && eventControlled == false) {
			if (objectOfInterest.activeSelf && _playing == false) {
				TriggerAS ();
			}
			if(objectOfInterest.activeSelf == false &&_playing==true) {
				TriggerR ();
			}
		}
		// If we received an event from Animation
		if (eventControlled == true) {
			if (active == 1 && _playing == false) {
				TriggerAS ();
			}
			if (active == 0 &&_playing==true) {
				TriggerR ();
			}
		}
	}

	void TriggerAS ()
	{
		_playing = true;
		var time = AudioSettings.dspTime + 0.1f;
		if (_clipASR != null) {
			soundTriggers [0].PlaySoundScheduled (audioclipAttack, time);
			soundTriggers [0].SetScheduledEndTime (time + audioclipAttack.length);
			soundTriggers [1].PlaySoundScheduled (audioclipSustain, time + audioclipAttack.length);
		}
		else {
			if (_surroundASR != null) {
				soundTriggers [0].PlaySurroundSoundScheduled (surroundAttack.gameObject, time);
				soundTriggers [0].SetScheduledEndTime (time + surroundAttack.audioClipC.length);
				soundTriggers [1].PlaySurroundSoundScheduled (surroundSustain.gameObject, time + surroundAttack.audioClipC.length);
			}
			else {
				Debug.Log (gameObject + "Nothing to play.");
			}
		}
	}

	void TriggerR ()
	{
		_playing = false;
		var time = AudioSettings.dspTime + 0.1f;
		if (_clipASR != null) {
			soundTriggers [1].SetScheduledEndTime (time);
			soundTriggers [2].PlaySoundScheduled (audioclipRelease, time);
		}
		else {
			if (_surroundASR != null) {
				soundTriggers [1].SetScheduledEndTime (time);
				soundTriggers [2].PlaySurroundSoundScheduled (surroundRelease.gameObject, time);
			}
		}
	}
}
