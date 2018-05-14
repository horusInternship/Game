using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelsMenu : MonoBehaviour {
 

	public GameObject Levels;

	public void StartLevel(int level_n){


		SoundControl.PlaySFX(GlobalData.SFX_Paths[1], false, true, true);
		
		Debug.Log (level_n);
		if(level_n <=10){
			GlobalData.current_level = level_n;
		}else{
			GlobalData.current_difficulty++;
			GlobalData.current_level=1;
		}


		if(GlobalData.current_difficulty==3)
		{
			GlobalData.current_difficulty=2;
			GlobalData.current_level=10;
		}

		Debug.Log (PlayerData.lastunlockeddificulty+" difficulty compare "+GlobalData.current_difficulty);
		Debug.Log (PlayerData.lastunlockedlevel+" level compare "+GlobalData.current_level);
		if(PlayerData.lastunlockeddificulty<GlobalData.current_difficulty){
			PlayerData.lastunlockeddificulty=GlobalData.current_difficulty;
			PlayerData.lastunlockedlevel=GlobalData.current_level;
		}else if(PlayerData.lastunlockeddificulty==GlobalData.current_difficulty){
			if(PlayerData.lastunlockedlevel<GlobalData.current_level){
				PlayerData.lastunlockedlevel = GlobalData.current_level;
			}
		}
		if(GlobalData.current_level == 1 && GlobalData.current_difficulty ==0){
			GlobalData.current_tutorial = -1;
		}else{
			GlobalData.current_tutorial = -2;
		}
 		SaveLoadData.SavePlayerData();

		Debug.Log ("PlayerData Unlocked level "+PlayerData.lastunlockedlevel);
		Debug.Log ("level "+(GlobalData.current_level-1)+" difficulty "+(GlobalData.current_difficulty));
		if(GlobalData.LevelsUnlocked[GlobalData.current_difficulty][GlobalData.current_level-1]){
			GameObject quiz = (GameObject)Instantiate(Resources.Load ("Prefabs/Quiz"));
			quiz.transform.parent = this.transform.parent;
 
			quiz.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
			quiz.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
			quiz.transform.position = new Vector3(Screen.width/2, Screen.height/2, 0);
 			quiz.GetComponent<QuizControl>().LoadQuiz(3);
		}
 
	}

	public void PlayLevelSFX(){
		SoundControl.PlaySFX(GlobalData.SFX_Paths[1], false, true, true);
		
	}
	public void BackToPlayMenu(){
		SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
		
		GameObject.Find ("MainMenu").transform.Find("PlayMenu").gameObject.SetActive(true);
		this.gameObject.SetActive(false);
	}



	//Joao2309
	public void Update_LockedLevels(int difficulty){
		Sprite difficulty_btn_sprite = Resources.Load<Sprite>(difficulty == 0? "LevelsMenu/lvlselect_easy" : 
		                                                      difficulty == 1? "LevelsMenu/lvlselect_normal" : 
		                                                      "LevelsMenu/lvlselect_hard");
		for(int i=0;i<Levels.transform.childCount;i++) {
			Debug.Log ("level "+i+" is unlocked "+GlobalData.LevelsUnlocked[difficulty][i]);
			Transform btn_lvl =Levels.transform.GetChild(i);
				btn_lvl.GetComponent<Image>().sprite = difficulty_btn_sprite;
				btn_lvl.GetChild(0).gameObject.SetActive(GlobalData.LevelsUnlocked[difficulty][i]);
				btn_lvl.GetChild(1).gameObject.SetActive(!GlobalData.LevelsUnlocked[difficulty][i]);
		}
	}
	//Joao2309
}
