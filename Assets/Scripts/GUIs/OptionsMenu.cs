using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {
	
	public GameObject btn_Music,btn_SFX,btn_inGameMusic,btn_inGameSFX;
	string PL_Name;
	int enabledMusic,enabledSFX;


	// Use this for initialization
	void Start () {

		updateMusicSFXImages();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	

	public void ShowResetGUI(){
		SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
		
		this.gameObject.transform.Find("OptionsMainScreenObjects").gameObject.SetActive(false);
		this.gameObject.transform.Find("ResetGUI").gameObject.SetActive(true);
	}


	public void HideResetGUI(){
		SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
		
		this.gameObject.transform.Find("ResetGUI").gameObject.SetActive(false);
		this.gameObject.transform.Find("OptionsMainScreenObjects").gameObject.SetActive(true);
	}
	
	public void ResetGame(){
		SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
		
		//JoaoReset
			ResetAllData();
		//JoaoReset
			HideResetGUI();
	}

	public void ShowCredits(){
		SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
		

	//	this.gameObject.transform.FindChild("OptionsLabel").GetComponent<Text>().text="Options";
		this.gameObject.transform.Find("OptionsLabel").GetComponent<Image>().sprite = Resources.Load<Sprite>("OptionsGUI/title_credits");
		this.gameObject.transform.Find("OptionsMainScreenObjects").gameObject.SetActive(false);

		this.gameObject.transform.Find("Credits").gameObject.SetActive(true);
		//this.gameObject.transform.FindChild("CreditsScreenObjects").gameObject.SetActive(true);
	}

	public void HideCreditsGUI(){
		SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
		
		//this.gameObject.transform.FindChild("CreditsScreenObjects").gameObject.SetActive(true);
	//	this.gameObject.transform.FindChild("OptionsLabel").GetComponent<Text>().text="Settings";
		this.gameObject.transform.Find("OptionsLabel").GetComponent<Image>().sprite = Resources.Load<Sprite>("OptionsGUI/title_settings");
		
		this.gameObject.transform.Find("Credits").gameObject.SetActive(false);
		this.gameObject.transform.Find("OptionsMainScreenObjects").gameObject.SetActive(true);

	}

	public void CloseCredits(){
		HideCreditsGUI();
	}

 


	public void EnableDisableMusic(){
		SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
		
		if(PlayerData.music)
		{
			PlayerData.music=false;
			PlayerData.SetMusicVolume();
		}
		else
		{
			PlayerData.music=true;
			PlayerData.SetMusicVolume();
		}
		updateMusicSFXImages();
	}
	
	public void EnableDisableSFX(){
		
		
		if(PlayerData.sfx)
		{
			PlayerData.sfx=false;
			PlayerData.SetSFXVolume();
		}
		else
		{
			PlayerData.sfx=true;
			PlayerData.SetSFXVolume();
		}
		
		updateMusicSFXImages();

		SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
		
	}
	
	public void updateMusicSFXImages(){
		if(PlayerData.music) {
			btn_Music.GetComponent<Image>().sprite=Resources.Load <Sprite> ("OptionsGUI/music_on_button");
			btn_inGameMusic.GetComponent<Image>().sprite=Resources.Load <Sprite> ("OptionsGUI/music_on_button");
		} else {
			btn_Music.GetComponent<Image>().sprite=Resources.Load <Sprite> ("OptionsGUI/music_off_button");
			btn_inGameMusic.GetComponent<Image>().sprite=Resources.Load <Sprite> ("OptionsGUI/music_off_button");
		} 


		if(PlayerData.sfx){
			btn_SFX.GetComponent<Image>().sprite=Resources.Load <Sprite> ("OptionsGUI/sfx_on_button");
			btn_inGameSFX.GetComponent<Image>().sprite=Resources.Load <Sprite> ("OptionsGUI/sfx_on_button");
			
		} else {
			btn_SFX.GetComponent<Image>().sprite=Resources.Load <Sprite> ("OptionsGUI/sfx_off_button");
			btn_inGameSFX.GetComponent<Image>().sprite=Resources.Load <Sprite> ("OptionsGUI/sfx_off_button");
		}
	}


	//JoaoResetData
	void ResetAllData(){
		PlayerData.current_energy=50;
		PlayerData.energy_queue.Clear();
		PlayerData.level_state=0;
		PlayerData.picked_playerid=0;
		PlayerData.playername="";
		PlayerData.lastunlockeddificulty=0;
		PlayerData.lastunlockedlevel=1;
		GlobalData.LevelsUnlocked = new Dictionary<int, List<bool>>(){
			{0, new List<bool>(){true, false, false, false, false, false, false, false, false, false}},
			{1, new List<bool>(){true, false, false, false, false, false, false, false, false, false}},
			{2, new List<bool>(){true, false, false, false, false, false, false, false, false, false}}
		};
		GlobalData.DifficultiesUnlocked = new List<bool>(){true, false, false};
		updateMusicSFXImages();
		
		string saveddata=JSONMaker.MakeSaveFile();
		PlayerPrefs.SetString("PlayerSavedData",saveddata);

		GameObject.Find("MainMenu").transform.Find("PlayMenu").transform.Find("LevelSelectMenu").transform.Find("InputNameBG").transform.Find("InputName").GetComponent<InputField>().text=PlayerData.playername;
	}
	//JoaoResetData
}
