using System;
using System.Collections;
using UnityEngine;
using U3DXT.Core;
using U3DXT.iOS.Native.Foundation;
using U3DXT.iOS.Native.UIKit;
using U3DXT.iOS.Native.Internals;
using U3DXT.iOS.IAP;
using U3DXT.Utils;
using System.IO;
using U3DXT.iOS.Native.StoreKit;

public class IAPTest : MonoBehaviour {
	
	// CHANGE THESE IN EDITOR
	public string premiumProductID = "com.vitapoly.gamekittest.premium";
	public string noAdsProductID = "com.vitapoly.gamekittest.noads";
	public string coinsProductID = "com.vitapoly.gamekittest.coins1";
	
	void Start() {
		
		if (CoreXT.IsDevice) {
			// uncomment next line to change the encryption key
			// but if longer than 3 characters, which is 48 bits, check export laws
//			IAPXT.encryptionKey = "my encryption key";
			
			Log("can make payments: " + IAPXT.canMakePayments);
			Log("has bought premium: " + IAPXT.HasBought(premiumProductID));
			Log("has bought no ads: " + IAPXT.HasBought(noAdsProductID));
			Log("has bought coins: " + IAPXT.HasBought(coinsProductID));
			
			// subscribe to events
			IAPXT.InitializationCompleted += OnInitializationCompleted;
			IAPXT.InitializationFailed += OnInitializationFailed;
			
			IAPXT.TransactionCompleted += OnTransactionCompleted;
			IAPXT.TransactionFailed += OnTransactionFailed;
			
			IAPXT.DownloadUpdated += OnDownloadUpdated;
			
			IAPXT.RestorationCompleted += OnRestorationCompleted;
			IAPXT.RestorationFailed += OnRestorationFailed;
			
			// initialize IAPXT with a list of product IDs
			IAPXT.Init(new string[] {premiumProductID, noAdsProductID, coinsProductID});
		}
	}
	
	void OnDestroy() {
		if (CoreXT.IsDevice) {
			// unsubscribe to events
			IAPXT.InitializationCompleted -= OnInitializationCompleted;
			IAPXT.InitializationFailed -= OnInitializationFailed;
			
			IAPXT.TransactionCompleted -= OnTransactionCompleted;
			IAPXT.TransactionFailed -= OnTransactionFailed;
			
			IAPXT.DownloadUpdated -= OnDownloadUpdated;
			
			IAPXT.RestorationCompleted -= OnRestorationCompleted;
			IAPXT.RestorationFailed -= OnRestorationFailed;	
		}
	}

	void OnGUI() {
		
		KitchenSink.OnGUIBack();
		
		if (CoreXT.IsDevice) {

			GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, Screen.height/2 - 50));
			
				GUILayout.Label("MUST first setup iTunesConnect and change IAPTest.cs with correct product IDs.");
				GUILayout.BeginHorizontal();

					if (GUILayout.Button("Buy Coins", GUILayout.ExpandHeight(true))) {
						// buy 2 coins
						IAPXT.Buy(coinsProductID, 2);
					}
			
					if (GUILayout.Button("Premium", GUILayout.ExpandHeight(true))) {
						// buy premium upgrade
						IAPXT.Buy(premiumProductID);
					}
				
					if (GUILayout.Button("No Ads", GUILayout.ExpandHeight(true))) {
						// buy no ads upgrade
						IAPXT.Buy(noAdsProductID);
					}
			
					if (GUILayout.Button("Restore", GUILayout.ExpandHeight(true))) {
						// restore all previously bought products
						IAPXT.RestoreCompletedTransactions();
					}
			
					if (GUILayout.Button("Store View", GUILayout.ExpandHeight(true))) {
						// show the store view in game
						IAPXT.ShowStore("571059586");
					}

				GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		
		OnGUILog();
	}
	
	void OnInitializationCompleted(object sender, InitializationEventArgs e) {
		Log("InitializationCompleted");
		
		SKProduct product = IAPXT.GetProduct(premiumProductID);
		
		if ( product == null )
		{
			Log ("Error: Product not defined: " + premiumProductID);
			return;
		}
		
		Log("title: " + product.localizedTitle);
		Log("description: " + product.localizedDescription);
		var price = product.price;
		Log("price type: " + price.GetType());
		Log("price: " + price);
		Log("localized price: " + product.localizedPrice);
	}
	
	void OnInitializationFailed(object sender, InitializationEventArgs e) {
		Log("InitializationFailed: " + Json.Serialize(e.invalidIDs));
	}
	
	void OnTransactionCompleted(object sender, TransactionEventArgs e) {
		Log("TransactionCompleted: " + e.productID + ", " + e.quantity);
		
		if (e.hasDownloads) {
			var srcFile = Application.persistentDataPath + "/downloads/" + e.productID;
			Log("has downloads at: " + srcFile);
			PrintFile(srcFile, srcFile);
		}
	}
	
	void OnTransactionFailed(object sender, TransactionEventArgs e) {
		Log("TransactionFailed: " + e.productID + ", " + e.quantity + ", " + e.error.LocalizedDescription());
	}
	
	void OnDownloadUpdated(object sender, DownloadEventArgs e) {
		Log("DownloadUpdated: " + e.downloads[0].downloadState + ", " + e.downloads[0].progress);
	}
	
	void OnRestorationCompleted(object sender, EventArgs e) {
		Log("RestorationCompleted");
	}
	
	void OnRestorationFailed(object sender, U3DXTErrorEventArgs e) {
		Log("RestorationFailed: " + e.domain + ", " + e.code + ", " + e.description);
	}
	
	void PrintFile(string filePath, string basePath) {
		try {
			if (Directory.Exists(filePath)) {
				Log("D\t" + MakeRelativePath(basePath, filePath));
				foreach (var child in Directory.GetFiles(filePath)) {
					PrintFile(child, basePath);
				}
			} else {
				var file = new FileInfo(filePath);
				Log(file.Length + "\t" + MakeRelativePath(basePath, filePath));
			}
		} catch (Exception e) {
			Debug.Log("Exception: " + e);
		}
	}
	
	public static String MakeRelativePath(String fromPath, String toPath)
    {
        Uri fromUri = new Uri(fromPath);
        Uri toUri = new Uri(toPath);

        Uri relativeUri = fromUri.MakeRelativeUri(toUri);
        String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

        return relativePath.Replace('/', Path.DirectorySeparatorChar);
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
