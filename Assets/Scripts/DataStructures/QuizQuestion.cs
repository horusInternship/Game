using UnityEngine;
using System.Collections;

public class QuizQuestion {
	public int difficulty=0;
	public string Q = "";
	public string RA = "";
	public string WA1 = "";
	public string WA2 = "";
	public string WA3 = "";
	public string OW = "";
	public string OL = "";
	public QuizQuestion( int difficulty, string Q, string RA, string WA1, string WA2, string WA3, string OW, string OL){
		this.difficulty = difficulty;
		this.Q = Q;
		this.RA = RA;
		this.WA1 = WA1;
		this.WA2 = WA2;
		this.WA3 = WA3;
		this.OW = OW;
		this.OL = OL;
	}

	public string GetAnswerById(int aid){
		if(aid==0){
			return RA;
		}else if(aid==1){
			return WA1;
		}else if(aid==2){
			return WA2;
		}else if(aid==3){
			return WA3;
		}else{
			return "";
		}
	}
}
