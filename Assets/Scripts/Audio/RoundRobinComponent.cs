using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RoundRobinComponent : MonoBehaviour {
    
	public int lastIndex = -1; // The index of the last triggered sound
	[SerializeField][HideInInspector]public List<AudioClip> roundRobins = new List<AudioClip> ();

	void Awake() {
		lastIndex = -1;
	}

	void OnDestroy() {
		lastIndex = -1;
	}
	void OnApplicationQuit() {
		lastIndex = -1;
	}
	void Reset() {
		lastIndex = -1;
	}
	void OnDisable() {
		lastIndex = -1;
	}
}
