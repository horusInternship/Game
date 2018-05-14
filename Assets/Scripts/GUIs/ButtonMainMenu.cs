using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ButtonMainMenu : MonoBehaviour {
	public GameObject splash;
	bool ended_splash = false;
	GameObject btn_LeadMenu,btn_OptMenu,btn_MainQuizMenu,btn_HomeMenu;
	GameObject APIs;

	string dbg = "";
/*	void OnGUI(){
		if(PlayerPrefs.HasKey("PlayerSavedData")){
			GUI.Label(new Rect(0,0, Screen.width, Screen.height), dbg);
		}

		if(GUI.Button(new Rect(Screen.width/2, 0, 100, 100), "load")){
			LoadQuiz(-1);
		}
	}
*/
	// Use this for initialization
	void Awake() {
 		Init ();
	}

	IEnumerator CheckConnection()
	{
		WWW www = new WWW("https://www.google.com");
		yield return www;
		if (www.error == null)
		{
			APIs.GetComponent<DeltaDNAManager>().DeltaDNA_PostEvent("reached_ingame",true);
			APIs.GetComponent<DeltaDNAManager>().DeltaDNA_PostEvent("reached_mainmenu",true);
		}
	} 

	void Start(){
		Debug.Log("curr dificulty "+GlobalData.current_difficulty);
		//GameObject.Find ("InGame").transform.FindChild("Tutorial").gameObject.transform.FindChild("TutorialObjects").gameObject.transform.FindChild("INES_Tutorial").GetComponent<Animator>().SetInteger("state",1);
		//GameObject.Find ("InGame").transform.FindChild("IntroAnim").gameObject.transform.FindChild("Screen01").gameObject.transform.FindChild("Player").gameObject.GetComponent<Animator>().SetInteger("player",1);
		APIs=GameObject.Find("APIs").gameObject;
		
		//GameObject.Find ("TESTE").transform.GetChild(0).GetComponent<Text>().text="First Message";
		#if UNITY_IOS && !UNITY_EDITOR
		//APIs.GetComponent<APIManager>().GameCenterConection();
		
		//APIs.GetComponent<APIManager>().RetrieveTopTenScores();
		#endif
		
		#if UNITY_ANDROID && !UNITY_EDITOR
		APIs.GetComponent<APIManager>().GameCenterConection();
		
		#endif
		


		StartCoroutine(CheckConnection());
		

	}


	private void Init(){

		//PlayerPrefs.DeleteAll();
		//JoaoResetData
		CSVReader.LoadLanguageCSV();
		//JoaoResetData
		if(!PlayerPrefs.GetString("PlayerSavedData").Equals("")) {
			//dbg += "from save";
			Debug.Log (PlayerPrefs.GetString("PlayerSavedData"));


			SaveLoadData.LoadPlayerData();
			if(GlobalData.SAVEDQUIZQUESTIONS.Count>0){
			//	dbg+="has loaded quiz questions from save";
				SaveLoadData.SavePlayerData();
			}else{
			//	dbg+="not loaded quiz questions from save";
				LoadQuiz(-1);
			}
		} else {
			//dbg += "loading_quiz\n";
			Debug.Log ("loadquiz");
			LoadQuiz(-1);
		}
		SoundControl.PlayMusic(GlobalData.Music_Paths[0], true);
		this.transform.Find("OptionsMenu").GetComponent<OptionsMenu>().updateMusicSFXImages();
	}
	// Update is called once per frame
	void Update () {
		if(!ended_splash){
			if(splash.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Base Layer.End")){
				splash.SetActive(false);
				this.gameObject.transform.Find("HomeMenu").gameObject.transform.Find("InternetMessage").gameObject.SetActive(true);
				ended_splash = true;
			}
		}

		
		//JoaoAdjustTextScale
		CheckOritentation();
		//JoaoAdjustTextScale
	}

	//JoaoAdjustTextScale
	void CheckOritentation(){
		if(GlobalData.SavedScreenWidth==0)
		{
			GlobalData.SavedScreenWidth=Screen.width;
			if(Screen.width>Screen.height)
			{
				GlobalData.orientationlandscape=true;
			}
			else
			{
				GlobalData.orientationlandscape=false;
			}
		}else{
			if(Screen.width>GlobalData.SavedScreenWidth)
			{
				GlobalData.orientationlandscape=true;
				GlobalData.SavedScreenWidth=Screen.width;
			}
			else if(Screen.width<GlobalData.SavedScreenWidth)
			{
				GlobalData.orientationlandscape=false;
				GlobalData.SavedScreenWidth=Screen.width;
			}
			
		}
		
		
	}
	//JoaoAdjustTextScale

	public void GoToLeaderBoard(){

		#if UNITY_IOS && !UNITY_EDITOR
		//APIs.GetComponent<APIManager>().RetrieveTopTenScores();
		#endif
		#if !UNITY_EDITOR
		GameObject.Find("MainMenu").transform.FindChild("LeaderBoardMenu").gameObject.GetComponent<LeaderBoard>().ShowLeaderBoard();
		//APIs.GetComponent<APIManager>().RetrieveTopTenScores();
		#endif

		#if UNITY_EDITOR
		GameObject.Find("MainMenu").transform.Find("LeaderBoardMenu").gameObject.GetComponent<LeaderBoard>().ShowLeaderBoard();
		#endif

		/*SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
		this.transform.FindChild("LeaderBoardMenu").gameObject.SetActive(true);
		this.transform.FindChild("OptionsMenu").gameObject.SetActive(false);
		this.transform.FindChild("QuizMenu").gameObject.SetActive(false);
		this.transform.FindChild("HomeMenu").FindChild("btn_PlayButton").gameObject.SetActive(false);
		BottomButtonsColors(0);
		this.transform.FindChild("PlayMenu").gameObject.SetActive(false);
		this.transform.FindChild("LevelsMenu").gameObject.SetActive(false);


	//	this.transform.FindChild("OptionsMenu").gameObject.transform.FindChild("OptionsLabel").GetComponent<Text>().text="Settings";
		this.transform.FindChild("OptionsMenu").gameObject.transform.FindChild("OptionsMainScreenObjects").gameObject.SetActive(true);
		this.transform.FindChild("OptionsMenu").gameObject.transform.FindChild("Credits").gameObject.SetActive(false);*/


	}


	public void MainShowLeaderBoard(){
		this.gameObject.transform.Find("HomeMenu").gameObject.transform.Find("InternetMessage").gameObject.SetActive(false);
		//Show Leaderboard
		SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
		GameObject.Find("MainMenu").transform.Find("LeaderBoardMenu").gameObject.SetActive(true);
		GameObject.Find("MainMenu").transform.Find("OptionsMenu").gameObject.SetActive(false);
		GameObject.Find("MainMenu").transform.Find("QuizMenu").gameObject.SetActive(false);
		GameObject.Find("MainMenu").transform.Find("HomeMenu").Find("btn_PlayButton").gameObject.SetActive(false);
		Transform mbb = GameObject.Find("MainMenu").transform.Find("HomeMenu").Find("MainBottomBar");
		for(int i=0; i<4; i++){
			if(i == 0){
				mbb.GetChild(i).GetComponent<Button>().image.color = btn_selected_color;
				Debug.Log (i);
			}else{
				mbb.GetChild(i).GetComponent<Image>().color = btn_unselected_color;
			}
		}
		GameObject.Find("MainMenu").transform.Find("PlayMenu").gameObject.SetActive(false);
		GameObject.Find("MainMenu").transform.Find("LevelsMenu").gameObject.SetActive(false);
		
		
		//	this.transform.FindChild("OptionsMenu").gameObject.transform.FindChild("OptionsLabel").GetComponent<Text>().text="Settings";
		GameObject.Find("MainMenu").transform.Find("OptionsMenu").gameObject.transform.Find("OptionsMainScreenObjects").gameObject.SetActive(true);
		GameObject.Find("MainMenu").transform.Find("OptionsMenu").gameObject.transform.Find("Credits").gameObject.SetActive(false);
	}


	public void GoToOptions(){
		SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);

		this.gameObject.transform.Find("HomeMenu").gameObject.transform.Find("InternetMessage").gameObject.SetActive(false);
		
		this.transform.Find("LeaderBoardMenu").gameObject.SetActive(false);
		GameObject.Find("MainMenu").gameObject.transform.Find("OptionsMenu").gameObject.transform.Find("OptionsLabel").GetComponent<Image>().sprite = Resources.Load<Sprite>("OptionsGUI/title_settings");
		this.transform.Find("OptionsMenu").gameObject.SetActive(true);
		this.transform.Find("QuizMenu").gameObject.SetActive(false);
		this.transform.Find("HomeMenu").Find("btn_PlayButton").gameObject.SetActive(false);
		BottomButtonsColors(1);
		this.transform.Find("PlayMenu").gameObject.SetActive(false);
		this.transform.Find("LevelsMenu").gameObject.SetActive(false);

	//	this.transform.FindChild("OptionsMenu").gameObject.transform.FindChild("OptionsLabel").GetComponent<Text>().text="Settings";
		this.transform.Find("OptionsMenu").gameObject.transform.Find("OptionsMainScreenObjects").gameObject.SetActive(true);
		this.transform.Find("OptionsMenu").gameObject.transform.Find("Credits").gameObject.SetActive(false);


		//Update SFX and Music Icons


	}

	public void GoToQuiz(){

		this.gameObject.transform.Find("HomeMenu").gameObject.transform.Find("InternetMessage").gameObject.SetActive(false);

		SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);

		this.transform.Find("LeaderBoardMenu").gameObject.SetActive(false);
		this.transform.Find("OptionsMenu").gameObject.SetActive(false);
		this.transform.Find("QuizMenu").gameObject.SetActive(true);
		this.transform.Find("HomeMenu").Find("btn_PlayButton").gameObject.SetActive(false);
		BottomButtonsColors(2);
		this.transform.Find("PlayMenu").gameObject.SetActive(false);
		this.transform.Find("LevelsMenu").gameObject.SetActive(false);
		
		this.transform.Find("QuizMenu").Find("QuizDificulty").gameObject.SetActive(true);
		this.transform.Find("QuizMenu").Find("QuestionsMask").gameObject.SetActive(false);
		this.transform.Find("QuizMenu").Find("QuestionAnswerWindow").gameObject.SetActive(false);
		this.transform.Find("QuizMenu").Find("btn_back").gameObject.SetActive(false);
		
		//this.transform.FindChild("OptionsMenu").gameObject.transform.FindChild("OptionsLabel").GetComponent<Text>().text="Settings";
		this.transform.Find("OptionsMenu").gameObject.transform.Find("OptionsMainScreenObjects").gameObject.SetActive(true);
		this.transform.Find("OptionsMenu").gameObject.transform.Find("Credits").gameObject.SetActive(false);
	}

	public void GoToHome(){

		this.gameObject.transform.Find("HomeMenu").gameObject.transform.Find("InternetMessage").gameObject.SetActive(true);

		SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
		
		this.transform.Find("bg").gameObject.SetActive(true);
		this.transform.Find("LeaderBoardMenu").gameObject.SetActive(false);
		this.transform.Find("OptionsMenu").gameObject.SetActive(false);
		this.transform.Find("QuizMenu").gameObject.SetActive(false);
		this.transform.Find("HomeMenu").gameObject.SetActive(true);
		this.transform.Find("HomeMenu").Find("btn_PlayButton").gameObject.SetActive(true);
		BottomButtonsColors(3);
		this.transform.Find("PlayMenu").gameObject.SetActive(false);
		this.transform.Find("LevelsMenu").gameObject.SetActive(false);

		//this.transform.FindChild("OptionsMenu").gameObject.transform.FindChild("OptionsLabel").GetComponent<Text>().text="Settings";
		this.transform.Find("OptionsMenu").gameObject.transform.Find("OptionsMainScreenObjects").gameObject.SetActive(true);
		this.transform.Find("OptionsMenu").gameObject.transform.Find("Credits").gameObject.SetActive(false);
	}

	public void GoToPlayMenu(){
		SoundControl.PlaySFX(GlobalData.SFX_Paths[1], false, true, true);

		this.gameObject.transform.Find("HomeMenu").gameObject.transform.Find("InternetMessage").gameObject.SetActive(false);
		this.transform.Find("LeaderBoardMenu").gameObject.SetActive(false);
		this.transform.Find("OptionsMenu").gameObject.SetActive(false);
		this.transform.Find("QuizMenu").gameObject.SetActive(false);
		this.transform.Find("HomeMenu").Find("btn_PlayButton").gameObject.SetActive(false);
		BottomButtonsColors(4);
		
 		this.transform.Find("PlayMenu").GetComponent<PlayerSelectMenu>().Update_LockedDifficulties();
		this.transform.Find("PlayMenu").gameObject.SetActive(true);
		this.transform.Find("LevelsMenu").gameObject.SetActive(false);
		
		//	this.transform.FindChild("OptionsMenu").FindChild("OptionsLabel").GetComponent<Text>().text="Settings";
		this.transform.Find("OptionsMenu").Find("OptionsMainScreenObjects").gameObject.SetActive(true);
		this.transform.Find("OptionsMenu").Find("Credits").gameObject.SetActive(false);

		Debug.Log("PL_Choose"+PlayerData.picked_playerid);
	}


	public void GoToInGame(){
		this.gameObject.transform.Find("HomeMenu").gameObject.transform.Find("InternetMessage").gameObject.SetActive(false);
		this.transform.Find("PlayMenu").gameObject.SetActive(false);
		this.transform.Find("LevelsMenu").gameObject.SetActive(false);
		this.transform.Find("HomeMenu").gameObject.SetActive(false);
		this.transform.Find("bg").gameObject.SetActive(false);
		this.transform.parent.GetChild(1).gameObject.SetActive(true);

	}

	Color btn_selected_color = new Color(1/255f, 32/255f, 131/255f);
	Color btn_unselected_color = new Color(0/255f, 11/255f, 82/255f);
	//0 leaderboard, 1 options, 2 quiz, 3 home
	private void BottomButtonsColors(int menu_id){
		Transform mbb = this.transform.Find("HomeMenu").Find("MainBottomBar");
		for(int i=0; i<4; i++){
			if(menu_id == i){
				mbb.GetChild(i).GetComponent<Button>().image.color = btn_selected_color;
				Debug.Log (i);
			}else{
				mbb.GetChild(i).GetComponent<Image>().color = btn_unselected_color;
			}
		}
	}



	public void LoadQuiz(int difficulty){
		StartCoroutine(
			GeneralServerOperations.HandleServerPOST(
			"http://178.79.152.107/IS_LoadQuiz.php", 
			new Dictionary<string, string>(){{"difficulty", difficulty.ToString()}}, 
		new string[]{"-900"},
		delegate(string s){
			//Debug.Log (s);
			GlobalData.ParseQuizFromJSON(s);
			//dbg+="loaded "+ s+"\n";
		},
		delegate {
			Debug.Log ("Load Fail");//dbg+="error";
	}));
	}

	
}
