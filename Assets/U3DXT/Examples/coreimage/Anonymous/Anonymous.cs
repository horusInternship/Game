using UnityEngine;
using System.Collections;
using U3DXT.iOS.GUI;
using U3DXT.Core;
using U3DXT.iOS.Native.UIKit;
using U3DXT.iOS.CoreImage;
using U3DXT.iOS.Native.CoreImage;
using U3DXT.iOS.Native.Internals;
using System.Collections.Generic;

public class Anonymous : MonoBehaviour {
	
	private Texture2D _photo;
	private const float CONVERT_SCALE = 0.25f;
	
	private FaceDetector _faceDetector;
	private Face[] _faces;
	
	private ImageFilter _imageFilter;
	private Texture2D[] _scrambledFaces;

	void Start () {
		if (CoreXT.IsDevice) {
			// subscribes to events
			GUIXT.MediaPicked += OnMediaPicked;

			_faceDetector = new FaceDetector(true, false);
			_imageFilter = new ImageFilter();
		}
	}
	
	void OnDestroy() {
		if (CoreXT.IsDevice) {
			// unsubscribe to events
			GUIXT.MediaPicked -= OnMediaPicked;
			
			// clean up old stuff
			if (_photo != null)
				Texture2D.Destroy(_photo);
			
			if (_scrambledFaces != null) {
				foreach (var face in _scrambledFaces) {
					Texture2D.Destroy(face);
				}
				_scrambledFaces = null;
			}
		}
	}
	
	void OnMediaPicked(object sender, MediaPickedEventArgs e) {
		
		// clean up old stuff
		if (_photo != null)
			Texture2D.Destroy(_photo);
		
		if (_scrambledFaces != null) {
			foreach (var face in _scrambledFaces) {
				Texture2D.Destroy(face);
			}
			_scrambledFaces = null;
		}
		
//		_photo = e.image.ToTexture2D(true, 0.25f);
		
		System.Random random = new System.Random();
		
		// set input
		_imageFilter.SetInput(e.image);
		
		// randomly apply some filter to the image first
		switch (random.Next(7)) {
			case 0:
				Log("Applying auto-adjust.");
				_imageFilter.AutoAdjust();
				break;
			case 1:
				Log("Applying sepia and sharpen.");
				_imageFilter.SepiaTone(1.0f);
				break;
			case 2:
				Log("Applying bloom and vignette.");
				// chaining multiple filters together
				_imageFilter.Bloom(10.0f, 1.0f)
				.Filter("CIVignette", new Dictionary<string, object> {
					{"inputRadius", 1.0f},
					{"inputIntensity", 0.5f}
				});
				break;
			case 3:
				Log("Applying color invert.");
				_imageFilter.ColorInvert();
				break;
			case 4:
				Log("Applying red monochrome.");
				_imageFilter.ColorMonochrome(new Color32(0xff, 0x00, 0x00, 0xff), 1.0f);
				break;
			case 5:
				Log("Applying yellow monochrome.");
				_imageFilter.ColorMonochrome(new Color32(0xff, 0xff, 0x00, 0xff), 1.0f);
				break;
			case 6:
				Log("Applying blue monochrome.");
				_imageFilter.ColorMonochrome(new Color32(0x00, 0x00, 0xff, 0xff), 1.0f);
				break;
		}
		
		// render the image
		_photo = _imageFilter.Render(
			new Rect(0, 0, e.image.size.Width, e.image.size.Height),
			null, CONVERT_SCALE, e.image.imageOrientation.ToCorrectedRotateAngle());
		
		// detect faces
		_faces = _faceDetector.DetectInImage(e.image);
		
		if (_faces.Length > 0) {
			
			_scrambledFaces = new Texture2D[_faces.Length];
			
			for (int i=0; i<_faces.Length; i++) {
				var face = _faces[i];
//				Log("face: " + face.bounds + ", " + face.hasMouthPosition + ", " + face.leftEyePosition + ", " + face.rightEyePosition);
				
				// randomly scramble the faces
				_imageFilter.SetInput(_photo);
				
				switch (random.Next(3)) {
					case 0:
						Log("Pixellating face.");
						_imageFilter.Pixellate(new float[] {0, 0}, 10);
						break;
					case 1:
						Log("Applying blur to face.");
						_imageFilter.GaussianBlur(30);
						break;
					case 2:
						Log("Applying vortex distortion to face.");
						_imageFilter.VortexDistortion(
							new float[] {face.bounds.x * CONVERT_SCALE, face.bounds.y * CONVERT_SCALE},
							3000, 9000
						);
						break;
				}
		
				// render the face only
				_scrambledFaces[i] = _imageFilter.Render(
					new Rect(
						face.bounds.x * CONVERT_SCALE,
						face.bounds.y * CONVERT_SCALE,
						face.bounds.width * CONVERT_SCALE,
						face.bounds.height * CONVERT_SCALE
					));
			}
		}
	}
	
	void OnGUI() {
		
		KitchenSink.OnGUIBack();
		
		if (CoreXT.IsDevice) {
			if (_photo != null) {
				
				// figure out a scale that fits on screen
				float scale = (float)Screen.width / (float)_photo.width;
				float heightScale = ((float)Screen.height - 330.0f) / (float)_photo.height;
				scale = (scale < heightScale) ? scale : heightScale;

				// draw the filtered image
				GUI.DrawTexture(new Rect(0, 280, _photo.width * scale, _photo.height * scale), _photo);
				
				// draw the scrambled faces
				if (_faces != null) {
					for (int i=0; i<_faces.Length; i++) {
						var face = _faces[i];
						var rect = face.bounds;
						rect.Set(rect.x * scale * CONVERT_SCALE, rect.y * scale * CONVERT_SCALE + 280,
							rect.width * scale * CONVERT_SCALE, rect.height * scale * CONVERT_SCALE);
						GUI.DrawTexture(rect, _scrambledFaces[i]);
					}
				}
			}
			
			if (GUI.Button(new Rect(50, 50, 100, 100), "Photos Library")) {
				GUIXT.ShowImagePicker();
			}
			
			if (GUI.Button(new Rect(200, 50, 100, 100), "Take Photo")) {
				GUIXT.ShowImagePicker(UIImagePickerControllerSourceType.Camera);
			}
		}
		
		OnGUILog();
	}


#region Debug logging
	string _log = "Debug log:";
	Vector2 _scrollPosition = Vector2.zero;
	
	void OnGUILog() {
		GUILayout.BeginArea(new Rect(50, 160, Screen.width - 100, 100));
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
