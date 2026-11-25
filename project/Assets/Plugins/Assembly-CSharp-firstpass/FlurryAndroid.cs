using System;
// (stub: SDK de terceros desactivado; el original llamaba a Java o a la red)
using System.Collections.Generic;
using UnityEngine;

public class FlurryAndroid
{
	private static AndroidJavaClass _flurryAgent;

	private static AndroidJavaObject _plugin;

	static FlurryAndroid()
	{
	}

	public static void onStartSession(string apiKey)
	{
	}

	public static void onEndSession()
	{
	}

	public static void setContinueSessionMillis(long milliseconds)
	{
	}

	public static void logEvent(string eventName)
	{
	}

	public static void logEvent(string eventName, bool isTimed)
	{
	}

	public static void logEvent(string eventName, Dictionary<string, string> parameters)
	{
	}

	public static void logEvent(string eventName, Dictionary<string, string> parameters, bool isTimed)
	{
	}

	public static void endTimedEvent(string eventName)
	{
	}

	public static void onPageView()
	{
	}

	public static void onError(string errorId, string message, string errorClass)
	{
	}

	public static void setUserID(string userId)
	{
	}

	public static void setAge(int age)
	{
	}

	public static void setLogEnabled(bool enable)
	{
	}

	public static void enableAppCircle(string intentName)
	{
	}

	public static float getScreenDensity()
	{
		return default(float);
	}

	public static void showBanner(string hookname, int leftMargin, int topMargin)
	{
	}

	public static void destroyBanner()
	{
	}

	public static void addUserCookie(string key, string value)
	{
	}

	public static void clearUserCookies()
	{
	}

	public static void openCatalog(string hookname)
	{
	}

	public static void launchCatalogOnBannerClicked(bool shouldLaunch)
	{
	}

	public static void setDefaultNoAdsMessage(string msg)
	{
	}

	public static FlurryOffer getOffer(string hookname)
	{
		return default(FlurryOffer);
	}

	public static List<FlurryOffer> getAllOffers(string hookname)
	{
		return default(List<FlurryOffer>);
	}

	public static void acceptOffer(long offerId)
	{
	}

	public static void removeOffer(long offerId)
	{
	}
}
