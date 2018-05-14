using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class TowerSpotsGUIs : MonoBehaviour {
	public GameObject pnl_info;
	public GameObject btns;
	public int spotid=-1;
	public GameObject Tower1,upgd;

	Dictionary<int, GameObject> towers_by_spots = new Dictionary<int, GameObject>();
	GameObject UI_Tutorial;

	void Start () {
		UI_Tutorial=GameObject.Find("Canvas").transform.Find("InGame").transform.Find("Tutorial").gameObject;
		//GetMousePosition

	}
	
	void Update () {
		//if(GlobalData.tower_spot_refs.ContainsKey(spotid)){
		//	Vector2 TowerSpotPosition= RectTransformUtility.WorldToScreenPoint(Camera.main,
		//	                                                                   GlobalData.tower_spot_refs[spotid].transform.position);
			//this.GetComponent<RectTransform>().position= new Vector3 (TowerSpotPosition.x,TowerSpotPosition.y,0);
		//}
	}

	private void SetTowerRUDBtns(int active_id){
		for(int i=0; i<3; i++){
			if(active_id==-1){ // disable all
				btns.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
			}else{ // enable just the respective active_id btn disable the rest
				btns.transform.GetChild(i).GetChild(0).gameObject.SetActive(active_id == i);
			}
		}
	}
	private void SetBuildBtns(int active_id){
		for(int i=3; i<btns.transform.childCount; i++){
			if(active_id==-1){ // disable all
				btns.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
			}else{ // enable just the respective active_id btn disable the rest
				btns.transform.GetChild(i).GetChild(0).gameObject.SetActive(active_id == (i-3));
			}
		}
	}

	public void Click_Outside(){
		UI_Tutorial=GameObject.Find("Canvas").transform.Find("InGame").transform.Find("Tutorial").gameObject;
		if(!UI_Tutorial.activeSelf || GlobalData.TutCanBuildFree)
		{
			Debug.Log ("Clickoutside");
			if(pnl_info.activeSelf){
				pnl_info.SetActive(false);
				SetBuildBtns(-1);
			}

			if(Tower1.activeSelf)
				Tower1.SetActive(false);

			if(upgd.activeSelf)
				upgd.SetActive(false);

			this.gameObject.SetActive(false);
		}

	}


	public void ShowTowerRepairInfo(){
		if((!UI_Tutorial.activeSelf || GlobalData.TutCanBuildFree) || GlobalData.current_tutorial==5)
		{
			if(towers_by_spots.ContainsKey(spotid)){
				if(towers_by_spots[spotid]!=null){
					int tower_type = towers_by_spots[spotid].GetComponent<TowerControl>().status.type;
					int tower_lvl = towers_by_spots[spotid].GetComponent<TowerControl>().status.upgrade_level;
					int repair_cost = GlobalData.TOWER_Repair_COSTS[tower_type][tower_lvl];
					/*pnl_info.transform.GetChild(1).GetComponent<Text>().text = "Reparação";  
					pnl_info.transform.GetChild(2).GetComponent<Text>().text = GlobalData.Languages[15] + repair_cost;
					pnl_info.gameObject.SetActive(true);
					SetTowerRUDBtns(0);*/
				}
			}
		}
	}
	public void ShowTowerUpgradeInfo(){
		if((!UI_Tutorial.activeSelf || GlobalData.TutCanBuildFree) || GlobalData.current_tutorial==6)
		{
			if(towers_by_spots.ContainsKey(spotid)){
				if(towers_by_spots[spotid]!=null){
					int tower_type = towers_by_spots[spotid].GetComponent<TowerControl>().status.type;
					int tower_lvl = towers_by_spots[spotid].GetComponent<TowerControl>().status.upgrade_level;
					int repair_cost = GlobalData.TOWER_UPGRADE_COSTS[tower_type][tower_lvl];
					/*pnl_info.transform.GetChild(1).GetComponent<Text>().text = "Upgrade";  
					pnl_info.transform.GetChild(2).GetComponent<Text>().text = GlobalData.Languages[15] + repair_cost;
					pnl_info.gameObject.SetActive(true);
					SetTowerRUDBtns(1);*/
				}
			}
		}
	}
	public void ShowTowerDestroyInfo(){
		if((!UI_Tutorial.activeSelf || GlobalData.TutCanBuildFree)|| GlobalData.current_tutorial==7)
		{
			if(towers_by_spots.ContainsKey(spotid)){
				if(towers_by_spots[spotid]!=null){
					/*pnl_info.transform.GetChild(1).GetComponent<Text>().text = "Remover Torre";  
					pnl_info.transform.GetChild(2).GetComponent<Text>().text = "";
					pnl_info.gameObject.SetActive(true);
					SetTowerRUDBtns(2);*/
				}
			}
		}
	}

	public void ShowTowerBuildInfo(int towerid){

		//Debug.Log (ShowTowerBuildInfo);
		if(GlobalData.current_tutorial==3 && towerid==1){
			GameObject.FindGameObjectWithTag("TD_Level").GetComponent<TDLevelControl>().ActedOnTutorial();
		}

		if((!UI_Tutorial.activeSelf || GlobalData.TutCanBuildFree)  || (towerid==0 && GlobalData.current_tutorial==3)){
			pnl_info.transform.GetChild(1).GetComponent<Text>().text = GlobalData.Languages[15+towerid];
			pnl_info.transform.GetChild(2).GetComponent<Text>().text = GlobalData.Languages[15] +" "+ GlobalData.TOWER_BUILD_COSTS[towerid];
			pnl_info.gameObject.SetActive(true);
			SetBuildBtns(towerid-1);
		}
	}

	public void InstantiateTower(int towerid){
		//SetBuildBtns(-1);
		//pnl_info.SetActive(false);

		Debug.Log ("InstantiateTower "+GlobalData.current_tutorial+" towerid "+towerid);
		if((!UI_Tutorial.activeSelf || GlobalData.TutCanBuildFree)|| (GlobalData.current_tutorial==3 && towerid==1))
		{
			if((PlayerData.current_energy > GlobalData.TOWER_BUILD_COSTS[towerid])){
				if(GlobalData.current_tutorial==3){
					GameObject.FindGameObjectWithTag("TD_Level").GetComponent<TDLevelControl>().ActedOnTutorial();
				}

				Debug.Log ("InstantiateTower");
				GameObject Tower = (GameObject)Instantiate(Resources.Load(GlobalData.TOWER_PATHS[towerid]));
				//Tower.GetComponent<SpriteRenderer>().sprite= Resources.Load<Sprite>(GlobalData.TOWERSPRITEPATHS[towerid][0]);
				Tower.transform.parent=GameObject.Find(spotid.ToString()).transform;
				Tower.transform.localPosition=new Vector3(0,0,0);
				Tower.GetComponent<TowerControl>().Init(GlobalData.TOWERSUPGRADEVALUES[towerid][0]);
				if(towers_by_spots.ContainsKey(spotid)){
					towers_by_spots[spotid] = Tower;
				}else{
					towers_by_spots.Add(spotid, Tower);
				}

				PlayerData.energy_queue.Add(-GlobalData.TOWER_BUILD_COSTS[towerid]);
				SoundControl.PlaySFX(GlobalData.SFX_Paths[10], true, true, true);
				for(int i=1;i<=5;i++) {
					btns.transform.Find("btn_Tower"+i).gameObject.SetActive(false);
				}
			
				
				if(upgd.activeSelf)
					upgd.SetActive(false);

				Click_Outside();
			}
		}
	}



	public void UpgradeTower(){
		Debug.Log ("Teste UpgradeTower");

		if((!UI_Tutorial.activeSelf || GlobalData.TutCanBuildFree) || GlobalData.current_tutorial==6)
		{
			if(GlobalData.current_tutorial==6){
				GameObject.FindGameObjectWithTag("TD_Level").GetComponent<TDLevelControl>().ActedOnTutorial();
			}
			//SetTowerRUDBtns(-1);
			if(towers_by_spots.ContainsKey(spotid)){
				if(towers_by_spots[spotid]!=null){
					TowerControl tcontrol = towers_by_spots[spotid].GetComponent<TowerControl>();
					if(tcontrol.status.upgrade_level<3){

						bool can_upgrade = PlayerData.current_energy>GlobalData.TOWER_UPGRADE_COSTS[tcontrol.status.type][tcontrol.status.upgrade_level];
						if(can_upgrade){
							Debug.Log ("upgrade_level "+ tcontrol.status.upgrade_level);
							SoundControl.PlaySFX(GlobalData.SFX_Paths[13], true, true, true);
							towers_by_spots[spotid].GetComponent<TowerControl>().UpgradeTower();
							this.gameObject.SetActive(false);
						}
						else
						{
							pnl_info.SetActive(false);
						}
					}
					else
					{
						pnl_info.SetActive(false);
					}
				}
			}

			if(Tower1.activeSelf)
				Tower1.SetActive(false);
			
			if(upgd.activeSelf)
				upgd.SetActive(false);

			Click_Outside();
		}

	}
	
	public void DeleteTower(){

		Debug.Log ("Delete Tower Test "+UI_Tutorial.activeSelf+" "+GlobalData.TutCanBuildFree+" "+GlobalData.current_tutorial);
		if(!UI_Tutorial.activeSelf || GlobalData.TutCanBuildFree || GlobalData.current_tutorial==7)
		{
			if(GlobalData.current_tutorial==7){
				Debug.Log ("DeleteTower acted on tutorial");
				GameObject.FindGameObjectWithTag("TD_Level").GetComponent<TDLevelControl>().ActedOnTutorial();
			}
			//SetTowerRUDBtns(-1);
			if(towers_by_spots.ContainsKey(spotid)){
				if(towers_by_spots[spotid]!=null){

					//Give back 50% of cost
					TowerControl tcontrol = towers_by_spots[spotid].GetComponent<TowerControl>();
					int last_towercost=GlobalData.TOWER_BUILD_COSTS[tcontrol.status.type];

					Debug.Log ("upgd lev "+tcontrol.status.upgrade_level);
					if(tcontrol.status.upgrade_level==3)
					{
						last_towercost+=GlobalData.TOWER_UPGRADE_COSTS[tcontrol.status.type][2];
						last_towercost+=GlobalData.TOWER_UPGRADE_COSTS[tcontrol.status.type][1];
						last_towercost+=GlobalData.TOWER_UPGRADE_COSTS[tcontrol.status.type][0];
					}
					if(tcontrol.status.upgrade_level==2)
					{
						last_towercost+=GlobalData.TOWER_UPGRADE_COSTS[tcontrol.status.type][1];
						last_towercost+=GlobalData.TOWER_UPGRADE_COSTS[tcontrol.status.type][0];
					}
					else if(tcontrol.status.upgrade_level==1)
					{
						last_towercost+=GlobalData.TOWER_UPGRADE_COSTS[tcontrol.status.type][0];
					}
					last_towercost=(int)(last_towercost*0.5f);
					int curruserenergy=PlayerData.current_energy;
					int totalenergyafterdelete=last_towercost+curruserenergy;
					if(totalenergyafterdelete<=100)
					{
						PlayerData.energy_queue.Add(last_towercost);
					}
					else
					{
						int valuetogive=100-curruserenergy;
						PlayerData.energy_queue.Add(valuetogive);
					}

					SoundControl.PlaySFX(GlobalData.SFX_Paths[11], true, true, true);
					GameObject t = towers_by_spots[spotid];
					towers_by_spots.Remove(spotid);
					t.transform.parent = null;
					Destroy(t);
					this.gameObject.SetActive(false);
				}
			}

			if(Tower1.activeSelf)
				Tower1.SetActive(false);
			
			if(upgd.activeSelf)
				upgd.SetActive(false);

			Click_Outside();
		}
	}
	
	public void RepairTower(){
		if((!UI_Tutorial.activeSelf || GlobalData.TutCanBuildFree) || GlobalData.current_tutorial==5)
		{
			if(GlobalData.current_tutorial==5){
				GameObject.FindGameObjectWithTag("TD_Level").GetComponent<TDLevelControl>().ActedOnTutorial();
			}
			//SetTowerRUDBtns(-1);
			if(towers_by_spots.ContainsKey(spotid)){
				if(towers_by_spots[spotid]!=null){
					TowerControl tcontrol = towers_by_spots[spotid].GetComponent<TowerControl>();
					
					bool can_repair = tcontrol.status.health<GlobalData.TOWERSUPGRADEVALUES[tcontrol.status.type][tcontrol.status.upgrade_level].health &&
					PlayerData.current_energy>GlobalData.TOWER_Repair_COSTS[tcontrol.status.type][tcontrol.status.upgrade_level];
					
					if(can_repair){
						SoundControl.PlaySFX(GlobalData.SFX_Paths[12], false, true,true);
						towers_by_spots[spotid].GetComponent<TowerControl>().RepairTower();
						this.gameObject.SetActive(false);
					}
					else
					{
						pnl_info.SetActive(false);
					}
				}
			}

			if(Tower1.activeSelf)
				Tower1.SetActive(false);
			
			if(upgd.activeSelf)
				upgd.SetActive(false);

			Click_Outside();
		}
	}
}
