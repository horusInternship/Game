using UnityEngine;
using System.Collections;

public class PlayerStateAnim : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Animator>().SetInteger("player",PlayerData.picked_playerid);
	}
}
