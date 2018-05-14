using UnityEngine;
using System.Collections;

public class InternetMessage : MonoBehaviour {

	float offsetymin,offsetymax,startxmin,startxmax;
	// Use this for initialization
	void Start () {
		startxmin=this.GetComponent<RectTransform>().offsetMin.x;
		startxmax=this.GetComponent<RectTransform>().offsetMax.x;
	}
	
	// Update is called once per frame
	void Update () {
		if(this.GetComponent<RectTransform>().offsetMin.x>-500)
		{
			offsetymin=this.GetComponent<RectTransform>().offsetMin.x;
			offsetymin-=2f;
			
			offsetymax=this.GetComponent<RectTransform>().offsetMax.x;
			offsetymax+=2f;

			this.GetComponent<RectTransform>().offsetMin = new Vector2(offsetymin, this.GetComponent<RectTransform>().offsetMin.y);
			this.GetComponent<RectTransform>().offsetMax = new Vector2(offsetymax, this.GetComponent<RectTransform>().offsetMax.y);
		}
		else
		{
			this.GetComponent<RectTransform>().offsetMin = new Vector2(startxmin, this.GetComponent<RectTransform>().offsetMin.y);
			this.GetComponent<RectTransform>().offsetMax = new Vector2(startxmax, this.GetComponent<RectTransform>().offsetMax.y);
		}
	}
}
