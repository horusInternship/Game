using UnityEngine;
using System.Collections;
using System;
public class ActionAfterTimer : MonoBehaviour {
	//TO USE CALL StartCoroutine(ActionAfterTimer.Set(2, delegate{ blabla bla}));
	public static IEnumerator Set(float time, Action a){
		yield return new WaitForSeconds(time);
		a();
	}
 
}
