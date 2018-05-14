#pragma strict

import U3DXT.Core;
import U3DXT.iOS.Native.Foundation;
import U3DXT.iOS.Native.GameKit;
import U3DXT.iOS.GameKit;
import System.Linq;

var leaderboardID:String = "com.vitapoly.gamekittest.leaderboard";
var achievementID:String = "com.vitapoly.gamekittest.achievement";

function Start () {
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

function OnDestroy() {
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

function OnLocalPlayerAuthenticated(sender:Object, e:EventArgs) {
			
	var localPlayer:LocalPlayer = GameKitXT.localPlayer;
	Log("Local player authenticated: " + localPlayer.playerID);
	
	localPlayer.LoadFriends(function(players:Player[]) {
		Log("Loaded friends:");
		for (var player:Player in players) {
			Log(player.playerID + ": " + player.displayName);
		}
	});
	
	localPlayer.LoadPhoto(GKPhotoSize.Normal, function(photo:Texture2D) {
		if (photo != null) {
			Log("Loaded photo");
			//GameObject.Find("PlayerPhoto").GetComponent<GUITexture>().texture = photo;
		} else {
			Log("Local player has no photo or error loading photo.");
		}
	});
}

function LocalPlayerAuthenticationFailed(sender:Object, e:U3DXTErrorEventArgs) {
	Log("Local player authentication failed: " + e.description);
}

function OnScoreReported(sender:Object, e:EventArgs) {
	Log("Reported score.");
}

function OnScoreReportFailed(sender:Object, e:U3DXTErrorEventArgs) {
	Log("Score report failed: " + e.description);
}

function OnAchievementReported(sender:Object, e:EventArgs) {
	Log("Reported achievement.");
}

function OnAchievementReportFailed(sender:Object, e:U3DXTErrorEventArgs) {
	Log("Achievement report failed: " + e.description);
}

function RetrieveTopTenScores() {
	var leaderboardRequest:GKLeaderboard = new GKLeaderboard();
	if (leaderboardRequest != null) {
		// configure request
		leaderboardRequest.playerScope = GKLeaderboardPlayerScope.Global;
		leaderboardRequest.timeScope = GKLeaderboardTimeScope.AllTime;
		leaderboardRequest.category = leaderboardID;
		leaderboardRequest.range = new NSRange(1, 10);
		
		// load scores; it calls delegate back when done
		leaderboardRequest.LoadScores(function(scores:Object[], error:NSError) {
			if (error != null) {
				Log("Error retrieving scores: " + error.LocalizedDescription());
			} else if (scores != null) {
				
				// got the scores, but the low-level GKScore only has player ID.
				// if you want player display name, you need to combine it with
				// the high-level API to load the players
				var playerIDs:String[] = scores.Cast.<GKScore>().Select(function(x) {return x.playerID;}).ToArray();
				Player.LoadPlayersByIDs(playerIDs, function(players:Player[]) {
					
					// print localized title of leaderboard
					Log("Top 10 for " + leaderboardRequest.title);
					
					for (var i:int=0; i<scores.Length; i++) {
						var score:GKScore = scores[i] as GKScore;
						var player:Player = players[i];
						Log(score.rank + ". " + score.formattedValue + ", " + score.date + ", " + player.displayName);
					}
				});
			}
		});
	}
}

var scoreText:String = "";
var achievementText:String = "";

function OnGUI() {

	if (CoreXT.IsDevice) {
		
		GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, Screen.height/2 - 50));
		GUILayout.BeginHorizontal();

		if (GUILayout.Button("Show Game Center", GUILayout.ExpandHeight(true))) {
			GameKitXT.ShowGameCenter();
		}

		if (GUILayout.Button("Show Banner", GUILayout.ExpandHeight(true))) {
			GameKitXT.ShowBanner("Game Kit Basics", "Hello from U3DXT!");
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

var _log:String = "Debug log for JS:";
var _scrollPosition:Vector2 = Vector2.zero;

function OnGUILog() {
	GUILayout.BeginArea(new Rect(50, Screen.height / 2, Screen.width - 100, Screen.height / 2 - 50));
	_scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
	GUI.skin.box.wordWrap = true;
	GUI.skin.box.alignment = TextAnchor.UpperLeft;
	GUILayout.Box(_log, GUILayout.ExpandHeight(true));
	GUILayout.EndScrollView();
	GUILayout.EndArea();
}

function Log(str:String) {
	_log += "\n" + str;
	_scrollPosition.y = Mathf.Infinity;
}
