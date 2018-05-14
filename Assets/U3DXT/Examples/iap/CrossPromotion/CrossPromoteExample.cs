using UnityEngine;
using System.Collections;

public class CrossPromoteExample : MonoBehaviour {
	
	/// <summary>
	/// The store product view controller. Be sure to assign it
	/// from the Unity interface
	/// </summary>
	public StoreProductViewController storeProductViewController = null;
	
	void OnGUI()
	{
		
		GUILayout.BeginArea(new Rect(50, 50, Screen.width-100, Screen.height-100));
		GUILayout.BeginVertical();
			GUILayout.Label("Cross Promotion Example");
			if (GUILayout.Button("Show Default Store Product View", GUILayout.ExpandWidth(true), GUILayout.Height(100))) {
				storeProductViewController.Show();
			}
			else if (GUILayout.Button("Show DeusEx:The Fall", GUILayout.ExpandWidth(true), GUILayout.Height(100))) {
				storeProductViewController.LoadProduct(633443676);
				storeProductViewController.Show();
			}
			else if (GUILayout.Button("Show Year Walk Companion", GUILayout.ExpandWidth(true), GUILayout.Height(100))) {
				storeProductViewController.LoadProduct(597879895);
				storeProductViewController.Show();
			}			
			else if (GUILayout.Button("Show All Games from Simogo", GUILayout.ExpandWidth(true), GUILayout.Height(100))) {
				storeProductViewController.LoadProduct(404446783);
				storeProductViewController.Show();
			}
		GUILayout.EndVertical();	
		GUILayout.EndArea();		
	}
	
}
