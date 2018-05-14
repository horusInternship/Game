using System;
using System.Collections;
using UnityEngine;
using U3DXT.Core;
using U3DXT.iOS.Native.Foundation;
using U3DXT.iOS.Native.UIKit;
using U3DXT.Utils;
using System.Collections.Generic;
using U3DXT.iOS.Native.AddressBook;
using U3DXT.iOS.Native.AddressBookUI;
using U3DXT.iOS.Native.Internals;
using U3DXT.iOS.Native.CoreFoundation;
using U3DXT.iOS.Native.CoreLocation;
using U3DXT.iOS.Native.EventKit;
using U3DXT.iOS.Native.EventKitUI;
using U3DXT.iOS.Personal;
using U3DXT.iOS.GUI;

public class PersonalTest : MonoBehaviour {
	
	PeoplePickerNavigationControllerDelegate pickerDelegate;	
	private EKEvent ekEvent;
	private object[] eventsList; 
	private CLLocationManager manager;
		
	static string _log = "Debug log:";
	static Vector2 _scrollPosition = Vector2.zero;
	
	void Start() {
		
		if (CoreXT.IsDevice) {
			
			//eventStore, calender, etc will be null if Init() is not called.  Manually initilizing helps save memory and must be done before using.
			PersonalXT.Init();
			
			manager = new CLLocationManager();
			
			//response for calling PersonalXT.RequestCalendarAccess(); 
			//optional handler if you want to add custom messages to the user
			PersonalXT.CalendarAccess += delegate(object sender, GrantedEventArgs e) {
				Log ("Calendar grant access: " + e.granted);
			};
			
			//if reminder is granted access, then add reminder as well as retrieve location data
			PersonalXT.ReminderAccess += delegate(object sender, GrantedEventArgs e) {
				Log ("Reminder grant access: " + e.granted);
				if(e.granted){
					manager.StartUpdatingLocation(); //will update delagate function manager.DidUpdateLocations				
				}
			};
			
			//removing reminders after it is found.  or you can do whatever else with it.
			PersonalXT.RemindersFound += delegate(object sender, ReminderArgs e) {
				foreach(EKReminder obj  in e.objList){
					PersonalXT.eventStore.RemoveReminder(obj, true, null);
				}			
				Log("Reminders Removed");
			};
			
			//alternate way to do call back functions instead of using delegate file (see example PeoplePickerNavigationControllerDelegate.cs)
			manager.DidUpdateLocations += delegate(object sender, CLLocationManager.DidUpdateLocationsEventArgs e){
				manager.StopUpdatingLocation();
					
				EKStructuredLocation location = new EKStructuredLocation();			
				location.title = "Current location";
				location.geoLocation = e.locations[e.locations.Length-1] as CLLocation;
				
				EKReminder reminder = EKReminder.Reminder(PersonalXT.eventStore);
				reminder.title = "Buy U3DXT at coordinates: " + location.geoLocation.coordinate;
				reminder.calendar = PersonalXT.eventStore.defaultCalendarForNewReminders;
	
				NSError error = null;			
				PersonalXT.eventStore.SaveReminder(reminder,true,error);
				
				if(error != null)
					Log ("error: " + error);	
				Log ("current location coordinates: " + location.geoLocation.coordinate);
			};
			manager.desiredAccuracy = CLLocation.kCLLocationAccuracyNearestTenMeters;
			manager.distanceFilter = 1;
		}
	}	
	
	private void OnGUI() {		
		KitchenSink.OnGUIBack();

		GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, Screen.height / 2 - 50));
		
		//this is the original way of how objective C use call back functions: Delegates using separate files.
		//in this case the file is PeoplePickerNavigationControllerDelegate.cs
		//we have have it easier to use delegates instead of creating new files for each delegate.
		//see examples above.  Ex: PersonalXT.CalendarAccess += delegate( ....
		if (GUILayout.Button("pick/select from contacts", GUILayout.ExpandHeight(true))) {
			ABPeoplePickerNavigationController picker = new ABPeoplePickerNavigationController();
			if (pickerDelegate == null)
				pickerDelegate = new PeoplePickerNavigationControllerDelegate();
			picker.peoplePickerDelegate = pickerDelegate;
			UIApplication.deviceRootViewController.PresentViewController(picker, true, null);
		}
		
		if (GUILayout.Button("get all contacts", GUILayout.ExpandHeight(true)))	{
			Log("Address book authorization status: " + ABAddressBook.GetAuthorizationStatus());
			
			var addressBook = ABAddressBook.Create(null, null);
			addressBook.RequestAccess(delegate(bool granted, NSError error) {
				Log("Granted: " + granted);
				
				//convienent function to get the names of the contacts
				string[] contactList = PersonalXT.GetAllContactNames();			
				for(int i=0; i < contactList.Length; i++){
					Log("Contact " + i + ": " +  contactList[i]);
				}				
			});
		}
		
		if (GUILayout.Button("add new contacts", GUILayout.ExpandHeight(true)))	{
			addNewContact();
		}
		
		if (GUILayout.Button("init Calendar and show events within 30 days", GUILayout.ExpandHeight(true))) {
			checkEventStoreAccessForCalendar();
		}
		
		if (GUILayout.Button("add an event for tomorrow", GUILayout.ExpandHeight(true))) {
			addEventForTomorrow();
		}
		
		if (GUILayout.Button("add alarm to events", GUILayout.ExpandHeight(true))) {
			createAlarmForEvents();
		}
		
		if (GUILayout.Button("add reminder with geolocation of current location", GUILayout.ExpandHeight(true))) {
			PersonalXT.RequestReminderAccess();			
		}
		
		if (GUILayout.Button("reverse geocode happiest place on earth", GUILayout.ExpandHeight(true))) {
			CLLocation location = new CLLocation(33.809, -117.919);
			CLGeocoder geocoder = new CLGeocoder();
			geocoder.ReverseGeocodeLocation(location, delegate(object[] placemarks, NSError error) {
				if (error != null)
					Debug.Log(error.LocalizedDescription());
				else {
					foreach (var p in placemarks) {
						var placemark = p as CLPlacemark;
						
						Debug.Log("placemark: " + placemark.name + "\n"
							+ ABAddressFormatting.ABCreateString(placemark.addressDictionary, true));
					}
				}
			});
		}
		
		if (GUILayout.Button("Significant location change", GUILayout.ExpandHeight(true))) {
			if (!CLLocationManager.LocationServicesEnabled() || !CLLocationManager.SignificantLocationChangeMonitoringAvailable()) {
				Debug.Log("Significant change monitoring not available.");
			} else {
//				CLLocationManager manager = new CLLocationManager();
				
				manager.StartMonitoringSignificantLocationChanges();
			}
		}

		
		//commented out remove all events and reminders so users don't accidentally remove important events
		/*
		if (GUILayout.Button("remove all Events", GUILayout.ExpandHeight(true))) {
			PersonalXT.RemoveAllEvents();			
			Log ("Removed events");
		}
		
		if (GUILayout.Button("remove all Reminders", GUILayout.ExpandHeight(true))) {
			PersonalXT.GetAllReminders(); //you can get all the reminders and handle them in line 59 above
			//PersonalXT.RemoveAllReminders(); //or you can simply call removeAllReminders
		}*/
		
		GUILayout.EndArea();
		OnGUILog();
	}
	
	private void addNewContact() {
		ABAddressBook addressBook = ABAddressBook.Create();

		ABRecord person = ABPerson.Create();
		person.SetValue(ABPerson.kABPersonFirstNameProperty, CFType.FromObject("vitapoly"), null);
		person.SetValue(ABPerson.kABPersonBirthdayProperty, CFType.FromObject(new DateTime(2000, 1, 1)), null);
		
		addressBook.AddRecord(person, null);
		
		addressBook.Save(null);
		
		Log("Added vitapoly to address book.");
	}
	
	private void createAlarmForEvents(){
		EKEvent currentEvent;
		eventsList = PersonalXT.GetEventsFromTo(DateTime.Today, DateTime.Today.AddMonths(1)); // 30 days
		if(eventsList == null )
		{
			Log ("No Events detected within 30 days");
		}
		else
		{
			foreach (object obj in eventsList){	
				currentEvent = obj as EKEvent;
				EKAlarm testAlarm = new EKAlarm();
				testAlarm.absoluteDate = currentEvent.startDate;
				currentEvent.AddAlarm(testAlarm);
				PersonalXT.eventStore.SaveEvent(currentEvent,EKSpan.ThisEvent, null);
				Log ("Adding alert to event named: " + currentEvent.title);
			}
		}

	}
	
	private void checkEventStoreAccessForCalendar() {
		switch(PersonalXT.GetCalendarAccessStatus()){
			case "Authorized":
				showAllEventsInLog();
			break;			
			case "NotDetermined":
				PersonalXT.RequestCalendarAccess(); 
			break;
			case "StatusDenied":
				Log ("User denied access to calendar");
			break;			
			case "StatusRestricted":
				Log ("Permission was not granted for Calendar");
			break;
		}
	}
	
	private void showAllEventsInLog(){		
		//gets 30 days worth of events
		eventsList = PersonalXT.GetEventsFromTo(DateTime.Today, DateTime.Today.AddMonths(1)); 
		EKEvent tempEvent;
		
		foreach(object obj  in eventsList){
			tempEvent = obj as EKEvent;
			Log ("Event Detected.  Start Date: "+ tempEvent.startDate + "  End Date: " + tempEvent.endDate  );	
		}
	}
		
	private void addEventForTomorrow(){
		string eventTitle =  "Buy U3dxt";
		if(PersonalXT.GetCalendarAccessStatus()== "Authorized")
		{
			//creates and adds event to calendar: title, start date, end date.  
			//creates and adds event to calendar: title, start date, end date.  
			DateTime startDate = DateTime.Now.AddDays(1);
			DateTime endDate = startDate.AddHours(2);
			PersonalXT.CreateSimpleEvent(eventTitle, startDate, endDate);

			//PersonalXT.CreateSimpleEvent(eventTitle, DateTime.Parse("2014-01-24"),DateTime.Parse("2014-01-24").AddHours(2));
			
			Log ("adding event : " + eventTitle + " from " + startDate + " to " + endDate);}
		else
		{
			Log ("No permission for calendar granted.  Click the init calendar button above.");
			
		}
	

	}
	
	private void OnGUILog() {
		GUILayout.BeginArea(new Rect(50, Screen.height / 2, Screen.width - 100, Screen.height / 2 - 50));
		_scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
		GUI.skin.box.wordWrap = true;
		GUI.skin.box.alignment = TextAnchor.UpperLeft;
		GUILayout.Box(_log, GUILayout.ExpandHeight(true));
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}
	
	public static void Log(string str) {
		_log += "\n" + str;
		_scrollPosition.y = Mathf.Infinity;
	}
}
