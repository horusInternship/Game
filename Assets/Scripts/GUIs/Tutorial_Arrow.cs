using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial_Arrow : MonoBehaviour {
	GameObject obj_ref;
	Vector2 offset = Vector2.zero;
	bool obj_world_coords = true;
	int curr_tutorial=0;
	public void LateUpdate(){

		if(GlobalData.current_tutorial==8 || GlobalData.current_tutorial==0)
		{
			FollowObj();
		}
		else
		{
			if(curr_tutorial!=GlobalData.current_tutorial)
			{
				Debug.Log ("Entered Here");
				FollowObj();

			}
		}
	}

	 
	public void Init(GameObject obj_ref, Vector2 offset, bool obj_world_coords){
		this.obj_ref = obj_ref;
		this.offset = offset;
		this.obj_world_coords = obj_world_coords;
		FollowObj();
	}

	private void FollowObj(){
		if(obj_ref != null){
			Vector2 spot_pos;
			if(obj_world_coords){
				spot_pos = RectTransformUtility.WorldToScreenPoint(Camera.main, obj_ref.transform.position);
			}else{
				spot_pos = obj_ref.GetComponent<RectTransform>().position;
			}
			float angle = (Mathf.Rad2Deg * Mathf.Atan2(spot_pos.x-(spot_pos.x+offset.x*100), 
			                                           spot_pos.y-(spot_pos.y+offset.y*100)));
			this.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0,0,angle);
			Vector3 newpos = new Vector3 (spot_pos.x+100*offset.x, spot_pos.y+100*offset.y,0);

			//Debug.Log (angle);
			this.GetComponent<RectTransform>().position= new Vector3 (spot_pos.x+100*offset.x, spot_pos.y+100*offset.y,0);

		}else{
			this.GetComponent<RectTransform>().position = new Vector3(-Screen.width, -Screen.height, 0);
		}
		curr_tutorial=GlobalData.current_tutorial;
	}
	
}
