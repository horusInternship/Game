using UnityEngine;
using System.Collections;
using DeltaDNA;

public class DeltaDNAManager : MonoBehaviour {

	//DeveloperKey
	string DeltaDNAkey="40228554261038763674597410514435";
	//LiveKey
	//string DeltaDNAkey="40248515546901480966290469114435";


	bool deltadnainit=false;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void DeltaDNA_PostEvent(string dnaevent, int value){
		if(!deltadnainit)
			Deltadna_Initialization();
		EventBuilder eventParams = new EventBuilder()
			.AddParam(dnaevent, value);
		
		DDNA.Instance.RecordEvent(dnaevent, eventParams);
	}

	public void DeltaDNA_PostEvent(string dnaevent, bool value){
		if(!deltadnainit)
			Deltadna_Initialization();

		EventBuilder eventParams = new EventBuilder()
			.AddParam(dnaevent, value);
		
		DDNA.Instance.RecordEvent(dnaevent, eventParams);
	}


	void Deltadna_Initialization(){
		DDNA.Instance.Init(
			DeltaDNAkey,
			"http://collect3371cntrg.deltadna.net/collect/api",
			"http://engage3371cntrg.deltadna.net",
			DDNA.AUTO_GENERATED_USER_ID
			);
	}

}
