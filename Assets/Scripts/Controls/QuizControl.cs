using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
public class QuizControl : MonoBehaviour {
	public Sprite answer_possible;
	public Sprite answer_wrong;
	public Sprite answer_right;

	public GameObject UI_txt_q;
	public GameObject[] UI_btns_a;
	public GameObject UI_timer;
	public GameObject UI_timer_txt;
	public GameObject UI_energy;
	public GameObject UI_btn_skip;
	//quiz_load -1 not loaded 0 loaded 1 initialized
	private int quiz_load = -1;
	private int total_questions = 0;
	private int correct_questions = 0;

	private List<int> questions_to_pick = new List<int>();
	private List<int> questions_picked = new List<int>();


	private List<int> answer_order = new List<int>();


	private int current_question = 0;
	// Use this for initialization
	void Start () {
		//LoadQuiz(GlobalData.current_difficulty, 3);
  	}

	void Update(){
		InitQuiz();
		Timer_Quiz();
		Timer_RightAnswer();
	}

 

	void LateUpdate(){
		if(Screen.width!= current_w || Screen.height!= current_h){

			this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
			this.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
			this.transform.position = new Vector3(Screen.width/2, Screen.height/2, 0);
			current_w = Screen.width;
			current_h =  Screen.height;
		}
	}

	public void InitQuiz(){
		if(quiz_load==0){
			PickRandomQuestions();

			SetGUI ();
			SetSkipBtn();
			quiz_load = 1;
			
		}else if(quiz_load == 2){//intro anim
			CheckIntroAnimEnded();
		}
	}


	private void QuizAnim_Appear(){
		StartCoroutine(ActionAfterTimer.Set(0.5f, delegate {
			UI_txt_q.transform.parent.gameObject.SetActive(true);
		}));
		for(int i=0; i<UI_btns_a.Length; i++){
			int btn_index = i;
			StartCoroutine(ActionAfterTimer.Set(0.75f+btn_index*0.15f, delegate {
				UI_btns_a[btn_index].GetComponent<Image>().sprite = answer_possible;
				if(activebuttons[btn_index])
					UI_btns_a[btn_index].SetActive(true);
				else
					UI_btns_a[btn_index].SetActive(false);
				if(btn_index == UI_btns_a.Length-1){
					start_quiz_timer = true;
					UI_timer.SetActive(start_quiz_timer);
					UI_timer_txt.SetActive(start_quiz_timer);
					quiz_timer = 0;
				}
			}));
		}
	}

	string[] CorrectAnswer_Motivtext={"Muito bem!","Fantástico","É isso mesmo!"};
	string[] WrongAnswer_Motivtext={"Oh não!","Não desistas!","Acho que não era essa a resposta"};
	private void QuizAnim_Picked(int btn_id_picked, int aid){
		start_quiz_timer = false;
		quiz_timer = 0;
		for(int i=0; i<UI_btns_a.Length; i++){
			int btn_index = i;
			UI_btns_a[btn_index].GetComponent<Button>().onClick.RemoveAllListeners();
			if(btn_index!=btn_id_picked){
				StartCoroutine(ActionAfterTimer.Set(0.25f+btn_index*0.25f, delegate {
					UI_btns_a[btn_index].SetActive(false);
				})); 
			}else{
				UI_btns_a[btn_index].GetComponent<Image>().sprite = aid==0? answer_right : answer_wrong;
			}
			 
		}

		StartCoroutine(ActionAfterTimer.Set(1f, delegate { //SETTING WRONG OR CORRECT ANSWER
		
			if(aid==0){
				correct_questions++;


				int motivrange=Random.Range (0,3);

				UI_txt_q.GetComponent<Text>().text = GlobalData.SAVEDQUIZQUESTIONS[d][questions_picked[current_question]].OW;



				//StartCoroutine(ActionAfterTimer.Set(2, delegate {
				//UI_txt_q.GetComponent<Text>().text = CorrectAnswer_Motivtext[motivrange];
				
					StartCoroutine(ActionAfterTimer.Set(1f, delegate { //RESETTING GUI and GOING INTO THE NEXT QUESTION
						UI_txt_q.transform.parent.gameObject.SetActive(false);
						if(btn_id_picked!=-1){
							UI_btns_a[btn_id_picked].SetActive(false);
						}
						current_question++;
						SetGUI ();
					}));
				//}));
			
			}else{
				int motivrange=Random.Range (0,3);


				UI_txt_q.GetComponent<Text>().text = GlobalData.SAVEDQUIZQUESTIONS[d][questions_picked[current_question]].OL;


				//StartCoroutine(ActionAfterTimer.Set(2, delegate {
				//	UI_txt_q.GetComponent<Text>().text = WrongAnswer_Motivtext[motivrange];

					StartCoroutine(ActionAfterTimer.Set(1f, delegate {
						UI_txt_q.transform.parent.gameObject.SetActive(false);
						if(btn_id_picked!=-1){
							UI_btns_a[btn_id_picked].SetActive(false);
						}
						//current_question++;
						//SetGUI ();
						QuizAnim_Lose();
					}));
				//}));
			}

		}));
 
	}

	IEnumerator Example() {
		yield return new WaitForSeconds(20);
	}

	private void QuizAnim_Win(){
		SoundControl.PlayMusic(GlobalData.Music_Paths[1], false);
		
		start_right_answer_timer = true;
		current_correct_answer = 0;
		current_energy = 50;
		UI_energy.transform.GetChild(0).GetComponent<Image>().fillAmount = current_energy;
		UI_energy.transform.GetChild(1).GetComponent<Text>().text = Mathf.CeilToInt(current_energy).ToString();
		

		UI_energy.SetActive(true);
		UI_timer.SetActive(false);
		UI_timer_txt.SetActive(false);
		UI_btn_skip.SetActive(false);

	}
	private void QuizAnim_Lose(){
		if(correct_questions > 0){
			QuizAnim_Win();
		}else{
			SoundControl.PlayMusic(GlobalData.Music_Paths[1], false);
			UI_btn_skip.SetActive(false);
			UI_timer.SetActive(false);
			UI_timer_txt.SetActive(false);
			EndQuiz();
		}
	}

	private void PickRandomQuestions(){
		Debug.Log ("can pick random questions? " + (GlobalData.SAVEDQUIZQUESTIONS[d].Count >= 4 && total_questions > 0 && total_questions<=GlobalData.SAVEDQUIZQUESTIONS[d].Count) );

		if(GlobalData.SAVEDQUIZQUESTIONS[d].Count >= 4 && total_questions > 0 && total_questions<=GlobalData.SAVEDQUIZQUESTIONS[d].Count){
			for(int i=0; i<total_questions; i++){
				int question_index = Random.Range(0, questions_to_pick.Count);
				questions_picked.Add(questions_to_pick[question_index]);
				questions_to_pick.RemoveAt(question_index);
				
			}
			 
		}
		current_question = 0;
		correct_questions = 0;
	}

	private void ShuffleAnswers(){
		List<int> ordered_answers = new List<int>(){0, 1, 2, 3};
		answer_order.Clear();
		for(int i=0; i<4; i++){
			int answer_index = Random.Range(0, ordered_answers.Count);
			answer_order.Add(ordered_answers[answer_index]);
			ordered_answers.RemoveAt(answer_index);
		}

	}


	bool []activebuttons={true,true,true,true};
	private void SetGUI(){

		if(current_question < questions_picked.Count){
			ShuffleAnswers();
		//	UI_txt_q.SetActive(true);//remove this add to delayed display
			UI_txt_q.GetComponent<Text>().text = GlobalData.SAVEDQUIZQUESTIONS[d][questions_picked[current_question]].Q;



			for(int i=0; i<UI_btns_a.Length; i++){
				int aid = answer_order[i];
				int btn_picked = i;
				//UI_btns_a[i].SetActive(true);
				UI_btns_a[i].transform.GetChild(0).GetComponent<Text>().text = GlobalData.SAVEDQUIZQUESTIONS[d][questions_picked[current_question]].GetAnswerById(aid);
				Debug.Log (GlobalData.SAVEDQUIZQUESTIONS[d][questions_picked[current_question]].GetAnswerById(aid));
				if(GlobalData.SAVEDQUIZQUESTIONS[d][questions_picked[current_question]].GetAnswerById(aid)=="-")
				{
					activebuttons[i]=false;
					UI_btns_a[i].SetActive(false);
				}
				else
				{
					activebuttons[i]=true;
					//UI_btns_a[i].SetActive(true);
				}
				UI_btns_a[i].GetComponent<Button>().onClick.RemoveAllListeners();
				UI_btns_a[i].GetComponent<Button>().onClick.AddListener(()=>{
					SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
					QuizAnim_Picked(btn_picked, aid);
				 
				});

			}


			QuizAnim_Appear();
		}else{
			QuizAnim_Win();
		}
	}


	private void SetSkipBtn(){
		UI_btn_skip.GetComponent<Button>().onClick.RemoveAllListeners();
		UI_btn_skip.GetComponent<Button>().onClick.AddListener(()=>{
			SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
			
			QuizAnim_Picked(-1, 3);
		});

	}

	private void Won(){

	}


	private void Lost(){

	}
//TODO delay buttons appearing
	private int d = 0;
	public void LoadQuiz( int total_questions){
	/*	this.gameObject.SetActive(true);
		StartCoroutine(
			GeneralServerOperations.HandleServerPOST(
			Server.Http()+"/IS_LoadQuiz.php", 
			new Dictionary<string, string>(){{"difficulty", difficulty.ToString()}}, 
		new string[]{"-900"}, 
		delegate(string s){
				Debug.Log (s);
				ParseQuizFromJSON(s, difficulty);
				quiz_load = 0;
				this.total_questions = total_questions;

		}));*/
		this.d = GlobalData.current_difficulty;
		this.gameObject.SetActive(true);
		this.questions_to_pick.Clear();
		this.questions_picked.Clear();
		Debug.Log (GlobalData.SAVEDQUIZQUESTIONS[d].Count);
		for(int i=0 ;  i<GlobalData.SAVEDQUIZQUESTIONS[d].Count; i++){
			questions_to_pick.Add(i);
		}
	//	Debug.Log (GlobalData.current_difficulty +" | "+ GlobalData.current_level);
		if(GlobalData.current_difficulty==0 && GlobalData.current_level == 1){
			StartIntroAnim();
		}else{
			this.quiz_load = 0;
		}
		this.total_questions = total_questions;


		current_w = Screen.width;
		current_h = Screen.height;
	}
	int current_w;
	int current_h;

	public void ParseQuizFromJSON(string s, int difficulty){
		questions_to_pick.Clear();
		questions_picked.Clear();
		GlobalData.quiz_questions.Clear();
		JSONNode json = JSON.Parse(s);
		JSONArray jsonarr = json.AsArray;
		for(int i=0; i<jsonarr.Count; i++){
			int d = 0;
			string Q = "";
			string RA = "";
			string WA1 = "";
			string WA2 = "";
			string WA3 = "";
			string OW = "";
			string OL = "";
			if(difficulty == -1){
				if(jsonarr[i]["difficulty"]!=null){
					d = int.Parse(jsonarr[i]["difficulty"].Value);
				}
			}else{
				d = difficulty;
			}
			
			if(jsonarr[i]["Q"]!=null){
				Q = jsonarr[i]["Q"].Value;
			}
			if(jsonarr[i]["RA"]!=null){
				RA = jsonarr[i]["RA"].Value;
			}
			if(jsonarr[i]["WA1"]!=null){
				WA1 = jsonarr[i]["WA1"].Value;
			}
			if(jsonarr[i]["WA2"]!=null){
				WA2 = jsonarr[i]["WA2"].Value;
			}
			if(jsonarr[i]["WA3"]!=null){
				WA3 = jsonarr[i]["WA3"].Value;
			}
			if(jsonarr[i]["OW"]!=null){
				OW = jsonarr[i]["OW"].Value;
			}
			if(jsonarr[i]["OL"]!=null){
				OL = jsonarr[i]["OL"].Value;
			}
			
			GlobalData.quiz_questions.Add(new QuizQuestion(d, Q, RA, WA1, WA2, WA3, OW, OL));
			questions_to_pick.Add(i);
		}
	}


	
	private float time_per_answer = 20;
	private float quiz_timer = 0;
	private bool start_quiz_timer = false;

	private void Timer_Quiz(){
		if(start_quiz_timer){
			if(quiz_timer<=time_per_answer){
				UI_timer.GetComponent<Image>().fillAmount = (time_per_answer-quiz_timer)/time_per_answer;
				UI_timer_txt.GetComponent<Text>().text = Mathf.CeilToInt(Mathf.Clamp(time_per_answer-quiz_timer, 0, time_per_answer)).ToString();
				quiz_timer+=Time.deltaTime;
			}else{
				UI_timer.GetComponent<Image>().fillAmount = 0;
				UI_timer_txt.GetComponent<Text>().text = "0";
				for(int i=0; i<UI_btns_a.Length; i++){
					UI_btns_a[i].GetComponent<Button>().onClick.RemoveAllListeners();
				} 
				UI_txt_q.GetComponent<Text>().text =GlobalData.SAVEDQUIZQUESTIONS[d][questions_picked[current_question]].OL;
				StartCoroutine(ActionAfterTimer.Set(2, delegate {
					UI_txt_q.transform.parent.gameObject.SetActive(false);
					for(int i=0; i<UI_btns_a.Length; i++){
						UI_btns_a[i].SetActive(false);
					} 
					QuizAnim_Lose();
				}));
				start_quiz_timer = false;
			}
		}
	}

	private float time_per_rightanswer = 1f;
	private float right_answer_timer = 0;
	private bool start_right_answer_timer= false;
	private int current_correct_answer = 0;
	private float current_energy = 50;
	private void Timer_RightAnswer(){
		if(start_right_answer_timer){
			if(right_answer_timer<=time_per_rightanswer){
				UI_energy.transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = 
					Mathf.Lerp(current_energy/100f, 
					           (current_energy + GlobalData.quiz_energy_bonus[current_correct_answer])/100f, 
					           right_answer_timer/time_per_rightanswer);

				UI_energy.transform.GetChild(1).GetComponent<Text>().text = 
					Mathf.CeilToInt(Mathf.Lerp(current_energy, 
				  			  	(current_energy + GlobalData.quiz_energy_bonus[current_correct_answer]), 
				   				right_answer_timer/time_per_rightanswer)).ToString();
				right_answer_timer+=Time.deltaTime;
				Debug.Log ("quiz_timer: " +right_answer_timer);

			}else{
				
				current_energy = Mathf.Clamp( current_energy + GlobalData.quiz_energy_bonus[current_correct_answer], 0, 100);
				UI_energy.transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = current_energy/100;
				UI_energy.transform.GetChild(1).GetComponent<Text>().text = Mathf.CeilToInt(current_energy).ToString();

				current_correct_answer++;
				if(current_correct_answer < correct_questions){
					right_answer_timer = 0;

				}else{
					start_right_answer_timer = false;
					EndQuiz();
				}
			}
		}
	}




	private void EndQuiz(){
		PlayerData.current_energy = Mathf.CeilToInt(current_energy);
		Instantiate(Resources.Load ("Prefabs/TD_Level"));
		if(transform.parent!=null){
			transform.parent.GetComponent<ButtonMainMenu>().GoToInGame();
		}

		Destroy (this.gameObject);
	}

	Animator intro_anim,player;
	Transform intro_anim_tr;
	private void StartIntroAnim(){
		Debug.Log ("Before intro anim start");

		intro_anim_tr = transform.parent.parent.Find("InGame").Find("IntroAnim");
		intro_anim_tr.gameObject.SetActive(true);
		GameObject IntroAnim=transform.parent.parent.Find("InGame").gameObject.transform.Find("IntroAnim").gameObject;
		//player=
		GameObject.Find ("InGame").transform.Find("IntroAnim").transform.Find("Screen01").gameObject.transform.Find("Player").gameObject.GetComponent<Animator>().SetInteger("player",PlayerData.picked_playerid);
		//player.SetInteger("player",1);
		GameObject.Find ("InGame").transform.Find("IntroAnim").transform.Find("Screen01").gameObject.transform.Find("frame02").transform.Find("Player").gameObject.GetComponent<Animator>().SetInteger("player",PlayerData.picked_playerid);

		//GameObject.Find ("InGame").transform.FindChild("IntroAnim").transform.FindChild("Screen02").gameObject.transform.FindChild("Player").gameObject.GetComponent<Animator>().SetInteger("player",1);

		intro_anim_tr.GetComponent<Button>().onClick.RemoveAllListeners();
		/*intro_anim_tr.GetComponent<Button>().onClick.AddListener(()=>{
			SoundControl.PlaySFX(GlobalData.SFX_Paths[1], false, true, true);
			
			quiz_load = 0;
			intro_anim_tr.gameObject.SetActive(false);
		});*/
		intro_anim = intro_anim_tr.GetComponent<Animator>();
		quiz_load=2;
		Debug.Log ("INTRO ANIM?");
	}

	private void CheckIntroAnimEnded(){
	
		if(intro_anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.End")){
			Debug.Log ("checking");
			intro_anim_tr.gameObject.SetActive(false);
			quiz_load = 0;
		}
	}
}
