using System;
using U3DXT.iOS.Native.AddressBook;
using U3DXT.iOS.Native.AddressBookUI;
using U3DXT.iOS.Native.UIKit;
using U3DXT.iOS.Native.CoreFoundation;
using U3DXT.Utils;
using System.Collections.Generic;

public class PeoplePickerNavigationControllerDelegate : ABPeoplePickerNavigationControllerDelegate
{
	
	public PeoplePickerNavigationControllerDelegate()
	{
	}
	
	public override bool ShouldContinueAfterSelectingPerson(ABPeoplePickerNavigationController peoplePicker, ABRecord person) {
		PersonalTest.Log("Selected: " + person.CopyCompositeName());
		
		// 0 means person, 1 means organization
		CFType kindCFType = person.CopyValue(ABPerson.kABPersonKindProperty);
		PersonalTest.Log("Type: " + kindCFType.ToInt());
		
		CFType phonesCFType = person.CopyValue(ABPerson.kABPersonPhoneProperty);
		ABMultiValue phones = phonesCFType.Cast<ABMultiValue>();
		for (int i=0; i<phones.GetCount(); i++) {
			string phoneNumStr = phones.CopyValueAtIndex(i).ToString();
			PersonalTest.Log("Phone number: " + phoneNumStr);
		}
		
		CFType addressesCFType = person.CopyValue(ABPerson.kABPersonAddressProperty);
		ABMultiValue addresses = addressesCFType.Cast<ABMultiValue>();
		for (int i=0; i<addresses.GetCount(); i++) {
			Dictionary<object, object> address = addresses.CopyValueAtIndex(i).ToDictionary();
			PersonalTest.Log("Address: " + Json.Serialize(address));
		}
		
		CFType emailsCFType = person.CopyValue(ABPerson.kABPersonEmailProperty);
		ABMultiValue emails = emailsCFType.Cast<ABMultiValue>();
		for (int i=0; i<emails.GetCount(); i++) {
			string emailStr = emails.CopyValueAtIndex(i).ToString();
			PersonalTest.Log("Email: " + emailStr);
		}

		CFType birthdayCFType = person.CopyValue(ABPerson.kABPersonBirthdayProperty);
		if (birthdayCFType != null)
			PersonalTest.Log("Birthday: " + birthdayCFType.ToDateTime());

		CFType creationDateCFType = person.CopyValue(ABPerson.kABPersonCreationDateProperty);
		PersonalTest.Log("Creation date: " + creationDateCFType.ToDateTime());
		
		UIApplication.deviceRootViewController.DismissViewController(true, null);
		return false;
	}
	
	public override bool ShouldContinueAfterSelectingPersonWithPropertyAndIdentifier(ABPeoplePickerNavigationController peoplePicker, ABRecord person, int property, int identifier){
		PersonalTest.Log("ShouldContinueAfterSelectingPerson: " + person.CopyCompositeName() + ", " + property + ", " + identifier);
		return false;
	}
	
	public override void DidCancel(ABPeoplePickerNavigationController peoplePicker) {
		PersonalTest.Log("Cancel button pressed");
		UIApplication.deviceRootViewController.DismissViewController(true, null);
	}
}
