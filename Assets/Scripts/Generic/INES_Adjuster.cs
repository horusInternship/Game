using UnityEngine;
using System.Collections;

public class INES_Adjuster : MonoBehaviour {

	void LateUpdate () {
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (Screen.height/480f)*276);
		this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (Screen.height/480f)*512);
	}
}
