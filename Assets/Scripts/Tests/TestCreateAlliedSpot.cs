using UnityEngine;
using System.Collections;

public class TestCreateAlliedSpot : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateAlliedSpot(){
		GameObject SelectedIngameAlly = (GameObject)Instantiate(Resources.Load("Towers/TowerSpot"));
		SelectedIngameAlly.name="TowerSpot1";
	}
}
