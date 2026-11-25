using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MoPubAndroidManager : MonoBehaviour
{
	[method: MethodImpl(32)]
	public static event Action onAdLoadedEvent;

	[method: MethodImpl(32)]
	public static event Action onAdFailedEvent;

	[method: MethodImpl(32)]
	public static event Action onInterstitialLoadedEvent;

	[method: MethodImpl(32)]
	public static event Action onInterstitialFailedEvent;

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void onAdLoaded(string empty)
	{
		if (MoPubAndroidManager.onAdLoadedEvent != null)
		{
			MoPubAndroidManager.onAdLoadedEvent();
		}
	}

	public void onAdFailed(string empty)
	{
		if (MoPubAndroidManager.onAdFailedEvent != null)
		{
			MoPubAndroidManager.onAdFailedEvent();
		}
	}

	public void onInterstitialLoaded(string empty)
	{
		if (MoPubAndroidManager.onInterstitialLoadedEvent != null)
		{
			MoPubAndroidManager.onInterstitialLoadedEvent();
		}
	}

	public void onInterstitialFailed(string empty)
	{
		if (MoPubAndroidManager.onInterstitialFailedEvent != null)
		{
			MoPubAndroidManager.onInterstitialFailedEvent();
		}
	}
}
