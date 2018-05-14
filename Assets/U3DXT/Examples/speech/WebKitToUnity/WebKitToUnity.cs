using UnityEngine;
using System.Collections;
using U3DXT.iOS.Native.UIKit;
using U3DXT.iOS.Native.Foundation;
using U3DXT.iOS.Speech;

public class WebKitToUnity : MonoBehaviour {
	
	// use pixels, automatically converts to DPI
	public Rect locationAndSize = new Rect(10,50,460,260);
			
	// Use this for initialization
	UIWebView _webview = null;
	WebViewDelegate _webViewDelegate = null;

	void Start () {		
		
		_webview = new UIWebView(locationAndSize);
		UIApplication.SharedApplication().keyWindow.rootViewController.view.AddSubview(_webview);
		
		// SayStuff is an offline html file under the Resources/ folder of Unity
		string html = (Resources.Load("SayStuff") as TextAsset).text;
		Debug.Log ("HTML: " + html);
		_webview.LoadHTMLString(html, null);
		
		
		_webViewDelegate = new WebViewDelegate();
		_webview.Delegate = _webViewDelegate;
		
		
		_webview.backgroundColor = UIColor.ClearColor();
		_webview.opaque = false;
		
	}
	
	
	internal class WebViewDelegate : UIWebViewDelegate
	{
		public override bool ShouldStartLoad (UIWebView webView, NSURLRequest request, UIWebViewNavigationType navigationType)
		{
			Debug.Log ("ShouldStartLoad");
			// handle the special url schema as a way to communicate from the web
			NSURL url = request.URL();
			
			if (url!=null && url.Scheme().Equals("u3dxt"))
			{				object[] paths = url.PathComponents();
				Debug.Log (paths.Length);
				string command = paths[1] as string;
				Debug.Log ("command: " + command);
				if ( command.Equals("say") )
				{
					string phrase = paths[2] as string;
					Debug.Log ("Phrase: " + phrase);
					SpeechXT.Speak(phrase);
				}
				
				// do not actually load this
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}
