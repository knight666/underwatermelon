using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Audio
{
	public class SoundTrigger : MonoBehaviour
	{
		public AudioSource audioCenter;
		public AudioSource audioMono;
		public AudioSource audioMulti;
		public AudioSource[] audioOptimized;
		GameObject audioCenterObject;
		public int roundRobinIndex = 0;
		public bool pan = true;
		private bool manageMulti = false;
		private bool manageMono = false;
		public GameObject optimizeFor = null;
		public bool enableLFE;
		public bool enableFiltering;
		public bool enableBackChannels;
		public bool enableLoop = false;
		public float volume = 1;
		private SurroundTools surroundToolsMulti;
		private SurroundTools surroundToolsMono;
		private SurroundTools surroundToolsCenter;
		public float originalVolume;
		private float _reverbDecay;

		private AudioSpeakerMode currentSpeakerMode;
		static AudioSpeakerMode[] speakerModes = {
			(AudioSpeakerMode)AudioSpeakerMode.Mono,
			(AudioSpeakerMode)AudioSpeakerMode.Stereo,
			(AudioSpeakerMode)AudioSpeakerMode.Quad,
			(AudioSpeakerMode)AudioSpeakerMode.Surround,
			(AudioSpeakerMode)AudioSpeakerMode.Prologic,
			(AudioSpeakerMode)AudioSpeakerMode.Mode5point1,
			(AudioSpeakerMode)AudioSpeakerMode.Mode7point1
		};
		[HideInInspector]public bool enableSurroundTools = false;

		public bool EnableFiltering {
			get {
				return enableFiltering;
			}
			set {
				enableFiltering = value;
				if(surroundToolsMono)
					surroundToolsMono.enableFiltering = value;
				surroundToolsMulti.enableFiltering = value;
				surroundToolsCenter.enableFiltering = value;
			}
		}

		public bool EnableLFE {
			get {
				return enableLFE;
			}
			set {
				enableLFE = value;
				if(surroundToolsMono)
					surroundToolsMono.enableLFE = value;
				surroundToolsMulti.enableLFE = value;
				surroundToolsCenter.enableLFE = value;
			}
		}

		public bool EnableBackChannels {
			get {
				return enableBackChannels;
			}
			set {
				enableBackChannels = value;
				if(surroundToolsMono)
					surroundToolsMono.enableBackChannels = value;
				surroundToolsMulti.enableBackChannels = value;
				surroundToolsCenter.enableBackChannels = value;
			}
		}

		public bool EnableLoop {
			get {
				return enableLoop;
			}
			set {
				enableLoop = value;
				audioCenter.loop = value;
				audioMulti.loop = value;
				audioMono.loop = value;
			}
		}

	
		// Use this for initialization
		void Start ()
		{
			audioCenterObject = GameObject.Find ("Main Camera/CenterSpeaker");
			var audioCenterObjectChild = new GameObject (this.gameObject.name + "Center");
			audioCenterObjectChild.transform.parent = audioCenterObject.transform;
			audioCenterObjectChild.transform.localPosition = new Vector3 ();

			if (audioMono == null) {
				audioMono = this.gameObject.AddComponent<AudioSource> ();
				manageMono = true;
			}
			if (audioMulti == null) {
				audioMulti = this.gameObject.AddComponent<AudioSource> ();
				manageMulti = true;
			}

			// Check for existing SurroundTools and add if no one is found. Also check if
			// AudioMulti and AudioMono are on the same gameobject (= one ST is enough)
			if (audioMulti.gameObject == audioMono.gameObject) {
				if(audioMulti.gameObject.GetComponent<SurroundTools>() == null)
					surroundToolsMulti = audioMulti.gameObject.AddComponent<SurroundTools> ();
			} else {
				if(audioMulti.gameObject.GetComponent<SurroundTools>() == null) 
					surroundToolsMulti = audioMulti.gameObject.AddComponent<SurroundTools> ();
				if(audioMono.gameObject.GetComponent<SurroundTools>() == null)
					surroundToolsMono = audioMono.gameObject.AddComponent<SurroundTools> ();
			}

			audioCenter = audioCenterObjectChild.AddComponent<AudioSource> ();
			surroundToolsCenter = audioCenterObjectChild.AddComponent<SurroundTools> ();
	

			// Settings
			if (manageMulti == true) {
				// OnAudioFilterRead is a custom DSP, so we need effects enabled
				audioMulti.bypassEffects = false;
				audioMulti.bypassListenerEffects = true;
				audioMulti.bypassReverbZones = true;
				audioMulti.dopplerLevel = 0;
				audioMulti.playOnAwake = false;
				audioMulti.spatialBlend = 0;
				audioMulti.loop = enableLoop;
				audioMulti.volume = volume;
			}
			if (manageMono == true) {
				audioMono.bypassEffects = false;
				audioMono.bypassListenerEffects = true;
				audioMono.bypassReverbZones = true;
				audioMono.dopplerLevel = 0;
				audioMono.playOnAwake = false;
				audioMono.spatialBlend = 1;
				audioMono.maxDistance = 5000;
				audioMono.minDistance = 4999;
				audioMono.loop = enableLoop;
				audioMono.volume = volume;
			}

			audioCenter.bypassEffects = false;
			audioCenter.bypassListenerEffects = true;
			audioCenter.bypassReverbZones = true;
			audioCenter.dopplerLevel = 0;
			audioCenter.playOnAwake = false;
			audioCenter.loop = enableLoop;
			audioCenter.volume = volume;

			// Die folgenden Optionen ermöglichen die erzwungene Wiedergabe über den Center-Kanal
			audioCenter.spatialBlend = 1;
			audioCenter.rolloffMode = AudioRolloffMode.Linear;
			audioCenter.minDistance = 20;
			audioCenter.dopplerLevel = 0;

			if (optimizeFor != null) {
				for(int i = 0; i < optimizeFor.GetComponent<RoundRobinComponent>().roundRobins.Count;) {
					audioOptimized[i] = this.gameObject.AddComponent<AudioSource> ();
					audioOptimized[i].bypassEffects = true;
					audioOptimized[i].bypassListenerEffects = true;
					audioOptimized[i].bypassReverbZones = true;
					audioOptimized[i].dopplerLevel = 0;
					audioOptimized [i].playOnAwake = false;
					audioOptimized[i].clip = optimizeFor.GetComponent<RoundRobinComponent> ().roundRobins [i];
					Debug.Log (audioOptimized [i].clip.loadState);
					i++;
				}
			}

			if (enableLFE == true) {
				if(surroundToolsMono)
					surroundToolsMono.enableLFE = true;
				surroundToolsMulti.enableLFE = true;
				surroundToolsCenter.enableLFE = true;
			}
			if (enableFiltering == true) {
				if(surroundToolsMono)
					surroundToolsMono.enableFiltering = true;
				surroundToolsMulti.enableFiltering = true;
				surroundToolsCenter.enableFiltering = true;
			}
			if (enableBackChannels == true) {
				if(surroundToolsMono)
					surroundToolsMono.enableBackChannels = true;
				surroundToolsMulti.enableBackChannels = true;
				surroundToolsCenter.enableBackChannels = true;
			}

			ChangeSpeakerConfiguration ();

			// We disable all custom DSPs if the audiosource doesn't play on start 
			// and only activate them if we trigger a sound
			if (audioMulti.gameObject != audioMono.gameObject) // If we don't have the mono object, we don't need to check these
				EnableSurroundToolsIfPlayOnAwake(surroundToolsMono, audioMono);
			EnableSurroundToolsIfPlayOnAwake(surroundToolsMulti, audioMulti);
			EnableSurroundToolsIfPlayOnAwake (surroundToolsCenter, audioCenter);

			originalVolume = audioMulti.volume;
		}

		void OnDestroy ()
		{
			CancelInvoke ("DisableDSPIfInactive");
			if (audioCenter != null) {
				Destroy (audioCenter.gameObject);
			}
			if(audioMono !=null && manageMono==true)
				Destroy (audioMono);
			if(audioMulti !=null && manageMulti==true)
				Destroy (audioMulti);
		}

		void OnAudioConfigurationChanged(bool deviceWasChanged)
		{
			ChangeSpeakerConfiguration ();
		}

		void ChangeSpeakerConfiguration () {
//			currentSpeakerMode = AudioSettings.speakerMode;
			currentSpeakerMode = AudioSettings.driverCapabilities;
			if (currentSpeakerMode == speakerModes [0] || currentSpeakerMode == speakerModes [1])
				enableSurroundTools = false;
			else
				enableSurroundTools = true;
//			Debug.Log (AudioSettings.driverCapabilities);
//			Debug.Log (currentSpeakerMode);
		}

		void EnableSurroundToolsIfPlayOnAwake(SurroundTools surroundToolInstance, AudioSource audioSourceInstance) {
			if (surroundToolInstance == true) {
				if (audioSourceInstance.playOnAwake == true && enableSurroundTools == true)
					surroundToolInstance.enabled = true;
				else // safety ...
					surroundToolInstance.enabled = false;
			} else {
				Debug.Log (gameObject.name + ": Übergebenes SurroundTool " + surroundToolInstance + " existiert nicht für Audiosource '" + audioSourceInstance.gameObject.name + "'.");
			}
		}

		public void PlaySound (AudioClip audio)
		{
			if (pan == true) {
				ManageSurroundTools (audio, surroundToolsMulti);
				audioMulti.PlayOneShot (audio);
			} else {
				ManageSurroundTools (audio, surroundToolsCenter);
				audioCenter.PlayOneShot (audio);
			}
		}

		public void PlaySoundScheduled (AudioClip audio, double time)
		{
			if (pan == true) {
				ManageSurroundTools (audio, surroundToolsMulti);
				audioMulti.clip = audio;
				audioMulti.PlayScheduled (time);
			} else {
				ManageSurroundTools (audio, surroundToolsCenter);
				audioCenter.clip = audio;
				audioCenter.PlayScheduled (time);
			}
		}

		public void SetScheduledEndTime (double time)
		{
			audioMulti.SetScheduledEndTime (time);
			audioCenter.SetScheduledEndTime (time);
			audioMono.SetScheduledEndTime (time);
		}

		public void PlaySurroundSound (GameObject gameObject)
		{
			if (pan == false) {
				ManageSurroundTools (gameObject.GetComponent<SurroundSoundClip> ().audioClipLR, surroundToolsMulti);
				ManageSurroundTools (gameObject.GetComponent<SurroundSoundClip> ().audioClipC, surroundToolsCenter);
				audioMulti.PlayOneShot (gameObject.GetComponent<SurroundSoundClip> ().audioClipLR);
				audioCenter.PlayOneShot (gameObject.GetComponent<SurroundSoundClip> ().audioClipC);
			} else {
				ManageSurroundTools (gameObject.GetComponent<SurroundSoundClip> ().audioClipLR, surroundToolsMulti);
//				ManageSurroundTools (gameObject.GetComponent<SurroundSoundClip> ().audioClipC, surroundToolsMono);
				audioMulti.PlayOneShot (gameObject.GetComponent<SurroundSoundClip> ().audioClipLR);
				audioMono.PlayOneShot (gameObject.GetComponent<SurroundSoundClip> ().audioClipC);
			}
		}

		public void PlaySurroundSoundScheduled (GameObject gameObject, double time)
		{
			if (pan == false) {
				ManageSurroundTools (gameObject.GetComponent<SurroundSoundClip>().audioClipLR, surroundToolsMulti);
				ManageSurroundTools (gameObject.GetComponent<SurroundSoundClip>().audioClipC, surroundToolsCenter);
				audioMulti.clip = gameObject.GetComponent<SurroundSoundClip> ().audioClipLR;
				audioCenter.clip = gameObject.GetComponent<SurroundSoundClip> ().audioClipC;
				audioMulti.PlayScheduled (time);
				audioCenter.PlayScheduled (time);
			} else {
				ManageSurroundTools (gameObject.GetComponent<SurroundSoundClip>().audioClipLR, surroundToolsMulti);
//				ManageSurroundTools (gameObject.GetComponent<SurroundSoundClip>().audioClipC, surroundToolsMono);
				audioMulti.clip = gameObject.GetComponent<SurroundSoundClip> ().audioClipLR;
				audioMono.clip = gameObject.GetComponent<SurroundSoundClip> ().audioClipC;
				audioMulti.PlayScheduled (time);
				audioMono.PlayScheduled (time);
			}
		}

		public void PlayRoundRobinSound (GameObject gameObject)
		{
			if (gameObject.GetComponent<RoundRobinComponent> ().roundRobins.Count > 1) {
				do {
					roundRobinIndex = Random.Range (0, gameObject.GetComponent<RoundRobinComponent> ().roundRobins.Count);
//					Debug.Log("Aktueller Indexersuch: " + roundRobinIndex);
				} while (roundRobinIndex == gameObject.GetComponent<RoundRobinComponent> ().lastIndex);
			}
//			Debug.Log("Aktueller Index: " + roundRobinIndex + " Letzter Index: " + gameObject.GetComponent<RoundRobinComponent>().lastIndex);
			PlaySound (gameObject.GetComponent<RoundRobinComponent> ().roundRobins [roundRobinIndex]);
			gameObject.GetComponent<RoundRobinComponent> ().lastIndex = roundRobinIndex;
		}

		public void PlaySequencedSound (GameObject gameObject)
		{
			if (gameObject.GetComponent<RoundRobinComponent> ().lastIndex >= gameObject.GetComponent<RoundRobinComponent> ().roundRobins.Count -1) {
				gameObject.GetComponent<RoundRobinComponent> ().lastIndex = -1;
			}
			roundRobinIndex = gameObject.GetComponent<RoundRobinComponent> ().lastIndex + 1;


			if(optimizeFor != null) {
//				Debug.Log ("Es ist: " + AudioSettings.dspTime); // Debugging Timing issues in Unity's animation system
				var temp = 0.2d + AudioSettings.dspTime;
				audioOptimized [roundRobinIndex].PlayScheduled (temp);
//				Debug.Log ("Clip spielt um: " + temp);
			}
			else {
//				PlaySound (gameObject.GetComponent<RoundRobinComponent> ().roundRobins [roundRobinIndex]);
			}
			gameObject.GetComponent<RoundRobinComponent> ().lastIndex = roundRobinIndex;
		}

		public void ActivatePanning () {
			pan = true;
		}
		public void DisablePanning () {
			pan = false;
		}

		// Checks, if and when SurroundTools need to be enabled and disabled.
		void ManageSurroundTools (AudioClip audio, SurroundTools surroundTools)
		{
			if (enableSurroundTools == true) {
				CancelInvoke ();
				surroundTools.enabled = true;
				if (gameObject.GetComponent<AudioReverbFilter> ()) {
					_reverbDecay = gameObject.GetComponent<AudioReverbFilter> ().decayTime;
				}
				else {
					_reverbDecay = 0;
				}
				Invoke ("DisableDSPIfInactive", audio.length + 0.2f + _reverbDecay);
			}
		}

		// Deactivate CPU consuming custom surround DSPs if no audio source is playing.
		public void DisableDSPIfInactive() {
			if (audioCenter.isPlaying==false) {
				surroundToolsCenter.enabled = false;
			}
			if (audioMono.isPlaying==false) {
				if(surroundToolsMono)
					surroundToolsMono.enabled = false;
			}
			if (audioMulti.isPlaying==false) {
				surroundToolsMulti.enabled = false;
			}
		}
	}
}
