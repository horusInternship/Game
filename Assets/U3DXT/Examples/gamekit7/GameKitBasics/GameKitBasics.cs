using System;
using System.Collections;
using UnityEngine;
using U3DXT.Core;
using U3DXT.iOS.Native.Foundation;
using U3DXT.iOS.Native.UIKit;
using U3DXT.Utils;
using U3DXT.iOS.Native.Internals;
using System.IO;
using U3DXT.iOS.GameKit;
using U3DXT.iOS.Native.GameKit;
using System.Text;
using System.Linq;

public class GameKitBasics : MonoBehaviour {
	
	private string leaderboardID = "com.vitapoly.gamekittest.leaderboard";
	private string achievementID = "com.vitapoly.gamekittest.achievement";
	
	void Start() {
		if (CoreXT.IsDevice) {
			
			// subscribe to events
			GameKitXT.LocalPlayerAuthenticated += OnLocalPlayerAuthenticated;
			GameKitXT.LocalPlayerAuthenticationFailed += LocalPlayerAuthenticationFailed;
			
			GameKitXT.ScoreReported += OnScoreReported;
			GameKitXT.ScoreReportFailed += OnScoreReportFailed;
	
			GameKitXT.AchievementReported += OnAchievementReported;
			GameKitXT.AchievementReportFailed += OnAchievementReportFailed;

			// finally authenticate player
			GameKitXT.AuthenticateLocalPlayer();
		}
	}
	
	void OnDestroy() {
		if (CoreXT.IsDevice) {
			// unsubscribe to events
			GameKitXT.LocalPlayerAuthenticated -= OnLocalPlayerAuthenticated;
			GameKitXT.LocalPlayerAuthenticationFailed -= LocalPlayerAuthenticationFailed;
			
			GameKitXT.ScoreReported -= OnScoreReported;
			GameKitXT.ScoreReportFailed -= OnScoreReportFailed;
	
			GameKitXT.AchievementReported -= OnAchievementReported;
			GameKitXT.AchievementReportFailed -= OnAchievementReportFailed;
		}
	}
	
	void OnLocalPlayerAuthenticated(object sender, EventArgs e) {
			
		var localPlayer = GameKitXT.localPlayer;
		Log("Local player authenticated: " + localPlayer.playerID);
		
		localPlayer.LoadFriends(delegate(Player[] players) {
			Log("Loaded friends:");
			foreach (var player in players) {
				Log(player.playerID + ": " + player.displayName);
			}
		});
	}
	
	void LocalPlayerAuthenticationFailed(object sender, U3DXTErrorEventArgs e) {
		Log("Local player authentication failed: " + e.description);
	}
	
	void OnScoreReported(object sender, EventArgs e) {
		Log("Reported score.");
	}
	
	void OnScoreReportFailed(object sender, U3DXTErrorEventArgs e) {
		Log("Score report failed: " + e.description);
	}
	
	void OnAchievementReported(object sender, EventArgs e) {
		Log("Reported achievement.");
	}
	
	void OnAchievementReportFailed(object sender, U3DXTErrorEventArgs e) {
		Log("Achievement report failed: " + e.description);
	}
	
	void RetrieveTopTenScores() {
		GKLeaderboard leaderboardRequest = new GKLeaderboard();
		if (leaderboardRequest != null) {
			// configure request
			leaderboardRequest.playerScope = GKLeaderboardPlayerScope.Global;
			leaderboardRequest.timeScope = GKLeaderboardTimeScope.AllTime;
			leaderboardRequest.category = leaderboardID;
			leaderboardRequest.range = new NSRange(1, 10);
			
			// load scores; it calls delegate back when done
			leaderboardRequest.LoadScores(delegate(object[] scores, NSError error) {
				if (error != null) {
					Log("Error retrieving scores: " + error.LocalizedDescription());
				} else if (scores != null) {
					
					// got the scores, but the low-level GKScore only has player ID.
					// if you want player display name, you need to combine it with
					// the high-level API to load the players
					string[] playerIDs = scores.Cast<GKScore>().Select(x => x.playerID).ToArray();
					Player.LoadPlayersByIDs(playerIDs, delegate(Player[] players) {
						
						// print localized title of leaderboard
						Log("Top 10 for " + leaderboardRequest.title);
						
						for (int i=0; i<scores.Length; i++) {
							GKScore score = scores[i] as GKScore;
							Player player = players[i];
							Log(score.rank + ". " + score.formattedValue + ", " + score.date + ", " + player.displayName);
						}
					});
				}
			});
		}
	}
	
	
	string scoreText = "";
	string achievementText = "";
	
	void OnGUI() {

		KitchenSink.OnGUIBack();

		if (CoreXT.IsDevice) {
			
			GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, Screen.height/2 - 50));
			GUILayout.BeginHorizontal();
			
			if (GUILayout.Button("Load Player Photo", GUILayout.ExpandHeight(true))) {
				GameKitXT.localPlayer.LoadPhoto(GKPhotoSize.Normal, delegate(Texture2D photo) {
					if (photo != null) {
						Log("Loaded photo");
						GameObject.Find("PlayerPhoto").GetComponent<GUITexture>().texture = photo;
					} else {
						Log("Local player has no photo or error loading photo.");
					}
				});
			}

			if (GUILayout.Button("Show Game Center", GUILayout.ExpandHeight(true))) {
				GameKitXT.ShowGameCenter();
			}
	
			if (GUILayout.Button("Show Banner", GUILayout.ExpandHeight(true))) {
				GameKitXT.ShowBanner("Game Kit Basics", "Hello from U3DXT!");
				
		long score = 100;
		Debug.Log ("Reporting score " + score + " on leaderboard " + leaderboardID);
		Social.ReportScore (score, leaderboardID, success => {
			Debug.Log(success ? "Reported score successfully" : "Failed to report score");
		});
			}
		
			if (GUILayout.Button("Show Leaderboard", GUILayout.ExpandHeight(true))) {
				GameKitXT.ShowLeaderboard(leaderboardID);
			}

			if (GUILayout.Button("Show Achievement", GUILayout.ExpandHeight(true))) {
				GameKitXT.ShowAchievements();
			}
		
			if (GUILayout.Button("Get Leaderboard", GUILayout.ExpandHeight(true))) {
				RetrieveTopTenScores();
			}
	
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			
			scoreText = GUILayout.TextField(scoreText, GUILayout.ExpandWidth(true));
			if (GUILayout.Button("Report Score", GUILayout.ExpandHeight(true))) {
				GameKitXT.ReportScore(leaderboardID, Convert.ToInt64(scoreText));
			}
	
			achievementText = GUILayout.TextField(achievementText, GUILayout.ExpandWidth(true));
			if (GUILayout.Button("Report Achievement", GUILayout.ExpandHeight(true))) {
				GameKitXT.ReportAchievement(achievementID, Convert.ToDouble(achievementText));
			}
			
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		
		OnGUILog();
	}
	

#region Debug logging
	string _log = "Debug log:";
	Vector2 _scrollPosition = Vector2.zero;
	
	void OnGUILog() {
		GUILayout.BeginArea(new Rect(50, Screen.height / 2, Screen.width - 100, Screen.height / 2 - 50));
		_scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
		GUI.skin.box.wordWrap = true;
		GUI.skin.box.alignment = TextAnchor.UpperLeft;
		GUILayout.Box(_log, GUILayout.ExpandHeight(true));
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}
	
	void Log(string str) {
		_log += "\n" + str;
		_scrollPosition.y = Mathf.Infinity;
	}
#endregion
}
