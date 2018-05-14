using UnityEngine;
using System.Collections;

public class energyparticle : MonoBehaviour {

	Vector3 FinalPosition=new Vector3(10,10,0);
	Vector3 anim_start;
	float anim_timer=0f;

	// Use this for initialization
	void Start () {
		//FinalPosition=transform.TransformPoint(GameObject.Find("InGame").gameObject.transform.FindChild("TD").gameObject.transform.FindChild("energy_bar").transform.FindChild("txt").GetComponent<RectTransform>().anchoredPosition);
		GameObject Energynum=GameObject.Find("InGame").gameObject.transform.Find("TD").gameObject.transform.Find("energy_bar").transform.Find("txt").gameObject;

		FinalPosition=Camera.main.ScreenToWorldPoint(Energynum.GetComponent<RectTransform>().transform.position);
	}

	public void Init(int bonus){
	

		//PlayerData.energy_queue.Add(bonus);
		//PlayerData.current_score += bonus;
	}

	
	// Update is called once per frame
	void FixedUpdate () {
		anim_start = transform.position;
		if(anim_timer <= 0.5f){
			anim_timer+=Time.fixedDeltaTime;
			transform.position = Vector3.Lerp(anim_start, FinalPosition, anim_timer);
		}
		else
		{
			Destroy (this.gameObject);
		}
	}
}
