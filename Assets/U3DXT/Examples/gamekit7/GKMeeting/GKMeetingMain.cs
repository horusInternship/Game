using UnityEngine;
using System.Collections;
using U3DXT.Core;
using U3DXT.iOS.GameKit;
using System;
using U3DXT.iOS.Native.GameKit;
using U3DXT.Utils;
using U3DXT.iOS.Native.Foundation;

public class GKMeetingMain : MonoBehaviour {

	void Start () {
		// only do it on device
		if (CoreXT.IsDevice) {
			// subscribe to events
			GameKitXT.LocalPlayerAuthenticated += OnLocalPlayerAuthenticated;
			GameKitXT.LocalPlayerAuthenticationFailed += LocalPlayerAuthenticationFailed;

			// init real time multiplayer events and controller
			RealTimeMatchesController.MatchMakerCancelled += OnMatchMakerCancelled;
			RealTimeMatchesController.MatchMakerFailed += OnMatchMakerFailed;
			RealTimeMatchesController.MatchMakerFoundMatch += OnMatchMakerFoundMatch;
			RealTimeMatchesController.InviteAccepted += OnInviteAccepted;
			RealTimeMatchesController.PlayersInvited += OnPlayersInvited;
			RealTimeMatchesController.Init();

			// finally authenticate player
			GameKitXT.AuthenticateLocalPlayer();
		}
	}
	
	void OnDestroy() {
		if (CoreXT.IsDevice) {
			// unsubscribe all events
			GameKitXT.LocalPlayerAuthenticated -= OnLocalPlayerAuthenticated;
			GameKitXT.LocalPlayerAuthenticationFailed -= LocalPlayerAuthenticationFailed;

			RealTimeMatchesController.MatchMakerCancelled -= OnMatchMakerCancelled;
			RealTimeMatchesController.MatchMakerFailed -= OnMatchMakerFailed;
			RealTimeMatchesController.MatchMakerFoundMatch -= OnMatchMakerFoundMatch;
			RealTimeMatchesController.InviteAccepted -= OnInviteAccepted;
			RealTimeMatchesController.PlayersInvited -= OnPlayersInvited;
		}
	}
	
	void OnLocalPlayerAuthenticated(object sender, EventArgs e) {
		Log("Local player authenticated: " + GameKitXT.localPlayer.alias);
	}
	
	void LocalPlayerAuthenticationFailed(object sender, U3DXTErrorEventArgs e) {
		Log("Local player authentication failed: " + e.description);
	}
	
	void OnMatchMakerCancelled(object sender, EventArgs e) {
		Log("User cancelled matchmaking.");
	}
	
	void OnMatchMakerFailed(object sender, U3DXTErrorEventArgs e) {
		Log("Matchmaker failed because: (" + e.code + ") " + e.description);
	}
	
	// when matchmaker finds a match, start the meeting if not expecting anymore to join
	void OnMatchMakerFoundMatch(object sender, MatchEventArgs e) {
		Log("Matchmaker found a match.");
		
//		if (e.realTimeMatch.expectedPlayerCount == 0)
		StartMeeting();
	}
	
	// when the local player accepted an invite, clean up the current meeting
	void OnInviteAccepted(object sender, InviteAcceptedEventArgs e) {
		Log("Accepted invite from: " + e.inviter.displayName);
		
		LeaveMeeting();
	}
	
	// when the local player invites others from game center app
	void OnPlayersInvited(object sender, PlayersInvitedEventArgs e) {
		Log("Invited users from Game Center app: " + e.playersToInvite);
		
		// bring up match making interface with the array of invited players
		RealTimeMatchesController.StartMatch(2, 4, 0, 0, e.playersToInvite);
	}
	
	void CreateMeeting() {
		Log("Creating meeting.");

		// start a match with 2 to 4 players
		RealTimeMatchesController.StartMatch(2, 4);
	}
	
	void StartMeeting() {
		Log("Starting meeting.");
		
		// add a meeting script component
		gameObject.AddComponent<Meeting>();
	}
	
	public void LeaveMeeting() {
		var meeting = gameObject.GetComponent<Meeting>();
		if (meeting == null)
			return;
		
		Log("Leaving meeting.");
		
		// clean up
		Destroy(meeting);
	}
	
	void CreateMeetingCustom() {
		Log("Creating meeting with custom UI.");
		
		var request = new GKMatchRequest();
		request.minPlayers = 2;
		request.maxPlayers = 4;

		GKMatchmaker.SharedMatchmaker().FindMatch(request, OnCustomFoundMatch);
	}
	
	void OnCustomFoundMatch(GKMatch match, NSError error) {
	    if (error != null) {
	        Log("Error with matchmaker: " + error.LocalizedDescription());
	    } else if (match != null) {
			// set the high-level match, and it will raise MatchMakerFoundMatch event
	        RealTimeMatchesController.SetCurrentMatch(match);
	    }
	}
	
	void OnGUI() {
		
		var meeting = gameObject.GetComponent<Meeting>();
		if (meeting == null) {
			KitchenSink.OnGUIBack();

			GUI.Label(new Rect(50, 50, 500, 50), "MUST first setup Game Center and Multiplayer in iTunesConnect. See README.txt.");

			if (GUI.Button(new Rect(Screen.width / 2 - 50, 100, 100, 100), "Create / Join")) {
				CreateMeeting();
			}
			
			if (GUI.Button(new Rect(Screen.width / 2 - 50, 220, 100, 100), "Create / Join\nCustom UI\n(Takes a while)")) {
				CreateMeetingCustom();
			}
			
			//TODO: need cancel button
		}
	
		OnGUILog();
	}
	
	static string _log = "Debug log:";
	static Vector2 _scrollPosition = Vector2.zero;
	
	void OnGUILog() {
		GUILayout.BeginArea(new Rect(50, Screen.height / 4 * 3, Screen.width - 100, Screen.height / 4 - 50));
		_scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
		GUI.skin.box.wordWrap = true;
		GUI.skin.box.alignment = TextAnchor.UpperLeft;
		GUILayout.Box(_log, GUILayout.ExpandHeight(true));
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}
	
	public static void Log(string str) {
		_log += "\n" + str;
		_scrollPosition.y = Mathf.Infinity;
	}
}
