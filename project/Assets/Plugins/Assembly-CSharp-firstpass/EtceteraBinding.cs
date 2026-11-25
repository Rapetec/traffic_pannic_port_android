using System;
// (stub: SDK de terceros desactivado; el original llamaba a Java o a la red)
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class EtceteraBinding
{
	public static IEnumerator takeScreenShot(string filename)
	{
		return default(IEnumerator);
	}


	public static string getCurrentLanguage()
	{
		return default(string);
	}


	public static string getLocalizedString(string key, string defaultValue)
	{
		return default(string);
	}


	public static void showAlertWithTitleMessageAndButton(string title, string message, string buttonTitle)
	{
	}


	public static void showAlertWithTitleMessageAndButtons(string title, string message, string buttonTitle, string otherButtonTitle)
	{
	}


	public static void setPromptColors(uint borderColor, uint gradientStopOne, uint gradientStopTwo)
	{
	}


	public static void showPromptWithOneField(string title, string message, string placeHolder, bool autocomplete)
	{
	}


	public static void showPromptWithTwoFields(string title, string message, string placeHolder1, string placeHolder2, bool autocomplete)
	{
	}


	public static void showWebPage(string url)
	{
	}


	public static bool isEmailAvailable()
	{
		return default(bool);
	}


	public static bool isSMSAvailable()
	{
		return default(bool);
	}


	public static void showMailComposer(string toAddress, string subject, string body, bool isHTML)
	{
	}

	public static IEnumerator showMailComposerWithScreenshot(MonoBehaviour mono, string toAddress, string subject, string body, bool isHTML)
	{
		return default(IEnumerator);
	}


	public static void showMailComposerWithAttachment(string filePathToAttachment, string attachmentMimeType, string attachmentFilename, string toAddress, string subject, string body, bool isHTML)
	{
	}


	public static void showSMSComposer(string body)
	{
	}


	public static void showActivityView()
	{
	}


	public static void hideActivityView()
	{
	}


	public static void showBezelActivityViewWithLabel(string label)
	{
	}


	public static void showBezelActivityViewWithImage(string label, string imagePath)
	{
	}


	public static void askForReview(int launchCount, int hoursBetweenPrompts, string title, string message, string iTunesUrl)
	{
	}


	public static void askForReview(string title, string message, string iTunesUrl)
	{
	}


	public static void promptForPhoto(float scaledToSize)
	{
	}


	public static void resizeImageAtPath(string filePath, float width, float height)
	{
	}


	public static Vector2 getImageSize(string filePath)
	{
		return default(Vector2);
	}


	public static void saveImageToPhotoAlbum(string filePath)
	{
	}


	public static void setUrbanAirshipCredentials(string appKey, string appSecret)
	{
	}


	public static void registerForRemoteNotifcations(RemoteNotificationType types)
	{
	}


	public static RemoteNotificationType getEnabledRemoteNotificationTypes()
	{
		return default(RemoteNotificationType);
	}


	public static int getBadgeCount()
	{
		return default(int);
	}


	public static void setBadgeCount(int badgeCount)
	{
	}


	public static bool unzipFile(string filePath, string destinationFolderName)
	{
		return default(bool);
	}


	public static UIInterfaceOrientation getStatusBarOrientation()
	{
		return default(UIInterfaceOrientation);
	}


	public static string uniqueDeviceIdentifier()
	{
		return default(string);
	}


	public static string uniqueGlobalDeviceIdentifier()
	{
		return default(string);
	}
}
