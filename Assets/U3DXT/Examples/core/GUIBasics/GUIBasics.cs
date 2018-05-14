using System;
using System.Collections;
using UnityEngine;
using U3DXT.Core;
using U3DXT.iOS.Native.Foundation;
using U3DXT.iOS.Native.UIKit;
using U3DXT.Utils;
using U3DXT.iOS.Native.Internals;
using System.IO;
using System.Text;
using System.Linq;
using U3DXT.iOS.GUI;

public class GUIBasics : MonoBehaviour {
	
	private Texture2D _image;
	
	void Start() {
		if (CoreXT.IsDevice) {
			// subscribes to events
			GUIXT.AlertDismissed += OnAlertDismissed;
			
			GUIXT.MediaPicked += OnMediaPicked;
			GUIXT.MediaPickCancelled += OnMediaCancelled;
		}
	}
	
	void OnDestroy() {
		if (CoreXT.IsDevice) {
			// unsubscribe to events
			GUIXT.AlertDismissed -= OnAlertDismissed;
			
			GUIXT.MediaPicked -= OnMediaPicked;
			GUIXT.MediaPickCancelled -= OnMediaCancelled;
			
			if (_picker != null)
				_picker.RemoveFromSuperview();
		}
	}
	
	void OnAlertDismissed(object sender, AlertViewDismissedEventArgs e) {
		Log("Alert view dismissed with button: " + e.selectedButtonTitle + " and inputs: " + e.inputString1 + ", " + e.inputString2);
	}
	
	void OnMediaPicked(object sender, MediaPickedEventArgs e) {
		if (e.url != null)
			Log("Image picked with url at: " + e.url.AbsoluteString());
		else
			Log("Image picked: " + e.image);
		
		// clean up previous texture2d first
		if (_image != null)
			Texture2D.Destroy(_image);

		// scaling to 25% because it's an expensive process to convert to texture 2d and upload to GPU
		_image = e.image.ToTexture2D(true, 0.25f);
	}
	
	void OnMediaCancelled(object sender, EventArgs e) {
		Log("Image pick cancelled.");
	}
	
	void ShowBatteryInfo() {
		UIDevice device = UIDevice.CurrentDevice();
		device.batteryMonitoringEnabled = true; // need to enable this first
		
		Log("Battery state: " + device.batteryState);
		Log("Battery level: " + device.batteryLevel);
	}
	
	private UIPickerView _picker; // keep it as a member variable
	void ShowPickerView() {
		if (_picker == null) {
			// define data
			string[][] data = new string[][] {
				new string[] {"Apple", "Banana", "Water melon"},
				new string[] {"Water", "Milk", "Juice", "Soda"}
			};
			
			// create with size
			_picker = new UIPickerView(new Rect(Screen.width / 4, Screen.height / 4, Screen.width / 4, Screen.height / 4));
			_picker.backgroundColor = UIColor.WhiteColor();
			
			// handlers to return data sizes
			_picker.numberOfComponentsInHandler = delegate(UIPickerView pickerView) {
				return data.Length;
			};
			_picker.numberOfRowsInComponentHandler = delegate(UIPickerView pickerView, int component) {
				return data[component].Length;
			};
			
			// handler to return the titles
			_picker.titleForRowHandler = delegate(UIPickerView pickerView, int row, int component) {
				return data[component][row];
			};
			
			// handler to return the views
			_picker.viewForRowHandler = delegate(UIPickerView pickerView, int row, int component, UIView view) {
				
				// reuse or create a label
				UILabel label = view as UILabel;
				if (label == null) {
					label = new UILabel(new Rect(0, 0, 200, 100));
					label.textAlignment = NSTextAlignment.Center;
				}
				
				// assign text
				label.text = data[component][row];
				return label;
			};
			
			// handler to return width and height
			_picker.widthForComponentHandler = delegate(UIPickerView pickerView, int component) {
				return 200;
			};
			_picker.rowHeightForComponentHandler = delegate(UIPickerView pickerView, int component) {
				return 100;
			};
			
			// event for a row being selected
			_picker.DidSelectRow += delegate(object sender, UIPickerView.DidSelectRowEventArgs e) {
				Log("Picker selected: " + data[e.component][e.row]);
			};
		}
		
		if (_picker.superview == null)
			UIApplication.deviceRootViewController.view.AddSubview(_picker);
	}
	
	void HidePickerView() {
		if (_picker != null) {
			Log("Picker selected: ");
			for (var i=0; i<_picker.numberOfComponents; i++) {
				var selectedRow = _picker.SelectedRowInComponent(i);
				Log("component " + i + ": " + _picker.titleForRowHandler(_picker, selectedRow, i));
			}
			_picker.RemoveFromSuperview();
		}
	}
	
	void OnGUI() {
		
		KitchenSink.OnGUIBack();
		
		if (CoreXT.IsDevice) {
			
			GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, Screen.height/2 - 50));
			GUILayout.BeginHorizontal();

			if (GUILayout.Button("Show alert", GUILayout.ExpandHeight(true))) {
				GUIXT.ShowAlert("Alert title", "Alert message", "Cancel", new string[] { "OK", "Hi", "Hello" });
			}
			
			if (GUILayout.Button("Show input", GUILayout.ExpandHeight(true))) {
				GUIXT.ShowAlert("Input Prompt", "Enter something", "Cancel", new string[] {"OK"}, UIAlertViewStyle.PlainTextInput);
			}
			
			if (GUILayout.Button("Pick image\nfrom\nphoto library", GUILayout.ExpandHeight(true))) {
				GUIXT.ShowImagePicker();
			}
			
			if (GUILayout.Button("Take photo\nfrom camera", GUILayout.ExpandHeight(true))) {
				GUIXT.ShowImagePicker(UIImagePickerControllerSourceType.Camera);
			}
			
			if (GUILayout.Button("Show battery info", GUILayout.ExpandHeight(true))) {
				ShowBatteryInfo();
			}
			
			if (GUILayout.Button("Show picker view", GUILayout.ExpandHeight(true))) {
				ShowPickerView();
			}
			
			if (GUILayout.Button("Hide picker view", GUILayout.ExpandHeight(true))) {
				HidePickerView();
			}

			GUILayout.EndHorizontal();
			GUILayout.EndArea();
			
			if (_image != null)
				GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, Screen.width / 2 - 100, Screen.height / 2 - 100), _image);
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
