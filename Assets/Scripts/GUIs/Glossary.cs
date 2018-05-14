using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Glossary : MonoBehaviour 
{
	
	public GameObject QuizDificulty,QuestionContent,QuestionAnswerWindow;
	//todoJoao Change Limit of Credits
	
	//public GameObject HGE_Logo;
	
	float maxindex=0;
	
	void Start(){
		/*
		if(!PlayerPrefs.HasKey("PlayerSavedData")){
			LoadQuiz(-1);
		}else{
		//	LoadQuizNew();
			LoadQuiz(-1);
		}*/
	}
	
	string [] easyquestions,mediumquestions,hardquestions; 
	
	void Update () {
		
		if(QuestionContent.GetComponent<RectTransform>().offsetMax.y<0)
		{
			QuestionContent.GetComponent<RectTransform>().offsetMin = new Vector2(QuestionContent.GetComponent<RectTransform>().offsetMin.x, 0);
			QuestionContent.GetComponent<RectTransform>().offsetMax = new Vector2(QuestionContent.GetComponent<RectTransform>().offsetMax.x, 0);
		}
		
		if(-QuestionContent.GetComponent<RectTransform>().offsetMax.y<(maxindex))
		{
			QuestionContent.GetComponent<RectTransform>().offsetMin = new Vector2(QuestionContent.GetComponent<RectTransform>().offsetMin.x, -maxindex);
			QuestionContent.GetComponent<RectTransform>().offsetMax = new Vector2(QuestionContent.GetComponent<RectTransform>().offsetMax.x, -maxindex);
		}
	}
	
	public void opendifficulty(int difficulty){
		SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
		
		
		foreach (Transform child in QuestionContent.transform) {
			GameObject.Destroy(child.gameObject);
		}
		QuizDificulty.SetActive(false);
		QuestionContent.transform.parent.gameObject.SetActive(true);
		this.transform.Find("btn_back").gameObject.SetActive(true);
		
		int curr_numofquestions=0;
		float h=0,yvalue=0;
		for(int i=0;i<GlobalData.SAVEDQUIZQUESTIONS[difficulty].Count;i++)
		{
			if(GlobalData.SAVEDQUIZQUESTIONS[difficulty][i].difficulty==difficulty)
			{
				

				GameObject QuestionPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/GlossaryQuestion"));
				QuestionPrefab.transform.parent=QuestionContent.transform;
				
				//Joao2409
				float height=Screen.height;
				h=120*height/640;
				
				
				yvalue=-60*curr_numofquestions+Screen.height/640-h*curr_numofquestions;
				//Joao2409
				
				QuestionPrefab.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, yvalue ,0);
				QuestionPrefab.transform.GetComponent<RectTransform>().sizeDelta=new Vector2 (0,0);
				
				QuestionPrefab.transform.transform.Find("Questionnumber").GetComponent<Text>().text=((curr_numofquestions+1)+".").ToString();
				QuestionPrefab.transform.transform.Find("Question").GetComponent<Text>().text=GlobalData.SAVEDQUIZQUESTIONS[difficulty][i].Q.ToString();
				
				QuestionPrefab.name=i.ToString();
				
				QuestionPrefab.GetComponent<Button>().onClick.RemoveAllListeners();
				QuestionPrefab.GetComponent<Button>().onClick.AddListener(()=>{
					//code
					SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
					
					QuestionAnswerWindow.SetActive(true);
					this.transform.Find("btn_back").gameObject.SetActive(false);
					QuestionAnswerWindow.transform.Find("Question").GetComponent<Text>().text="Q. "+GlobalData.SAVEDQUIZQUESTIONS[difficulty][int.Parse(QuestionPrefab.gameObject.name)].Q.ToString();
					QuestionAnswerWindow.transform.Find("Answer").GetComponent<Text>().text="R. "+GlobalData.SAVEDQUIZQUESTIONS[difficulty][int.Parse(QuestionPrefab.gameObject.name)].RA.ToString();
					QuestionContent.SetActive(false);
				});
				maxindex=yvalue+h;
				curr_numofquestions++;
			}
		}
	}
	
	
	
	public void Close_QuestionAnswer()
	{
		SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
		
		this.transform.Find("btn_back").gameObject.SetActive(true);
		QuestionAnswerWindow.SetActive(false);
		QuestionContent.SetActive(true);
	}
	
	public void GoToDificulty_Menu(){

		Debug.Log ("HERE");
		SoundControl.PlaySFX(GlobalData.SFX_Paths[0], false, true, true);
		
		QuizDificulty.SetActive(true);
		QuestionContent.transform.parent.gameObject.SetActive(false);
		this.transform.Find("btn_back").gameObject.SetActive(false);
	}
	
	

	

}
