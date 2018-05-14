using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TowerSpotControl : MonoBehaviour {
	GameObject TowerSpotsGUIs,TowerButtons, TowerInfo,CheckFirstTower,CheckUpgdTower;

	void Start () {
		TowerSpotsGUIs = GameObject.Find ("InGame").gameObject.transform.Find("TD").Find("TowerSpotsGUIs").gameObject;
		TowerButtons = TowerSpotsGUIs.transform.Find("tbtns").gameObject;
		TowerInfo = TowerSpotsGUIs.transform.Find("pnl_info").gameObject;
		CheckFirstTower=TowerButtons.transform.Find("btn_Tower1").gameObject;
		CheckUpgdTower=TowerButtons.transform.Find("btn_Upgrade").gameObject;
	}
	
	void Update () {
	
	}
	
	void OnMouseUp () {

		//Check if there isn't any GUI's open for build towers or upgrade/repair/remove towers


			GameObject UI_Tutorial=GameObject.Find("Canvas").transform.Find("InGame").transform.Find("Tutorial").gameObject;

			bool samespot=false;
			GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Tower");

			float swidth=Screen.width;
			float sheight=Screen.height;

			string SelectedTower=this.gameObject.name;
			int towerid=int.Parse(this.gameObject.name);
		Debug.Log (CheckFirstTower.activeSelf+" "+!CheckUpgdTower.activeSelf);
		if(!CheckFirstTower.activeSelf && !CheckUpgdTower.activeSelf)
		{
			if(UI_Tutorial.activeSelf && !GlobalData.TutCanBuildFree)
			{
				TowerSpotsGUIs.GetComponent<TowerSpotsGUIs>().spotid=0;
			}
			else
				TowerSpotsGUIs.GetComponent<TowerSpotsGUIs>().spotid=towerid;


			int TowerNum=this.gameObject.transform.childCount;

			Vector2 mousepos = Input.mousePosition;

			float constant_offset_x = 200 * Screen.width/(GlobalData.orientationlandscape? 1024f : 768f);
			float constant_offset_y = 200 * Screen.height/(GlobalData.orientationlandscape? 768 : 1024);
			Vector3 info_offset = new Vector3(mousepos.x<Screen.width/2? constant_offset_x : -constant_offset_x,
			                                  mousepos.y<Screen.height/2? constant_offset_y : -constant_offset_y, 0);


			Debug.Log ("OnMouseUp TutCanBuild"+GlobalData.TutCanBuildFree);
			if((!UI_Tutorial.activeSelf || GlobalData.TutCanBuildFree) || (towerid==0))
			{
				TowerInfo.GetComponent<TowerButtonsPositions>().SetStartPos(this.gameObject, info_offset);
				TowerButtons.GetComponent<TowerButtonsPositions>().SetStartPos(this.gameObject);
			}


			Debug.Log ("On Mouse Up "+TowerNum);
			if(TowerNum==0){
				//X to Y
				//Z to X


				
						Debug.Log (UI_Tutorial.activeSelf+" currtut "+GlobalData.current_tutorial+" towerid "+towerid);

				if(!GlobalData.introlevelanim && ((!UI_Tutorial.activeSelf || GlobalData.TutCanBuildFree) || (towerid==0 && GlobalData.current_tutorial==1)))
					{
						if(GlobalData.current_tutorial==1){
							GameObject.FindGameObjectWithTag("TD_Level").GetComponent<TDLevelControl>().ActedOnTutorial();
						}
							
							
						   
								Debug.Log ("curtutorial "+GlobalData.current_tutorial);
							
							TowerButtons.transform.Find("btn_Repair").gameObject.SetActive(false);
							TowerButtons.transform.Find("btn_Upgrade").gameObject.SetActive(false);
							TowerButtons.transform.Find("btn_Delete").gameObject.SetActive(false);
							for(int i=1;i<=5;i++) {

								bool can_build = (PlayerData.current_energy > GlobalData.TOWER_BUILD_COSTS[i] );
								TowerButtons.transform.Find("btn_Tower"+i).gameObject.SetActive(true);
								if(can_build){
									TowerButtons.transform.Find("btn_Tower"+i).GetComponent<Image>().color = Color.white;
								}else{
									TowerButtons.transform.Find("btn_Tower"+i).GetComponent<Image>().color = Color.grey;
								}
							} 
			


					}
			} else {
				if(!GlobalData.introlevelanim && ((!UI_Tutorial.activeSelf || GlobalData.TutCanBuildFree) || (towerid==0 && GlobalData.current_tutorial==4)))
				{
					if(GlobalData.current_tutorial==4){
						GlobalData.enemyatacktower_tut=true;

					}
					TowerControl tc = transform.GetChild(0).GetComponent<TowerControl>();
					int tupgrade_level = tc.status.upgrade_level;
					int ttype = tc.status.type;
					int thealth = tc.status.health;
					//ToDo Check what is the level of the tower in the spot and the health of it
					//Vector2 TowerSpotPosition= RectTransformUtility.WorldToScreenPoint(Camera.main, this.transform.position);
				//	Debug.Log (TowerSpotPosition);
					//TowerButtons.GetComponent<RectTransform>().position= new Vector3 (TowerSpotPosition.x,TowerSpotPosition.y,0);
					//TowerInfo.GetComponent<TowerButtonsPositions>().SetStartPos(this.gameObject, new Vector3(200, 200, 0));
					//TowerButtons.GetComponent<TowerButtonsPositions>().SetStartPos(this.gameObject);
					bool can_repair = thealth<GlobalData.TOWERSUPGRADEVALUES[ttype][tupgrade_level].health &&
										PlayerData.current_energy>GlobalData.TOWER_Repair_COSTS[ttype][tupgrade_level];
					TowerButtons.transform.Find("btn_Repair").gameObject.SetActive(true);

					if(can_repair){
						TowerButtons.transform.Find("btn_Repair").GetComponent<Image>().color = Color.white;
					}else{
						TowerButtons.transform.Find("btn_Repair").GetComponent<Image>().color = Color.grey;
						
					}


						

						TowerButtons.transform.Find("btn_Upgrade").gameObject.SetActive(true);
					if(tupgrade_level<3){
						bool can_upgrade = PlayerData.current_energy>GlobalData.TOWER_UPGRADE_COSTS[ttype][tupgrade_level];

						if(can_upgrade){
							TowerButtons.transform.Find("btn_Upgrade").GetComponent<Image>().color = Color.white;
						}else{
							TowerButtons.transform.Find("btn_Upgrade").GetComponent<Image>().color = Color.grey;
						}

					}else{
						TowerButtons.transform.Find("btn_Upgrade").GetComponent<Image>().color = Color.grey;
					}
					TowerButtons.transform.Find("btn_Delete").gameObject.SetActive(true);




					for(int i=1;i<=5;i++)
					{
						TowerButtons.transform.Find("btn_Tower"+i).gameObject.SetActive(false);
					}
				}
			}
			TowerSpotsGUIs.SetActive(true);
		}
	}


	public void OpenMenu(){
		
		bool samespot=false;
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Tower");
		
		float swidth=Screen.width;
		float sheight=Screen.height;
		
		string SelectedTower=this.gameObject.name;
		int towerid=int.Parse(this.gameObject.name);
		TowerSpotsGUIs.GetComponent<TowerSpotsGUIs>().spotid=towerid;
		
		
		int TowerNum=this.gameObject.transform.childCount;
		
		Vector2 mousepos = Input.mousePosition;
		
		float constant_offset_x = 200 * Screen.width/(GlobalData.orientationlandscape? 1024f : 768f);
		float constant_offset_y = 200 * Screen.height/(GlobalData.orientationlandscape? 768 : 1024);
		Vector3 info_offset = new Vector3(mousepos.x<Screen.width/2? constant_offset_x : -constant_offset_x,
		                                  mousepos.y<Screen.height/2? constant_offset_y : -constant_offset_y, 0);
		TowerInfo.GetComponent<TowerButtonsPositions>().SetStartPos(this.gameObject, info_offset);
		TowerButtons.GetComponent<TowerButtonsPositions>().SetStartPos(this.gameObject);
		
		if(TowerNum==0){
			//X to Y
			//Z to X
	
			
			//	Vector2 TowerSpotPosition= RectTransformUtility.WorldToScreenPoint(Camera.main, this.transform.position);
			//	Debug.Log (TowerSpotPosition);
			//	TowerButtons.GetComponent<RectTransform>().position= new Vector3 (TowerSpotPosition.x,TowerSpotPosition.y,0);
			TowerButtons.transform.Find("btn_Repair").gameObject.SetActive(false);
			TowerButtons.transform.Find("btn_Upgrade").gameObject.SetActive(false);
			TowerButtons.transform.Find("btn_Delete").gameObject.SetActive(false);
			for(int i=1;i<=5;i++) {
				bool can_build = (PlayerData.current_energy > GlobalData.TOWER_BUILD_COSTS[i] );
				TowerButtons.transform.Find("btn_Tower"+i).gameObject.SetActive(true);
				if(can_build){
					TowerButtons.transform.Find("btn_Tower"+i).GetComponent<Image>().color = Color.white;
				}else{
					TowerButtons.transform.Find("btn_Tower"+i).GetComponent<Image>().color = Color.grey;
				}
			} 
		} else {
			TowerControl tc = transform.GetChild(0).GetComponent<TowerControl>();
			int tupgrade_level = tc.status.upgrade_level;
			int ttype = tc.status.type;
			int thealth = tc.status.health;
			//ToDo Check what is the level of the tower in the spot and the health of it
			//Vector2 TowerSpotPosition= RectTransformUtility.WorldToScreenPoint(Camera.main, this.transform.position);
			//	Debug.Log (TowerSpotPosition);
			//TowerButtons.GetComponent<RectTransform>().position= new Vector3 (TowerSpotPosition.x,TowerSpotPosition.y,0);
			//TowerInfo.GetComponent<TowerButtonsPositions>().SetStartPos(this.gameObject, new Vector3(200, 200, 0));
			//TowerButtons.GetComponent<TowerButtonsPositions>().SetStartPos(this.gameObject);
			bool can_repair = thealth<GlobalData.TOWERSUPGRADEVALUES[ttype][tupgrade_level].health &&
				PlayerData.current_energy>GlobalData.TOWER_Repair_COSTS[ttype][tupgrade_level];
			TowerButtons.transform.Find("btn_Repair").gameObject.SetActive(true);
			
			if(can_repair){
				TowerButtons.transform.Find("btn_Repair").GetComponent<Image>().color = Color.white;
			}else{
				TowerButtons.transform.Find("btn_Repair").GetComponent<Image>().color = Color.grey;
				
			}
			
			
			
			
			TowerButtons.transform.Find("btn_Upgrade").gameObject.SetActive(true);
			if(tupgrade_level<3){
				bool can_upgrade = PlayerData.current_energy>GlobalData.TOWER_UPGRADE_COSTS[ttype][tupgrade_level];
				
				if(can_upgrade){
					TowerButtons.transform.Find("btn_Upgrade").GetComponent<Image>().color = Color.white;
				}else{
					TowerButtons.transform.Find("btn_Upgrade").GetComponent<Image>().color = Color.grey;
				}
				
			}else{
				TowerButtons.transform.Find("btn_Upgrade").GetComponent<Image>().color = Color.grey;
			}
			TowerButtons.transform.Find("btn_Delete").gameObject.SetActive(true);
			
			
			
			
			for(int i=1;i<=5;i++)
			{
				TowerButtons.transform.Find("btn_Tower"+i).gameObject.SetActive(false);
			}
		}
		TowerSpotsGUIs.SetActive(true);
	}
}
