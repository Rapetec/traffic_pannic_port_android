using System.Collections;
// (stub: SDK de terceros desactivado; el original llamaba a Java o a la red)
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FacebookBinding
{

	public static void init(string applicationId)
	{
	}


	public static bool isLoggedIn()
	{
		return default(bool);
	}


	public static string getFacebookAccessToken()
	{
		return default(string);
	}


	public static void login()
	{
	}


	public static void loginWithRequestedPermissions(string[] permissions)
	{
	}


	public static void logout()
	{
	}


	public static void getLoggedinUsersName()
	{
	}


	public static void postMessage(string message)
	{
	}


	public static void postMessageWithLink(string message, string link, string linkName)
	{
	}


	public static void postMessageWithLinkAndLinkToImage(string message, string link, string linkName, string linkToImage, string caption)
	{
	}


	public static void postImage(string pathToImage, string caption)
	{
	}


	public static void getFriends()
	{
	}


	public static void showPostMessageDialog()
	{
	}


	public static void showPostMessageDialogWithOptions(string link, string linkName, string linkToImage, string caption)
	{
	}


	public static void showPostMessageDialogWithOptions(Hashtable options)
	{
	}


	public static void showDialog(string dialogType, Dictionary<string, string> options)
	{
	}


	public static void graphRequest(string graphPath, string httpMethod, Hashtable keyValueHash)
	{
	}


	public static void restRequest(string restMethod, string httpMethod, Hashtable keyValueHash)
	{
	}
}
