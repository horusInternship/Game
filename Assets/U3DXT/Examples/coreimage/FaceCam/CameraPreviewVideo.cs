using System;
using UnityEngine;
using U3DXT.Core;
using U3DXT.iOS.Native.UIKit;
using U3DXT.iOS.Native.Foundation;
using U3DXT.iOS.CoreImage;

public class CameraPreviewVideo : MonoBehaviour {
	internal WebCamTexture webCamTexture;
	
	public bool useFrontCamera = true;

	public bool isMirrored {
		get;
		private set;
	}
	
	public int requestedCameraFPS = 15;
	public int requestedCameraWidth = -1;
	public int requestedCameraHeight = -1;
	
	public Rect projectedRect = new Rect(0, 0, -1, -1);
	
	private bool _usingFrontCamera;
	
	public bool autoPlay = true;
	
	private float _rotateAngle = 0;
	private Vector2 _pivot;
	private Rect _drawRect;
	private int _cameraWidth;
	private int _cameraHeight;
	
	private bool _needScreenChecking = false;
//	private int _restartCount = 0;
	private DeviceOrientation _oldOrientation;
	private int _oldScreenWidth;
	
	void Awake() {
		if (CoreXT.IsDevice) {
			UIDevice.CurrentDevice().OrientationDidChange += _OnOrientationChanged;

			if (autoPlay)
				Play();
		}
	}
	
	void OnDestroy() {
		if (CoreXT.IsDevice) {
			UIDevice.CurrentDevice().OrientationDidChange -= _OnOrientationChanged;
			
			if (webCamTexture != null)
				WebCamTexture.Destroy(webCamTexture);
		}
	}
	
	private void _OnOrientationChanged(object sender, NSNotificationEventArgs e) {
		if (isPlaying) {
			
			DeviceOrientation newOrientation = Input.deviceOrientation;
			if (newOrientation == _oldOrientation)
				return;
			
			// check whether to reinit draw data now or later
			switch (newOrientation) {
				case DeviceOrientation.Portrait:
				case DeviceOrientation.PortraitUpsideDown:
					if ((_oldOrientation == DeviceOrientation.LandscapeLeft) || (_oldOrientation == DeviceOrientation.LandscapeRight))
						_needScreenChecking = true;
					else
						_InitDrawData();
					break;
				case DeviceOrientation.LandscapeRight:
				case DeviceOrientation.LandscapeLeft:
					if ((_oldOrientation == DeviceOrientation.Portrait) || (_oldOrientation == DeviceOrientation.PortraitUpsideDown))
						_needScreenChecking = true;
					else
						_InitDrawData();
					break;
				default:
					break;
			}
		}
	}
	
	public void Play() {
		if (isPlaying)
			return;
		
		// if changing cameras
		if ((_usingFrontCamera != useFrontCamera) && (webCamTexture != null)) {
			WebCamTexture.Destroy(webCamTexture);
			webCamTexture = null;
		}
		
		_usingFrontCamera = useFrontCamera;
		
		// Unity 3 and 4 has flipped camera textures
		isMirrored = Application.unityVersion.StartsWith("4") ? useFrontCamera : !useFrontCamera;
		
		_InitDrawData();
		
		if (webCamTexture == null) {
			webCamTexture = new WebCamTexture((useFrontCamera ? "Front Camera" : "Back Camera"));
		}

		webCamTexture.requestedWidth = _cameraWidth;
		webCamTexture.requestedHeight = _cameraHeight;
		webCamTexture.requestedFPS = requestedCameraFPS;

		webCamTexture.Play();
	}
	
	private void _InitDrawData() {
		_oldOrientation = Input.deviceOrientation;
		switch (_oldOrientation) {
			case DeviceOrientation.Portrait:
				_rotateAngle = 90;
				break;
			case DeviceOrientation.PortraitUpsideDown:
				_rotateAngle = 270;
				break;
			case DeviceOrientation.LandscapeRight:
				if (!isMirrored)
					_rotateAngle = 180;
				else
					_rotateAngle = 0;
				break;
			case DeviceOrientation.LandscapeLeft:
			default:
				if (!isMirrored)
					_rotateAngle = 0;
				else
					_rotateAngle = 180;
				_oldOrientation = DeviceOrientation.LandscapeLeft;
				break;
		}
		
		_oldScreenWidth = Screen.width;
		
		_cameraWidth = requestedCameraWidth;
		if (_cameraWidth == -1)
			_cameraWidth = Screen.width;
		_cameraHeight = requestedCameraHeight;
		if (_cameraHeight == -1)
			_cameraHeight = Screen.height;
		
		// always get the wider ratio for camera
		if (_cameraWidth < _cameraHeight) {
			int temp = _cameraWidth;
			_cameraWidth = _cameraHeight;
			_cameraHeight = temp;
		}
		
		float projectedWidth = projectedRect.width;
		if (projectedWidth == -1)
			projectedWidth = Screen.width;
		float projectedHeight = projectedRect.height;
		if (projectedHeight == -1)
			projectedHeight = Screen.height;
		
		_drawRect = new Rect(projectedRect.x, projectedRect.y, projectedWidth, projectedHeight);
	    _pivot = new Vector2(_drawRect.xMin + _drawRect.width * 0.5f, _drawRect.yMin + _drawRect.height * 0.5f);
		
		Debug.Log("Orientation: " + _oldOrientation
			+ "\ndetector orientation: " + cameraOrientationForFaceDetector
			+ "\nscreen size: " + Screen.width + "x" + Screen.height
			+ "\nprojected size: " + projectedWidth + "x" + projectedHeight
			+ "\nCamera requested size: " + _cameraWidth + "x" + _cameraHeight
			+ "\nDraw rect: " + _drawRect
		);
	}

	public void Stop() {
		if (webCamTexture != null)
			webCamTexture.Pause();
	}
	
	public bool isPlaying {
		get { return ((webCamTexture != null) && webCamTexture.isPlaying); }
	}
	
	public float videoToCameraScale {
		get {
			return (webCamTexture == null) ? 1f
				: ((((_rotateAngle == 90) || (_rotateAngle == 270)) ? _drawRect.height : _drawRect.width) / webCamTexture.width);
		}
	}
	
	public CGImageOrientation cameraOrientationForFaceDetector {
		get {
			if (!isMirrored) {
				switch (_oldOrientation) {
					case DeviceOrientation.Portrait:
						return CGImageOrientation.RotatedLeft;
					case DeviceOrientation.PortraitUpsideDown:
						return CGImageOrientation.RotatedRight;
					case DeviceOrientation.LandscapeRight:
						return CGImageOrientation.UpsideDown;
					default:
						return CGImageOrientation.Default;
				}
			} else {
				switch (_oldOrientation) {
					case DeviceOrientation.Portrait:
						return CGImageOrientation.RotatedLeft;
					case DeviceOrientation.PortraitUpsideDown:
						return CGImageOrientation.RotatedRight;
					case DeviceOrientation.LandscapeRight:
						return CGImageOrientation.Default;
					default:
						return CGImageOrientation.UpsideDown;
				}
			}
		}
	}
	
	void OnGUI() {
		if ((webCamTexture != null) && webCamTexture.isPlaying) {
			
			// need to let unity finish rotating before reiniting draw data
			if (_needScreenChecking) {
				if (Screen.width != _oldScreenWidth) {
					_needScreenChecking = false;
					_InitDrawData();
				}
			}
			
		    Matrix4x4 matrixBackup = UnityEngine.GUI.matrix;
			
			if (_rotateAngle != 0)
			    GUIUtility.RotateAroundPivot(_rotateAngle, _pivot);
			
			// scale width and height if rotated by 90 degree either direction because camera always feeds in wider ratio
			if ((_rotateAngle == 90) || (_rotateAngle == 270)) {
				GUIUtility.ScaleAroundPivot(
					new Vector2(_drawRect.width/_drawRect.height, _drawRect.height/_drawRect.width),
					_pivot);
			}
			
			if (isMirrored)
				GUIUtility.ScaleAroundPivot(new Vector2(-1,1), _pivot);
			
		    UnityEngine.GUI.DrawTexture(_drawRect, webCamTexture);
			
		    UnityEngine.GUI.matrix = matrixBackup;
		}
	}
}
