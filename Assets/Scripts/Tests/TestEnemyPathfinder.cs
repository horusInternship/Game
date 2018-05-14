using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestEnemyPathfinder : MonoBehaviour {
	public GameObject tile;
 
	int ex=15;
	int ey=0;
	int edir = 1; //0 - up, 1-down, 2-left, 3-right
 
	int nsteps =0;
	// Use this for initialization
	void Start () {
		LoadMap();
		SetNextDirPos();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LoadMap(){
		GlobalData.map = CSVReader.GridToArray("CSV/testlevel");
		List<int[]> empty_spaces = new List<int[]>();
		for(int x = 0; x < GlobalData.map.GetLength(0)-1; x++){
			for(int y= 0; y< GlobalData.map.GetLength(1)-1; y++){
				int y_inv = GlobalData.map.GetLength(1)-2-y;
 
				if(!GlobalData.map[x,y_inv].Equals("x") && !GlobalData.map[x,y_inv].Equals("")){
					GameObject t = (GameObject)Instantiate(tile);
					t.name = GlobalData.map[x,y_inv];
					t.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(GlobalData.TILESPRITEPATHS[GlobalData.map[x,y_inv]]);
					t.transform.position = new Vector3(x,y,0);
				}else if(GlobalData.map[x, y_inv].Equals("x")){
					empty_spaces.Add(new int[]{x, y});
				}
			}
		}



		
		int random_object_count = GlobalData.max_random_objects;
		for(int i = 0; i<GlobalData.max_random_objects; i++){

		}
	}

	void SetNextDirPos(){
		while(!GlobalData.map[ex,ey].Equals("x") && !GlobalData.map[ex,ey].Equals("g") && nsteps<1000){
			if(GlobalData.TILEDIR.ContainsKey(GlobalData.map[ex,ey])){
				bool u = GlobalData.TILEDIR[GlobalData.map[ex,ey]][0];
				bool d = GlobalData.TILEDIR[GlobalData.map[ex,ey]][1];
				bool l = GlobalData.TILEDIR[GlobalData.map[ex,ey]][2];
				bool r = GlobalData.TILEDIR[GlobalData.map[ex,ey]][3];

				if(edir==0){ //going up
					if(r){
						edir = 3;
 						ex++;
					}else if(u){
						ey--;
					}else if(l){
						edir = 2;
						ex--;
					}
				}else if(edir == 1){ //going down
					if(d){
						ey++;
					}else if(r){
						edir = 3;
						ex++;
					}else if(l){
						edir = 2;
						ex--;
					}
				}else if(edir == 2){ //going left
					if(d){
						edir = 1;
						ey++;
					}else if(l){
						ex--;
					}else if(u){
						edir = 0;
						ey--;
					}
				}else if(edir == 3){ //going right
					if(r){
						ex++;
					}else if(d){
						edir = 1;
						ey++;
					}else if(u){
						edir = 0;
						ey--;
					}
				}
			}
			Debug.Log (GlobalData.map[ex,ey] + " at pos(" +ex+ ", "+ ey+") edir: "+edir );
			nsteps++;
		}
	}
}
