using UnityEngine;
using System.Collections.Generic;

public class GameStateController : MonoBehaviour {

	public GameObject m_melonPrefab;
	public GameObject fishPrefab;
	public List<GameObject> watermelons = new List<GameObject>();
	public List<Fish> fishes = new List<Fish>();
	public List<Vector3> fishPositions = new List<Vector3>();
	public bool isResetting = false;

	// Use this for initialization
	void Start () {
		foreach(Fish f in fishes){
			f.startPosition = f.gameObject.transform.position;
		}
		SpawnWatermelon();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.R)) {
			ResetGame();
		}
	}


	public void SpawnWatermelon(){
		if(isResetting){
			return;
		}
		GameObject newMelon = (GameObject)Instantiate(m_melonPrefab);
		watermelons.Add(newMelon);

	}

	public void OnWatermelonDestroyed(GameObject g){
		watermelons.Remove(g);

		if(watermelons.Count <= 0){
			SpawnWatermelon();
		}
	}



	public void ResetGame(){

		foreach(Fish f in fishes){
			f.Reset();
		}
		foreach(GameObject g in watermelons){
			Destroy(g);
		}
		SpawnWatermelon();
	}


}
