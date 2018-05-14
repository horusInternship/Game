/*
SoundControl
#music #sfx
Description:
Static class the creates new gameobjects for each sound or music that it plays, storing the sounds and music loaded in dictionaries
Usage

PlayMusic and PlaySound(listener, sound_location, restart_if_playing, play_once)
 listener - usually on camera
 music_location, sound_location - resource path
 restart_if_playing - if true then if the sound is already playing it will restart
 play_once - if false then the sound will loop
 
Stop_Music(path) - stop a music given its resources path
Stop_Sound(path) - stops a sound given its resources path
StopAndClearAll - stops all sounds and music playing

GetMusic - returns the audiosource of a given music
GetSound - returns the audiosource of a given sound

GetMusicVolume - gets the music volume from PlayerPrefs
GetSoundVolume - gets the sound volume from PlayerPrefs

ChangeMusicVolume(volume) - changes the music volume(0..1);
ChangeSoundVolume(volume) - changes the sounds volume(0..1);

FadeOutAll(time) - fades out all sounds and music in a given time

In case of using a Playlist
SetPlaylistVolume(volume) - in case of using the Playlist Script this sets the volume of the playlist
SetGameObjectsVolume(volume) - in case of having gameobjects with audiosources in their animators, 
								searches from the given gameObject to all its child objects for audiosources to wich the volume is altered 

WARNING:
This class can only play a sound X if there's no sound X playing,
if X lasts 10 seconds and starts playing and we want X to play again in 5 seconds 
we can either use the option restart_if_playing
or play it sequentially after 10 seconds
 */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class SoundControl {

	static bool initialized=false;
	private static Dictionary<string,AudioSource> loaded_sounds;
	private static Dictionary<string,AudioSource> loaded_musics;
	private static AudioListener camera_listener;
	private static GameObject sound_obj;
//	private static float sound_volume=1.0f;
//	private static float music_volume=1.0f;
	private static float sound_volume=1.0f;
	private static float music_volume=1.0f;

	public static AudioSource PlaySound(AudioListener listener, string sound_location, bool restart_if_playing, bool play_once){
		Init ();
		if (loaded_sounds==null)
			loaded_sounds=new Dictionary<string, AudioSource>();
		if (restart_if_playing || play_once || !loaded_sounds.ContainsKey(sound_location) || loaded_sounds[sound_location]==null || !loaded_sounds[sound_location].isPlaying){
			//create empty gameobject if it does not exist yet
			if (sound_obj==null){
				sound_obj=new GameObject();
				sound_obj.name="SoundControl";
			}
			if (play_once){
				AudioClip clip=Resources.Load (sound_location) as AudioClip;
				if (!clip)
					throw new FileNotFoundException("Asset \""+sound_location+"\" not found on Resources folder");
				AudioSource audio=sound_obj.AddComponent<AudioSource>();
				audio.clip=clip;
				//DynamicClick dyn=audio.gameObject.AddComponent<DynamicClick>();
				/*dyn.SetOnUpdate(delegate {
					if (!audio.isPlaying){
						MonoBehaviour.DestroyObject(audio);
						MonoBehaviour.DestroyObject(dyn);
					}
				});*/
//				audio.volume=sound_volume;
				audio.Play ();
				return audio;
			} else {
				if (!loaded_sounds.ContainsKey(sound_location)|| loaded_sounds[sound_location]==null){
					AudioClip clip=Resources.Load (sound_location) as AudioClip;
					if (!clip)
						throw new FileNotFoundException("Asset \""+sound_location+"\" not found on Resources folder");
					loaded_sounds[sound_location]=sound_obj.AddComponent<AudioSource>();
					loaded_sounds[sound_location].clip=clip;
				}
//				loaded_sounds[sound_location].volume=sound_volume;
				loaded_sounds [sound_location].Play ();
				return loaded_sounds[sound_location];
			}
		}
		if (loaded_sounds.ContainsKey (sound_location) && loaded_sounds [sound_location] != null) {
			return loaded_sounds [sound_location];
		}
		return null;
	}


	
	public static AudioSource PlaySFX(string sound_location, bool restart_if_playing, bool play_once, bool no_duplicates){
		AudioSource a_s = PlaySFX(Camera.main.GetComponent<AudioListener>(), sound_location, restart_if_playing, play_once, no_duplicates);
		a_s.volume = sound_volume;
		//Debug.Log (sound_location+" sfx v "+sound_volume);
		a_s.loop = !play_once;
		return a_s;
	}
	public static AudioSource PlaySFX(AudioListener listener, string sound_location, bool restart_if_playing, bool play_once, bool no_duplicates){
		Init ();
		if (loaded_sounds==null){
			loaded_sounds=new Dictionary<string, AudioSource>();
		}
		if (sound_obj==null){
				sound_obj=new GameObject();
				sound_obj.name="SoundControl";
			}



		if(no_duplicates && loaded_sounds.ContainsKey(sound_location)){
			if(loaded_sounds[sound_location]!=null){

					if(restart_if_playing){
						loaded_sounds[sound_location].Stop();
					}else{
						if(!loaded_sounds[sound_location].isPlaying){
							loaded_sounds[sound_location].Play();
						}
					}
			
			}else{
				AudioClip clip=Resources.Load (sound_location) as AudioClip;
				if (!clip)
					throw new FileNotFoundException("Asset \""+sound_location+"\" not found on Resources folder");
			
				AudioSource s = sound_obj.AddComponent<AudioSource>();
				s.clip = clip;
				s.loop = !play_once;
				loaded_sounds[sound_location]=s;
			}
		}else{
			//loaded_sounds.Add(sound_location
			AudioClip clip=Resources.Load (sound_location) as AudioClip;
			if (!clip)
				throw new FileNotFoundException("Asset \""+sound_location+"\" not found on Resources folder");

			AudioSource s = sound_obj.AddComponent<AudioSource>();
			s.clip = clip;
			s.loop = !play_once;
			loaded_sounds.Add(sound_location, s); 
		}


 
		 if(loaded_sounds.ContainsKey(sound_location)){
			return loaded_sounds[sound_location];
		}else{
			return null;
		}
	}





	public static AudioSource PlaySound(AudioListener listener, string sound_location, bool restart_if_playing){
		return PlaySound (listener, sound_location, restart_if_playing, false);
	}

	public static AudioSource PlaySound(string sound_location, bool restart_if_playing, bool play_once){
		if (camera_listener==null){
			camera_listener=Camera.main.GetComponent<AudioListener>();
		}
		return PlaySound (camera_listener, sound_location, restart_if_playing, play_once);
	}

	public static AudioSource PlaySound(string sound_location, bool restart_if_playing, bool play_once, float stereopos){
		if (camera_listener==null){
			camera_listener=Camera.main.GetComponent<AudioListener>();
		}
		AudioSource a_s = PlaySound(camera_listener, sound_location, restart_if_playing, play_once);
		a_s.panStereo = stereopos;
		return a_s;
	}
	



	public static AudioSource PlaySound(string sound_location, bool restart_if_playing){
		return PlaySound (camera_listener, sound_location, restart_if_playing, false);
	}

	public static AudioSource PlaySound(string sound_location){
		return PlaySound (sound_location, true);
	}

	private static void Init(){
		if (!initialized) {
			initialized=true;
//			if (PlayerPrefs.HasKey("music_volume")){
//				sound_volume=PlayerPrefs.GetFloat("sound_volume");
//				music_volume=PlayerPrefs.GetFloat("music_volume");
//			} else {
//				SetMusicVolume(music_volume);
//				SetSoundVolume(sound_volume);
//			}
		}
	}
	public static void PlayMusic(string path, bool loop){
		Camera.main.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(path);
		Camera.main.GetComponent<AudioSource>().loop = loop;
		Camera.main.GetComponent<AudioSource>().Play();
	}
 
	public static AudioSource GetSound(string sound_location){
		if (loaded_sounds!=null && loaded_sounds.ContainsKey(sound_location))
			return loaded_sounds [sound_location];
		return null;
	}

	public static AudioSource GetMusic(string music_location){
		if (loaded_musics!=null && loaded_musics.ContainsKey(music_location))
			return loaded_musics [music_location];
		return null;
	}
	
	public static void StopSound(string sound_location){
		AudioSource sound = GetSound (sound_location);
		if (sound && sound.isPlaying)
			sound.Stop();
	}

	public static void StopMusic(string music_location){
		AudioSource music = GetMusic (music_location);
		if (music && music.isPlaying)
			music.Stop();
	}

	public static void FadeOutAll(float time){
		if (time >= 1 && loaded_sounds.Count > 0) {
			StopAndClearAll ();
		} 
//		else {
//			ChangeSoundVolume(time*sound_volume);
//			ChangeMusicVolume(time*music_volume);
//		}
	}

	public static void StopAndClearAll(){
		if (loaded_sounds!=null){
			foreach (AudioSource source in loaded_sounds.Values) {
				if (source!=null){
					source.Stop();
					MonoBehaviour.DestroyImmediate(source);
				}
			}
			loaded_sounds.Clear();
		}
		if (loaded_musics!=null){
			foreach (AudioSource source in loaded_musics.Values) {
				if (source!=null){
					source.Stop();
					MonoBehaviour.DestroyImmediate(source);
				}
			}
			loaded_musics.Clear();
		}
		MonoBehaviour.DestroyObject(sound_obj);
		sound_obj = null;
		camera_listener = null;
	}
	
	//JoaoMusicSFX
	public static void SetSoundVolume(float volume){
		if(sound_obj!=null){
		if(sound_obj.GetComponents<AudioSource>()!=null){
			AudioSource[] sfxs = sound_obj.GetComponents<AudioSource>();
		for(int i=0; i<sfxs.Length; i++){
			sfxs[i].volume = volume;
		}

		/*	Init ();
		ChangeSoundVolume (volume);
		//PlayerPrefs.SetFloat ("sound_volume", volume);
		sound_volume = volume;
		//SetGameObjectsVolume(volume);*/
		}
		}

		sound_volume = volume;
	}
	
	public static void SetMusicVolume(float volume){
		music_volume = volume;
		Camera.main.GetComponent<AudioSource>().volume = volume;
	}
	//JoaoMusicSFX



	private static void ChangeMusicVolume(float volume){
		if (loaded_musics!=null){
			foreach (AudioSource source in loaded_musics.Values) {
				if (source!=null)
					source.volume=volume;
			}
		}
	}

	private static void ChangeSoundVolume(float volume){
		if (loaded_sounds!=null){
			foreach (AudioSource source in loaded_sounds.Values) {
				if (source!=null)
					source.volume=volume;
			}
		}
	}

//	public static float GetMusicVolume(){
//		Init ();
//		return music_volume;
//	}
//
//	public static float GetSoundVolume(){
//		Init ();
//		return sound_volume;
//	}

/*

	WRTW SPECIFIC METHODS

	SetPlaylistVolume - instead of using the playmusic method we use the playlist function and so we have to change its volume
	SetGameObjectsVolume - sets the volume to any gameobject that uses soundeffects in the animator:
		-UnitAllies
		-EnemyUnits
		(for now...)
*/
	private static GameObject playlist=null;
	private static void SetPlaylistVolume(float volume){
		if(playlist==null){
			if(GameObject.Find ("Playlist")!=null){
				 playlist = GameObject.Find("Playlist");
			}
		}

		playlist.GetComponent<AudioSource>().volume = volume;
	}



	private static void SetGameObjectsVolume(float volume){
		GameObject overmap = GameObject.FindGameObjectWithTag("Overmap");
		if(overmap!=null){
		if(overmap.transform.Find("MyUnits")!=null){
			Transform myunits = overmap.transform.Find("MyUnits");
			for(int i=0; i<myunits.childCount; i++){
				if(myunits.GetChild(i).childCount == 1){
					if(myunits.GetChild(i).GetChild(0).childCount==1){
						if(myunits.GetChild(i).GetChild(0).GetChild(0).GetComponent<AudioSource>()!=null){
							myunits.GetChild(i).GetChild(0).GetChild(0).GetComponent<AudioSource>().volume= volume;
						}
					}
				}
			}
			}
	

		if(overmap.transform.Find("EnemyUnits")!=null){
			Transform enemyunits = overmap.transform.Find("EnemyUnits");
			for(int i=0; i<enemyunits.childCount; i++){
				if(enemyunits.GetChild(i).childCount == 1){
					if(enemyunits.GetChild(i).GetChild(0).childCount==1){
						if(enemyunits.GetChild(i).GetChild(0).GetChild(0).GetComponent<AudioSource>()!=null){
							enemyunits.GetChild(i).GetChild(0).GetChild(0).GetComponent<AudioSource>().volume= volume;
						}
					}
				}
			}
		}
	}
	}
}
