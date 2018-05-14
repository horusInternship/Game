using UnityEngine;
using System.Collections;
using U3DXT.iOS.Multipeer;

public class MultipeerAdvertiser : MonoBehaviour {
	
	// Local Peer Name to Show Others
	public string displayName = "Advertiser";
	
	// Must be 1-15 LowerCase ASCII CHARACTERS!
	public string serviceType = "u3dxt-peer";
	
	// Use this for initialization
	void Start () {
		if (displayName == "Advertiser")
			displayName = SystemInfo.deviceName;
		
		MultipeerXT.StartAdvertiserAssistant(displayName, serviceType);
	}
}
