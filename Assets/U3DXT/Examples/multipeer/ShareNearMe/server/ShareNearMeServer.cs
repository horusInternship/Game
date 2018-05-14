using UnityEngine;
using System.Collections;
using U3DXT.iOS.Multipeer;
using U3DXT.iOS.Native.MultipeerConnectivity;
using System.IO;
using U3DXT.iOS.Native.Foundation;
using U3DXT.Core;
using U3DXT.Utils;

public class ShareNearMeServer : MonoBehaviour {
	
	private string text = "some text";

	// Use this for initialization
	void Start () {
		MultipeerXT.SessionChanged += SessionChanged;
		
		Log("Waiting for client to connect.");
	}
	
	void OnDestroy () {
		MultipeerXT.SessionChanged -= SessionChanged;
	}

	private MCSession _session;
	private MCPeerID _peerId;
	
	private void SessionChanged(object sender, SessionChangedEventArgs e) {
		Log("Session Changed " + e.state);

		if (e.state == U3DXT.iOS.Native.MultipeerConnectivity.MCSessionState.Connected )
		{
			Log("CONNECTED");
		
			string path = Application.temporaryCachePath+"/u3dxt.jpg";
			// create some arbitary binary data. or load from existing location
			FileStream someFile = new FileStream(path, FileMode.Create);
			someFile.WriteByte(0x42);
			someFile.Close();
			
			path = "http://u3dxt.com/wp-content/uploads/2013/06/gears_14662320_s-225x225.jpg";
			_session = e.session;
			_peerId = e.peerID;
			_session.SendResourceAtURL(new U3DXT.iOS.Native.Foundation.NSURL(path), "u3dxt.jpg", e.peerID,  sendResourceCompleted);
		}
	}
	
	private void sendResourceCompleted(U3DXT.iOS.Native.Foundation.NSError err)
	{
		if ( err != null )
			Log("ERROR: " + err.LocalizedDescription());
	}
	
	private void SendData(string text) {
		if (_session == null) {
			Log("Not connected.");
			return;
		}
		
		NSData data = NSData.FromByteArray(text.ToStraightBytes());
		_session.SendData(data, new object[] { _peerId }, MCSessionSendDataMode.Reliable, null);
	}
	
	void OnGUI()
	{
		KitchenSink.OnGUIBack();
		
		if (CoreXT.IsDevice) {
			
			GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, Screen.height/2 - 50));
			GUILayout.BeginHorizontal();

			text = GUILayout.TextField(text, GUILayout.Width(Screen.width - 200), GUILayout.ExpandHeight(true));
			if (GUILayout.Button("Send", GUILayout.ExpandHeight(true))) {
				SendData(text);
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
