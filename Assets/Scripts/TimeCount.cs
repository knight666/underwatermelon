using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System;
using System.Timers;


public class TimeCount : MonoBehaviour {

	public Text timerTextfield;
	private int countTime;
	private Timer timer;

		// Use this for initialization
		void Start () {

			countTime = 120;

			timer = new Timer(1000);

		timerTextfield = GameObject.Find ("Timer").GetComponent <Text> ();

			timer.Elapsed += new ElapsedEventHandler(TimerTick);
			timer.Start();

		}
		void TimerTick(object o, System.EventArgs e)
		{
			countTime--;
			//Debug.Log(countTime);

		}
		// Update is called once per frame
		void Update () {

			if (countTime > 0) {

			} else {

				timer.Stop();

			}

			TimeSpan t = TimeSpan.FromSeconds (countTime);

			timerTextfield.text = string.Format ("{0:0}:{1:00}", t.Minutes, t.Seconds);

		}
}