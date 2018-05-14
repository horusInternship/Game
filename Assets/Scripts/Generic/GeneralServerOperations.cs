/*

General Server Operations

Description:
General Script with functions that allow for sending a POST to a server and receiving the response as string

Usage:
It is advisable to call this static function as a Coroutine.


FunctionDescriptions:
HandleServerPOST(string url, Dictionary<string, string> formvalues, string[] error_codes, Action<string> a):
This Function sends a post to the designated url with the formvalues as its parameters, on receiving the response it will check for error codes and do the delegated action
url - the url to the page of the request
formvalues - the values to send to the server, convert numbers to string
error_codes - if the response allows for error messages, add the expected strings to this array
a - the action to be performed after receiving the response from the server, it can receive the server response as a string

Sha1Sum(string strToEncrypt): returns en encrypted string using Sha1 encryption
strToEncrypt - the string you want to encrypt


Usage Example:

		StartCoroutine(GeneralServerOperations.HandleServerPOST(
			"http://www.yoursite.biz/login.php", 
	 		new Dictionary<string, string>(){
				{"username", username},
				{"password", GeneralServerOperations.Sha1Sum(password)}
			},
			new string[]{
			"-1", 
			"-2", 
			"-3", 
			"bla"
			},
			delegate(string s) {
 				your_function(s);
			}
		));

  */

using UnityEngine;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic; 
public class GeneralServerOperations : MonoBehaviour {

	//Returns encryped string
	public static string Sha1Sum(string strToEncrypt){
		UTF8Encoding ue = new UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);
		
		// encrypt bytes
		SHA1 sha = new SHA1CryptoServiceProvider();
		byte[] hashBytes = sha.ComputeHash(bytes);
		
		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";
		for (int i = 0; i < hashBytes.Length; i++){
			hashString += Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}
		return hashString.PadLeft(32, '0');
		
	} 

	//sends a POST to server with no encryption
	public static IEnumerator HandleServerPOST(string url, Dictionary<string, string> formvalues, string[] error_codes, Action<string> a){

		WWWForm form = new WWWForm();
		foreach(string k in formvalues.Keys){
			form.AddField(k, formvalues[k]);
		}

		WWW www_reader = new WWW(url, form);
		
		yield return www_reader;
		
		if(www_reader.error != null){
			Debug.Log(www_reader.error);//TODO GOING INTO OFFLINE MODE
	
		}else{
			bool noerror=true;
			for(int err_code = 0; err_code<error_codes.Length; err_code++){
				if(error_codes[err_code].Equals(www_reader.text)){
					noerror=false;
					Debug.Log (error_codes[err_code]);
					break;
				}
			}
			if(noerror) 
				a(www_reader.text);
		}
	}

	public static IEnumerator HandleServerPOST(string url, Dictionary<string, string> formvalues, string[] error_codes, Action<string> a, Action onfail){
		
		WWWForm form = new WWWForm();
		foreach(string k in formvalues.Keys){
			form.AddField(k, formvalues[k]);
		}
		
		WWW www_reader = new WWW(url, form);
		
		yield return www_reader;
		
		if(www_reader.error != null){
			string serverip = Server.Http().TrimStart(new char[]{'h','t','p', ':', '/'});
			if(www_reader.error.Equals("Failed to connect to "+serverip+": Network is unreachable")){
				//TODO WARNING POPUP
				/*if(PlayerData.offline_popup==0){
						Debug.Log ("Open warning popup once");
					GameObject.Find("OverCanvas").transform.FindChild("pnl_offline").gameObject.SetActive(true);
					PlayerData.offline_popup=1;
				}*/
				Debug.Log ("you are offline");
			}
			//if(www_reader.error.Equals("Failed to reach"))
			Debug.Log(www_reader.error);//TODO GOING INTO OFFLINE MODE
			if(onfail!=null) onfail();
		}else{
			bool noerror=true;
			for(int err_code = 0; err_code<error_codes.Length; err_code++){
				if(error_codes[err_code].Equals(www_reader.text)){
					noerror=false;
					Debug.Log (url + " error: " + error_codes[err_code]);
					if(onfail!=null) onfail();
					break;
				}
			}
			if(noerror) 
				a(www_reader.text);
		}
	}


	//sends a POST to server with no encryption
	public static IEnumerator HandleServerPOST(string url, Dictionary<string, string> formvalues, Dictionary<string, Action>  error_codes, Action<string> a){
		
		WWWForm form = new WWWForm();
		foreach(string k in formvalues.Keys){
			form.AddField(k, formvalues[k]);
		}
		
		WWW www_reader = new WWW(url, form);
		
		yield return www_reader;
		
		if(www_reader.error != null){
			Debug.Log(www_reader.error);//TODO GOING INTO OFFLINE MODE
			
		}else{
			byte[] bytes = Encoding.Default.GetBytes(www_reader.text);
			string s = Encoding.UTF8.GetString(bytes);

			bool noerror=true;
			foreach(string err_code in error_codes.Keys){
				if(err_code.Equals(s)){
					noerror=false;
					Debug.Log (error_codes[err_code]);
					if(error_codes[err_code]!=null) error_codes[err_code]();
					break;
				}
			}
			if(noerror) 
				a(s);
		}
	}
	
	

	private static WWWForm PassArrayOnForm(WWWForm f, string[] arr, string array_name){
		for(int i=0; i<arr.Length; i++){
			f.AddField(array_name+"["+i+"]", arr[i]);
		}
		return f;
	}




}