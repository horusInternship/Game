using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerData : MonoBehaviour {
	public static int current_energy = 50;
	public static int current_score = 0;
	public static List<int> energy_queue = new List<int>();

	public static GameObject main_tower;
	//0 - not won or lost
	//1 - won
	//-1 - lost
	public static int level_state = 0;

	public static int picked_playerid = 0;


	//JoaoMusicSFX
	public static bool music = true;
	
	public static bool sfx = true;

	public static string playername = "";

	public static int lastunlockeddificulty=0;
	public static int lastunlockedlevel=1;
	public static bool loggedinGameCenter=false;



	public static void SetMusicVolume(){
		SoundControl.SetMusicVolume(music? 1 : 0);
		string saveddata=JSONMaker.MakeSaveFile();
		PlayerPrefs.SetString("PlayerSavedData",saveddata);
		
	}
	
	public static void SetSFXVolume(){
		SoundControl.SetSoundVolume(sfx? 1 : 0);
		string saveddata=JSONMaker.MakeSaveFile();
		PlayerPrefs.SetString("PlayerSavedData",saveddata);
		
	}

}
