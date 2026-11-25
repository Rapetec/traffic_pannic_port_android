using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MoPubManager : MonoBehaviour
{
	[method: MethodImpl(32)]
	public static event Action adViewDidFailToLoadAdEvent;

	[method: MethodImpl(32)]
	public static event Action adViewDidLoadAdEvent;

	[method: MethodImpl(32)]
	public static event Action interstitialDidLoadAdEvent;

	[method: MethodImpl(32)]
	public static event Action interstitialDidFailToLoadAdEvent;

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void adViewDidFailToLoadAd(string empty)
	{
		if (MoPubManager.adViewDidFailToLoadAdEvent != null)
		{
			MoPubManager.adViewDidFailToLoadAdEvent();
		}
	}

	public void adViewDidLoadAd(string empty)
	{
		if (MoPubManager.adViewDidLoadAdEvent != null)
		{
			MoPubManager.adViewDidLoadAdEvent();
		}
	}

	public void interstitialDidLoadAd(string empty)
	{
		if (MoPubManager.interstitialDidLoadAdEvent != null)
		{
			MoPubManager.interstitialDidLoadAdEvent();
		}
	}

	public void interstitialDidFailToLoadAd(string empty)
	{
		if (MoPubManager.interstitialDidFailToLoadAdEvent != null)
		{
			MoPubManager.interstitialDidFailToLoadAdEvent();
		}
	}
}
