using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour {

	GameObject LeaderboardContentObj;
	string [] name={"teste1","teste2","teste3","teste4","teste5","Joao","teste2","teste3","teste4","teste5","teste1","teste2","teste3","teste4","teste5","teste4","teste5"};
	string [] score={"50","40","30","20","10","50","40","30","20","10","50","40","30","20","10","20","10"};
	int maxindex,userindex;

	// Use this for initialization
	void Start () {
		LeaderboardContentObj=this.transform.Find("LeaderBoardMask").gameObject.transform.Find("LeaderBoardContent").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(LeaderboardContentObj.GetComponent<RectTransform>().offsetMax.y<40)
		{
			LeaderboardContentObj.GetComponent<RectTransform>().offsetMin = new Vector2(LeaderboardContentObj.GetComponent<RectTransform>().offsetMin.x, 40);
			LeaderboardContentObj.GetComponent<RectTransform>().offsetMax = new Vector2(LeaderboardContentObj.GetComponent<RectTransform>().offsetMax.x, 40);
		}

		int bottomindex=maxindex+(3*70*Screen.height/640);
		Debug.Log ("bottom index "+maxindex);
		if(280*Screen.height/640>(-1*maxindex))
		{
			LeaderboardContentObj.GetComponent<RectTransform>().offsetMin = new Vector2(LeaderboardContentObj.GetComponent<RectTransform>().offsetMin.x, 0);
			LeaderboardContentObj.GetComponent<RectTransform>().offsetMax = new Vector2(LeaderboardContentObj.GetComponent<RectTransform>().offsetMax.x, 0);
		}
		else
		{
			if(-LeaderboardContentObj.GetComponent<RectTransform>().offsetMax.y<(bottomindex))
			{
					LeaderboardContentObj.GetComponent<RectTransform>().offsetMin = new Vector2(LeaderboardContentObj.GetComponent<RectTransform>().offsetMin.x, -bottomindex);
					LeaderboardContentObj.GetComponent<RectTransform>().offsetMax = new Vector2(LeaderboardContentObj.GetComponent<RectTransform>().offsetMax.x, -bottomindex);
			}
		}


	}



	public void ShowLeaderBoard(){
		//Fill Leaderboard



		GameObject Classificationobj;
		LeaderboardContentObj=this.transform.Find("LeaderBoardMask").gameObject.transform.Find("LeaderBoardContent").gameObject;

		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("LeaderboardElement");
		foreach (GameObject target in gameObjects) {
			GameObject.Destroy(target);
		}

		Text ClassificationText;
		for(int i=1;i<=GlobalData.leaderboard_name.Length;i++)
		{
			Debug.Log ("HERE");
			int indexforlist=i-1;
			int yvalue=0;
			int h=0;



			Classificationobj = Instantiate(Resources.Load("Prefabs/Leaderboard/Classificationprefab")) as GameObject;
			Classificationobj.name="Classification"+i;
			Classificationobj.transform.parent=LeaderboardContentObj.transform;
			ClassificationText=Classificationobj.transform.Find("Classificationnumber").GetComponent<Text>();
			ClassificationText.text=i.ToString();
			ClassificationText=Classificationobj.transform.Find("Name").GetComponent<Text>();
			//Change to Global Data Var
			ClassificationText.text=GlobalData.leaderboard_name[indexforlist];
			ClassificationText=Classificationobj.transform.Find("Points").GetComponent<Text>();
			//Change to Global Data Var
			ClassificationText.text=GlobalData.leaderboard_score[indexforlist]+" pontos";
			
			int w=0;
			h=0;
			yvalue=-70*(i-1)*Screen.height/640;
			
			maxindex=yvalue;

			if(GlobalData.leaderboard_name[indexforlist]==GlobalData.LocalUserName)
			{
				userindex=yvalue+(2*70*Screen.height/640);

				LeaderboardContentObj.GetComponent<RectTransform>().offsetMin = new Vector2(LeaderboardContentObj.GetComponent<RectTransform>().offsetMin.x, -userindex);
				LeaderboardContentObj.GetComponent<RectTransform>().offsetMax = new Vector2(LeaderboardContentObj.GetComponent<RectTransform>().offsetMax.x, -userindex);
			}
			
			Classificationobj.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, yvalue ,0);
			Classificationobj.transform.GetComponent<RectTransform>().sizeDelta=new Vector2 (w,h);
			
		}
		GameObject.Find("MainMenu").gameObject.GetComponent<ButtonMainMenu>().MainShowLeaderBoard();
		

	}
}
