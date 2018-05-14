using UnityEngine;
using System.Collections;
using U3DXT.iOS.MediaPlayer;

public class StreamingIntroMovie : MonoBehaviour {
	
	public string URL = "http://devimages.apple.com/iphone/samples/bipbop/gear1/prog_index.m3u8";
	public bool autoPlay = true;
	public ScreenOrientation screenOrientation = ScreenOrientation.Landscape;
	
	void Start() {
		Screen.orientation = screenOrientation;
		
		StartCoroutine(WaitForRotation());

		MediaPlayerXT.PlayStreamingFullscreenMovie(URL, autoPlay);		
	}
	
	IEnumerator WaitForRotation()
	{
		yield return new WaitForEndOfFrame();

	}
	
	
}
