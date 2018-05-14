using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
//using U3DXT.iOS.Native.AudioToolbox;
using U3DXT.iOS.GUI;

public class KitchenSink : MonoBehaviour {
	
	class SceneEntry {
		public string name;
		public string fileName;
		public SceneEntry(string name, string fileName) {
			this.name = name;
			this.fileName = fileName;
		}
	}
	
	List<SceneEntry> _scenes = new List<SceneEntry>();
	
	void AddScene(string name, string fileName) {
//		if (Application.CanStreamedLevelBeLoaded(fileName))
			_scenes.Add(new SceneEntry(name, fileName));
	}
	
	void Start () {
		AddScene("GUI Basics", "GUIBasics");
		AddScene("In-App Purchase", "IAPTest");
		AddScene("Social Networking", "SocialTest");
		AddScene("Game Kit Basics", "GameKitBasics");
		AddScene("Game Kit Real-time Multiplayer", "GKMeeting");
		AddScene("Web View", "WebViewTest");
		AddScene("iCloud and Data", "iCloudTest");
		AddScene("Media Streaming and Export", "StreamingMoviePlayer");
		AddScene("Multipeer Server", "ShareNearMeServer");
		AddScene("Multipeer Client", "ShareNearMeClient");
		AddScene("Speech Synthesis", "SpeechTest");
		AddScene("AddressBook, EventKit", "PersonalTest");
		AddScene("Maps", "MapsTest");
		AddScene("iBeacon", "iBeaconTest");
		AddScene("Face Detection", "FaceCam");
		AddScene("Image Filters", "Anonymous");
	}
	
	void OnGUI()  {
		GUI.Label(new Rect(10, Screen.height - 40, 200, 30), "U3DXT Kitchen Sink");
		
		int i = 0;
		for (int j=0; j<2; j++) {

			GUILayout.BeginArea(new Rect((j == 0) ? 50 : (Screen.width / 2), 50, (Screen.width - 100)/2, Screen.height - 100));
			GUILayout.BeginVertical();
			
			for (; i<_scenes.Count/(2-j); i++) {
				var scene = _scenes[i];

				if (GUILayout.Button(scene.name, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true))) {	
					if (Application.CanStreamedLevelBeLoaded(scene.fileName))
						Application.LoadLevel(scene.fileName);
					else
						GUIXT.ShowAlert("U3DXT Kitchen Sink", "The required module is not enabled.", "OK", new string[] {});
				}
			}

			GUILayout.EndVertical();
			GUILayout.EndArea();
		}
	}
	
	public static void OnGUIBack() {
		if (GUI.Button(new Rect(10, Screen.height - 40, 200, 30), "Back to Kitchen Sink")) {	
//			AudioServices.PlaySystemSound(AudioServices.kSystemSoundID_Vibrate);
			Application.LoadLevel("KitchenSink");
		}
	}
}
