using UnityEngine;

public class MoPubEventListener : MonoBehaviour
{
	private void OnEnable()
	{
		MoPubManager.adViewDidFailToLoadAdEvent += adViewDidFailToLoadAdEvent;
		MoPubManager.adViewDidLoadAdEvent += adViewDidLoadAdEvent;
		MoPubManager.interstitialDidLoadAdEvent += interstitialDidLoadAdEvent;
		MoPubManager.interstitialDidFailToLoadAdEvent += interstitialDidFailToLoadAdEvent;
	}

	private void OnDisable()
	{
		MoPubManager.adViewDidFailToLoadAdEvent -= adViewDidFailToLoadAdEvent;
		MoPubManager.adViewDidLoadAdEvent -= adViewDidLoadAdEvent;
		MoPubManager.interstitialDidLoadAdEvent -= interstitialDidLoadAdEvent;
		MoPubManager.interstitialDidFailToLoadAdEvent -= interstitialDidFailToLoadAdEvent;
	}

	private void adViewDidFailToLoadAdEvent()
	{
	}

	private void adViewDidLoadAdEvent()
	{
	}

	private void interstitialDidLoadAdEvent()
	{
	}

	private void interstitialDidFailToLoadAdEvent()
	{
	}
}
