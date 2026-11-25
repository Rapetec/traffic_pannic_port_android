using UnityEngine;

public class MoPubAndroidEventListener : MonoBehaviour
{
	private void OnEnable()
	{
		MoPubAndroidManager.onAdLoadedEvent += onAdLoadedEvent;
		MoPubAndroidManager.onAdFailedEvent += onAdFailedEvent;
		MoPubAndroidManager.onInterstitialLoadedEvent += onInterstitialLoadedEvent;
		MoPubAndroidManager.onInterstitialFailedEvent += onInterstitialFailedEvent;
	}

	private void OnDisable()
	{
		MoPubAndroidManager.onAdLoadedEvent -= onAdLoadedEvent;
		MoPubAndroidManager.onAdFailedEvent -= onAdFailedEvent;
		MoPubAndroidManager.onInterstitialLoadedEvent -= onInterstitialLoadedEvent;
		MoPubAndroidManager.onInterstitialFailedEvent -= onInterstitialFailedEvent;
	}

	private void onAdLoadedEvent()
	{
	}

	private void onAdFailedEvent()
	{
	}

	private void onInterstitialLoadedEvent()
	{
	}

	private void onInterstitialFailedEvent()
	{
	}
}
