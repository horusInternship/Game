using System;
using UnityEngine;
using U3DXT.iOS.GameKit;
using System.Text;
using U3DXT.iOS.Native.GameKit;
using System.Collections.Generic;

public class Meeting : MonoBehaviour {
	
	private RealTimeMatch match;
	private Dictionary<Player, MeetingParticipant> participants = new Dictionary<Player, MeetingParticipant>();
	private string chatText = "hello";
	
	void Start () {
		match = RealTimeMatchesController.currentMatch;
		
		// add local player and then other players
		AddParticipant(GameKitXT.localPlayer);
		foreach (var player in match.players) {
			AddParticipant(player);
		}
		
		// subscribe to events
		match.DataReceived += OnReceiveData;
		match.PlayerStateChanged += OnPlayerStateChanged;
		
		// start voice chat
		var voiceChat = match.GetVoiceChat("all");
		voiceChat.PlayerStateChanged += OnVoiceChatPlayerStateChanged;
		voiceChat.Join();
		voiceChat.isTalking = true;
	}
	
	void OnDestroy() {
		// disconnect the match
		match.Disconnect();
		
		match = null;
		
		// manually remove this participant
		RemoveParticipant(GameKitXT.localPlayer);
	}
	
	// receive data handler
	void OnReceiveData(object sender, DataReceivedEventArgs e) {
		GKMeetingMain.Log(e.player.displayName + " said: " + e.dataString);
	}
	
	// handler for when a player is connected or disconnected
	void OnPlayerStateChanged(object sender, PlayerStateChangedEventArgs e) {
		if (e.isConnected) {
			AddParticipant(e.player);
		} else {
			RemoveParticipant(e.player);
		}
	}
	
	void AddParticipant(Player player) {
		if (participants.ContainsKey(player))
			return;

		GKMeetingMain.Log("Joining meeting: " + player.displayName);
		
		// add a new participant script component
		var participant = gameObject.AddComponent<MeetingParticipant>();
		participant.player = player;
		participant.index = participants.Count;
		
		participants[player] = participant;
	}
	
	void RemoveParticipant(Player player) {
		MeetingParticipant oldParticipant = null;
		if (!participants.TryGetValue(player, out oldParticipant))
			return;
		
		GKMeetingMain.Log("Leaving meeting: " + player.displayName);

		participants.Remove(player);
		
		// set the index of the remaining participants
		foreach (var participant in participants.Values) {
			if (participant.index > oldParticipant.index)
				participant.index--;
		}
		
		// clean up
		Destroy(oldParticipant);
	}
	
	// handler for when a player is speaking or silent in a voice chat
	void OnVoiceChatPlayerStateChanged(object sender, VoiceChatPlayerStateChangedEventArgs e) {
		MeetingParticipant participant = null;
		if (!participants.TryGetValue(e.player, out participant))
			return;
		
		participant.isSpeaking = (e.state == GKVoiceChatPlayerState.Speaking);
	}
	
	void SendData(string msg) {
		GKMeetingMain.Log("You said: " + msg);
		
		// send the msg to all players
		match.SendDataToAll(msg, true);
	}
	
	void OnGUI() {
		
		GUILayout.BeginArea(new Rect(50, Screen.width / 4 + 100, Screen.width - 100, 300));
		
		GUILayout.BeginHorizontal();
		chatText = GUILayout.TextField(chatText, GUILayout.Width(Screen.width - 200), GUILayout.ExpandHeight(true));
		if (GUILayout.Button("Send", GUILayout.ExpandHeight(true))) {
			SendData(chatText);
		}
		GUILayout.EndHorizontal();
		
		if (GUILayout.Button("Leave Meeting", GUILayout.ExpandHeight(true))) {
			gameObject.GetComponent<GKMeetingMain>().LeaveMeeting();
		}
		
		GUILayout.EndArea();
	}
}
