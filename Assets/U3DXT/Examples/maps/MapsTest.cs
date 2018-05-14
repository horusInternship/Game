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
using U3DXT.iOS.Native.MapKit;
using U3DXT.iOS.Native.CoreLocation;

public class MapsTest : MonoBehaviour {
	
	MKMapView mapView;
	
	void Start() {
		if (CoreXT.IsDevice) {
			// subscribes to events
		}
	}
	
	void OnDestroy() {
		if (CoreXT.IsDevice) {
			// unsubscribe to events
			
			if (mapView != null)
				mapView.RemoveFromSuperview();
		}
	}
	
	void OnGUI() {
		
		KitchenSink.OnGUIBack();
		
		if (CoreXT.IsDevice) {
			
			GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, 100));
			GUILayout.BeginHorizontal();

			if (GUILayout.Button("Show Map", GUILayout.ExpandHeight(true))) {
				if (mapView == null) {
					mapView = new MKMapView(new Rect(25, 100, Screen.width/2 - 50, Screen.height/2 - 150));
					
					// create custom annotation view
					mapView.viewHandler = delegate(MKMapView view, MKAnnotation annotation) {
						// try to reuse old view
						var annotationView = mapView.DequeueReusableAnnotationView("annoview");
						if (annotationView == null) {
							Debug.Log("creating new annotation view");
//							annotationView = new MKPinAnnotationView(annotation, "annoview");
							annotationView = new MKAnnotationView(annotation, "annoview");
							annotationView.canShowCallout = true;

							var image = new UIImage(new NSData(new NSURL("http://u3dxt.com/wp-content/uploads/2013/06/gears_14662320_s-225x225.jpg")));
							annotationView.image = image;
						} else {
							Debug.Log("reusing old annotation view");
							annotationView.annotation = annotation;
						}
						
						return annotationView;
					};
				}
				
				UIApplication.deviceRootViewController.view.AddSubview(mapView);
				
				MKCoordinateRegion newRegion = new MKCoordinateRegion();
				newRegion.center.latitude = 37.786996;
				newRegion.center.longitude = -122.440100;
				newRegion.span.latitudeDelta = 0.112872;
				newRegion.span.longitudeDelta = 0.109863;
				
				mapView.SetRegion(newRegion, true);
			}
			
			if (GUILayout.Button("Hide Map", GUILayout.ExpandHeight(true))) {
				mapView.RemoveFromSuperview();
			}
			
			if (GUILayout.Button("Add marker", GUILayout.ExpandHeight(true))) {
				Annotation ggBridge = new Annotation(new CLLocationCoordinate2D(37.810000, -122.477450),
					"Golden Gate Bridge", "Opened: May 27, 1937");
				mapView.AddAnnotation(ggBridge);
			}

			if (GUILayout.Button("Clear marker", GUILayout.ExpandHeight(true))) {
				mapView.RemoveAnnotations(mapView.annotations);
			}

			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		
//		OnGUILog();
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
