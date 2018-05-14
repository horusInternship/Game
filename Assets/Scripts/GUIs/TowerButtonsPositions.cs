using UnityEngine;
using System.Collections;

public class TowerButtonsPositions : MonoBehaviour {
	private GameObject tower_spot;
	private GameObject UI_Tutorial;
	private Vector3 start_pos;
	private Vector3 offset = Vector3.zero;
	private float Cameraorto, oldorto;
	private float camerstartortosize=4.221169f;

	void Start(){
		oldorto=camerstartortosize;
	}
	void FixedUpdate () {
		Reposition();
	}


	void Reposition(){
		Cameraorto=Camera.main.orthographicSize;
		UI_Tutorial=GameObject.Find("Canvas").transform.Find("InGame").transform.Find("Tutorial").gameObject;
		if(tower_spot != null){
			Vector2 spot_pos = RectTransformUtility.WorldToScreenPoint(Camera.main, tower_spot.transform.position);

				this.GetComponent<RectTransform>().position= new Vector3 (spot_pos.x+offset.x, spot_pos.y+offset.y,0);
		}
		if(Cameraorto!=oldorto)
		{
			float currvalue=this.GetComponent<RectTransform>().localScale.x;
			currvalue=currvalue*oldorto/Cameraorto;
			this.GetComponent<RectTransform>().localScale=new Vector2(currvalue,currvalue);
			oldorto=Cameraorto;
		}
	}


	public void SetStartPos(GameObject tower_spot){


		this.tower_spot = tower_spot;
		Reposition();
	}

	public void SetStartPos(GameObject tower_spot, Vector3 offset){
		this.offset = offset;
		this.tower_spot = tower_spot;
		Reposition();
	}
}
