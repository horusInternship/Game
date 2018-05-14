using System;
using UnityEngine;
using U3DXT.Core;
using U3DXT.iOS.Data;
using U3DXT.iOS.Native.UIKit;

public class iCloudTest : MonoBehaviour {
	
	const string NAME_KEY = "NAME";
	const string HIGH_SCORE_KEY = "HIGH_SCORE";
	const string MONEY_KEY = "MONEY";
	
	void Start() {
		
		if (CoreXT.IsDevice) {
			
			Log("iCloud ID: " + iCloudPrefs.iCloudID);
			
			iCloudPrefs.AccountChanged += _OnAccountChanged;
			iCloudPrefs.ValuesChangedExternally += _OnValuesChanged;

			iCloudPrefs.Synchronize();
			
			if (iCloudPrefs.HasKey(NAME_KEY)) {
				nameText = iCloudPrefs.GetString(NAME_KEY);
				Log("Name from iCloud: " + nameText);
			} else {
				Log("Setting default name.");
				SetName();
			}
			
			if (iCloudPrefs.HasKey(HIGH_SCORE_KEY)) {
				scoreText = iCloudPrefs.GetInt(HIGH_SCORE_KEY).ToString();
				Log("High score from iCloud: " + scoreText);
			} else {
				Log("Setting default high score.");
				SetHighScore();
			}
			
			if (iCloudPrefs.HasKey(MONEY_KEY)) {
				moneyText = iCloudPrefs.GetFloat(MONEY_KEY).ToString();
				Log("Money from iCloud: " + moneyText);
			} else {
				Log("Setting default money.");
				SetMoney();
			}
		}
	}
	
	void OnDestroy() {
		if (CoreXT.IsDevice) {
			iCloudPrefs.AccountChanged -= _OnAccountChanged;
			iCloudPrefs.ValuesChangedExternally -= _OnValuesChanged;
		}
	}
	
	void _OnAccountChanged(object sender, EventArgs e) {
		Log("iCloud account changed to new ID: " + iCloudPrefs.iCloudID);
	}
	
	void _OnValuesChanged(object sender, iCloudPrefsChangedEventArgs e) {
		Log("iCloud change reason: " + e.reason);
		
		foreach (iCloudPrefsChange change in e.changes) {
			Log("Change for " + change.key + ": " + change.oldValue + " -> " + change.newValue);
			
			// only resolve high score conflict
			if (change.key == HIGH_SCORE_KEY) {
				
				object resolvedValue = change.newValue;
				
				// if new high score is lower, change it back
				if ((change.newValue != null) && (change.oldValue != null)
					&& (((int)change.newValue) < ((int)change.oldValue))) {
					
					Log("New high score is lower, changing it back.");
					resolvedValue = change.oldValue;
					iCloudPrefs.SetInt(HIGH_SCORE_KEY, (int)resolvedValue);
				}

				scoreText = (resolvedValue != null) ? resolvedValue.ToString() : "";
			
			} else if (change.key == NAME_KEY) {
				nameText = (change.newValue != null) ? (change.newValue as string) : "";
			} else if (change.key == MONEY_KEY) {
				moneyText = (change.newValue != null) ? change.newValue.ToString() : "";
			}
		}
	}
	
	string nameText = "Player";
	string scoreText = "1";
	string moneyText = "100.0";
	
	void OnGUI() {
		
		KitchenSink.OnGUIBack();
		
		if (CoreXT.IsDevice) {

			GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, Screen.height/2 - 50));
			
				GUILayout.Label("MUST first setup iCloud in iOS Developer Portal and Xcode.");
			
				GUILayout.BeginHorizontal();
					nameText = GUILayout.TextField(nameText, GUILayout.ExpandWidth(true), GUILayout.Height(100));
					if (GUILayout.Button("Set Name", GUILayout.ExpandHeight(true))) {
						SetName();
					}
				GUILayout.EndHorizontal();
			
				GUILayout.BeginHorizontal();
					scoreText = GUILayout.TextField(scoreText, GUILayout.ExpandWidth(true), GUILayout.Height(100));
					if (GUILayout.Button("Set High Score", GUILayout.ExpandHeight(true))) {
						SetHighScore();
					}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
					moneyText = GUILayout.TextField(moneyText, GUILayout.ExpandWidth(true), GUILayout.Height(100));
					if (GUILayout.Button("Set Money", GUILayout.ExpandHeight(true))) {
						SetMoney();
					}
				GUILayout.EndHorizontal();
			
				if (GUILayout.Button("Manual Sync", GUILayout.ExpandHeight(true))) {
					// don't really need to do this manually
					Log("Manually syncing.");
					iCloudPrefs.Synchronize();
				}
			
				if (GUILayout.Button("Delete All", GUILayout.ExpandHeight(true))) {
					Log("Deleting all iCloudPrefs data.");
					iCloudPrefs.DeleteAll();
				}

				GUILayout.BeginHorizontal();
					if (GUILayout.Button("Copy name into clipboard", GUILayout.ExpandHeight(true))) {
						UIPasteboard pasteboard = UIPasteboard.GeneralPasteboard();
						pasteboard.String = nameText;
						Log("Copied name into clipboard.");
					}
					if (GUILayout.Button("Copy clipboard to name", GUILayout.ExpandHeight(true))) {
						UIPasteboard pasteboard = UIPasteboard.GeneralPasteboard();
						nameText = pasteboard.String;
						Log("Copied clipboard to name.");
					}
				GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		
		OnGUILog();
	}
	
	void SetName() {
		if (nameText.Length == 0)
			Log("Set a name.");
		
		iCloudPrefs.SetString(NAME_KEY, nameText);
		Log("Set name to: " + iCloudPrefs.GetString(NAME_KEY));
	}
	
	void SetHighScore() {
		int score = Convert.ToInt32(scoreText);
		if (score <= 0)
			Log("Set a score higher than 0.");
		
		iCloudPrefs.SetInt(HIGH_SCORE_KEY, score);
		Log("Set high score to: " + iCloudPrefs.GetInt(HIGH_SCORE_KEY));
	}
	
	void SetMoney() {
		float money = (float)Convert.ToDouble(moneyText);
		
		iCloudPrefs.SetFloat(MONEY_KEY, money);
		Log("Set money to: " + iCloudPrefs.GetFloat(MONEY_KEY));
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
