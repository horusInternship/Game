using System;
using System.Collections;
using UnityEngine;
using U3DXT.Core;
using U3DXT.iOS.Social;
using U3DXT.iOS.Native.Foundation;
using U3DXT.iOS.Native.UIKit;
using U3DXT.iOS.Native.Social;
using U3DXT.iOS.UserMedia;
using U3DXT.Utils;

public class SocialTest : MonoBehaviour {
	
	Texture2D _logo;
	Twitter _twitter;
	Facebook _facebook;
	SinaWeibo _sinaWeibo;
	
	//TODO: replace with your app's Facebook App ID; see your Facebook Developer page
	private string facebookAppID = "1234567890";
	
	void Start() {
	
		_logo = GameObject.Find("Logo").GetComponent<GUITexture>().texture as Texture2D;

		if (CoreXT.IsDevice) {
			// subscribe to events
			SocialXT.ShareCompleted += OnShareCompleted;
			SocialXT.MailCompleted += OnMailCompleted;
			SocialXT.SMSCompleted += OnSMSCompleted;
			SocialXT.PostCompleted += OnPostCompleted;
		
			CreateDirectServices();
			
			PhotosLibrary.ExportCompleted += OnExported;
			PhotosLibrary.ExportFailed += OnExportFailed;
		}
	}
	
	void OnDestroy() {
		if (CoreXT.IsDevice) {
			// unsubscribe to events
			SocialXT.ShareCompleted -= OnShareCompleted;
			SocialXT.MailCompleted -= OnMailCompleted;
			SocialXT.SMSCompleted -= OnSMSCompleted;
			SocialXT.PostCompleted -= OnPostCompleted;

			_twitter.InitializationCompleted -= OnDirectServiceInit;
			_twitter.InitializationFailed -= OnDirectServiceInitFailed;
			_sinaWeibo.InitializationCompleted -= OnDirectServiceInit;
			_sinaWeibo.InitializationFailed -= OnDirectServiceInitFailed;
			_facebook.InitializationCompleted -= OnDirectServiceInit;
			_facebook.InitializationFailed -= OnDirectServiceInitFailed;

			PhotosLibrary.ExportCompleted -= OnExported;
			PhotosLibrary.ExportFailed -= OnExportFailed;
		}
	}
	
	void CreateDirectServices() {
		
		// create direct services to social networks
		_twitter = new Twitter();
		_twitter.InitializationCompleted += OnDirectServiceInit;
		_twitter.InitializationFailed += OnDirectServiceInitFailed;
		_twitter.Init();
		
		_sinaWeibo = new SinaWeibo();
		_sinaWeibo.InitializationCompleted += OnDirectServiceInit;
		_sinaWeibo.InitializationFailed += OnDirectServiceInitFailed;
		_sinaWeibo.Init();

		// init facebook with your app ID and an array of permissions
		_facebook = new Facebook();
		_facebook.InitializationCompleted += OnDirectServiceInit;
		_facebook.InitializationFailed += OnDirectServiceInitFailed;
		_facebook.Init(facebookAppID, new string[] {"read_stream", "email", "publish_stream"});
	}
	
	void OnDirectServiceInit(object sender, EventArgs e) {
		DirectRequestService service = sender as DirectRequestService;
		Log("Direct service init'd:"
			+ "\n\tType: " + service.accountType.accountTypeDescription
			+ "\n\tUsername: " + service.account.username);
		if (service.account.credential != null)
			Log("\tOAuth token: " + service.account.credential.oauthToken);
	}
	
	void OnDirectServiceInitFailed(object sender, U3DXTErrorEventArgs e) {
		DirectRequestService service = sender as DirectRequestService;
		Log("Direct service init failed: "
			+ "\n\tType: " + service.accountType.accountTypeDescription
			+ "\n\tError: " + e.description);
	}

	void OnShareCompleted(object sender, ShareCompletedEventArgs e) {
		Log("Share to " + e.activityType + " completed: " + e.completed);
	}
	
	void OnMailCompleted(object sender, MailCompletedEventArgs e) {
		Log("Mail result: " + e.result + " error: " + e.error);
	}
	
	void OnSMSCompleted(object sender, SMSCompletedEventArgs e) {
		Log("SMS result: " + e.result);
	}
	
	void OnPostCompleted(object sender, PostCompletedEventArgs e) {
		Log("Post completed: " + e.completed);
	}
	
	void OnExported(object sender, PhotosLibraryExportedEventArgs e) {
		Log("exported to: " + e.assetURL);
	}
	
	void OnExportFailed(object sender, U3DXTErrorEventArgs e) {
		Log("export failed: " + e.description);
	}
	
	void OnGUI() {
		
		KitchenSink.OnGUIBack();
		
		if (CoreXT.IsDevice) {
			
			GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, Screen.height/2 - 50));
				GUILayout.BeginHorizontal();
					OnGUIActivitySheet();
					OnGUIEmail();
					OnGUISMS();
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
					OnGUIPost();
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
					OnGUIDirectTwitter();
					OnGUIDirectFacebook();
					OnGUIDirectSinaWeibo();
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
					OnGUISaveToPhotoLibrary();
					OnGUIInstagram();
				GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
		
		OnGUILog();
	}
	
	void OnGUIActivitySheet() {
		if (GUILayout.Button("Activity Sheet", GUILayout.ExpandHeight(true))) {
			SocialXT.Share(new object[] {
				"I am using @vitapoly's awesome U3DXT #Unity3D Plugin. U3DXT.COM #gamedev",
				"http://www.U3DXT.com",
				_logo
			},
			new string[] { UIActivity.TypeAssignToContact, UIActivity.TypeCopyToPasteboard });
		}
	}
	
	void OnGUIEmail() {
		if (GUILayout.Button("Email", GUILayout.ExpandHeight(true))) {
			SocialXT.Mail(new string[] { "support@U3DXT.com" },
				"U3DXT",
				"I am using your Unity plugin to send this email. Cool.",
				true,
				null as UIImage,
				false
			);
		}
	}

	void OnGUISMS() {
		if (GUILayout.Button("SMS", GUILayout.ExpandHeight(true))) {
			SocialXT.SMS(new string[] { "15555555555" }, "hello");
		}
	}

	void OnGUIPost() {
		if (GUILayout.Button("Twitter Post", GUILayout.ExpandHeight(true))) {
			SocialXT.Post(SLRequest.SLServiceTypeTwitter,
				"I am using @vitapoly's awesome U3DXT #Unity3D Plugin. U3DXT.COM #gamedev",
				_logo,
				"http://www.U3DXT.com"
			);
		}
		
		if (GUILayout.Button("Facebook Post", GUILayout.ExpandHeight(true))) {
			SocialXT.Post(SLRequest.SLServiceTypeFacebook,
				"Super fun games from vitapoly!",
				null,
				"http://www.vitapoly.com"
			);
		}
		
		if (GUILayout.Button("Sina Weibo Post", GUILayout.ExpandHeight(true))) {
			SocialXT.Post(SLRequest.SLServiceTypeSinaWeibo,
				"My first weibo from U3DXT Social by vitapoly.",
				_logo,
				"http://www.U3DXT.com"
			);
		}
	}
	
	void OnGUIDirectTwitter() {
		if (GUILayout.Button("Twitter API", GUILayout.ExpandHeight(true))) {

			// get a list of tweets
			// when done, it calls the callback function with a JSON object
			_twitter.GetTweets(delegate(object obj) {
				Log("tweets: " + Json.Serialize(obj));
			});
			
			// update status
			_twitter.Update("I am using @vitapoly's awesome U3DXT #Unity3D Plugin. http://www.U3DXT.com", delegate(object obj) {
				Log("update: " + Json.Serialize(obj));
			});
			
			// search for tweets
			_twitter.SearchTweet("vitapoly", delegate(object obj) {
				Log("tweets containing vitapoly: " + Json.Serialize(obj));
			});
			
			// you can also query twitter directly if a functionality is not built-in to the Twitter class
			// get a list of suggested users
			// supply a callback function with more arguments
			_twitter.GetFromURL("https://api.twitter.com/1.1/users/suggestions.json",
				delegate(object obj, NSHTTPURLResponse urlResponse, NSError error) {
					Log("suggested users: " + Json.Serialize(obj));
				}
			);
		}
	}
	
	void OnGUIDirectFacebook() {
		if (GUILayout.Button("Facebook API\nMUST set Facebook app ID\nin SocialTest.cs", GUILayout.ExpandHeight(true))) {
			
			// get the user's feed
			// when done, it calls the callback function with a JSON object
			_facebook.GetFeed(delegate(object obj) {
				Log("feed: " + Json.Serialize(obj));
			});

			// get info about current user
			_facebook.GetUser(delegate(object obj) {
				Log("user: " + Json.Serialize(obj));
			});

			// get the user's friends
			_facebook.GetFriends(delegate(object obj) {
				Log("friends: " + Json.Serialize(obj));
			});

			// post a link
			_facebook.PostLink(
				"You can find the best Unity Plugins at www.U3DXT.com!",
				"http://www.U3DXT.com",
				delegate(object obj) {
					Log("post: " + Json.Serialize(obj));
				}
			);

			// you can also query the Facebook Graph API directly if a functionality is not built-in to the Facebook class
			// get feed from vitapoly with query strings
			// supply a callback function with more arguments
			_facebook.GetFromURL("https://graph.facebook.com/vitapoly/feed?fields=message,name&limit=10",
				delegate(object obj, NSHTTPURLResponse urlResponse, NSError error) {
					Log("vitapoly: " + Json.Serialize(obj));
				}
			);
		}
	}
	
	void OnGUIDirectSinaWeibo() {
		if (GUILayout.Button("Sina Weibo API", GUILayout.ExpandHeight(true))) {
			// get the user's timeline
			// when done, it calls the callback function with a JSON object
			_sinaWeibo.GetHomeTimeline(delegate(object obj) {
				Log("timeline: " + Json.Serialize(obj));
			});

			// get a list of favorites
			_sinaWeibo.GetFavorites(delegate(object obj) {
				Log("favorites: " + Json.Serialize(obj));
			});
			
			// post a weibo
			_sinaWeibo.PostWeibo("Hello from SocialXT Unity Plugin by vitapoly", delegate(object obj) {
				Log("post: " + Json.Serialize(obj));
			});

			// you can also query the Weibo API directly if a functionality is not built-in to the Weibo class
			// get a list of hot users
			// supply a callback function with more arguments
			_sinaWeibo.GetFromURL("https://api.weibo.com/2/suggestions/users/hot.json",
				delegate(object obj, NSHTTPURLResponse urlResponse, NSError error) {
					Log("hot users: " + Json.Serialize(obj));
				}
			);
		}
	}
	
	void OnGUISaveToPhotoLibrary() {
		if (GUILayout.Button("Save to Photo Library as PNG", GUILayout.ExpandHeight(true))) {
			PhotosLibrary.ExportPNG(_logo);
		}

		if (GUILayout.Button("Save to Photo Library as JPEG", GUILayout.ExpandHeight(true))) {
			PhotosLibrary.ExportJPEG(_logo, 0.80f);
		}
	}
	
	void OnGUIInstagram() {
		if (GUILayout.Button("Instagram", GUILayout.ExpandHeight(true))) {
			if (!SocialXT.Instagram(_logo, "Sharing U3DXT on Instagram."))
				Log("Instagram is not installed.");
		}
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

