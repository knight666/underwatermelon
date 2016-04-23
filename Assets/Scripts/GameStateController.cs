using UnityEngine;
using System.Collections.Generic;

public class GameStateController : MonoBehaviour {

	public GameObject m_melonPrefab;
	public List<GameObject> watermelons = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void SpawnWatermelon(){
		GameObject newMelon = (GameObject)Instantiate(m_melonPrefab);
		watermelons.Add(newMelon);

	}

	public void OnWatermelonDestroyed(GameObject g){
		watermelons.Remove(g);

		if(watermelons.Count <= 0){
			SpawnWatermelon();
		}
	}


}
