using UnityEngine;
using System.Collections;
using U3DXT.iOS.Multipeer;
using System.IO;
using U3DXT.Utils;

/// <summary>
/// Share near me client. When connected, it will listen for a file to be sent to it.
/// </summary>
public class ShareNearMeClient : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	
		MultipeerXT.BrowserCompleted += BrowserCompleted;
		MultipeerXT.SessionStartedReceivingResourceWithName += StartReceivingResourceWithName;
		MultipeerXT.SessionFinishedReceivingResourceWithName += FinishedReceivingResourceWithName;
		MultipeerXT.SessionChanged += SessionChanged;
		MultipeerXT.SessionReceived += SessionReceived;
	}
	
	void OnDestroy () {
	
		MultipeerXT.BrowserCompleted -= BrowserCompleted;
		MultipeerXT.SessionStartedReceivingResourceWithName -= StartReceivingResourceWithName;
		MultipeerXT.SessionFinishedReceivingResourceWithName -= FinishedReceivingResourceWithName;
		MultipeerXT.SessionChanged -= SessionChanged;
		MultipeerXT.SessionReceived -= SessionReceived;
	}
	
	private void BrowserCompleted(object sender, System.EventArgs e)
	{
		Log ("Browser Completed");
	}
	
	private void SessionChanged(object sender, SessionChangedEventArgs e) {
		Log ("Session Changed " + e.state);
		if ( e.state == U3DXT.iOS.Native.MultipeerConnectivity.MCSessionState.Connected )
		{
			Log ("Session Connected");
		}
	}

	private void StartReceivingResourceWithName(object sender, SessionStartedReceivingResourceWithNameEventArgs e) {
		Log ("Receiving Resource With Name " + e.resourceName);
	}

	private void FinishedReceivingResourceWithName(object sender, SessionFinishedReceivingResourceWithNameEventArgs e) {
		Log ("Finished Resource With Name " + e.resourceName);
		Log ("File Written to: " + e.localURL.Path ());
		byte[] bytes = File.ReadAllBytes( e.localURL.Path() );
		Log ("Contents of File: " + bytes);
	}
	
	private void SessionReceived(object sender, SessionReceivedEventArgs e) {
		Log("Received data: " + e.data.ToByteArray().ToStraightString());
	}
		
	void OnGUI()  {
		KitchenSink.OnGUIBack();
		
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
