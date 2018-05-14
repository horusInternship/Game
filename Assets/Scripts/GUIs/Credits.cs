using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Credits : MonoBehaviour 
{

	//todoJoao Change Limit of Credits


	public GameObject HGE_Logo,Fund_Logo;
	bool Creditsrunning=false;

	float offsetymin=0, offsetymax=0,offsethgeymin=0, offsethgeymax=0,offsetfundymin=0, offsetfundymax=0,startymin,startymax,starthgeymin=5600,starthgeymax=5600,startfundymin=875,startfundymax=875;
	
	void Start(){
		startymin=this.GetComponent<RectTransform>().offsetMin.y;
		startymax=this.GetComponent<RectTransform>().offsetMax.y;
		/*
		starthgeymin=HGE_Logo.GetComponent<RectTransform>().offsetMin.y;
		starthgeymax=HGE_Logo.GetComponent<RectTransform>().offsetMax.y;
		*/
	}

	void Update () {
		
		if(Creditsrunning)
		{
			//Debug.Log (HGE_Logo.GetComponent<RectTransform>().offsetMin.y);
			//Debug.Log (HGE_Logo.GetComponent<RectTransform>().offsetMax.y);


			 
			if(this.GetComponent<RectTransform>().offsetMin.y>-6400)
			{
				offsetymin=this.GetComponent<RectTransform>().offsetMin.y;
				offsetymin-=2;

				offsetymax=this.GetComponent<RectTransform>().offsetMax.y;
				offsetymax+=2;

				this.GetComponent<RectTransform>().offsetMin = new Vector2(this.GetComponent<RectTransform>().offsetMin.x, offsetymin);
				this.GetComponent<RectTransform>().offsetMax = new Vector2(this.GetComponent<RectTransform>().offsetMax.x, offsetymax);


				offsethgeymin=HGE_Logo.GetComponent<RectTransform>().offsetMin.y;
				offsethgeymin+=2;
				
				offsethgeymax=HGE_Logo.GetComponent<RectTransform>().offsetMax.y;
				offsethgeymax+=2;
				
				HGE_Logo.GetComponent<RectTransform>().offsetMin = new Vector2(HGE_Logo.GetComponent<RectTransform>().offsetMin.x, offsethgeymin);
				HGE_Logo.GetComponent<RectTransform>().offsetMax = new Vector2(HGE_Logo.GetComponent<RectTransform>().offsetMax.x, offsethgeymax);

				offsetfundymin=Fund_Logo.GetComponent<RectTransform>().offsetMin.y;
				offsetfundymin+=2;
				
				offsetfundymax=Fund_Logo.GetComponent<RectTransform>().offsetMax.y;
				offsetfundymax+=2;
				
				Fund_Logo.GetComponent<RectTransform>().offsetMin = new Vector2(HGE_Logo.GetComponent<RectTransform>().offsetMin.x, offsetfundymin);
				Fund_Logo.GetComponent<RectTransform>().offsetMax = new Vector2(HGE_Logo.GetComponent<RectTransform>().offsetMax.x, offsetfundymax);
			}
			else
			{
				this.GetComponent<RectTransform>().offsetMin = new Vector2(this.GetComponent<RectTransform>().offsetMin.x, startymin);
				this.GetComponent<RectTransform>().offsetMax = new Vector2(this.GetComponent<RectTransform>().offsetMax.x, startymax);

				HGE_Logo.GetComponent<RectTransform>().offsetMin = new Vector2(HGE_Logo.GetComponent<RectTransform>().offsetMin.x, starthgeymin);
				HGE_Logo.GetComponent<RectTransform>().offsetMax = new Vector2(HGE_Logo.GetComponent<RectTransform>().offsetMax.x, starthgeymax);

				Fund_Logo.GetComponent<RectTransform>().offsetMin = new Vector2(HGE_Logo.GetComponent<RectTransform>().offsetMin.x, startfundymin);
				Fund_Logo.GetComponent<RectTransform>().offsetMax = new Vector2(HGE_Logo.GetComponent<RectTransform>().offsetMax.x, startfundymax);

			}
		}
	}
	
	public void CloseCredits(){
		
		Creditsrunning=false;
		//Settings.GetComponent<CanvasRenderer>().enabled=false;
	}
	
	public void ActivateCredits()
	{
		
		//bottom
		this.GetComponent<RectTransform>().offsetMin = new Vector2(this.GetComponent<RectTransform>().offsetMin.x, startymin);
		//top
		this.GetComponent<RectTransform>().offsetMax = new Vector2(this.GetComponent<RectTransform>().offsetMax.x, startymax);

		/*
		HGE_Logo.GetComponent<RectTransform>().offsetMin = new Vector2(0, -4200);
		HGE_Logo.GetComponent<RectTransform>().offsetMax = new Vector2(0, -4200);
		*/

		Creditsrunning=true;
		//newstartgui.CSVArrayLanguages();
		//ChangeLanguageText();
	}

	//todoJoao Change Limit of Credits

}
