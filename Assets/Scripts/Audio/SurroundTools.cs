using UnityEngine;
using System.Collections;

public class SurroundTools : MonoBehaviour { 

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
	public bool enableLFE;
	public bool enableFiltering;
	public bool enableBackChannels;

	private float cutoff = 10f;
	private float resonance = 0f;

	private float c;
	private float r;
	private float v0;
	private float v1;


	void Start ()
	{
		AudioSettings.OnAudioConfigurationChanged += OnAudioConfigurationChanged;
		currentSpeakerMode = AudioSettings.speakerMode;
//		Debug.Log (currentSpeakerMode);

		// If these values need to be changed in-game, the following lines 
		// should be executed in OnAudioFilterRead()
		c = Mathf.Pow (0.5f, (128.0f - cutoff) / 16.0f);
		r = Mathf.Pow (0.5f, (resonance + 24.0f) / 16.0f);
	}


	void OnAudioConfigurationChanged(bool deviceWasChanged)
	{
		currentSpeakerMode = AudioSettings.speakerMode;
	}

	public void OnAudioFilterRead(float[] data, int channels) {
		if (enableLFE == false && (currentSpeakerMode == speakerModes[5] || currentSpeakerMode == speakerModes[6]))
			for (var i = 3; i < data.Length; i=i+channels)
				data[i] = 0;

		if (enableFiltering == true && (currentSpeakerMode == speakerModes [5] || currentSpeakerMode == speakerModes [6])) {
			for (var i = 3; i < data.Length; i=i+channels) {
				v0 = ((1.0f - r * c) * v0) - (c * v1) + (c * data [i]);
				v1 = ((1.0f - r * c) * v1) + (c * v0);
				// Clip, if neccessary
				data [i] = Mathf.Clamp (v1, -1.0f, 1.0f);
			}
		}

		if(enableBackChannels == false) {
			// 5.1 and 7.1
			if(currentSpeakerMode == speakerModes[5] || currentSpeakerMode == speakerModes[6]) {
				// We add the SurroundLeft Channel(fifth channel) to the FrontLeft Channel and drop the SurroundLeft Channel.
				for (var i = 4; i < data.Length; i = i + channels) {
					data [i - 4] = data [i - 4] + data [i];
					data [i] = 0;
				}
				// We add the SurroundRight Channel(sixth channel) to the FrontRight Channel and drop the SurroundRight Channel.
				for (var i = 5; i < data.Length; i = i + channels) {
					data [i - 4] = data [i - 4] + data [i];
					data [i] = 0;
				}
			}
			// 7.1
			if(currentSpeakerMode == speakerModes[6]) {
				// We add the RearLeft Channel(seventh channel) to the FrontLeft Channel and drop the RearLeft Channel.
				for (var i = 6; i < data.Length; i = i + channels) {
					data [i - 6] = data [i - 6] + data [i];
					data [i] = 0;
				}
				// We add the RearRight Channel(eighth channel) to the FrontRight Channel and drop the RearRight Channel.
				for (var i = 7; i < data.Length; i = i + channels) {
					data [i - 6] = data [i - 6] + data [i];
					data [i] = 0;
				}
			}
			// 5.0 and Quad - does that still exist ...
			if(currentSpeakerMode == speakerModes[2] || currentSpeakerMode == speakerModes[3]) {
				// We add the SurroundLeft Channel(third channel) to the FrontLeft Channel and drop the SurroundLeft Channel.
				for (var i = 2; i < data.Length; i = i + channels) {
					data [i - 2] = data [i - 2] + data [i];
					data [i] = 0;
				}
				// We add the SurroundRight Channel(fourth channel) to the FrontRight Channel and drop the SurroundRight Channel.
				for (var i = 3; i < data.Length; i = i + channels) {
					data [i - 2] = data [i - 2] + data [i];
					data [i] = 0;
				}
			}
		}
	}
}
