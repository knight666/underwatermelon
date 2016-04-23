using UnityEngine;
using System.Collections;

public class WaterMelonController : MonoBehaviour {

	public GameObject m_melonPrefab;
	public int m_level = 0;
	public static int MaxLevel = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Return)) {
			Split(m_level);
		}
	}

	void Split(int currentLevel) {
		if (currentLevel == MaxLevel)
			return;

		GameObject splitMelon = (GameObject)Instantiate(m_melonPrefab);
		splitMelon.GetComponent<WaterMelonController>().m_level = currentLevel + 1;
		splitMelon.transform.localScale = transform.localScale / 2;
	}
}
