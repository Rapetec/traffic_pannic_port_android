using UnityEngine;

public static class ADManager
{
	private static string bannerID = "agltb3B1Yi1pbmNyDQsSBFNpdGUYjsjcEQw";

	private static string padBannerID = "agltb3B1Yi1pbmNyDQsSBFNpdGUY2_z7EQw";

	private static string fullScreenID = "agltb3B1Yi1pbmNyDQsSBFNpdGUYsYLVEQw";

	private static string bannerIDAndroid = "agltb3B1Yi1pbmNyDQsSBFNpdGUYgpPGEgw";

	private static string padBannerIDAndroid = "agltb3B1Yi1pbmNyDQsSBFNpdGUYx5fHEgw";

	private static string fullScreenIDAndroid = "agltb3B1Yi1pbmNyDQsSBFNpdGUYyJfHEgw";

	public static bool hasInitialized;

	public static void InitializeAds()
	{
		if (AppController.Instance.EnableAds && !hasInitialized)
		{
			hasInitialized = true;
			CreateBanner();
		}
	}

	public static void CreateBanner()
	{
		MoPubAndroid.createBanner(bannerIDAndroid, (int)((float)(Screen.width / 2) - 160f * MoPubAndroid.getScreenDensity()), 0);
	}

	public static bool GetInit()
	{
		return hasInitialized;
	}

	public static void SetHidden(bool _state)
	{
		if (AppController.Instance.EnableAds)
		{
			if (PurchaseManager.Instance.PurchaseMade)
			{
				MoPubAndroid.destroyBanner();
			}
			else
			{
				MoPubAndroid.hideBanner(_state);
			}
		}
	}

	public static void DestroyBanner()
	{
		if (AppController.Instance.EnableAds)
		{
			MoPubAndroid.destroyBanner();
		}
	}
}
