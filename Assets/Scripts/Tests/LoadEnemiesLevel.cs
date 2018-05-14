using UnityEngine;
using System.Collections;

public class LoadEnemiesLevel : MonoBehaviour {

	bool inicializationscompleted=false;

	int totalenemiesperwave=0, currenemiesinwave=0;
	int totalwaves=0, currwave=0;
	float timebeweenwaves=4f, timesincelastwave;
	float timebeweenenemies=2f, timesincelastenemie;
	int totalPaths=4,currpath=1;
 
	Vector2[] EnemyPos = {new Vector2(-8.42f,3.39f), new Vector2(-8.42f,1.25f),new Vector2(-2.85f,5.47f), new Vector2(0,5.47f)};
	// Use this for initialization
	void Start () {
		CSVLoadEnemyWaves();

	}
	
	// Update is called once per frame
	void Update () {
		//if(inicializationscompleted)
		Instantiate_Waves();
	}
	void CSVLoadEnemyWaves(){
		LoadCSVLine(0,4);
		totalwaves=GlobalData.ENEMIESLEVEL[7];
		totalenemiesperwave=GlobalData.ENEMIESLEVEL[8];
		
		timesincelastwave=timebeweenwaves;
		timesincelastenemie=timebeweenenemies;
		
		//Choose the first path for Wave
		//currpath=Random.Range (0,totalPaths);
	}


	//change this to line to array
	void LoadCSVLine(int difficulty,int level){
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



	void Instantiate_Waves(){
		if(currwave<totalwaves)
		{
			//First Loop Enemy Waves
			if(timesincelastwave<timebeweenwaves)
			{
				timesincelastwave+=Time.deltaTime;
			}
			else
			{
				if(currenemiesinwave==totalenemiesperwave)
				{
					currwave++;
					currenemiesinwave=0;
					timesincelastwave=0;

					currpath=Random.Range (0,totalPaths);
				}
				else
				{
					// Second Loop Enemies Per Wave
					if(timesincelastenemie<timebeweenenemies)
					{
						timesincelastenemie+=Time.deltaTime;
					}
					else
					{
						currenemiesinwave++;


						Instantiate_Enemies();
						timesincelastenemie=0;
					}

				}
			}
		}else{
			Debug.Log ("BossWave");
		}
	}

	GameObject Enemy;
	void Instantiate_Enemies(){
		int numberofenemies=GlobalData.ENEMIESAVAILABLE.Count;
		if(numberofenemies>0)
		{
			int randomenemytype=Random.Range (0,numberofenemies);
			int enemytype=GlobalData.ENEMIESAVAILABLE[randomenemytype];

			//EnemyInstantiation
			Debug.Log("Instantiate enemy here, enemytype is the type of enemy to be instantiated");

			//Enemy = (GameObject)Instantiate(Resources.Load("EnemiesTest/EnemyType"+enemytype));
			//Enemy.transform.position=new Vector3 (EnemyPos[currpath][0],EnemyPos[currpath][1],0);

			GlobalData.ENEMIESTOTALPERWAVE[enemytype]--;
			if(GlobalData.ENEMIESTOTALPERWAVE[enemytype]==0){
				GlobalData.ENEMIESTOTALPERWAVE.Remove(enemytype);
				GlobalData.ENEMIESAVAILABLE.RemoveAt(randomenemytype);
			}
		}

	
	}

}
