using System;
using System.Collections;
using UnityEngine;
using U3DXT.Core;
using U3DXT.iOS.Native.Foundation;
using U3DXT.iOS.Native.UIKit;
using U3DXT.Utils;
using U3DXT.iOS.Native.Internals;
using System.IO;
using U3DXT.iOS.GUI;
using U3DXT.iOS.CoreImage;
using U3DXT.iOS.Native.CoreImage;
using U3DXT.iOS.Native.CoreGraphics;

public class WebCamFaceDetector : MonoBehaviour {
	
	private CameraPreviewVideo _cameraVideo;
	private FaceDetector _faceDetector;
	private Face[] _faces;
	
	public bool autoStart = true;
	private bool _isDetecting = false;
	
	// increase this to make it faster
	public int detectEveryXFrames = 1;
	private int _frameCount = 0;
	
	void Start() {
		
		_cameraVideo = gameObject.GetComponent<CameraPreviewVideo>();
		if (_cameraVideo == null)
			_cameraVideo = gameObject.AddComponent<CameraPreviewVideo>();
		
		if (autoStart)
			StartDetection();
	}
	
	void OnDestroy() {
		_cameraVideo.Stop();
	}
	
	public void StartDetection() {
		if (CoreXT.IsDevice) {
			if (_faceDetector == null) {
				// create detector with low accuracy and don't track faces
				_faceDetector = new FaceDetector(false, false);
				
				// shrink image to 12.5% first for faster detection
				_faceDetector.preprocessImageScale = 0.125f;
			}
			
			_isDetecting = true;
		} else {
			Debug.Log("Not on device.");
		}
	}
	
	public void StopDetection() {
		_isDetecting = false;
		_faces = null;
	}
	
	void OnGUI() {
		
		KitchenSink.OnGUIBack();
		
		if (CoreXT.IsDevice) {
			
			GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, 100));//Screen.height/2 - 50));
			GUILayout.BeginHorizontal();
		
			if (GUILayout.Button("Front/Back", GUILayout.ExpandHeight(true))) {
				_cameraVideo.Stop();
				_cameraVideo.useFrontCamera = !_cameraVideo.useFrontCamera;
				_cameraVideo.Play();
			}
			
			if (GUILayout.Button("Detection On/Off", GUILayout.ExpandHeight(true))) {
				if (_isDetecting)
					StopDetection();
				else
					StartDetection();
			}
			
			if (GUILayout.Button("Show/Hide Log", GUILayout.ExpandHeight(true))) {
				_showingLog = !_showingLog;
			}
			
			if (GUILayout.Button("Clear Log", GUILayout.ExpandHeight(true))) {
				_log = "";
			}
	
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
			
			// draw faces
			if (_faces != null) {
				foreach (var face in _faces) {
					GUI.Box(face.bounds, "");
					
					// approximate mouth and eye sizes because face detection doesn't return size
					if (face.hasLeftEyePosition) {
						var rect = new Rect(face.leftEyePosition.x, face.leftEyePosition.y,
							face.bounds.width / 5, face.bounds.width / 10);
						rect.x -= rect.width / 2;
						rect.y -= rect.height / 2;
						GUI.Box(rect, "");
					}
					
					if (face.hasRightEyePosition) {
						var rect = new Rect(face.rightEyePosition.x, face.rightEyePosition.y,
							face.bounds.width / 5, face.bounds.width / 10);
						rect.x -= rect.width / 2;
						rect.y -= rect.height / 2;
						GUI.Box(rect, "");
					}

					if (face.hasMouthPosition) {
						var rect = new Rect(face.mouthPosition.x, face.mouthPosition.y,
							face.bounds.width / 3, face.bounds.width / 6);
						rect.x -= rect.width / 2;
						rect.y -= rect.height / 2;
						GUI.Box(rect, "");
					}
				}
			}
		}
		
		if (_showingLog)
			OnGUILog();
	}
	
	void Update() {
		
		if (CoreXT.IsDevice) {
			if (_isDetecting && _cameraVideo.webCamTexture.didUpdateThisFrame) {
				
				// detect every x frames
				_frameCount++;
				if (_frameCount % detectEveryXFrames == 0) {
					CGImageOrientation orientation = _cameraVideo.cameraOrientationForFaceDetector;
					
					_faceDetector.isMirrored = _cameraVideo.isMirrored;
					_faceDetector.projectedScale = _cameraVideo.videoToCameraScale;
					
					// detect
					_faces = _faceDetector.DetectInPixels32(_cameraVideo.webCamTexture.GetPixels32(),
						_cameraVideo.webCamTexture.width, _cameraVideo.webCamTexture.height, orientation);
					
					foreach (var face in _faces) {
						Log("face: " + face.bounds + ", " + face.hasMouthPosition + ", " + face.leftEyePosition + ", " + face.rightEyePosition);
					}
				}
			}
		}
	}

	string _log = "Debug log:";
	Vector2 _scrollPosition = Vector2.zero;
	bool _showingLog = false;
	
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
}
