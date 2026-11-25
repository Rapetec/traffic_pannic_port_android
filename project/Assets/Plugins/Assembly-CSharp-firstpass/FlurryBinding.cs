using System.Collections.Generic;
// (stub: SDK de terceros desactivado; el original llamaba a Java o a la red)
using System.Runtime.InteropServices;
using UnityEngine;

public class FlurryBinding
{
	private static string dictionaryToString(Dictionary<string, string> dict)
	{
		return default(string);
	}


	public static void startSession(string apiKey)
	{
	}


	public static void logEvent(string eventName, bool isTimed)
	{
	}


	public static void logEventWithParameters(string eventName, Dictionary<string, string> parameters, bool isTimed)
	{
	}


	public static void endTimedEvent(string eventName)
	{
	}

	public static void endTimedEvent(string eventName, Dictionary<string, string> parameters)
	{
	}
}
