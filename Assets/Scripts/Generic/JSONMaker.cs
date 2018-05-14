using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class JSONMaker : MonoBehaviour {

	public static string MakeArray(string name, int[] arr, bool islast){
		string nameref = "\""+name+"\": ";
		string arrref = "[";
		for(int i=0; i<arr.Length; i++){
			if(i==arr.Length-1){
				arrref+=arr[i].ToString()+"]";
			}else{
				arrref+=arr[i].ToString()+", ";
			}
		}
		arrref = (arrref == "[") ? "[]": arrref;
		if(islast){
			return nameref+arrref;
		}else{
			return nameref+arrref+", ";
		}
	}

	public static string MakeArray(string name, string[] arr, bool islast){
		string nameref = "\""+name+"\": ";
		string arrref = "[";
		for(int i=0; i<arr.Length; i++){
			if(i==arr.Length-1){
				arrref+=arr[i].ToString()+"]";
			}else{
				arrref+=arr[i].ToString()+", ";
			}
		}
		arrref = (arrref == "[") ? "[]": arrref;
		if(islast){
			return nameref+arrref;
		}else{
			return nameref+arrref+", ";
		}
	}

	//Joao2409 
	public static string MakeArraysFromDictionaryGlossary(Dictionary<int, List<QuizQuestion>> dict){
		string arrsref = "";
		int count=0;
		foreach(int i in dict.Keys){
			arrsref+=MakeArrayGlossary(i.ToString(), dict[i].ToArray(), count==(dict.Count-1));
			count++;
			
		}
		return arrsref;
	}
	
	public static string MakeObjectGlossary(string name, string objectsinside, bool islast){
		if(islast){
			return "\""+name+"\": [{"+objectsinside+"}]";
		}else{
			return "\""+name+"\": [{"+objectsinside+"}], ";
		}
	}

	public static string MakeArrayGlossary(string name, QuizQuestion[] arr, bool islast){
		string nameref = "\""+name+"\": ";
		string arrref = "[";


		for(int i=0; i<arr.Length; i++){
			arrref += "{";
			/*if(i==arr.Length-1){
				arrref+="\"Q\":\""+arr[i].Q.ToString()+"\",\"RA\":\""+arr[i].RA.ToString()+"\",\"WA1\":\""+arr[i].WA1.ToString()+"\",\"WA2\":\""+arr[i].WA2.ToString()+"\",\"WA3\":\""+arr[i].WA3.ToString().Replace("\"", "")+"\",\"OW\":\""+arr[i].OW.ToString()+"\",\"OL\":\""+arr[i].OL.ToString()+"\",\"difficulty\":\""+arr[i].difficulty.ToString()+"\"}]";
			}else{
				arrref+="\"Q\":\""+arr[i].Q.ToString()+"\",\"RA\":\""+arr[i].RA.ToString()+"\",\"WA1\":\""+arr[i].WA1.ToString()+"\",\"WA2\":\""+arr[i].WA2.ToString()+"\",\"WA3\":\""+arr[i].WA3.ToString().Replace("\"", "")+"\",\"OW\":\""+arr[i].OW.ToString()+"\",\"OL\":\""+arr[i].OL.ToString()+"\",\"difficulty\":\""+arr[i].difficulty.ToString()+"\"}, ";
			}*/
			if(i==arr.Length-1)
				arrref+=MakeStringGlossary("Q", arr[i].Q.ToString(), false)+MakeStringGlossary("RA", arr[i].RA.ToString(), false)+MakeStringGlossary("WA1", arr[i].WA1.ToString(), false)+MakeStringGlossary("WA2", arr[i].WA2.ToString(), false)+MakeStringGlossary("WA3", arr[i].WA3.ToString(), false)+MakeStringGlossary("OW", arr[i].OW.ToString(), false)+MakeStringGlossary("OL", arr[i].OL.ToString(), false)+MakeStringGlossary("difficulty", arr[i].difficulty.ToString(), true)+"}]";
			else
				arrref+=MakeStringGlossary("Q", arr[i].Q.ToString(), false)+MakeStringGlossary("RA", arr[i].RA.ToString(), false)+MakeStringGlossary("WA1", arr[i].WA1.ToString(), false)+MakeStringGlossary("WA2", arr[i].WA2.ToString(), false)+MakeStringGlossary("WA3", arr[i].WA3.ToString(), false)+MakeStringGlossary("OW", arr[i].OW.ToString(), false)+MakeStringGlossary("OL", arr[i].OL.ToString(), false)+MakeStringGlossary("difficulty", arr[i].difficulty.ToString(), true)+"}, ";
				

		}
		arrref = (arrref == "[") ? "[]": arrref;
		if(islast){
			return nameref+arrref;
		}else{
			return nameref+arrref+", ";
		}


	}


	//try to remove replace
	public static string MakeStringGlossary(string name, string val, bool islast){
		if(islast){
			return "\""+name+"\": \""+val.ToString().Replace("\"","")+"\" ";
		}else{
			return "\""+name+"\": \""+val.ToString().Replace("\"","")+"\", ";
		}
	}
	//Joao2409


	public static string MakeString(string name, string val, bool islast){
		if(islast){
			return "\""+name+"\": \""+val.ToString()+"\" ";
		}else{
			return "\""+name+"\": \""+val.ToString()+"\", ";
		}
	}
 
	/*public static string MakeArrays(string[] names, object[][] arr){
		string arrsref = "";
		for(int i=0; i<arr.Length; i++){
			arrsref+=MakeArray(names[i], arr[i], i==arr.Length-1);
		}
		return arrsref;
	}*/
	public static string MakeArraysFromDictionary(Dictionary<int, List<int>> dict){
		string arrsref = "";
		int count=0;
		foreach(int i in dict.Keys){
			arrsref+=MakeArray(i.ToString(), dict[i].ToArray(), count==(dict.Count-1));
			count++;
		}
		return arrsref;
	}




	public static string MakeObject(string name, string objectsinside, bool islast){
		if(islast){
			return "\""+name+"\": {"+objectsinside+"}";
		}else{
			return "\""+name+"\": {"+objectsinside+"}, ";
		}
	}


	
	public static string MakeInt(string name, int val, bool islast){
		if(islast){
			return "\""+name+"\": "+val.ToString()+" ";
		}else{
			return "\""+name+"\": "+val.ToString()+", ";
		}
	}
	
	

	public static string MakeFloat(string name, float val, bool islast){
		if(islast){
			return "\""+name+"\": "+val.ToString()+" ";
		}else{
			return "\""+name+"\": "+val.ToString()+", ";
		}
	}
	
	




	public static string MakeBool(string name, bool val, bool islast){
		if(islast){
			return "\""+name+"\": "+(val? "true":"false") +" ";
		}else{
			return "\""+name+"\": "+(val?"true":"false")+", ";
		}
	}
 
 

	//Applicable to Turns to Spawn
	public static string MakeObjectWithInts(string name, int[] arr, bool islast){
		string objref="";
		for(int i=0; i<arr.Length; i++){
			objref+= MakeInt(i.ToString(), arr[i], i==(arr.Length-1));
		}
		objref = MakeObject(name, objref, islast);
		return objref;
	}
	//Applicable to Regions Unlocked
	public static string MakeObjectWithBool(string name, bool[] arr, bool islast){
		string objref="";
		for(int i=0; i<arr.Length; i++){
			objref+= MakeBool(i.ToString(), arr[i], i==(arr.Length-1));
		}
		objref = MakeObject(name, objref, islast);
		return objref;
	}
	 

	public static string MakeSaveFile(){
		string save_json = "{"+
			MakeInt("current_energy", PlayerData.current_energy,false)+	
				MakeString("player_name", PlayerData.playername ,false)+
				MakeInt("lastunlockeddificulty", PlayerData.lastunlockeddificulty,false)+	
				MakeInt("last_unlocked_level", PlayerData.lastunlockedlevel,false)+
				MakeInt("picked_playerid", PlayerData.picked_playerid,false)+
				MakeBool("loggedinGameCenter", PlayerData.loggedinGameCenter,false)+
				MakeBool("music", PlayerData.music,false)+
				MakeBool("sfx", PlayerData.sfx,false)+
				MakeObjectGlossary("QuizQuestions",MakeArraysFromDictionaryGlossary(GlobalData.SAVEDQUIZQUESTIONS),true)+
				"}";
		/*
		string save_json = "{" +
				MakeString("save_time", DateTime.UtcNow.ToString() ,false)+
				MakeInt("grapes", PlayerData.Grapes, false) +
				MakeObjectWithBool("player_characters_unlocked", PlayerData.player_characters_unlocked, false)+
				MakeString("profile_first_name", PlayerData.profile_first_name, false)+
				MakeString("profile_last_name", PlayerData.profile_last_name, false)+
				MakeInt("profile_gender", PlayerData.profile_gender, false)+
				MakeInt("profile_bday_year", PlayerData.profile_bday_year, false)+
				MakeInt("profile_bday_month", PlayerData.profile_bday_month, false)+
				MakeInt("profile_bday_day", PlayerData.profile_bday_day, false)+
				MakeInt("profile_weight", (int)PlayerData.profile_weight, false)+
				MakeInt("profile_height", (int)PlayerData.profile_height, false)+
				MakeInt("profile_limits", PlayerData.profile_limits, false)+
				MakeInt("profile_body_type", PlayerData.profile_body_type, false)+
				MakeInt("profile_eye_color", PlayerData.profile_eye_color, false)+
				MakeInt("profile_hair_color", PlayerData.profile_hair_color, false)+
				MakeInt("profile_looking_for_gender", PlayerData.profile_looking_for_gender, false)+
				MakeInt("current_spending_bonus", PlayerData.current_spending_bonus, false)+
				MakeFloat("spent_on_market", PlayerData.spent_on_market, false)+
				MakeInt("playtime_achievement_seconds", PlayerData.playtime_achievement_seconds, false)+
	 			MakeInt("selected_playercharacter", PlayerData.Selected_PlayerCharacter, false)+
				MakeString("last_challenge_load", PlayerData.last_challenge_load.Date.ToString(), false)+
				MakeInt ("selected_challenge_id", PlayerData.Selected_Challenge_Id, false)+
				MakeInt ("current_match_id", PlayerData.current_match_challenge.match_id, false)+
				MakeArray("challenge_levels_to_clear", PlayerData.Challenge_LevelsToClear.ToArray(), false)+
				MakeInt("challenge_grapes_collected", PlayerData.Challenge_GrapesCollected, false)+
				MakeInt("challenge_dates_counted", PlayerData.Challenge_DateCount, false)+
				MakeFloat("challenge_played_time", (float)PlayerData.Challenge_Playing_For_Seconds, false)+
				MakeChallenges(false)+
				MakeCoupons(false)+
				MakeCityLevelsUnlocked(false)+
				MakePlayerCharacterData(true);
				*/
//		Debug.Log (save_json);
		return save_json;
	}


  
}
