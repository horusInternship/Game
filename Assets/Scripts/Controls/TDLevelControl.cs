using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class TDLevelControl : MonoBehaviour {
	private GameObject UI_InGame;
	private GameObject UI_Tutorial;
	public GameObject map_go;
	public GameObject towers_go;
	public GameObject enemies_go;
	public GameObject tile;
	public GameObject test_enemy;
	

	private List<Vector2> enemy_spawn_points = new List<Vector2>();
	private GameObject boss;
	private float enemy_spawn_timer = 0;
	private float enemy_spawn_time = 0;
	private bool can_enemy_spawn = false;
	 
	private int current_wave = 0;

	Vector3 main_tower_pos = Vector3.zero;

	bool switched_to_ingame_music = false;
	/*private void OnGUI(){
		GUI.Label(new Rect(0,0, Screen.width, Screen.height), "energy: "+PlayerData.current_energy + " \n levelstate: " + PlayerData.level_state);
		if(boss!=null){
			GUI.Label(new Rect(0,Screen.height/2, Screen.width, Screen.height), "boss life: "+boss.GetComponent<EnemyControl>().status.health );
			
		}


		if(GUI.Button(new Rect(Screen.width/2, 0, 100, 100), "tutorial")){
			if(PlayerData.level_state == 0){
				InitTutorial(0);
			}else if(PlayerData.level_state==2){
				EndTutorial();
			}
		}
	}*/


	private bool beforefirstwavetime=true;
	private float before_firstwave_timer = 5;
	private float before_firstwave_time = 0;


	public void InitLevel(){

		tutorialenemy=false;

		string dif = "";

		if(GlobalData.current_difficulty == 0){
			dif = "Fácil";
		}else if(GlobalData.current_difficulty == 1){
			dif = "Médio";
		}else{
			dif = "Difícil";
		}

		//var lvlDifi = GameObject.Find ("InGame").transform.Find ("TD").transform.Find ("LevelText").gameObject.GetComponent<Text> ().text;
		object[] obj = GameObject.FindObjectsOfType(typeof (GameObject));
		foreach (object o in obj)
		{
			GameObject g = (GameObject) o;
			if (g.name == "LevelText") {
				g.transform.GetComponent<Text>().text = string.Format("Nível {0} - {1} ",GlobalData.current_level, dif);
			}

		}


		//lvlDifi = string.Format("Nível {0}  -    Dificuldade {1} ",GlobalData.current_level,dif);



		Time.timeScale = 1;
	

		GlobalData.LoadLevelMaps(GlobalData.current_difficulty,GlobalData.current_level);
		LoadMap();
		CSVLoadEnemyWaves();
		EnemyWaves();
		Camera.main.GetComponent<CameraControl>().Init(GlobalData.map.GetLength(0),
		                                               GlobalData.difficulty_ymap_size[GlobalData.current_difficulty], 
		                                               main_tower_pos);
		PlayerData.level_state = 0;
		UI_InGame = GameObject.Find("Canvas").transform.Find("InGame").gameObject;
		UI_InGame.transform.Find("TD").gameObject.SetActive(true);
		UI_InGame.transform.Find("TD").Find("energy_bar").GetComponent<TDEnergyBar>().Init();

		UI_Tutorial = UI_InGame.transform.Find("Tutorial").gameObject;
		UI_Tutorial.transform.Find("TutorialObjects").transform.Find("Baloon").gameObject.SetActive(true);
		UI_Tutorial.transform.Find("Arrows").GetChild(0).gameObject.SetActive(true);
		UI_Tutorial.SetActive(false);
		ingame_tutorial_refs.Clear();
		GlobalData.tutorial_state=0;
		GlobalData.enemyatacktower_tut=false;
		baloontutstatestate=-1;

		StartCoroutine(ActionAfterTimer.Set(7, delegate {
			Debug.Log ("changing"); 
			SoundControl.PlayMusic(GlobalData.Music_Paths[2], true);
			can_start_enemy_waves = true;

		}));
	}

	private void Start(){
		GlobalData.TutCanBuildFree=false;
		InitLevel();



	}



	private void Update(){
		EnemyWaves();
		CheckWinLoseConditions();
		CheckEndAnimEnded();
		TutorialStartCheck();
	}


	void SetxmaxSize(int x){
		if(GlobalData.dificulty_xmap_size<x)
			GlobalData.dificulty_xmap_size=x+1;
	}
	public void LoadMap(){
		GlobalData.tower_spot_refs.Clear();
		int level_y_start = (GlobalData.current_level-1)+(GlobalData.current_level-1)*GlobalData.difficulty_ymap_size[GlobalData.current_difficulty];
		Debug.Log ("Calculate level_y_start"+(GlobalData.current_level-1)+" "+(GlobalData.current_level-1)*GlobalData.difficulty_ymap_size[GlobalData.current_difficulty]);
		int level_y_end = level_y_start + GlobalData.difficulty_ymap_size[GlobalData.current_difficulty];

		int towerspot_count = 0;
		GlobalData.dificulty_xmap_size=0;
		//int ypos = level_y_end -y

		for(int x = 0; x < GlobalData.map.GetLength(0)-1; x++){
				
			for(int y = level_y_start; y<= level_y_end; y++){
				int y_inv =level_y_end-y;
				//Debug.Log (x+" "+level_y_end+" "+y+" "+GlobalData.map[x,y_inv+level_y_start]);
				//Debug.Log (x+","+y_inv + "  " + level_y_start + "  "+ level_y_end);
				if(GlobalData.map[x,y_inv+level_y_start]!=null && GlobalData.map[x,y_inv+level_y_start]!=""){
					//Debug.Log (GlobalData.map[x,y_inv+level_y_start]);
					//Debug.Log (y_inv+" "+GlobalData.map[x,y_inv]);
					if(!GlobalData.map[x,y_inv+level_y_start].Equals("x") && 
					   !GlobalData.map[x,y_inv+level_y_start].Equals("") && 
					   !GlobalData.map[x, y_inv+level_y_start].Equals("t") && 
					   !GlobalData.map[x, y_inv+level_y_start].Equals("g") && 
					   !GlobalData.NPCPATHS.ContainsKey(GlobalData.map[x, y_inv+level_y_start])){
				
						GameObject t = (GameObject)Instantiate(tile);
						t.name = GlobalData.map[x,y_inv+level_y_start];
						t.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(GlobalData.TILESPRITEPATHS[GlobalData.map[x,y_inv+level_y_start]]);
						t.GetComponent<SpriteRenderer>().sortingOrder = 1;
						t.GetComponent<SpriteRenderer>().color = Color.white;
						t.transform.position = new Vector2(x,y-level_y_start);
						t.transform.parent = map_go.transform;
						GameObject bgt = (GameObject)Instantiate(tile);
						bgt.name = "x";
						bgt.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(GlobalData.TILESPRITEPATHS["x"]);
						bgt.transform.position = t.transform.position;
						bgt.transform.parent = map_go.transform;


						if(GlobalData.map[x,y_inv+level_y_start].Equals("sv") || GlobalData.map[x,y_inv+level_y_start].Equals("sh")){
						//	Debug.Log ("HERE" + x + " | " + (y-level_y_start));
							enemy_spawn_points.Add( new Vector2(x,y_inv));
						}

						SetxmaxSize(x);
					}else if(GlobalData.map[x, y_inv+level_y_start].Equals("x") || GlobalData.map[x, y_inv+level_y_start].Equals("t")){
						GameObject t = (GameObject)Instantiate(tile);
						t.name = GlobalData.map[x,y_inv+level_y_start];
						t.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(GlobalData.TILESPRITEPATHS["x"]);
						t.transform.position =  new Vector2(x,y-level_y_start);
						t.transform.parent = map_go.transform;
						if(GlobalData.map[x, y_inv+level_y_start].Equals("t")){
							GameObject tower_spot = (GameObject) Instantiate(Resources.Load ("Prefabs/Towers/TowerSpot"));
							tower_spot.transform.position = t.transform.position;
							tower_spot.transform.parent = towers_go.transform;
							tower_spot.name = towerspot_count.ToString();
							GlobalData.tower_spot_refs.Add(towerspot_count, tower_spot);
							towerspot_count++;
						}
						SetxmaxSize(x);
					}else if(GlobalData.map[x, y_inv+level_y_start].Equals("g")){
						GameObject bgt = (GameObject)Instantiate(tile);
						bgt.name = "x";
						bgt.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(GlobalData.TILESPRITEPATHS["x"]);
						bgt.transform.position = new Vector2(x,y-level_y_start);
						bgt.transform.parent = map_go.transform;

						GameObject nt = (GameObject) Instantiate(Resources.Load ("Prefabs/MainTower"));
						nt.name = "main_tower";
						nt.transform.parent = towers_go.transform;
						nt.transform.position = new Vector2(x,y-level_y_start);
						PlayerData.main_tower = nt;
						PlayerData.main_tower.GetComponent<Animator>().SetInteger("player", PlayerData.picked_playerid);
						main_tower_pos = nt.transform.position;
						SetxmaxSize(x);
					} else if(GlobalData.NPCPATHS.ContainsKey(GlobalData.map[x, y_inv+level_y_start])){
						GameObject bgt = (GameObject)Instantiate(tile);
						bgt.name = "x";
						bgt.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(GlobalData.TILESPRITEPATHS["x"]);
						bgt.transform.position = new Vector2(x,y-level_y_start);
						bgt.transform.parent = map_go.transform;
					
						GameObject nt = (GameObject)Instantiate(tile);
						nt.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(GlobalData.NPCPATHS[GlobalData.map[x, y_inv+level_y_start]]);
						nt.GetComponent<SpriteRenderer>().color = Color.white;
						nt.name = "o";
						nt.transform.position = new Vector2(x,y-level_y_start);
						nt.transform.parent = map_go.transform;
						SetxmaxSize(x);
					}
					//JoaoNPCTile
				}else{

				}
			}
		}


	//	test_enemy.GetComponent<EnemyControl>().Init(new EnemyStatus(0,0,0,0,1, 1, 0), Mathf.RoundToInt(enemy_spawn_points[0].x), Mathf.RoundToInt(enemy_spawn_points[0].y), 1);
	//	test_enemy.SetActive(true);
	}




	bool inicializationscompleted=false;
	
	int totalenemiesperwave=0, currenemiesinwave=0;
	int totalwaves=0, currwave=0;
	float timebeweenwaves=8f, timesincelastwave;
	float timebeweenenemies=2f, timesincelastenemie;
	int totalPaths=4,currpath=1;
	
	Vector2[] EnemyPos = {new Vector2(-8.42f,3.39f), new Vector2(-8.42f,1.25f),new Vector2(-2.85f,5.47f), new Vector2(0,5.47f)};
	bool can_start_enemy_waves = false;
//JOAO CODE
	void CSVLoadEnemyWaves(){
		LoadCSVLine(GlobalData.current_difficulty, GlobalData.current_level);
		totalwaves=GlobalData.ENEMIESLEVEL[7];
		totalenemiesperwave=GlobalData.ENEMIESLEVEL[8];
		
		timesincelastwave=timebeweenwaves;
		timesincelastenemie=timebeweenenemies;
		//can_start_enemy_waves = true;
		//Choose the first path for Wave
		//currpath=Random.Range (0,totalPaths);
	}
	
	
	//change this to line to array
	void LoadCSVLine(int difficulty,int level){
		//GlobalData.ENEMIESLEVEL.Clear();
		GlobalData.ENEMIESAVAILABLE.Clear();
		GlobalData.ENEMIESTOTALPERWAVE.Clear();

		int levellineincsv=level+difficulty*10;
		TextAsset csv;
		csv = (TextAsset)Resources.Load("CSV/EnemiesLevel");
		string[,] csvarr = CSVReader.SplitCsvGrid(csv.text);
		for(int i=0;i<=8;i++)
		{
			int numberofenemiesoftype=int.Parse(csvarr[i+2,levellineincsv]);
			GlobalData.ENEMIESLEVEL[i]=numberofenemiesoftype;
			
			
			if(i>=0 && i<=6 && numberofenemiesoftype>0)
			{
				GlobalData.ENEMIESAVAILABLE.Add(i+1);
				GlobalData.ENEMIESTOTALPERWAVE.Add(i+1,numberofenemiesoftype);
			}
		}
		
		
	}
	
	bool tutorialenemy;
	
	void EnemyWaves(){
		if(beforefirstwavetime)
		{
			if(GlobalData.current_level==1 && GlobalData.current_difficulty==0)
			{
				beforefirstwavetime=false;
			}
			else
			{
				if(can_start_enemy_waves)
				{
					if(before_firstwave_time<before_firstwave_timer)
					{
						before_firstwave_time+=Time.deltaTime;
					}
					else
					{
						beforefirstwavetime=false;
					}
				}
			}

		}
		else
		{
			if(PlayerData.level_state==0){
			if(can_start_enemy_waves){
				if(currwave<totalwaves) {
				//First Loop Enemy Waves
					if(timesincelastwave<timebeweenwaves) {
						timesincelastwave+=Time.deltaTime;
					//Debug.Log ("nextwave in "+ (timebeweenwaves - timesincelastwave));
					} else {
						if(currenemiesinwave==totalenemiesperwave) {
							currwave++;
							currenemiesinwave=0;
							timesincelastwave=0;
						
							currpath=Random.Range (0,totalPaths);
						} else {
							// Second Loop Enemies Per Wave
							if(timesincelastenemie<timebeweenenemies){
								timesincelastenemie+=Time.deltaTime;
							}else{
								if(currenemiesinwave==0){
									spawnpoint_index = Random.Range(0, enemy_spawn_points.Count);
								}
							currenemiesinwave++;
							
							
							Instantiate_Enemies();
							timesincelastenemie=0;
							}
					 	}
					}
				} else {
					Debug.Log ("BossWave");
					int enemytype =0 ;
					
					int x = Mathf.RoundToInt(enemy_spawn_points[spawnpoint_index].x);
					int y = Mathf.RoundToInt(enemy_spawn_points[spawnpoint_index].y);
					//EnemyInstantiation
					//	Debug.Log("ENEMY AT SPAW at "+ spawnpoint_index + " of " + enemy_spawn_points.Count + "pos " +  Mathf.RoundToInt(enemy_spawn_points[spawnpoint_index].x)+","+ Mathf.RoundToInt(enemy_spawn_points[spawnpoint_index].y));
					int dir = GlobalData.map[x, y].Equals("sv")? 1 : GlobalData.map[x, y].Equals("sh") ? 3 : 1;
					Debug.Log (GlobalData.ENEMYTILEPATHS[enemytype]);
					boss = (GameObject)Instantiate(Resources.Load(GlobalData.ENEMYTILEPATHS[enemytype]));
					boss.transform.parent = enemies_go.transform;
					boss.GetComponent<EnemyControl>().Init( GlobalData.EnemiesStatus[enemytype][GlobalData.current_difficulty] , x, y, dir);
					can_start_enemy_waves = false;
				}
			}
			}
		}
	}



	int spawnpoint_index = 0 ;
	GameObject Enemy;
	void Instantiate_Enemies(){
		int numberofenemies=GlobalData.ENEMIESAVAILABLE.Count;
		if(numberofenemies>0) {
			int randomenemytype=Random.Range (0,numberofenemies);
			int enemytype=GlobalData.ENEMIESAVAILABLE[randomenemytype];
			Debug.Log (numberofenemies+" "+enemytype+" "+randomenemytype);

			Debug.Log (!tutorialenemy+" "+GlobalData.current_level+" "+GlobalData.current_difficulty);
			if(!tutorialenemy && GlobalData.current_level==1 && GlobalData.current_difficulty==0)
			{
				Debug.Log ("inside");
				enemytype=6;
				randomenemytype=1;
				tutorialenemy=true;
			}
			int x = Mathf.RoundToInt(enemy_spawn_points[spawnpoint_index].x);
			int y = Mathf.RoundToInt(enemy_spawn_points[spawnpoint_index].y);

			//EnemyInstantiation
		//	Debug.Log("ENEMY AT SPAW at "+ spawnpoint_index + " of " + enemy_spawn_points.Count + "pos " +  Mathf.RoundToInt(enemy_spawn_points[spawnpoint_index].x)+","+ Mathf.RoundToInt(enemy_spawn_points[spawnpoint_index].y));
			int dir = GlobalData.map[x, y].Equals("sv")? 1 : GlobalData.map[x, y].Equals("sh") ? 3 : 1;
		
			Enemy = (GameObject)Instantiate(Resources.Load(GlobalData.ENEMYTILEPATHS[enemytype]));
			Enemy.transform.parent = enemies_go.transform;
			Enemy.GetComponent<EnemyControl>().Init( GlobalData.EnemiesStatus[enemytype][GlobalData.current_difficulty] , x, y, dir);
			//Enemy.transform.position=new Vector3 (EnemyPos[currpath][0],EnemyPos[currpath][1],0);
			
			GlobalData.ENEMIESTOTALPERWAVE[enemytype]--;
			Debug.Log (randomenemytype+" "+GlobalData.ENEMIESTOTALPERWAVE[enemytype]+" "+enemytype);
			if(GlobalData.ENEMIESTOTALPERWAVE[enemytype]==0)
			{
				
				GlobalData.ENEMIESTOTALPERWAVE.Remove(enemytype);
				GlobalData.ENEMIESAVAILABLE.RemoveAt(randomenemytype);
				
			}
		}
		
		
	}


	void CheckWinLoseConditions(){
		if(PlayerData.level_state == 0){
		if(PlayerData.current_energy <= 0){
				Lose ();
		}else{
			if(boss!=null){
				if(boss.GetComponent<EnemyControl>().isdead){
						/*if(GlobalData.current_level==10){
							StartEndAnim();
						}else{
							Win ();
						}*/
						Win ();
						Time.timeScale = 1;


						GameObject APIs=GameObject.Find("APIs").gameObject;
						//Submit Score
						#if !UNITY_EDITOR
						APIs.GetComponent<APIManager>().SubmitScore(PlayerData.current_score);
						#endif

						
				}
			}
		}
		}
	}

	void Win(){
		PlayerData.level_state = 1;

		StopAllEnemies();
		UnlockLevels();
		PlayerData.current_score+=Mathf.CeilToInt(5*(PlayerData.current_energy));
		UI_InGame.transform.Find("WinMenu").Find("ScorePopup").GetChild(0).GetComponent<Text>().text ="Score\n"+PlayerData.current_score;
		UI_InGame.transform.Find("WinMenu").gameObject.SetActive(true);
		PlayerData.main_tower.GetComponent<Animator>().SetInteger("p_status", 2);
		UI_InGame.transform.Find("TD").gameObject.SetActive(false);
	}
	void Lose(){
		PlayerData.level_state = -1;
		StopAllEnemies();
		PlayerData.current_score+=Mathf.CeilToInt(5*(PlayerData.current_energy));
		UI_InGame.transform.Find("LoseMenu").Find("ScorePopup").GetChild(0).GetComponent<Text>().text ="Score\n"+PlayerData.current_score;
		UI_InGame.transform.Find("LoseMenu").gameObject.SetActive(true);
		PlayerData.main_tower.GetComponent<Animator>().SetInteger("p_status", 3);
		UI_InGame.transform.Find("TD").gameObject.SetActive(false);
		Time.timeScale = 1;
		PlayerData.current_score = 0;
	}
	void StopAllEnemies(){
		int total = enemies_go.transform.childCount;
		for(int i=0; i<total; i++){
			enemies_go.transform.GetChild(i).GetComponent<EnemyControl>().Stop();
		}
	}

	void UnStopAllEnemies(){
		int total = enemies_go.transform.childCount;
		for(int i=0; i<total; i++){
			enemies_go.transform.GetChild(i).GetComponent<EnemyControl>().UnStop();
		}
	}

	 

	void StopAllTowers(){
		int total = towers_go.transform.childCount;
		for(int i=0; i<total; i++){
			//towers_go.transform.GetChild(i).GetChild(0).GetComponent<TowerControl>().Stop();
		}
	}
	
	void UnStopAllTowers(){
		int total = towers_go.transform.childCount;
		for(int i=0; i<total; i++){
			//towers_go.transform.GetChild(i).GetChild(0).GetComponent<TowerControl>().UnStop();
		}
	}




	void UnlockLevels(){
		if(GlobalData.current_level==10 && GlobalData.current_difficulty<2){
			if(GlobalData.current_difficulty+1< GlobalData.DifficultiesUnlocked.Count){
				GlobalData.DifficultiesUnlocked[GlobalData.current_difficulty+1] = true;
				GlobalData.LevelsUnlocked[GlobalData.current_difficulty][0] = true;
			}
		}else if(GlobalData.current_level<10){
			Debug.Log ("unlocking level " + GlobalData.current_level);
			GlobalData.LevelsUnlocked[GlobalData.current_difficulty][GlobalData.current_level] = true;
		}
	}


	Animator end_anim;
	Transform end_anim_tr;
	bool doing_endanim=false;
	private void StartEndAnim(){
		end_anim_tr =GameObject.Find("Canvas").transform.Find("InGame").gameObject.transform.Find("EndAnim");
		end_anim_tr.gameObject.SetActive(true);
		
		end_anim_tr.GetComponent<Button>().onClick.RemoveAllListeners();
		end_anim_tr.GetComponent<Button>().onClick.AddListener(()=>{
			end_anim_tr.gameObject.SetActive(false);
			//Win();
			GameObject.Find("MainMenu").transform.Find("LevelsMenu").GetComponent<LevelsMenu>().StartLevel(GlobalData.current_level+1);
		});
		end_anim = end_anim_tr.GetComponent<Animator>();
		doing_endanim=true;
		//quiz_load=2;
	}


	private void CheckEndAnimEnded(){
		if(doing_endanim){
		if(end_anim!=null){
		if(end_anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.End")){
			end_anim_tr.gameObject.SetActive(false);
			Win ();
		}
		}
		}
	}
 
	bool on_tutorial = false;
	public void TutorialStartCheck(){
		if(!on_tutorial){
			if(GlobalData.current_tutorial==-1){
				Tutorial_Setup(0); // enemy tutorial by time
			}else if(GlobalData.current_tutorial == 0){
				Tutorial_Setup(1); // tower spot tutorial by action
			}else if(GlobalData.current_tutorial == 1){
				Tutorial_Setup(2); //tower build tutorial2 - build by action
			}else if(GlobalData.current_tutorial == 2){
				Tutorial_Setup(3); //energy tutorial by time
			}else if(GlobalData.current_tutorial == 3){
				Tutorial_Setup(4); // tower rud - tap to see many options by action
			}else if(GlobalData.current_tutorial == 4){
				Tutorial_Setup(5); // tower rud - repair by action
			}else if(GlobalData.current_tutorial == 5){
				Tutorial_Setup(6); // tower rud - upgrade by action
			}else if(GlobalData.current_tutorial == 6){
				Tutorial_Setup(7); // Tower rud - destroy by action
			}else if(GlobalData.current_tutorial == 7){
				Tutorial_Setup(8); // Win Lose Conditions by time
			}
		}
	}

	public void ActedOnTutorial(){
		EndTutorial();
	}

	List<GameObject> ingame_tutorial_refs = new List<GameObject>();


	private Animator TutObjects,TutBaloon;
	private int baloontutstatestate=-1;
	private void Tutorial_Setup(int tutorial_id){
		TutObjects = UI_Tutorial.transform.Find("TutorialObjects").gameObject.transform.Find("INES_Tutorial").GetComponent<Animator>();
		TutBaloon = UI_Tutorial.transform.Find("TutorialObjects").gameObject.transform.Find("Baloon").GetComponent<Animator>();
		Debug.Log ("Tutorial_Setup"+tutorial_id);
		if(tutorial_id == 0){
			if(this.transform.Find("Enemies").childCount>0){
				ingame_tutorial_refs.Add(this.transform.Find("Enemies").GetChild(0).gameObject);
				UI_Tutorial.transform.Find("Arrows").GetChild(0).GetComponent<Tutorial_Arrow>().Init(ingame_tutorial_refs[0], new Vector2(1, 0), true);

				UI_Tutorial.transform.Find("TutorialObjects").transform.Find("Baloon").GetChild(0).GetComponent<Text>().text = "Cuidado! Não deixes os agentes do dark server chegar ao teu computador";
				GlobalData.current_tutorial=tutorial_id;
				on_tutorial = true;
				StartCoroutine(ActionAfterTimer.Set(1, delegate {
					PlayerData.level_state = 2;
				}));

				UI_Tutorial.SetActive(true);
				TutObjects.SetInteger("state",1);
				TutBaloon.SetInteger("state",1);

				StartCoroutine(ActionAfterTimer.Set(0.5f, delegate {
					TutBaloon.SetInteger("state",0);
				}));

				StartCoroutine(ActionAfterTimer.Set(5, delegate {
					EndTutorial();
				}));
			}

		}else if(tutorial_id == 1){
			ingame_tutorial_refs.Add(this.transform.Find("Towers").Find("0").gameObject);
			UI_Tutorial.transform.Find("Arrows").GetChild(0).GetComponent<Tutorial_Arrow>().Init(ingame_tutorial_refs[0], new Vector2(1,0), true);
			UI_Tutorial.transform.Find("TutorialObjects").transform.Find("Baloon").GetChild(0).GetComponent<Text>().text = "Constroi defesas nestas bases de construção";
			GlobalData.current_tutorial=tutorial_id;
			on_tutorial = true;
			PlayerData.level_state = 2;
			TutBaloon.SetInteger("state",1);
			StartCoroutine(ActionAfterTimer.Set(0.5f, delegate {
				TutBaloon.SetInteger("state",0);
			}));
			//UI_Tutorial.SetActive(true);
		}else if(tutorial_id == 2){
			UI_InGame.transform.Find("TD").Find("TowerSpotsGUIs").gameObject.SetActive(true);
			ingame_tutorial_refs.Add(UI_InGame.transform.Find("TD").Find("energy_bar").gameObject);
			UI_Tutorial.transform.Find("Arrows").GetChild(0).GetComponent<Tutorial_Arrow>().Init(ingame_tutorial_refs[0], new Vector2(1, 0), false);
			UI_Tutorial.transform.Find("TutorialObjects").transform.Find("Baloon").GetChild(0).GetComponent<Text>().text = "Construir consome energia! Cuidado para não chegares ao zero!";
			GlobalData.current_tutorial=tutorial_id;
			on_tutorial = true;
			PlayerData.level_state = 2;
			//UI_Tutorial.SetActive(true);
			TutBaloon.SetInteger("state",1);
			StartCoroutine(ActionAfterTimer.Set(0.5f, delegate {
				TutBaloon.SetInteger("state",0);
			}));
			StartCoroutine(ActionAfterTimer.Set(5, delegate {
				EndTutorial();
			}));
		}else if(tutorial_id == 3){
			ingame_tutorial_refs.Add(UI_InGame.transform.Find("TD").Find("TowerSpotsGUIs").Find("tbtns").Find("btn_Tower1").gameObject);
			UI_Tutorial.transform.Find("Arrows").GetChild(0).GetComponent<Tutorial_Arrow>().Init(ingame_tutorial_refs[0], new Vector2(1, 0), false);
			UI_Tutorial.transform.Find("TutorialObjects").transform.Find("Baloon").GetChild(0).GetComponent<Text>().text = "Toca num botão de uma defesa para construir";
			GlobalData.current_tutorial=tutorial_id;
			on_tutorial = true;
			PlayerData.level_state = 2;
			UI_Tutorial.SetActive(true);
			TutBaloon.SetInteger("state",1);
			StartCoroutine(ActionAfterTimer.Set(0.5f, delegate {
				TutBaloon.SetInteger("state",0);
			}));
		}else if(tutorial_id == 4){
			ingame_tutorial_refs.Add(this.transform.Find("Towers").Find("0").gameObject);
			UI_Tutorial.transform.Find("Arrows").GetChild(0).GetComponent<Tutorial_Arrow>().Init(ingame_tutorial_refs[0], new Vector2(1,0), true);
			UI_Tutorial.transform.Find("TutorialObjects").transform.Find("Baloon").GetChild(0).GetComponent<Text>().text = "Tocar numa defesa tens a opção para reparar, melhorar e remover";
			GlobalData.current_tutorial=tutorial_id;
			on_tutorial = true;
			PlayerData.level_state = 2;
			UI_Tutorial.SetActive(true);
			TutBaloon.SetInteger("state",1);
			StartCoroutine(ActionAfterTimer.Set(0.5f, delegate {
				TutBaloon.SetInteger("state",0);
			}));
		}else if(tutorial_id == 5){
			this.transform.Find("Towers").Find("0").GetComponent<TowerSpotControl>().OpenMenu();
			ingame_tutorial_refs.Add(UI_InGame.transform.Find("TD").Find("TowerSpotsGUIs").Find("tbtns").Find("btn_Repair").gameObject);
			UI_Tutorial.transform.Find("Arrows").GetChild(0).GetComponent<Tutorial_Arrow>().Init(ingame_tutorial_refs[0], new Vector2(1, 0), false);
			UI_Tutorial.transform.Find("TutorialObjects").transform.Find("Baloon").GetChild(0).GetComponent<Text>().text = "Aqui reparas a defesa!";
			GlobalData.current_tutorial=tutorial_id;
			on_tutorial = true;
			PlayerData.level_state = 2;
			UI_Tutorial.SetActive(true);
			TutBaloon.SetInteger("state",1);
			StartCoroutine(ActionAfterTimer.Set(0.5f, delegate {
				TutBaloon.SetInteger("state",0);
			}));
		}else if(tutorial_id == 6){
			this.transform.Find("Towers").Find("0").GetComponent<TowerSpotControl>().OpenMenu();
			
			ingame_tutorial_refs.Add(UI_InGame.transform.Find("TD").Find("TowerSpotsGUIs").Find("tbtns").Find("btn_Upgrade").gameObject);
			UI_Tutorial.transform.Find("Arrows").GetChild(0).GetComponent<Tutorial_Arrow>().Init(ingame_tutorial_refs[0], new Vector2(1, 0), false);
			UI_Tutorial.transform.Find("TutorialObjects").transform.Find("Baloon").GetChild(0).GetComponent<Text>().text = "Neste botão fazes uma melhoria à defesa";
			GlobalData.current_tutorial=tutorial_id;
			on_tutorial = true;
			PlayerData.level_state = 2;
			UI_Tutorial.SetActive(true);
			TutBaloon.SetInteger("state",1);
			StartCoroutine(ActionAfterTimer.Set(0.5f, delegate {
				TutBaloon.SetInteger("state",0);
			}));
		}else if(tutorial_id == 7){
			this.transform.Find("Towers").Find("0").GetComponent<TowerSpotControl>().OpenMenu();
			
			ingame_tutorial_refs.Add(UI_InGame.transform.Find("TD").Find("TowerSpotsGUIs").Find("tbtns").Find("btn_Delete").gameObject);
			UI_Tutorial.transform.Find("Arrows").GetChild(0).GetComponent<Tutorial_Arrow>().Init(ingame_tutorial_refs[0], new Vector2(1, 0), false);
			UI_Tutorial.transform.Find("TutorialObjects").transform.Find("Baloon").GetChild(0).GetComponent<Text>().text = "Este botão auto-destroi a tua Torre de defesa";
			GlobalData.current_tutorial=tutorial_id;
			on_tutorial = true;
			PlayerData.level_state = 2;
			UI_Tutorial.SetActive(true);
			TutBaloon.SetInteger("state",1);
			StartCoroutine(ActionAfterTimer.Set(0.5f, delegate {
				TutBaloon.SetInteger("state",0);
			}));


		}else if(tutorial_id == 8){
			//UI_Tutorial.SetActive(false);
			//GlobalData.current_tutorial=tutorial_id;
			GlobalData.TutCanBuildFree=true;
			UI_Tutorial.transform.Find("Arrows").GetChild(0).gameObject.SetActive(false);
			UI_Tutorial.transform.Find("TutorialObjects").transform.Find("Baloon").gameObject.SetActive(false);
			TutObjects.SetInteger("state",2);
			if(this.transform.Find("Enemies").Find("0")!=null){
				//Todo Center camera in dark server, red lights and 3 secondslowdown
				//Vector3 DarkServerPos=this.transform.FindChild("Enemies").gameObject.transform.FindChild("0").gameObject.transform.FindChild("Enemy").transform.position;
				//Camera.main.transform.position= new Vector3 (DarkServerPos.x,DarkServerPos.y,-10);
				//

				TutObjects.SetInteger("state",0);
				TutBaloon.SetInteger("state",1);
				StartCoroutine(ActionAfterTimer.Set(0.5f, delegate {
					TutBaloon.SetInteger("state",0);
				}));
				UI_Tutorial.transform.Find("Arrows").GetChild(0).gameObject.SetActive(true);
				UI_Tutorial.transform.Find("TutorialObjects").transform.Find("Baloon").gameObject.SetActive(true);
				ingame_tutorial_refs.Add(this.transform.Find("Enemies").Find("0").gameObject);
				UI_Tutorial.transform.Find("Arrows").GetChild(0).GetComponent<Tutorial_Arrow>().Init(ingame_tutorial_refs[0], new Vector2(1, 0), true);
				UI_Tutorial.transform.Find("TutorialObjects").transform.Find("Baloon").GetChild(0).GetComponent<Text>().text = "Destroi o Dark Server antes que ele te destrua...";

				on_tutorial = true;
				//UI_Tutorial.transform.FindChild("DarkServerAlert").gameObject.SetActive(true);
				UI_Tutorial.SetActive(true);
				StartCoroutine(ActionAfterTimer.Set(3, delegate {
					//UI_Tutorial.transform.FindChild("DarkServerAlert").gameObject.SetActive(false);
					TutObjects.SetInteger("state",3);
					ingame_tutorial_refs.Clear();
					PlayerData.level_state = 0;
					UI_Tutorial.transform.Find("Arrows").GetChild(0).gameObject.SetActive(false);
					UI_Tutorial.transform.Find("TutorialObjects").transform.Find("Baloon").gameObject.SetActive(false);
					UI_Tutorial.SetActive(false);
					/*StartCoroutine(ActionAfterTimer.Set(0.5f, delegate {
						UI_Tutorial.SetActive(false);
						UI_Tutorial.transform.FindChild("Arrows").GetChild(0).gameObject.SetActive(true);
						UI_Tutorial.transform.FindChild("TutorialObjects").transform.FindChild("Baloon").gameObject.SetActive(true);
					}));*/
				}));
			}
		}
	}

	/* SAved Tutorial Setup
	private void Tutorial_Setup(int tutorial_id){
		if(tutorial_id == 0){
			if(this.transform.FindChild("Enemies").childCount>0){
				ingame_tutorial_refs.Add(this.transform.FindChild("Enemies").GetChild(0).gameObject);
				UI_Tutorial.transform.FindChild("Arrows").GetChild(0).GetComponent<Tutorial_Arrow>().Init(ingame_tutorial_refs[0], new Vector2(1, 0), true);
				UI_Tutorial.transform.FindChild("ExplainBox").GetChild(1).GetComponent<Text>().text = "Cuidado! Não deixes os agentes do dark server chegar ao teu computador";

				GlobalData.current_tutorial=tutorial_id;
				on_tutorial = true;
				//PlayerData.level_state = 2;
				UI_Tutorial.SetActive(true);
				
				StartCoroutine(ActionAfterTimer.Set(5, delegate {
					EndTutorial();
				}));
			}
		}else if(tutorial_id == 1){
			ingame_tutorial_refs.Add(this.transform.FindChild("Towers").FindChild("0").gameObject);
			UI_Tutorial.transform.FindChild("Arrows").GetChild(0).GetComponent<Tutorial_Arrow>().Init(ingame_tutorial_refs[0], new Vector2(1,0), true);
			UI_Tutorial.transform.FindChild("ExplainBox").GetChild(1).GetComponent<Text>().text = "Para construíres defesas toca nos pontos de construção";
			GlobalData.current_tutorial=tutorial_id;
			on_tutorial = true;
			PlayerData.level_state = 2;
			UI_Tutorial.SetActive(true);
		}else if(tutorial_id == 2){
			ingame_tutorial_refs.Add(UI_InGame.transform.FindChild("TD").FindChild("TowerSpotsGUIs").FindChild("tbtns").FindChild("btn_Tower1").gameObject);
			UI_Tutorial.transform.FindChild("Arrows").GetChild(0).GetComponent<Tutorial_Arrow>().Init(ingame_tutorial_refs[0], new Vector2(1, 0), false);
			UI_Tutorial.transform.FindChild("ExplainBox").GetChild(1).GetComponent<Text>().text = "Se tocares num botão de uma defesa podes ver a sua descrião";
			GlobalData.current_tutorial=tutorial_id;
			on_tutorial = true;
			PlayerData.level_state = 2;
			UI_Tutorial.SetActive(true);
		}else if(tutorial_id == 3){
			ingame_tutorial_refs.Add(UI_InGame.transform.FindChild("TD").FindChild("TowerSpotsGUIs").FindChild("tbtns").FindChild("btn_Tower1").gameObject);
			UI_Tutorial.transform.FindChild("Arrows").GetChild(0).GetComponent<Tutorial_Arrow>().Init(ingame_tutorial_refs[0], new Vector2(1, 0), false);
			UI_Tutorial.transform.FindChild("ExplainBox").GetChild(1).GetComponent<Text>().text = "Se tocares novamente no mesmo botão podes construí-la";
			GlobalData.current_tutorial=tutorial_id;
			on_tutorial = true;
			PlayerData.level_state = 2;
			UI_Tutorial.SetActive(true);
		}else if(tutorial_id == 4){
			ingame_tutorial_refs.Add(UI_InGame.transform.FindChild("TD").FindChild("energy_bar").gameObject);
			UI_Tutorial.transform.FindChild("Arrows").GetChild(0).GetComponent<Tutorial_Arrow>().Init(ingame_tutorial_refs[0], new Vector2(1, 0), false);
			UI_Tutorial.transform.FindChild("ExplainBox").GetChild(1).GetComponent<Text>().text = "A construção de defesas consome energia, cuidado para não chegares a zero";
			GlobalData.current_tutorial=tutorial_id;
			on_tutorial = true;
			//PlayerData.level_state = 2;
			UI_Tutorial.SetActive(true);
			StartCoroutine(ActionAfterTimer.Set(5, delegate {
				EndTutorial();
			}));
		}else if(tutorial_id == 5){
			ingame_tutorial_refs.Add(this.transform.FindChild("Towers").FindChild("0").gameObject);
			UI_Tutorial.transform.FindChild("Arrows").GetChild(0).GetComponent<Tutorial_Arrow>().Init(ingame_tutorial_refs[0], new Vector2(1,0), true);
			UI_Tutorial.transform.FindChild("ExplainBox").GetChild(1).GetComponent<Text>().text = "Se tocares numa defesa tens a opção de fazer reparações, upgrades ou remover";
			GlobalData.current_tutorial=tutorial_id;
			on_tutorial = true;
			PlayerData.level_state = 2;
			UI_Tutorial.SetActive(true);
		}else if(tutorial_id == 6){
			this.transform.FindChild("Towers").FindChild("0").GetComponent<TowerSpotControl>().OpenMenu();
			ingame_tutorial_refs.Add(UI_InGame.transform.FindChild("TD").FindChild("TowerSpotsGUIs").FindChild("tbtns").FindChild("btn_Repair").gameObject);
			UI_Tutorial.transform.FindChild("Arrows").GetChild(0).GetComponent<Tutorial_Arrow>().Init(ingame_tutorial_refs[0], new Vector2(1, 0), false);
			UI_Tutorial.transform.FindChild("ExplainBox").GetChild(1).GetComponent<Text>().text = "Este botão repara a defesa, a um custo de energia";
			GlobalData.current_tutorial=tutorial_id;
			on_tutorial = true;
			PlayerData.level_state = 2;
			UI_Tutorial.SetActive(true);
		}else if(tutorial_id == 7){
			this.transform.FindChild("Towers").FindChild("0").GetComponent<TowerSpotControl>().OpenMenu();
			
			ingame_tutorial_refs.Add(UI_InGame.transform.FindChild("TD").FindChild("TowerSpotsGUIs").FindChild("tbtns").FindChild("btn_Upgrade").gameObject);
			UI_Tutorial.transform.FindChild("Arrows").GetChild(0).GetComponent<Tutorial_Arrow>().Init(ingame_tutorial_refs[0], new Vector2(1, 0), false);
			UI_Tutorial.transform.FindChild("ExplainBox").GetChild(1).GetComponent<Text>().text = "Este botão faz um upgrade à defesa, a um custo de energia ";
			GlobalData.current_tutorial=tutorial_id;
			on_tutorial = true;
			PlayerData.level_state = 2;
			UI_Tutorial.SetActive(true);
		}else if(tutorial_id == 8){
			this.transform.FindChild("Towers").FindChild("0").GetComponent<TowerSpotControl>().OpenMenu();
			
			ingame_tutorial_refs.Add(UI_InGame.transform.FindChild("TD").FindChild("TowerSpotsGUIs").FindChild("tbtns").FindChild("btn_Delete").gameObject);
			UI_Tutorial.transform.FindChild("Arrows").GetChild(0).GetComponent<Tutorial_Arrow>().Init(ingame_tutorial_refs[0], new Vector2(1, 0), false);
			UI_Tutorial.transform.FindChild("ExplainBox").GetChild(1).GetComponent<Text>().text = "Este botão remove a defesa";
			GlobalData.current_tutorial=tutorial_id;
			on_tutorial = true;
			PlayerData.level_state = 2;
			UI_Tutorial.SetActive(true);
		}else if(tutorial_id == 9){
			if(this.transform.FindChild("Enemies").FindChild("0")!=null){
				ingame_tutorial_refs.Add(this.transform.FindChild("Enemies").FindChild("0").gameObject);
				UI_Tutorial.transform.FindChild("Arrows").GetChild(0).GetComponent<Tutorial_Arrow>().Init(ingame_tutorial_refs[0], new Vector2(1, 0), true);
				UI_Tutorial.transform.FindChild("ExplainBox").GetChild(1).GetComponent<Text>().text = "Destroi o Dark Server antes que ele te destrua a ti!";
				GlobalData.current_tutorial=tutorial_id;
				on_tutorial = true;
				 
				UI_Tutorial.SetActive(true);
				StartCoroutine(ActionAfterTimer.Set(5, delegate {
					EndTutorial();
				}));
			}
		}
	}*/

 
	public void EndTutorial(){
		int count = 0;
		//UI_Tutorial.SetActive(false);


		ingame_tutorial_refs.Clear();
		on_tutorial = false;
		PlayerData.level_state = 0;
	}
}
