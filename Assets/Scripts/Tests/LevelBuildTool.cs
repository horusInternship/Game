using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LevelBuildTool : MonoBehaviour {

	public string map_path = "CSV/prelevel";
	string[,] map;
	string mapcsv="";
	void OnGUI(){
		if(GUI.Button(new Rect(0,0, 150, 50), "Convert")){
			ConvertMap();
		}


		GUI.TextField(new Rect(150, 50, Screen.width, Screen.height), mapcsv);
	 
	}


	void ConvertMap(){
		mapcsv = "";
		map = CSVReader.GridToArray(map_path);
		string[,] newmap = (string[,])map.Clone();
		for(int y= 0; y< map.GetLength(1)-1; y++){
			for(int x = 0; x < map.GetLength(0)-1; x++){
				string newcode = ConvertToMapCode(x,y);
				newmap[x,y] = newcode;
				mapcsv += newcode+ (x < map.GetLength(0)-2? "," : "\n");
				
			}
		}

		Debug.Log (mapcsv);
	}


	Dictionary<string, string> dir_to_codes = new Dictionary<string, string>(){
		{"False, False, False, True", "sh"},
		{"False, True, False, False", "sv"},
		{"False, False, True, True", "h"},
		{"True, True, False, False", "v"},
		{"True, True, True, True", "4"},
		{"False, True, True, False", "ld"},
		{"True, False, True, False", "lu"},
		{"False, True, False, True", "rd"},
		{"True, False, False, True", "ru"},
		{"False, True, True, True", "hd"},
		{"True, False, True, True", "hu"},
		{"True, True, True, False", "vl"},
		{"True, True, False, True", "vr"},
		{"False, False, False, False", "x"}
	};
 



	string ConvertToMapCode(int x, int y){
		bool u=false;
		bool d=false;
		bool l=false;
		bool r=false;
		if(map[x,y].Equals("1")){
		if(x>0 && x<map.GetLength(0)-2){
			l = map[x-1, y].Equals("1") || map[x-1, y].Equals("sh") || map[x-1, y].Equals("g");
			r = map[x+1, y].Equals("1") || map[x+1, y].Equals("g");
		}else if(x==map.GetLength(0)-2){
				l = map[x-1, y].Equals("1") || map[x-1, y].Equals("sh") || map[x-1, y].Equals("g");
		}else if(x==0){
				r = map[x+1, y].Equals("1") || map[x+1, y].Equals("g");
		}

		if(y>0 && y < map.GetLength(1)-2){
			u = map[x, y-1].Equals("1") || map[x, y-1].Equals("sv");
			d = map[x, y+1].Equals("1") || map[x, y+1].Equals("g");
		}else if(y==map.GetLength(1)-2){
			u = map[x, y-1].Equals("1") || map[x, y-1].Equals("sv");
		}else if(y==0){
				d = map[x, y+1].Equals("1") ||  map[x, y+1].Equals("g");;
		}
		}
		string dir = u+", "+d+", "+l+", "+r;
		Debug.Log (dir +" | at ("+ x + ", "+ y +")");
		if( map[x,y].Equals("sh") || map[x,y].Equals("sv") || map[x,y].Equals("g") ){
			return map[x,y];
		}else{
			return dir_to_codes[dir];
		}
	}
}
