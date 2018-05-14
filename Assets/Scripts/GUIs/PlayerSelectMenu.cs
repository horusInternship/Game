using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerSelectMenu : MonoBehaviour {

	public GameObject InputNameBG,BoyCharacter,GirlCharacter;
	public GameObject[] difficulty_btns;
	string PL_Name;

	// Use this for initialization
	void Start () {
		//PlayerPrefs.SetString("PlayerName","Joao Nuno");

		SavedUser();

		int selectedchar=PlayerData.picked_playerid;
		if(selectedchar==0)
			MaleCharacterShown();
		else
			FemaleCharacterShown();



		InputNameBG.transform.Find("InputName").GetComponent<InputField>().onValueChange.RemoveAllListeners();
		InputNameBG.transform.Find("InputName").GetComponent<InputField>().onValueChange.AddListener((value)=>{
			PlayerData.playername=this.transform.Find("LevelSelectMenu").transform.Find("InputNameBG").transform.Find("InputName").GetComponent<InputField>().text;
		});

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	

	public void SavedUser(){
		Debug.Log("plname "+PlayerData.playername);
		if(PlayerData.playername!=""){ 
			InputNameBG.transform.Find("InputName").GetComponent<InputField>().text=PlayerData.playername;
		}else{
			Debug.Log("HERE");
			InputNameBG.transform.Find("InputName").GetComponent<InputField>().text="Nick";
		}


	}



	public void InputFieldSelected(){
		Debug.Log ("Selected");
		if(this.transform.Find("LevelSelectMenu").transform.Find("InputNameBG").transform.Find("InputName").GetComponent<Text>().text=="Nick")
		{
			this.transform.Find("LevelSelectMenu").transform.Find("InputNameBG").transform.Find("InputName").GetComponent<InputField>().text="";
			this.transform.Find("LevelSelectMenu").transform.Find("InputNameBG").transform.Find("Insertnamehere").GetComponent<Text>().text="";
		}
	}

	public void InputFieldUnselected(){
		Debug.Log ("Unselected");
		if(this.transform.Find("LevelSelectMenu").transform.Find("InputNameBG").transform.Find("InputName").GetComponent<InputField>().text=="")
			this.transform.Find("LevelSelectMenu").transform.Find("InputNameBG").transform.Find("InputName").GetComponent<Text>().text="Nick";
	}

	//Joao2409
	public void MaleCharacterShown(){
		SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
		
		PlayerData.picked_playerid = 0;
		BoyCharacter.GetComponent<Image>().color= new Color(255f, 255f, 255f,255f); 
		GirlCharacter.GetComponent<Image>().color= new Color(0.7647f, 0.7647f, 0.7647f, 255f ); 
		string saveddata=JSONMaker.MakeSaveFile();
		PlayerPrefs.SetString("PlayerSavedData",saveddata);
 
	}
	
	public void FemaleCharacterShown(){
		SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
		
		PlayerData.picked_playerid = 1;
		GirlCharacter.GetComponent<Image>().color= new Color(255f, 255f, 255f,255f); 
		BoyCharacter.GetComponent<Image>().color= new Color(0.7647f, 0.7647f, 0.7647f, 255f ); 
		string saveddata=JSONMaker.MakeSaveFile();
		PlayerPrefs.SetString("PlayerSavedData",saveddata);
		 
	}
	//Joao2409



	public void ShowLevelMenu(int difficulty){
		if(PlayerData.playername == "" || InputNameBG.transform.Find("InputName").GetComponent<InputField>().text == ""){
			return;
		}

		SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
		
		if(GlobalData.DifficultiesUnlocked[difficulty])
		{
			GlobalData.current_difficulty = difficulty;

			GameObject.Find ("MainMenu").transform.Find("LevelsMenu").gameObject.transform.GetComponent<LevelsMenu>().Update_LockedLevels(difficulty);

		if(difficulty==0){
			GameObject.Find ("MainMenu").transform.Find("LevelsMenu").Find("BG").GetComponent<Image>().sprite = Resources.Load<Sprite>("OptionsGUI/level_select_easy_bg");
		
		}else if(difficulty == 1){
			GameObject.Find ("MainMenu").transform.Find("LevelsMenu").Find("BG").GetComponent<Image>().sprite = Resources.Load<Sprite>("OptionsGUI/level_select_normal_bg");
		}else if(difficulty == 2){
			GameObject.Find ("MainMenu").transform.Find("LevelsMenu").Find("BG").GetComponent<Image>().sprite = Resources.Load<Sprite>("OptionsGUI/level_select_hard_bg");
		}
		




		GameObject.Find ("MainMenu").transform.Find("LevelsMenu").gameObject.SetActive(true);
		this.gameObject.SetActive(false);
		}
	}

	public void ShowLevelMenu_Easy(){
		Debug.Log ("EasyMenu");
		GameObject.Find ("MainMenu").transform.Find("EasyMenu").gameObject.SetActive(true);
		this.gameObject.SetActive(false);
	}

	public void ShowLevelMenu_Normal(){
		Debug.Log ("NormalMenu");
		GameObject.Find ("MainMenu").transform.Find("NormalMenu").gameObject.SetActive(true);
		this.gameObject.SetActive(false);
	}

	public void ShowLevelMenu_Hard(){
		Debug.Log ("HardMenu");
		GameObject.Find ("MainMenu").transform.Find("HardMenu").gameObject.SetActive(true);
		this.gameObject.SetActive(false);
	}

	public void Update_LockedDifficulties(){
		for(int i=0; i<difficulty_btns.Length; i++){
			difficulty_btns[i].transform.GetChild(0).gameObject.SetActive(GlobalData.DifficultiesUnlocked[i]);
			difficulty_btns[i].transform.GetChild(1).gameObject.SetActive(!GlobalData.DifficultiesUnlocked[i]);
		}
	}

}
