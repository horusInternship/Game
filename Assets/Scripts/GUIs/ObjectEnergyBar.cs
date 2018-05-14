using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ObjectEnergyBar : MonoBehaviour {
	//JoaoBarFollow

	Vector2 pos,viewportPoint;
	RectTransform thisobjrect;
	GameObject FollowingOBJ;

	string obj_type="";

	//Bar Stuff
	float timer=0;
	float time=0.5f;
	public int max_health,curr_health;
	float energytobar;
	Image bar;
	public bool update_energy=false;
	
	public void Init(GameObject FollowedOBJ) {

		FollowingOBJ=FollowedOBJ;
		thisobjrect=this.gameObject.GetComponent<RectTransform>();
		obj_type=this.transform.parent.name;
	
		if(obj_type=="tower_bars")
		{
			max_health=GlobalData.TOWERSUPGRADEVALUES[FollowingOBJ.GetComponent<TowerControl>().status.type][FollowingOBJ.GetComponent<TowerControl>().status.upgrade_level].health;
		}
		else
		{
			max_health=FollowingOBJ.GetComponent<EnemyControl>().status.health;
			//Enemies Updates
		}
		bar =  this.transform.GetChild(1).GetComponent<Image>();
		bar.fillAmount = 1f;

		BarRepositioning();



	}
	
	void Update () {
		if(FollowingOBJ!=null) {
			BarRepositioning();
			if(update_energy)
				Update_EnergyBar();
		} else {
			Destroy(this.gameObject);
		}
	}


	void BarRepositioning(){


			pos=FollowingOBJ.transform.position;
			pos=new Vector2(pos.x,pos.y+0.6f);


			viewportPoint = Camera.main.WorldToViewportPoint(pos);
			
			thisobjrect.anchorMax=new Vector2(viewportPoint.x+0.04f,viewportPoint.y+0.005f); 
			thisobjrect.anchorMin=new Vector2(viewportPoint.x-0.04f,viewportPoint.y-0.005f); 
			
			thisobjrect.offsetMax=new Vector2(0,0);
			thisobjrect.offsetMin=new Vector2(0,0);
	}
	



	void Update_EnergyBar(){

		if(obj_type=="tower_bars")
		{
			curr_health=FollowingOBJ.GetComponent<TowerControl>().status.health;
			max_health=GlobalData.TOWERSUPGRADEVALUES[FollowingOBJ.GetComponent<TowerControl>().status.type][FollowingOBJ.GetComponent<TowerControl>().status.upgrade_level].health;
		}
		else
		{
			curr_health=FollowingOBJ.GetComponent<EnemyControl>().status.health;
			//Enemies Updates
		}

		energytobar = 1f*curr_health/max_health;
		bar.fillAmount = energytobar;
		update_energy=false;
	}
	//JoaoBarFollow
}
