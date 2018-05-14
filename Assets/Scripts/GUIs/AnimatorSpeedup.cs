using UnityEngine;
using System.Collections;

public class AnimatorSpeedup : MonoBehaviour {
	bool speedingup=false;
	public void SpeedUp(){
		Debug.Log ("SPEEDUP");
		if(!speedingup){
			ChangeSpeed(this.transform, true);
			speedingup=true;
		}
	}

	public void SpeedDown(){
		Debug.Log ("SPEEDDOWN");
		
		ChangeSpeed(this.transform, false);
		speedingup=false;
	}


	public void ChangeSpeed(Transform t, bool up){
		if(t.GetComponent<Animator>()!=null){
			t.GetComponent<Animator>().speed = up? 5 : 1;
		}
		for(int i=0; i<t.childCount; i++){
			ChangeSpeed(t.GetChild(i), up);
		}
	}
}
