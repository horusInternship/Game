using UnityEngine;
using System.Collections;

public class TestRotatingMap : MonoBehaviour {
	public GameObject map;
	/*void OnGUI(){
		if(GUI.Button(new Rect(Screen.width/2, 0, 50, 50), "+map")){
			GameObject m = (GameObject)Instantiate(this.gameObject);
			m.transform.parent = this.transform.parent;
			m.transform.localPosition = new Vector3(0, 0, 20);
		}
	}*/

	void Start(){
		int count = 2;
	#if UNITY_IOS && !UNITY_EDITOR
		count=2;
	#elif UNITY_ANDROID && !UNITY_EDITOR
		count=1;
	#endif
		for(int i=0; i<count; i++){
			StartCoroutine(ActionAfterTimer.Set(i, delegate{
				NewMap();
			}));
		}
	}


	void NewMap(){
		GameObject m = (GameObject)Instantiate(map);
		m.transform.parent = this.transform;
		m.transform.localPosition = new Vector3(0, 0, 20);
	}
}
