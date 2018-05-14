using UnityEngine;
using System.Collections;
using U3DXT.iOS.Native.UIKit;
using U3DXT.iOS.Native.Foundation;

public class WebViewGameObject : MonoBehaviour {

	public string homeURL = "http://u3d.as/content/vitapoly-inc/i-os-sdk-native-api-access-from-c-javascript-and-boo/50g";

	// use pixels, automatically converts to DPI
	public Rect locationAndSize = new Rect(10,50,460,260);
		
	// you probabably don't want this unless you want to0 create your own UI with UIKit. Unity will be completely covered.
	private bool fullScreen = false; 
	
	public bool showNavigationButtons = true;
	
	// Use this for initialization
	UIWebView _webview = null;

	void Start () {		
		// delay load to not add it to splash screen
		Invoke("Load", 1f);
	}
	
	public void Load() {
		if (_webview == null){ 
			// figure out parent
			UIView parentView = null;
			if ((UIApplication.deviceRootViewController != null)
			    && (UIApplication.deviceRootViewController.view != null))
				parentView = UIApplication.deviceRootViewController.view;
			else
				parentView = UIApplication.SharedApplication().keyWindow;
			
			// create view
			if ( fullScreen )
				_webview = new UIWebView(parentView.bounds);
			else
				_webview = new UIWebView(locationAndSize);

			// add it to parent
			parentView.AddSubview(_webview);

			this.setURL(homeURL);
		}
	}
	
	UIWebView getWebView()
	{
		return _webview;	
	}
	
	void OnGUI() {
		if ( !showNavigationButtons )
			return;
		
		GUILayout.BeginArea(new Rect(50, 25, Screen.width - 100, 50));
			GUILayout.BeginHorizontal();
				OnGUIBack();
				OnGUIForward();
				OnGUIHome();
				OnGUISearch();
				OnHideShow();
			GUILayout.EndHorizontal();
		GUILayout.EndArea();
	
	}
	
	
	void OnHideShow()
	{
		if (GUILayout.Button("Hide/Show", GUILayout.ExpandHeight(true))) {	
			lock(_webview){
				_webview.hidden = !_webview.hidden;
			}
		}
	}
	
	void OnGUIBack()
	{
		if (GUILayout.Button("Back", GUILayout.ExpandHeight(true))) {
			lock(_webview){
				_webview.GoBack();
			}
		}		
	}
	
	void OnGUIHome()
	{
		if (GUILayout.Button("Home", GUILayout.ExpandHeight(true))) {
	
				this.setURL("http://u3d.as/content/vitapoly-inc/i-os-sdk-native-api-access-from-c-javascript-and-boo/50g");
			
		}	
	}
	
	void OnGUISearch()
	{
		if (GUILayout.Button("Search", GUILayout.ExpandHeight(true))) {
			
			this.setURL("http://www.google.com");
			
		}	
	}
	
	void OnGUIForward()
	{
		if (GUILayout.Button("Forward", GUILayout.ExpandHeight(true))) {
			lock(_webview){
				_webview.Go();
			}
			
		}	
	}
	
	
	// NSURLRequest and NSURL must be a member variable otherwise you will get a crash
	// when mono decides to delete it before webview is done with it.
	NSURLRequest _request = null;
	NSURL _url = null;
	public void setURL(string url)
	{
		lock(_webview){
			_url = new NSURL(url);
			_request =  new NSURLRequest( _url );
			_webview.LoadRequest(_request);
		}
	}
	
	void OnDestroy()
	{
		// load a blank page before removing it
		_webview.StopLoading();
		_webview.LoadHTMLString("", new NSURL(""));
		
		_webview.RemoveFromSuperview();
//		_webview.hidden = true;
		_webview = null;
		_request = null;
		_url = null;
	}
		
	
	
	
	// Update is called once per frame
//	void Update () {
//	
//	}
	
}
