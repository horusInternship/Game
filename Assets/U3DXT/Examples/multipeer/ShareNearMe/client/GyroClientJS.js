#pragma strict
import U3DXT.iOS.Multipeer;
import U3DXT.iOS.Native.MultipeerConnectivity;
import U3DXT.iOS.Native.Foundation;
import U3DXT.Utils;

var _session:MCSession = null;
var _peerId:MCPeerID = null;
	
function Start() {
	MultipeerXT.BrowserCompleted += BrowserCompleted;
	MultipeerXT.SessionChanged += SessionChanged;
	MultipeerXT.SessionReceived += SessionReceived;
}

function OnDestroy() {
	MultipeerXT.BrowserCompleted -= BrowserCompleted;
	MultipeerXT.SessionChanged -= SessionChanged;
	MultipeerXT.SessionReceived -= SessionReceived;
}

function BrowserCompleted(sender:Object, e:System.EventArgs) {
	Log("Browser Completed");
}

function SessionChanged(sender:Object, e:SessionChangedEventArgs) {
	Log ("Session Changed " + e.state);
	if ( e.state == U3DXT.iOS.Native.MultipeerConnectivity.MCSessionState.Connected ) {
		Log ("Session Connected");
	}
}

function SessionReceived(sender:Object, e:U3DXT.iOS.Multipeer.SessionReceivedEventArgs) {
	var str:String = U3DXT.Utils.StringBytesExtension.ToStraightString(e.data.ToByteArray());
 	var arr:String[] = str.Split(","[0]);
 	var accelX:float = System.Convert.ToDouble(arr[0]);
 	var accelY:float = System.Convert.ToDouble(arr[1]);
 	var accelZ:float = System.Convert.ToDouble(arr[2]);
 	
 	Log("Received gyro accel: " + accelX + ", " + accelY + ", " + accelZ);
 	//TODO: do something with the numbers
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
