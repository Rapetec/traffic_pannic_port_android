using System.Collections.Generic;

public static class FlurryController
{
	public static void Init(string apiKey)
	{
		FlurryAndroid.onStartSession(apiKey);
	}

	public static void ReportSingleEvent(string eventName)
	{
		FlurryAndroid.logEvent(eventName, false);
	}

	public static void ReportPieChartEvent(string eventName, string activityName, string itemName)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add(activityName, itemName);
		FlurryAndroid.logEvent(eventName, dictionary, false);
	}
}
