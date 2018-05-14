using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
public class GlobalData : MonoBehaviour {
	// ----------------------------     GENERAL
	public const int DEFAULT_ENERGY = 50;
	public static List<int> font_sizes = new List<int>(){300,35,32,28,25,20};

	public static Dictionary<int, string> SFX_Paths = new Dictionary<int, string>(){
		{0, "SFX/btn_common"},
		{1, "SFX/btn_main"},
		{2, "SFX/Dark_explosion"},
		{3, "SFX/Dark_walk"},
		{4, "SFX/E_explosion"},
		{5, "SFX/E_walk"},
		{6, "SFX/T_shooter"}, //tower 1
		{7, "SFX/T_tesla_fire"},//tower 2
		{8, "SFX/T_firewall_shooting"},//tower 3
		{9, "SFX/T_encriptacao"},//tower 4, 5
		{10, "SFX/T_build"},
		{11, "SFX/T_destroy"},
		{12, "SFX/T_repair"},
		{13, "SFX/T_upgrade"},
		{14, "SFX/zoom"}
	};

	public static Dictionary<int, string> Music_Paths = new Dictionary<int, string>(){
		{0,"Music/main_INES"},
		{1,"Music/after_quiz"},
		{2,"Music/ingame_INES"}
	};
	//---------------------------------   QUIZ

	public static int[] quiz_energy_bonus = new int[]{25, 15, 10};
	public static List<QuizQuestion> quiz_questions = new List<QuizQuestion>();
	public static Dictionary<int, List<QuizQuestion>> SAVEDQUIZQUESTIONS = new Dictionary<int, List<QuizQuestion>>();
	
	//----------------------------------  TOWER DEFENSE
	//TD VARS
	public static string[,] map;
	public static int max_random_objects = 5;

	public static int current_level;
	public static int current_difficulty;
	public static Dictionary<int, int> difficulty_ymap_size = new Dictionary<int, int>(){{0, 12},{1, 16},{2, 32}};
	public static int dificulty_xmap_size=0;
	public static int level_y_start = 0;
	public static int level_y_end = 0;


	public static void LoadLevelMaps(int d, int lvl){
		map = CSVReader.GridToArray("CSV/"+d.ToString());
		current_level = lvl;
		current_difficulty = d;

		level_y_start =  (current_level-1)+(current_level-1)*difficulty_ymap_size[current_difficulty];
		level_y_end = level_y_start + difficulty_ymap_size[current_difficulty];
	}

	//TD CONSTANTS
	public static Dictionary<string, string> TILESPRITEPATHS = new Dictionary<string, string>(){
		{"sh",  "Sprites/Tiles/Paths/h"},
		{"sv", "Sprites/Tiles/Paths/v"},
		{"h",  "Sprites/Tiles/Paths/h"},
		{"v", "Sprites/Tiles/Paths/v"},
		{"4", "Sprites/Tiles/Paths/4"},
		{"ld", "Sprites/Tiles/Paths/ld"},
		{"lu", "Sprites/Tiles/Paths/lu"},
		{"rd", "Sprites/Tiles/Paths/rd"},
		{"ru", "Sprites/Tiles/Paths/ru"},
		{"hd", "Sprites/Tiles/Paths/hd"},
		{"hu", "Sprites/Tiles/Paths/hu"},
		{"vl", "Sprites/Tiles/Paths/vl"},
		{"vr", "Sprites/Tiles/Paths/vr"},
		{"g", "Sprites/Tiles/Paths/4"},
		{"x", "Sprites/Tiles/Paths/x_0"}
	};
	//TODO Change these
	public static List<string> RANDOBJSPRITEPATHS = new List<string>{
		"Sprites/Tiles/Other/rand_a",
		"Sprites/Tiles/Other/rand_b",
		"Sprites/Tiles/Other/rand_c"
	};

	public static Dictionary<int, string> ENEMYTILEPATHS = new Dictionary<int, string>(){
		{0, "Prefabs/Enemies/0"},
		{1, "Prefabs/Enemies/1"},
		{2, "Prefabs/Enemies/2"},
		{3, "Prefabs/Enemies/3"},
		{4, "Prefabs/Enemies/4"},
		{5, "Prefabs/Enemies/5"},
		{6, "Prefabs/Enemies/6"},
		{7, "Prefabs/Enemies/7"}
	};


	public static Dictionary<string, bool[]> TILEDIR = new Dictionary<string, bool[]>(){
		{"sh", new bool[]{false, false, false, true}},
		{"sv", new bool[]{false, true, false, false}},
		{"h", new bool[]{false, false, true, true}},
		{"v", new bool[]{true, true, false, false}},
		{"4", new bool[]{true, true, true, true}},
		{"ld", new bool[]{false, true, true, false}},
		{"lu", new bool[]{true, false, true, false}},
		{"rd", new bool[]{false, true, false, true}},
		{"ru", new bool[]{true, false, false, true}},
		{"hd", new bool[]{false, true, true, true}},
		{"hu", new bool[]{true, false, true, true}},
		{"vl", new bool[]{true, true, true, false}},
		{"vr", new bool[]{true, true, false, true}},
	};
	public static Dictionary<string, string> NPCPATHS = new Dictionary<string, string>(){
		{"o1",  "Sprites/Tiles/NPC/o1"},
		{"o2", "Sprites/Tiles/NPC/o2"},
		{"o3",  "Sprites/Tiles/NPC/o3"},
		{"o4", "Sprites/Tiles/NPC/o4"}
	};
 

	public static Dictionary<int, string> TOWER_PATHS= new Dictionary<int, string>(){
		{1, "Prefabs/Towers/1"},
		{2, "Prefabs/Towers/2"},
		{3, "Prefabs/Towers/3"},
		{4, "Prefabs/Towers/4"},
		{5, "Prefabs/Towers/5"}
	};
	public static Dictionary<int, List<string>> TOWERSPRITEPATHS = new Dictionary<int, List<string>>(){
		{1, new List<string>(){"Sprites/Towers/Tower1image_evo1", "Sprites/Towers/Tower1image_evo2", "Sprites/Towers/Tower1image_evo3", "Sprites/Towers/Tower1image_evo4"}},
		{2, new List<string>(){"Sprites/Towers/Tower2image_evo1", "Sprites/Towers/Tower2image_evo2", "Sprites/Towers/Tower2image_evo3", "Sprites/Towers/Tower2image_evo4"}},
		{3, new List<string>(){"Sprites/Towers/Tower3image_evo1", "Sprites/Towers/Tower3image_evo2", "Sprites/Towers/Tower3image_evo3", "Sprites/Towers/Tower3image_evo4"}},
		{4, new List<string>(){"Sprites/Towers/Tower4image_evo1", "Sprites/Towers/Tower4image_evo2", "Sprites/Towers/Tower4image_evo3", "Sprites/Towers/Tower4image_evo4"}},
		{5, new List<string>(){"Sprites/Towers/Tower5image_evo1", "Sprites/Towers/Tower5image_evo2", "Sprites/Towers/Tower5image_evo3", "Sprites/Towers/Tower5image_evo4"}}
		
	};

	//int attack_str, float cooldown_time, int attack_range, int health, int type, int upgrade_level,float weapon_attack_range,float weapon_atack_duration

	//Tower4 atackstr->percentageofreduced speed
	public static Dictionary<int, Dictionary<int, TowerStatus>> TOWERSUPGRADEVALUES = new Dictionary<int, Dictionary<int, TowerStatus>>(){
		{1, new Dictionary<int,TowerStatus>() {{0, new TowerStatus(5,1.5f,2,100,1,0,-1,-1)},{1, new TowerStatus(10,1.25f,3,120,1,1,0,0)},{2, new TowerStatus(15,1,4,150,1,2,0,0)},{3, new TowerStatus(15,0.5f,5,170,1,3,0,0)}}},
		{2, new Dictionary<int,TowerStatus>() {{0, new TowerStatus(4,0.5f,2,50,2,0,-1,-1)},{1, new TowerStatus(6,0.5f,3,60,2,1,0,0)},{2, new TowerStatus(8,0.4f,4,70,2,2,0,0)},{3, new TowerStatus(10,0.2f,8,80,2,3,0,0)}}},
		{3, new Dictionary<int,TowerStatus>() {{0, new TowerStatus(5,2,2,100,3,0,0,0)},{1, new TowerStatus(10,1.5f,2,100,3,1,0,0)},{2, new TowerStatus(11,1.25f,2,120,3,2,0,0)},{3, new TowerStatus(15,1,3,150,3,3,0,0)}}},
		{4, new Dictionary<int,TowerStatus>() {{0, new TowerStatus(2,5,0,50,4,0,1,2)},{1, new TowerStatus(5,4.5f,0,60,4,1,2,3)},{2, new TowerStatus(6,4,0,65,4,2,3,4)},{3, new TowerStatus(7,3,0,70,4,3,4,5)}}},
		{5, new Dictionary<int,TowerStatus>() {{0, new TowerStatus(5,10,2,100,5,0,2,0)},{1, new TowerStatus(15,5,2,100,5,1,3,0)},{2, new TowerStatus(17,3,2,100,5,2,4,0)},{3, new TowerStatus(20,2,2,100,5,3,5,0)}}}
	};

	public static float[]  Tower4percentslowdown = new float[]{0.2f, 0.4f, 0.6f,0.8f};
	
	//enemie 1,enemie 2,... enemie 7,nwaves, enemieswave
	public static List<int> ENEMIESLEVEL = new List<int>{
		1,2,3,4,5,6,7,2,3
	};
	
	public static List<int> ENEMIESAVAILABLE = new List<int>{
		//1,2
	};
	
	
	public static Dictionary <int,int> ENEMIESTOTALPERWAVE = new Dictionary<int, int>(){
		//{1,2},
		//{2,3}
	};

	//  type, level,  status
	public static Dictionary<int, Dictionary<int, EnemyStatus>> EnemiesStatus = new Dictionary<int, Dictionary<int, EnemyStatus>>(){
		//int energy_bonus, int main_attack, int attack_str, float cooldown_time, int health, float movement_speed, int type
		{0, new Dictionary<int, EnemyStatus>(){ // BOSS
				{0, new EnemyStatus(250, 9999, 25, 3, 150, 0.25f, 0)},
				{1, new EnemyStatus(300, 9999, 30, 3, 200, 0.3f, 0)},
				{2, new EnemyStatus(350, 9999, 35, 3, 250, 0.5f, 0)}
			}
		},
		
		{1, new Dictionary<int, EnemyStatus>(){ // SPAM
				{0, new EnemyStatus(5, 2, 0, 0, 10, 1f, 1)},
				{1, new EnemyStatus(10, 5, 0, 0, 15, 1.25f, 1)},
				{2, new EnemyStatus(20, 7, 0, 0, 20, 1.8f, 1)}
			}
		},
		
		{2, new Dictionary<int, EnemyStatus>(){ // Trojan
				{0, new EnemyStatus(10, 6, 0, 0, 20, 0.5f, 2)},
				{1, new EnemyStatus(20, 15, 0, 0, 30, 0.6f, 2)},
				{2, new EnemyStatus(30, 21, 0, 0, 45, 0.7f, 2)}
			}
		},
		
		{3, new Dictionary<int, EnemyStatus>(){ // DDoS
				{0, new EnemyStatus(5, 1, 0, 0, 2, 1.5f, 3)},
				{1, new EnemyStatus(10, 2, 0, 0, 5, 2f, 3)},
				{2, new EnemyStatus(15, 3, 0, 0, 7, 3f, 3)}
			}
		},
		
		{4, new Dictionary<int, EnemyStatus>(){ // Worms
				{0, new EnemyStatus(20, 10, 0, 3, 20, 0.8f, 4)},
				{1, new EnemyStatus(25, 20, 0, 2.5f, 30, 1.2f, 4)},
				{2, new EnemyStatus(35, 30, 0, 1.5f, 35, 1.5f, 4)}
			}
		},
		
		{5, new Dictionary<int, EnemyStatus>(){ // Malware
				{0, new EnemyStatus(40, 30, 10, 2f, 30, 0.5f, 5)},
				{1, new EnemyStatus(50, 35, 15, 1.5f, 40, 0.6f, 5)},
				{2, new EnemyStatus(50, 40, 20, 1f, 50, 0.8f, 5)}
			}
		},
		
		{6, new Dictionary<int, EnemyStatus>(){ // Virus
				{0, new EnemyStatus(20, 10, 5, 1, 15, 1f, 6)},
				{1, new EnemyStatus(25, 15, 10, 0.8f, 20, 1.25f, 6)},
				{2, new EnemyStatus(30, 20, 15, 0.7f, 30, 1.5f, 6)}
			}
		},
		
		{7, new Dictionary<int, EnemyStatus>(){ // Ads
				{0, new EnemyStatus(30, 5, 0, 0, 30, 1f, 7)},
				{1, new EnemyStatus(40, 10, 0, 0, 35, 1.15f, 7)},
				{2, new EnemyStatus(50, 20, 0, 0, 40, 1.42f, 7)}
			}
		}
	};

	public static List<bool> DifficultiesUnlocked = new List<bool>(){true, false, false};
	public static Dictionary<int, List<bool>> LevelsUnlocked = new Dictionary<int, List<bool>>(){
		{0, new List<bool>(){true, false, false, false, false, false, false, false, false, false}},
		{1, new List<bool>(){true, false, false, false, false, false, false, false, false, false}},
		{2, new List<bool>(){true, false, false, false, false, false, false, false, false, false}}
	};



	public static Dictionary<int, List<int>> weakness_enemies = new Dictionary<int, List<int>>(){
		{0, new List<int>(){}},
		{1, new List<int>(){1}},
		{2, new List<int>(){1}},
		{3, new List<int>(){1}},
		{4, new List<int>(){3}},
		{5, new List<int>(){4}},
		{6, new List<int>(){5}},
		{7, new List<int>(){2}}
	};
	
	public static Dictionary<int, List<int>> weakness_towers = new Dictionary<int, List<int>>(){
		{1, new List<int>(){2}},
		{2, new List<int>(){1,2}},
		{3, new List<int>(){5}},
		{4, new List<int>(){6}},
		{5, new List<int>(){7}}
	};
	public static Dictionary<int, GameObject> tower_spot_refs = new Dictionary<int, GameObject>();

	public static Dictionary<int, int> TOWER_BUILD_COSTS = new Dictionary<int, int>(){
		{1, 5},
		{2, 10},
		{3, 15},
		{4, 20},
		{5, 30},
	};

	/*
	public static Dictionary<int, int> TOWER_BUILD_COSTS = new Dictionary<int, int>(){
		{1, 0},
		{2, 0},
		{3, 0},
		{4, 0},
		{5, 0},
	};
	*/
	public static Dictionary<int, List<int>>  TOWER_UPGRADE_COSTS = new Dictionary<int, List<int>>(){
		{1, new List<int>(){5, 10, 20}},
		{2, new List<int>(){7, 14, 25}},
		{3, new List<int>(){10, 20, 25}},
		{4, new List<int>(){2, 7, 15}},
		{5, new List<int>(){20, 30, 50}}
	};




	public static Dictionary<int, List<int>>  TOWER_Repair_COSTS = new Dictionary<int, List<int>>(){
		{1, new List<int>(){2, 5, 10, 20}},
		{2, new List<int>(){3, 7, 14, 25}},
		{3, new List<int>(){5, 10, 20, 25}},
		{4, new List<int>(){4, 7, 15}},
		{5, new List<int>(){10, 20, 30, 50}}
	};




	
	
	public static void ParseQuizFromJSON(string s){
		SAVEDQUIZQUESTIONS.Clear();
		GlobalData.quiz_questions.Clear();
		JSONNode json = JSON.Parse(s);
		JSONArray jsonarr = json.AsArray;
		for(int i=0; i<jsonarr.Count; i++){
			int d=0;
			string Q = "";
			string RA = "";
			string WA1 = "";
			string WA2 = "";
			string WA3 = "";
			string OW = "";
			string OL = "";

			if(jsonarr[i]["difficulty"]!=null){
				d = int.Parse(jsonarr[i]["difficulty"].Value);
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
			
			
			quiz_questions.Add(new QuizQuestion(d, Q, RA, WA1, WA2, WA3, OW, OL));
			
			//Joao2409
			
			if(SAVEDQUIZQUESTIONS.ContainsKey(d)){
				SAVEDQUIZQUESTIONS[d].Add(new QuizQuestion(d, Q, RA, WA1, WA2, WA3, OW, OL));
			}else{
				SAVEDQUIZQUESTIONS.Add(d, new List<QuizQuestion>(){ new QuizQuestion(d, Q, RA, WA1, WA2, WA3, OW, OL)});
			}
			//Joao2409
		}
		
		//Joao2409
		//string saveddata=JSONMaker.MakeSaveFile();
		SaveLoadData.SavePlayerData();
		//Joao2409
	}


	public static List<string> Languages = new List<string>();


	
	//JoaoAdjustTextScale
	public static int SavedScreenWidth=0;
	public static bool orientationlandscape=false;
	//JoaoAdjustTextScale


	public static int current_tutorial=-1;
	public static int tutorial_state = 0;
	public static bool enemyatacktower_tut=false;
	public static List<GameObject> tutorial_objects;


	public static bool TutCanBuildFree=false;
	public static bool introlevelanim=false;
	//Leaderboards Data
	public static string[] leaderboard_name={"Maria","Rui","Marta","Pedro","Joao","Ana"};
	public static string[] leaderboard_score={"60","50","40","30","20","10"};
	public static string LocalUserName="Joao";


}
