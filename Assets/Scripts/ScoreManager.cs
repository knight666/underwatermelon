using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	public static int score1;		// Scorenumber Team 1
	public static int score2;		// Scorenumber Team 2
	Text scoreText1;  				// Score Text Team 1
	Text scoreText2;  				// Score Text Team 1

	void Awake ()
	{
		score1 = 0;
		score2 = 0;
		scoreText1 = GameObject.Find("Text1").GetComponent <Text> ();
		scoreText2 = GameObject.Find("Text2").GetComponent <Text> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		scoreText1.text = "" + score1;
		scoreText2.text = "" + score2;
	}
}