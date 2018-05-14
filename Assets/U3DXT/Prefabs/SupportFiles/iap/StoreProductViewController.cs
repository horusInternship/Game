using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using U3DXT.iOS.Native.StoreKit;
using U3DXT.iOS.Native.Foundation;
using U3DXT.iOS.Native.CoreFoundation;
using U3DXT.iOS.Native.UIKit;
using U3DXT.Core;

public class StoreProductViewController : MonoBehaviour {

	/// <summary>
	/// Replace with artistId or trackId using this API: 
	/// http://itunes.apple.com/lookup?bundleId=YOUR.APPS.BUNDLEID
	/// </summary>
	public int ItemIdentifier = 419568227;	
	
	private SKStoreProductViewController _productViewController;
	private StoreProductViewControllerDelegate _productViewControllerDelegate;
	private Dictionary<object, object> _product;
	
	void Start () {
		
		if ( !CoreXT.IsDevice )
		{
			Debug.Log ("StoreProductViewController is only available on the device.");
			return;
		}
		
		_productViewControllerDelegate = new StoreProductViewControllerDelegate();
		_productViewController = new SKStoreProductViewController();
		_productViewController.Delegate = _productViewControllerDelegate;
	
		LoadProduct(ItemIdentifier);
	}
	
	/// <summary>
	/// Loads the product based on identifier.
	/// Use this if you want to programmatically change the product to load
	/// By default, Show() will show what's defined in the Unity Editor
	/// </summary>
	/// <param name='itemIdentifier'>
	/// Item identifier, either AppID or ArtistID
	/// </param>
	public void LoadProduct(int itemIdentifier)
	{
		if ( !CoreXT.IsDevice ) return;

		if ( _product == null ){
			_product = new Dictionary<object, object>();
			_product.Add(
				SKStoreProductViewController.SKStoreProductParameterITunesItemIdentifier,
				ItemIdentifier
			);
		}
		else{
			_product[SKStoreProductViewController.SKStoreProductParameterITunesItemIdentifier] = itemIdentifier;
		}
		
		_productViewController.LoadProduct(_product,
			delegate(bool result, NSError error){
				Debug.Log ("StoreProductView Load Product " + ItemIdentifier + ", Successful: " + result);
			}
		);
	}
	
	/// <summary>
	/// Show this instance.
	/// </summary>
	public void Show()
	{
		if ( !CoreXT.IsDevice ) return;
		
		UIApplication.deviceRootViewController.PresentViewController(_productViewController, true, null);		
	}

	
	/// <summary>
	/// Store product view controller delegate.
	/// </summary>
	internal class StoreProductViewControllerDelegate : SKStoreProductViewControllerDelegate
	{
		public StoreProductViewControllerDelegate(){}
		
		public override void DidFinish(SKStoreProductViewController viewController)
		{
			viewController.DismissViewController(true, null);
		}	
	}
}
