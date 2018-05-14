using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ResizeButtons : MonoBehaviour {

	void Awake(){ 
		float size = (Screen.width>Screen.height ? Screen.width/1024f : Screen.height/768f);
	//	this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
	//	this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size); 
		transform.localScale = new Vector3(size, size, size);
	}
}
