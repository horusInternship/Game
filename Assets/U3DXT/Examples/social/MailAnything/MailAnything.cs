using UnityEngine;
using System.Collections;

using U3DXT.iOS.Native.MessageUI;
using U3DXT.iOS.Native.UIKit;
using U3DXT.iOS.Native.Foundation;

using System.IO;

public class MailAnything : MonoBehaviour {
	
	private MFMailComposeViewController _mailController;
	private MyMFMailComposeViewControllerDelegate _mailControllerDelegate;
	
	// Use this for initialization
	void Start () {
	
		// create some arbitary binary data. or load from existing location
		FileStream someFile = new FileStream(Application.temporaryCachePath+"/someFile.bin", FileMode.Create);
		someFile.WriteByte(0x42);
		someFile.Close();
		
		_mailController = new MFMailComposeViewController();
		
		_mailControllerDelegate = new MyMFMailComposeViewControllerDelegate();
		_mailController.mailComposeDelegate = _mailControllerDelegate;
		
		_mailController.SetToRecipients(new string[]{"nowhere@u3dxt.com"});
		_mailController.SetSubject("well hello");
		_mailController.SetMessageBody("just testing attachments", false);
		
		_mailController.AddAttachmentData(
			new NSData(Application.temporaryCachePath+"/someFile.bin"), 
			"application/octet-stream", 
			"someFile.bin"
		);
		
		UIApplication.deviceRootViewController.PresentViewController(_mailController, true, null);
		
		
	}
	
	internal class MyMFMailComposeViewControllerDelegate : MFMailComposeViewControllerDelegate
	{
		public MyMFMailComposeViewControllerDelegate(){}
		
		public override void DidFinish(MFMailComposeViewController viewController, MFMailComposeResult result, NSError error)
		{
			viewController.DismissViewController(true, null);
		}
	}

}
