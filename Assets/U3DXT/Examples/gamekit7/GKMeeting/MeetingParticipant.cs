using System;
using UnityEngine;
using U3DXT.iOS.GameKit;
using System.Text;
using U3DXT.iOS.Native.GameKit;

public class MeetingParticipant : MonoBehaviour {
	
	public Player player;
	public int index = 0;
	private string playerName;
	public bool isSpeaking;
	
	void Start() {
		playerName = player.displayName;
		isSpeaking = false;
		
		// load the player photo, specify a callback
		if (player.photo == null) {
			GKMeetingMain.Log("Loading photo for " + playerName);
			player.LoadPhoto(GKPhotoSize.Small, delegate(Texture2D photo) {
				GKMeetingMain.Log("Loaded photo for " + playerName);
			});
		}
	}
	
	void OnGUI() {
		int photoSize = Screen.width / 4 - 10;
		if (player.photo != null)
			GUI.DrawTexture(new Rect(Screen.width / 4 * index + 5, 10, photoSize, photoSize), player.photo);
		
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(Screen.width / 4 * index + 5, photoSize + 20, photoSize, 30), player.displayName);
		
		if (isSpeaking)
			GUI.Label(new Rect(Screen.width / 4 * index + 5, photoSize + 50, photoSize, 30), "Speaking");
	}
}
