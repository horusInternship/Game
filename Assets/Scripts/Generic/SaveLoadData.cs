using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
public class SaveLoadData : MonoBehaviour {
	//Joao2409
	public static void SavePlayerData(){
		
		string saveddata=JSONMaker.MakeSaveFile();
	//	Debug.Log ("onloading "+saveddata);
		
		PlayerPrefs.SetString("PlayerSavedData",saveddata);
	}

	public static void LoadPlayerData(){
		GlobalData.SAVEDQUIZQUESTIONS.Clear();
		
		string saveddatatoparse=PlayerPrefs.GetString("PlayerSavedData");
		JSONNode json = JSON.Parse(saveddatatoparse);
		PlayerData.current_energy = int.Parse(json["current_energy"].Value);
		PlayerData.playername = json["player_name"].Value;
		PlayerData.lastunlockeddificulty = int.Parse(json["lastunlockeddificulty"].Value);
		PlayerData.lastunlockedlevel = int.Parse(json["last_unlocked_level"].Value);

		PlayerData.music = bool.Parse(json["music"].Value);
		PlayerData.sfx = bool.Parse(json["sfx"].Value);
		
		PlayerData.SetMusicVolume();
		PlayerData.SetSFXVolume();
		/*

		for(int i=0; i<=2;i++)
		{
			if(i<PlayerData.lastunlockeddificulty){
				GlobalData.DifficultiesUnlocked[i]=true;
				for(int j=1; j<=10;j++)
				{
					GlobalData.LevelsUnlocked[i][j-1]=true;
				}
			}else{
				if(i==PlayerData.lastunlockeddificulty){
					GlobalData.DifficultiesUnlocked[i]=true;
					for(int j=1; j<=10;j++)
					{
						if(j<=PlayerData.lastunlockedlevel)
							GlobalData.LevelsUnlocked[i][j-1]=true;
						else
							GlobalData.LevelsUnlocked[i][j-1]=false;
					}
				}else{
					GlobalData.DifficultiesUnlocked[i]=false;
					for(int j=1; j<=10;j++)
					{
						GlobalData.LevelsUnlocked[i][j-1]=false;
					}
				}
			}
		}
		*/

		LoadUnlockedLevels();
		
		
		PlayerData.picked_playerid = int.Parse(json["picked_playerid"].Value);
		PlayerData.loggedinGameCenter = bool.Parse(json["loggedinGameCenter"].Value);
		JSONNode q_difficulties = json["QuizQuestions"].AsArray[0];
		for(int d=0; d<3; d++){
			JSONArray jarr = q_difficulties[d.ToString()].AsArray;
 				
				for(int qq=0;qq<jarr.Count;qq++){
						JSONNode question = jarr[qq];
						if(GlobalData.SAVEDQUIZQUESTIONS.ContainsKey(d)){
					Debug.Log ("adding to savedquizquestions");
							GlobalData.SAVEDQUIZQUESTIONS[d].Add(
						new QuizQuestion(d, question["Q"].Value, 
					                 		question["RA"].Value, 
					                 		question["WA1"].Value, 
					                 		question["WA2"].Value, 
					                 		question["WA3"].Value, 
					                		question["OW"].Value,
					                		question["OL"].Value));
							
						}else{
					Debug.Log ("NEW adding to savedquizquestions");
					
							GlobalData.SAVEDQUIZQUESTIONS.Add(d, new List<QuizQuestion>(){{
									new QuizQuestion(d, question["Q"].Value, 
						                				question["RA"].Value, 
						                				question["WA1"].Value, 
						                				question["WA2"].Value, 
						                				question["WA3"].Value, 
						                				question["OW"].Value,
							                 			 question["OL"].Value)
							}});
				
						
					}
				 
				
			}


		}


		Debug.Log(GlobalData.SAVEDQUIZQUESTIONS.Count);
	 
	/*	
		for(int i=0; i<jarr.Count; i++){
			for(int j=0;j<jarr[i].Count;j++)
			{

				for(int k=0;k<jarr[i][j].Count;k++)
				{
					if(GlobalData.SAVEDQUIZQUESTIONS.ContainsKey(j)){
						GlobalData.SAVEDQUIZQUESTIONS[j].Add(new QuizQuestion(int.Parse(jarr[i][j][k]["difficulty"].Value), jarr[i][j][k]["Q"].Value, jarr[i][j][k]["RA"].Value, jarr[i][j][k]["WA1"].Value, jarr[i][j][k]["WA2"].Value, jarr[i][j][k]["WA3"].Value, jarr[i][j][k]["OW"].Value, jarr[i][j][k]["OL"].Value));
						
					}else{
						GlobalData.SAVEDQUIZQUESTIONS.Add(j, new List<QuizQuestion>(){
							new QuizQuestion(int.Parse(jarr[i][j][k]["difficulty"].Value), jarr[i][j][k]["Q"].Value, jarr[i][j][k]["RA"].Value, jarr[i][j][k]["WA1"].Value, jarr[i][j][k]["WA2"].Value, jarr[i][j][k]["WA3"].Value, jarr[i][j][k]["OW"].Value, jarr[i][j][k]["OL"].Value)
						});
					}

				}
				
			}
			
		}
*/
		//Debug.Log ("SavedQuestions Count "+GlobalData.SAVEDQUIZQUESTIONS[0].Count);
		//Debug.Log ("Catch Q Value "+GlobalData.SAVEDQUIZQUESTIONS[0][0].Q);
	}

	//Requires lastdifficultyunlocked and lastlevelunlocked to be loaded
private static void LoadUnlockedLevels(){
		Debug.Log("last unlocked difficulty "+PlayerData.lastunlockeddificulty);
		for(int d=0; d<=PlayerData.lastunlockeddificulty; d++){
			GlobalData.DifficultiesUnlocked[d] = true;
			int lastlevel = (d==PlayerData.lastunlockeddificulty? PlayerData.lastunlockedlevel : 10);
			Debug.Log("last level "+PlayerData.lastunlockeddificulty);
			for(int l=0; l<=lastlevel-1; l++){
				GlobalData.LevelsUnlocked[d][l] = true;
			}
		}
	}
	//Joao2409
}
