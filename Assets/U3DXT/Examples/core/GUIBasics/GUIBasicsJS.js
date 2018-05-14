#pragma strict

import System;
import U3DXT.Core;
import U3DXT.iOS.GUI;
import U3DXT.iOS.Native.UIKit;

function Start () {
	if (CoreXT.IsDevice) {
		
		SubscribeEvents();
	}
}

function OnGUI() {
	
	if (CoreXT.IsDevice) {
		
		GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, Screen.height/2 - 50));
		GUILayout.BeginHorizontal();

		if (GUILayout.Button("Show alert", GUILayout.ExpandHeight(true))) {
			GUIXT.ShowAlert("Alert title", "Alert message", "Cancel", ["OK", "Hi", "Hello", "U3DXT"]);
		}
		
		if (GUILayout.Button("Show input", GUILayout.ExpandHeight(true))) {
			GUIXT.ShowAlert("Input Prompt", "Enter something", "Cancel", ["OK"], UIAlertViewStyle.PlainTextInput);
		}
		
		if (GUILayout.Button("Pick image\nfrom\nphoto library", GUILayout.ExpandHeight(true))) {
			GUIXT.ShowImagePicker(UIImagePickerControllerSourceType.PhotoLibrary, UIImagePickerControllerCameraDevice.Rear);
		}
		
		if (GUILayout.Button("Take photo\nfrom camera", GUILayout.ExpandHeight(true))) {
			GUIXT.ShowImagePicker(UIImagePickerControllerSourceType.Camera, UIImagePickerControllerCameraDevice.Rear);
		}

		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
	
	OnGUILog();
}

function SubscribeEvents() {
	
	GUIXT.AlertDismissed += function(sender:Object, e:AlertViewDismissedEventArgs) {
		Log("Alert view dismissed with button: " + e.selectedButtonTitle + " and inputs: " + e.inputString1 + ", " + e.inputString2);
	};
	
	GUIXT.MediaPicked += function(sender:Object, e:MediaPickedEventArgs) {
		Log("Image picked: " + e.image);
		
		// expensive process to convert to texture 2d and upload to GPU
		//Texture2D texture = e.image.ToTexture2D();
	};
	
	GUIXT.MediaPickCancelled += function(sender:Object, e:EventArgs) {
		Log("Image pick cancelled.");
	};
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
	_scrollPosition.y = Mathf.Infinity;
}
