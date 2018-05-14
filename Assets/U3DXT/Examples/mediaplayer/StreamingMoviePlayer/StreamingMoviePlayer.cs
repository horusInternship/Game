using UnityEngine;
using System.Collections;
using System.IO;
using U3DXT.Core;
using U3DXT.iOS.MediaPlayer;
using U3DXT.iOS.Native.MediaPlayer;
using U3DXT.iOS.Native.Foundation;
using U3DXT.Utils;

public class StreamingMoviePlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (CoreXT.IsDevice) {
			MediaExporter.ExportCompleted += OnExportCompleted;
			MediaExporter.ExportFailed += OnExportFailed;
		}
	}
	
	void OnDestroy() {
		if (CoreXT.IsDevice) {
			MediaExporter.ExportCompleted -= OnExportCompleted;
			MediaExporter.ExportFailed -= OnExportFailed;
		}
	}
	
	void OnExportCompleted(object sender, MediaExportedEventArgs e) {
		FileInfo fileInfo = new FileInfo(e.outputURL);
		Log("Exported to: " + e.outputURL);
		Log("File size: " + fileInfo.Length);
	}
	
	void OnExportFailed(object sender, U3DXTErrorEventArgs e) {
		Log("Export error: (" + e.domain + ": " + e.code + ") " + e.description);	
	}
	
	void OnGUI() 
	{
		KitchenSink.OnGUIBack();
		
		if (CoreXT.IsDevice) {
			GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, Screen.height/2 - 50));
				GUILayout.BeginVertical();
					if (GUILayout.Button("Stream 480p", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true))) {	
						MediaPlayerXT.PlayStreamingFullscreenMovie("http://trailers.apple.com/movies/focus_features/9/9-clip_480p.mov");
					}
					if (GUILayout.Button("Stream 720p", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true))) {
						MediaPlayerXT.PlayStreamingFullscreenMovie("http://trailers.apple.com/movies/focus_features/9/9-clip_720p.mov");
					}
					if (GUILayout.Button("Stream 1080p", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true))) {	
						MediaPlayerXT.PlayStreamingFullscreenMovie("http://trailers.apple.com/movies/focus_features/9/9-clip_1080p.mov");
					}
			
					if (GUILayout.Button("Export First Song in Music Library to Storage", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true))) {	
						ExportFirstSong();
					}
	
				GUILayout.EndVertical();
			GUILayout.EndArea();
		}
		
		OnGUILog();
	}
	
	void ExportFirstSong() {
		MPMediaQuery query = MPMediaQuery.SongsQuery();
		
		if (query.items.Length == 0) {
			Log("No songs in Music Library.");
			return;
		}
		
		// get first song
		MPMediaItem mediaItem = query.items[0] as MPMediaItem;
		
		Log("Exporting song: " + mediaItem.Value(MPMediaItem.PropertyTitle));

		// use null for outputFolder and outputFile to name it [artist] - [title]
		if (!MediaExporter.ExportAudio(mediaItem, null, null, true))
			Log("Export error or song has DRM.");
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
