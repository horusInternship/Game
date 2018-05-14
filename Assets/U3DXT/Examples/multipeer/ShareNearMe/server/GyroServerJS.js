#pragma strict
import U3DXT.iOS.Multipeer;
import U3DXT.iOS.Native.MultipeerConnectivity;
import U3DXT.iOS.Native.Foundation;
import U3DXT.Utils;

var _session:MCSession = null;
var _peerId:MCPeerID = null;
	
function Start() {
	MultipeerXT.SessionChanged += SessionChanged;
		
	Log("Waiting for client to connect.");
}

function OnDestroy() {
	MultipeerXT.SessionChanged -= SessionChanged;
}

function SessionChanged(sender:Object, e:SessionChangedEventArgs) {
	Log("Session Changed " + e.state);

	if (e.state == U3DXT.iOS.Native.MultipeerConnectivity.MCSessionState.Connected ) {
		Log("CONNECTED");
	
		_session = e.session;
		_peerId = e.peerID;
	}
}

function FixedUpdate() {

	if (_session == null)
		return;
		
	var accelX = Input.gyro.userAcceleration.x;
	var accelY = Input.gyro.userAcceleration.y;
	var accelZ = Input.gyro.userAcceleration.z;

	var str = accelX + "," + accelY + "," + accelZ;
	Log("Sending gyro accel: " + str);
	
	var data:NSData = NSData.FromByteArray(str.ToStraightBytes());
	_session.SendData(data, [_peerId], MCSessionSendDataMode.Reliable, null);
}

// debug logging
function OnGUI() {
	OnGUILog();
}

var _log:String = "Debug log:";
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
	if (_log.Length > 6000)
		_log = _log.Substring(2000);
	_scrollPosition.y = Mathf.Infinity;
}
