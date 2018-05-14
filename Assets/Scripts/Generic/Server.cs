using UnityEngine;
using System.Collections.Generic;
using System;
using System.Globalization;

/**
 * Fast replace implementation:
 * 		Search and replace every line of:
 * 			"http://famousgadget.pt
 * 		for
 * 			Server.Http ()+"
 * 		with the quotation marks and other signals included included.
 * 		Also replace every line of
 * 			"http://www.famousgadget.pt
 * 		and
 * 			"http://horusgaming.com
 * 		and 
 * 			"http://www.horusgaming.com
 * 		for the same as above
 * In order to help, you could use the Finder of Mac OS to search the scripts that have those values.
 */
public class Server {

	private static string http="";
	private static Dictionary<int,string> existent=new Dictionary<int, string>(){
		{9,"http://106.187.43.106"},
		{-8,"http://74.207.240.67"},
		{0,"http://178.79.152.107"},
	};

	public static string Http(){
		if (http == null || http == "") {
			TimeZone t_zone=TimeZone.CurrentTimeZone;
			TimeSpan t_span=t_zone.GetUtcOffset(DateTime.Now);
			int dif=1000;
			int selected=9;
			foreach (int hours in existent.Keys){
				int val=Mathf.Abs(t_span.Hours-hours);
				if (val<dif){
					dif=val;
					selected=hours;
				}
			}
			http=existent[selected];
		//	Debug.Log(http);
		}
		return http;
	}

}
