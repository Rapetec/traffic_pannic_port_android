using System;
// (stub: SDK de terceros desactivado; el original llamaba a Java o a la red)
using System.Collections.Generic;
using UnityEngine;

public class FacebookAndroid
{
	private static AndroidJavaObject _facebookPlugin;

	static FacebookAndroid()
	{
	}

	public static bool init(string appId)
	{
		return default(bool);
	}

	public static void showLoginDialog()
	{
	}

	public static void showLoginDialog(string[] permissions)
	{
	}

	public static bool isSessionValid()
	{
		return default(bool);
	}

	public static string getSessionToken()
	{
		return default(string);
	}

	public static void logout()
	{
	}

	public static void showPostMessageDialog()
	{
	}

	public static void showPostMessageDialogWithOptions(string link, string linkName, string linkToImage, string caption)
	{
	}

	public static void showDialog(string dialogType, Dictionary<string, string> parameters)
	{
	}

	public static void postImage(byte[] imageData, string caption)
	{
	}

	public static void graphRequest(string graphPath, string httpMethod, Dictionary<string, string> parameters)
	{
	}

	public static void restRequest(string restMethod, string httpMethod, Dictionary<string, string> parameters)
	{
	}
}
