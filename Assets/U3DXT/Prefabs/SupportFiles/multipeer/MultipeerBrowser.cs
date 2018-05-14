using UnityEngine;
using System.Collections;
using U3DXT.iOS.Multipeer;

public class MultipeerBrowser : MonoBehaviour {
	
	// Local Peer Name to Show Others
	public string displayName = "Browser";
	
	// Must be 1-15 LowerCase ASCII CHARACTERS!
	public string serviceType = "u3dxt-peer";


	// Use this for initialization
	void Start () {
		if (displayName == "Browser")
			displayName = SystemInfo.deviceName;
		
		MultipeerXT.ShowBrowser(displayName, serviceType);
	}
		
}
