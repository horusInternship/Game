using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
#if UNITY_IPHONE
using System;
using System.Collections;
using UnityEngine;
using U3DXT.Core;
using U3DXT.iOS.Native.Foundation;
using U3DXT.iOS.Native.GameKit;
using U3DXT.iOS.Native.UIKit;
using U3DXT.iOS.Native.Internals;
using U3DXT.iOS.IAP;
using U3DXT.iOS.Social;
using U3DXT.Utils;
using System.IO;
using U3DXT.iOS.Native.StoreKit;
using U3DXT.iOS.GameKit;
using System.Linq;
#endif

#if UNITY_ANDROID
/*using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Runtime.InteropServices;*/
#endif

public class APIManager : MonoBehaviour {



	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}




	#if UNITY_IOS && !UNITY_EDITOR
	private string leaderboardID = "ines_leaderboard";
	#endif
	#if UNITY_ANDROID && !UNITY_EDITOR
	//private string leaderboardID = "CgkItsLusNkLEAIQAg";
	#endif

	public void GameCenterConection(){

		#if UNITY_IOS && !UNITY_EDITOR
		if (CoreXT.IsDevice){
			
			
			GameKitXT.LocalPlayerAuthenticated += delegate(object sender, EventArgs e) {
				PlayerData.playername=GameKitXT.localPlayer.alias;
				
			};
			
			GameKitXT.LocalPlayerAuthenticationFailed += delegate(object sender, U3DXTErrorEventArgs e) {  };
			
			
			
			GameKitXT.AuthenticateLocalPlayer();
			
		}
		#endif

		#if UNITY_ANDROID && !UNITY_EDITOR
		/*GooglePlayGames.PlayGamesPlatform.Activate();
		Debug.Log("Activate");
		GooglePlayGames.OurUtils.Logger.DebugLogEnabled=true;

	

		if (!Social.localUser.authenticated) {
			// Authenticate
			Debug.Log("Authenticating");

			Social.localUser.Authenticate((bool success) => {
				//GameObject.Find ("TESTE").transform.GetChild(0).GetComponent<Text>().text="After Authenticate";
				if (success) {
					//GameObject.Find ("TESTE").transform.GetChild(0).GetComponent<Text>().text="Auth SUCCESS";
					Debug.Log("Authentication successful");
				}
				else
				{
					//GameObject.Find ("TESTE").transform.GetChild(0).GetComponent<Text>().text="Auth Failed";
					Debug.Log("Authentication failed");
				}
			});
		}*/
		#endif
	}

	public void SubmitScore(long score){
		#if UNITY_IOS && !UNITY_EDITOR
		GameKitXT.ReportScore(leaderboardID, score);
		#endif
		#if UNITY_ANDROID && !UNITY_EDITOR
		/*Social.ReportScore(score, leaderboardID, (bool success) => {
			LoadHighScores();
		});*/
		#endif
	}


	public void LoadHighScores(){

	}
	string testescore="";
	string androidstatus="";
	int useridinleaderboard=0;
	public void RetrieveTopTenScores() {

		#if UNITY_IOS && !UNITY_EDITOR
		GKLeaderboard leaderboardRequest = new GKLeaderboard();
		if (leaderboardRequest != null) {
			// configure request
			leaderboardRequest.playerScope = GKLeaderboardPlayerScope.Global;
			leaderboardRequest.timeScope = GKLeaderboardTimeScope.AllTime;
			leaderboardRequest.category = leaderboardID;
			leaderboardRequest.range = new NSRange(1, 1);
			// load scores; it calls delegate back when done
			leaderboardRequest.LoadScores(delegate(object[] scores, NSError error) {
				if (error != null) {
					//Log("Error retrieving scores: " + error.LocalizedDescription());
				} else if (scores != null) {
					
					// got the scores, but the low-level GKScore only has player ID.
					// if you want player display name, you need to combine it with
					// the high-level API to load the players
					string[] playerIDs = scores.Cast<GKScore>().Select(x => x.playerID).ToArray();
					Player.LoadPlayersByIDs(playerIDs, delegate(Player[] players) {
						
						// print localized title of leaderboard
						//Log("Top 100 for " + leaderboardRequest.title);


						//current user
						GKScore score = leaderboardRequest.localPlayerScore;
						int rank=score.rank;
						Player player = players[0];

						//GameObject.Find ("TESTE").transform.GetChild(0).GetComponent<Text>().text="rank "+rank+" Score "+score+" Name "+player.displayName;

						//All scores
						/*for (int i=0; i<scores.Length; i++) {
							GKScore score = scores[i] as GKScore;
							Player player = players[i];
							//plname = players[i];
							testescore+=score.rank + ". " + score.formattedValue +", "+player.displayName;
							//testescore+=score.rank + ". " + score.formattedValue;
							//GameObject.Find ("TESTE").transform.GetChild(0).GetComponent<Text>().text="ShowScore "+testescore;
							Debug.Log(score.rank + ". " + score.formattedValue + ", " + score.date + ", " + player.displayName);
						}*/
					});
				}
			});
		}
		#endif

		#if UNITY_ANDROID && !UNITY_EDITOR
		/*ILeaderboard lb = Social.CreateLeaderboard();
		lb.id = leaderboardID;
		lb.LoadScores(ok =>
		              {
			if (ok) {
				androidstatus="OK retrieving leaderboard";
				LoadUsersAndDisplay(lb);
			}
			else {
				androidstatus="Error retrieving leaderboard";
			}
		});*/
		#endif
	}
	
	#if UNITY_ANDROID && !UNITY_EDITOR
	/*
	void LoadUsersAndDisplay(ILeaderboard lb)
	{	
		androidstatus="LoadUsersAndDisplay";
		List<string> userIds = new List<string>();
		
		foreach(IScore score in lb.scores) {
			userIds.Add(score.userID);
		}
		// load the profiles and display (or in this case, log)
		Social.LoadUsers(userIds.ToArray(), (users) =>
		{
			string status = "Leaderboard loading: " + lb.title + " count = " +
				lb.scores.Length;
			int usercount=0;
			androidstatus="";
			GlobalData.leaderboard_name= new string[] {};
			GlobalData.leaderboard_score= new string[] {};
			foreach(IScore score in lb.scores) {
				IUserProfile user = users[usercount];

				androidstatus += "\n" + score.formattedValue + " username "+ user.userName;
				GlobalData.leaderboard_name[usercount]=user.userName.ToString();
				GlobalData.leaderboard_score[usercount]=score.formattedValue.ToString();
				usercount++;
			}
			GlobalData.LocalUserName=Social.localUser.userName;
			GameObject.Find("MainMenu").transform.FindChild("LeaderBoardMenu").gameObject.GetComponent<LeaderBoard>().ShowLeaderBoard();
			//GameObject.Find ("TESTE").transform.GetChild(0).GetComponent<Text>().text="Data From Leaderboard "+androidstatus;
		});

	}
	
	*/
	#endif
	
	
}
