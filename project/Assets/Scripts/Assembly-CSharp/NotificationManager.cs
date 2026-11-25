using System;
using System.Collections.Generic;
using NeonPlay;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
	private enum NotificationType
	{
		Tutorial1 = 0,
		Tutorial2 = 1,
		TrafficJam = 2,
		Crashed = 3,
		HiScore = 4,
		UnlockAll = 5,
		FreeCredits = 6,
		Shop_Vehicle_Taxi = 7,
		Shop_Vehicle_Police_Car = 8,
		Shop_Vehicle_Italiano = 9,
		Shop_Vehicle_Trailer = 10,
		Shop_Vehicle_Ambulance = 11,
		Shop_Vehicle_Fuel_Tanker = 12,
		Shop_Vehicle_Fire_Truck = 13,
		Shop_Vehicle_Das_Sporta = 14,
		Shop_Vehicle_Tank = 15,
		Shop_City_Gas_Station = 16,
		Shop_City_Demolition_Site = 17,
		Shop_City_Car_Showroom = 18,
		Shop_City_Fireworks_Factory = 19,
		InsuranceFraud = 20
	}

	private const int gamesUntilNextItemShown = 10;

	private const int baseGamesUntilNextIAPShown = 20;

	private const int incrementalGamesUntilNextIAPShown = 20;

	private static volatile NotificationManager _instance;

	private static object _lock;

	public SpriteText bannerCurrency;

	public GameObject notificationParent;

	public PackedSprite title;

	public SpriteText centerAlignedText;

	public SpriteText priceText;

	public PackedSprite image1;

	public PackedSprite image2;

	public PackedSprite image3;

	public GameObject buttonParent;

	public UIActionBtn buyButton;

	public UIActionBtn continueButton;

	public UIActionBtn skipButton;

	public UIActionBtn startButton;

	public UIActionBtn nextButton;

	public UIActionBtn likeButton;

	public UIActionBtn unlockButton;

	public UIPanel trafficTip;

	private NotificationType type;

	private List<NotificationType> eligibleTypes;

	public bool gameOverNotification;

	private bool hiScoreNotification;

	private bool isScore;

	private int score;

	private bool tipDismissed = true;

	private bool firstCrash;

	private bool firstJam;

	private bool firstHiScore;

	private bool likedOnFacebook;

	private int gamesUntilNextIAPShown = 20;

	public bool FirstCrash
	{
		get
		{
			return firstCrash;
		}
	}

	public bool FirstJam
	{
		get
		{
			return firstJam;
		}
	}

	public bool FirstHiScore
	{
		get
		{
			return firstHiScore;
		}
	}

	public static NotificationManager Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_lock)
				{
					GameObject gameObject = new GameObject();
					gameObject.AddComponent<NotificationManager>();
					_instance = gameObject.GetComponent<NotificationManager>();
					gameObject.name = _instance.GetType().ToString();
				}
			}
			return _instance;
		}
	}

	private NotificationManager()
	{
	}

	static NotificationManager()
	{
		_instance = null;
		_lock = new object();
	}

	private void MyAwake()
	{
		type = NotificationType.Tutorial1;
		eligibleTypes = new List<NotificationType>();
		firstCrash = PlayerPrefs.GetInt("FirstCrash") == 0;
		firstJam = PlayerPrefs.GetInt("FirstJam") == 0;
		firstHiScore = PlayerPrefs.GetInt("FirstHiScore") == 0;
		likedOnFacebook = ((PlayerPrefs.GetInt("LikedOnFacebook") != 0) ? true : false);
	}

	public void Show(bool tutorial)
	{
		Time.timeScale = 0f;
		notificationParent.SetActiveRecursively(true);
		if (!tutorial)
		{
			EligibleNotification();
		}
		else
		{
			type = NotificationType.Tutorial1;
		}
		ActivateDisplay(tutorial);
	}

	public void ShowTrafficJam()
	{
		if (firstJam)
		{
			gameOverNotification = true;
			firstJam = false;
			PlayerPrefs.SetInt("FirstJam", 1);
			notificationParent.SetActiveRecursively(true);
			type = NotificationType.TrafficJam;
			ActivateDisplay(true);
		}
	}

	public void ShowCrash()
	{
		if (firstCrash)
		{
			gameOverNotification = true;
			firstCrash = false;
			PlayerPrefs.SetInt("FirstCrash", 1);
			notificationParent.SetActiveRecursively(true);
			type = NotificationType.Crashed;
			ActivateDisplay(true);
		}
	}

	public void ShowHiScore(bool isScore, int score)
	{
		this.isScore = isScore;
		this.score = score;
		hiScoreNotification = true;
		firstHiScore = false;
		PlayerPrefs.SetInt("FirstHiScore", 1);
		notificationParent.SetActiveRecursively(true);
		type = NotificationType.HiScore;
		ActivateDisplay(false);
	}

	public void Dismiss()
	{
		Time.timeScale = 1f;
		if (gameOverNotification)
		{
			GameNav.Instance.BringInGameOver();
		}
		else if (hiScoreNotification)
		{
			GameNav.Instance.BringInHiscore(isScore, score);
		}
		gameOverNotification = (hiScoreNotification = false);
		notificationParent.SetActiveRecursively(false);
	}

	public void BringInTrafficTip()
	{
		trafficTip.gameObject.SetActiveRecursively(true);
		tipDismissed = false;
		trafficTip.BringIn();
	}

	public void DismissTrafficTip()
	{
		if (!tipDismissed)
		{
			tipDismissed = true;
			trafficTip.Dismiss();
		}
	}

	public void Next()
	{
		type = NotificationType.Tutorial2;
		ActivateDisplay(true);
	}

	public void Like()
	{
		if (PlayerPrefs.GetInt("DoneLikeAlready") == 0)
		{
			CurrencyManager.AddCurrency(10000);
			bannerCurrency.Text = CurrencyManager.GetCurrency().ToString("N0");
			PlayerPrefs.SetInt("DoneLikeAlready", 1);
		}
		SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDFacebookLike);
		EtceteraAndroid.showWebView("Traffic Panic 3D - Like us on facebook!", "http://www.facebook.com/neonplay");
		Dismiss();
	}

	public void Buy()
	{
		BuyNotification();
	}

	private void EligibleNotification()
	{
		int num = PlayerPrefs.GetInt("showCanAffordCount");
		if (num > 0)
		{
			num--;
			PlayerPrefs.SetInt("showCanAffordCount", num);
		}
		Debug.Log("showCanAffordCount = " + num);
		bool flag = ((num <= 0) ? true : false);
		int num2 = PlayerPrefs.GetInt("showIAPCount");
		if (num2 > 0)
		{
			num2--;
			PlayerPrefs.SetInt("showIAPCount", num2);
		}
		Debug.Log("showIAPCount = " + num2);
		bool flag2 = num2 <= 0;
		eligibleTypes.Clear();
		if (!SingletonBase<TrafficShopItemHandler>.Instance.IsBuildingUnlocked(0) && CurrencyManager.CanSpendCurrency(ShopPrices.GetBuildingPrice(0)) && flag)
		{
			eligibleTypes.Add(NotificationType.Shop_City_Fireworks_Factory);
		}
		if (!SingletonBase<TrafficShopItemHandler>.Instance.IsBuildingUnlocked(1) && CurrencyManager.CanSpendCurrency(ShopPrices.GetBuildingPrice(1)) && flag)
		{
			eligibleTypes.Add(NotificationType.Shop_City_Gas_Station);
		}
		if (!SingletonBase<TrafficShopItemHandler>.Instance.IsBuildingUnlocked(2) && CurrencyManager.CanSpendCurrency(ShopPrices.GetBuildingPrice(2)) && flag)
		{
			eligibleTypes.Add(NotificationType.Shop_City_Car_Showroom);
		}
		if (!SingletonBase<TrafficShopItemHandler>.Instance.IsBuildingUnlocked(3) && CurrencyManager.CanSpendCurrency(ShopPrices.GetBuildingPrice(3)) && flag)
		{
			eligibleTypes.Add(NotificationType.Shop_City_Demolition_Site);
		}
		if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(0) && CurrencyManager.CanSpendCurrency(ShopPrices.GetVehiclePrice(0)) && flag)
		{
			eligibleTypes.Add(NotificationType.Shop_Vehicle_Taxi);
		}
		if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(1) && CurrencyManager.CanSpendCurrency(ShopPrices.GetVehiclePrice(1)) && flag)
		{
			eligibleTypes.Add(NotificationType.Shop_Vehicle_Trailer);
		}
		if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(2) && CurrencyManager.CanSpendCurrency(ShopPrices.GetVehiclePrice(2)) && flag)
		{
			eligibleTypes.Add(NotificationType.Shop_Vehicle_Das_Sporta);
		}
		if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(3) && CurrencyManager.CanSpendCurrency(ShopPrices.GetVehiclePrice(3)) && flag)
		{
			eligibleTypes.Add(NotificationType.Shop_Vehicle_Police_Car);
		}
		if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(4) && CurrencyManager.CanSpendCurrency(ShopPrices.GetVehiclePrice(4)) && flag)
		{
			eligibleTypes.Add(NotificationType.Shop_Vehicle_Ambulance);
		}
		if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(5) && CurrencyManager.CanSpendCurrency(ShopPrices.GetVehiclePrice(5)) && flag)
		{
			eligibleTypes.Add(NotificationType.Shop_Vehicle_Italiano);
		}
		if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(6) && CurrencyManager.CanSpendCurrency(ShopPrices.GetVehiclePrice(6)) && flag)
		{
			eligibleTypes.Add(NotificationType.Shop_Vehicle_Fire_Truck);
		}
		if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(7) && CurrencyManager.CanSpendCurrency(ShopPrices.GetVehiclePrice(7)) && flag)
		{
			eligibleTypes.Add(NotificationType.Shop_Vehicle_Fuel_Tanker);
		}
		if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(8) && CurrencyManager.CanSpendCurrency(ShopPrices.GetVehiclePrice(8)) && flag)
		{
			eligibleTypes.Add(NotificationType.Shop_Vehicle_Tank);
		}
		bool flag3 = true;
		for (int i = 0; i < 4; i++)
		{
			if (!flag3)
			{
				break;
			}
			if (!SingletonBase<TrafficShopItemHandler>.Instance.IsBuildingUnlocked(i))
			{
				flag3 = false;
			}
		}
		for (int j = 0; j < 9; j++)
		{
			if (!flag3)
			{
				break;
			}
			if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(j))
			{
				flag3 = false;
			}
		}
		if (!SingletonBase<TrafficShopItemHandler>.Instance.UnlockAll && !flag3 && flag2)
		{
			Debug.Log("Unlock all is egigble type");
			eligibleTypes.Add(NotificationType.UnlockAll);
		}
		if (!SingletonBase<TrafficShopItemHandler>.Instance.InsuranceFraud && flag2)
		{
			Debug.Log("Insurance fraud is eligble type");
			eligibleTypes.Add(NotificationType.InsuranceFraud);
		}
		if (PlayerPrefs.GetInt("DoneLikeAlready") == 0 && flag2)
		{
			eligibleTypes.Add(NotificationType.FreeCredits);
			eligibleTypes.Add(NotificationType.FreeCredits);
		}
		if (eligibleTypes.Count >= 1)
		{
			gamesUntilNextIAPShown = 20;
			for (int k = eligibleTypes.Count; k < 3; k++)
			{
				gamesUntilNextIAPShown += 20;
			}
			Debug.Log("Eligible IAP popups = " + eligibleTypes.Count);
			Debug.Log("Games between IAP popups = " + gamesUntilNextIAPShown);
			int index = UnityEngine.Random.Range(0, eligibleTypes.Count);
			type = eligibleTypes[index];
		}
	}

	private void ActivateDisplay(bool tutorial)
	{
		if (eligibleTypes.Count < 1 && !tutorial)
		{
			notificationParent.SetActiveRecursively(false);
			Time.timeScale = 1f;
			return;
		}
		Debug.Log("Attempting to show notification");
		centerAlignedText.Text = string.Empty;
		priceText.Text = string.Empty;
		image1.gameObject.active = false;
		image2.gameObject.active = false;
		image3.gameObject.active = false;
		switch (type)
		{
		case NotificationType.Tutorial1:
			image2.gameObject.active = true;
			image2.PlayAnim("GetAcross");
			centerAlignedText.Text = "Keep the traffic flowing as long as you can to win credits.\n\nPlus...the bigger the crash damage, the more credits you win!";
			Combo_SkipNext();
			break;
		case NotificationType.Tutorial2:
			image2.gameObject.active = true;
			title.PlayAnim("Tutorial2");
			image2.PlayAnim("TrafficLights");
			centerAlignedText.Text = "Tap the screen to change the lights.\n\nIt's game over if you crash or cause a traffic jam!";
			Combo_ZeroStart();
			break;
		case NotificationType.TrafficJam:
			Debug.Log("Got into Notification type traffic jam");
			image2.gameObject.active = true;
			image2.PlayAnim("TrafficJam");
			title.PlayAnim("TrafficJam");
			centerAlignedText.Text = "If the traffic takes too long to get across, you get a traffic jam. Try to keep the vehicles flowing for a big score!";
			Combo_ZeroContinue();
			break;
		case NotificationType.Crashed:
			Debug.Log("Got into Notification type Crash");
			image2.gameObject.active = true;
			image2.PlayAnim("Crash");
			title.PlayAnim("FirstCrash");
			centerAlignedText.Text = "Oops, you caused your first crash! Don't worry though, big scores and massive crashes both earn juicy Credits!";
			Combo_ZeroContinue();
			break;
		case NotificationType.UnlockAll:
			image2.gameObject.active = true;
			title.PlayAnim("UnlockAll");
			image2.PlayAnim("UnlockAll");
			centerAlignedText.Text = "Torch the Firework Factory! Explode the Fuel Tanker! Unlock every vehicle and building with this AWESOME one-off purchase!";
			priceText.Text = string.Empty;
			Combo_SkipUnlock();
			PlayerPrefs.SetInt("showIAPCount", gamesUntilNextIAPShown);
			PlayerPrefs.SetInt("showCanAffordCount", 10);
			break;
		case NotificationType.FreeCredits:
			image3.gameObject.active = true;
			image3.PlayAnim("FaceBook");
			title.PlayAnim("Facebook");
			centerAlignedText.Text = "Like us on Facebook for 10,000 FREE credits. That's right, FREE! You can unlock a new vehicle with that...";
			Combo_SkipLike();
			PlayerPrefs.SetInt("showIAPCount", gamesUntilNextIAPShown);
			PlayerPrefs.SetInt("showCanAffordCount", 10);
			break;
		case NotificationType.InsuranceFraud:
			image3.gameObject.active = true;
			image3.PlayAnim("Insurance");
			title.PlayAnim("Insurance");
			centerAlignedText.Text = "Earn more Credits and double your damage score! Only with the Insurance Fraud power-up.";
			priceText.Text = string.Empty;
			Combo_SkipBuy();
			PlayerPrefs.SetInt("showIAPCount", gamesUntilNextIAPShown);
			PlayerPrefs.SetInt("showCanAffordCount", 10);
			break;
		case NotificationType.Shop_Vehicle_Taxi:
			image1.gameObject.active = true;
			title.PlayAnim("Taxi");
			image1.PlayAnim("Taxi");
			centerAlignedText.Text = "Why not kit out your city with an shiny yellow taxi? It's authentic and certainly likes to beeeeeeeeeep!";
			priceText.Text = "ONLY " + ShopPrices.GetVehiclePrice(0).ToString("N0") + " CREDITS" + AffordItMessage();
			Combo_SkipBuy();
			PlayerPrefs.SetInt("showCanAffordCount", 10);
			break;
		case NotificationType.Shop_Vehicle_Police_Car:
			image1.gameObject.active = true;
			title.PlayAnim("PoliceCar");
			image1.PlayAnim("PoliceCar");
			centerAlignedText.Text = "Take advantage of the siren-blazing police car, it stops the traffic so you can shoot straight across!";
			priceText.Text = "ONLY " + ShopPrices.GetVehiclePrice(3).ToString("N0") + " CREDITS" + AffordItMessage();
			Combo_SkipBuy();
			PlayerPrefs.SetInt("showCanAffordCount", 10);
			break;
		case NotificationType.Shop_Vehicle_Italiano:
			image1.gameObject.active = true;
			title.PlayAnim("Italiano");
			image1.PlayAnim("Italiano");
			centerAlignedText.Text = "Wow! Crashing one of these beauties will earn you THREE times as much damage! Plus it looks awesome.";
			priceText.Text = "ONLY " + ShopPrices.GetVehiclePrice(5).ToString("N0") + " CREDITS" + AffordItMessage();
			Combo_SkipBuy();
			PlayerPrefs.SetInt("showCanAffordCount", 10);
			break;
		case NotificationType.Shop_Vehicle_Trailer:
			image1.gameObject.active = true;
			title.PlayAnim("Trailer");
			image1.PlayAnim("Trailer");
			centerAlignedText.Text = "Ker-ching! Get double points by safely crossing the Trailer. Well, it is two vehicles in one!";
			priceText.Text = "ONLY " + ShopPrices.GetVehiclePrice(1).ToString("N0") + " CREDITS" + AffordItMessage();
			Combo_SkipBuy();
			PlayerPrefs.SetInt("showCanAffordCount", 10);
			break;
		case NotificationType.Shop_Vehicle_Ambulance:
			image1.gameObject.active = true;
			title.PlayAnim("Ambulance");
			image1.PlayAnim("Ambulance");
			centerAlignedText.Text = "Emergency! The Ambulance stops oncoming traffic, so use it wisely to get vehicles across!";
			priceText.Text = "ONLY " + ShopPrices.GetVehiclePrice(4).ToString("N0") + " CREDITS" + AffordItMessage();
			Combo_SkipBuy();
			PlayerPrefs.SetInt("showCanAffordCount", 10);
			break;
		case NotificationType.Shop_Vehicle_Fuel_Tanker:
			image2.gameObject.active = true;
			title.PlayAnim("FuelTanker");
			image2.PlayAnim("FuelTanker");
			centerAlignedText.Text = "WARNING: Fuel Tankers are EXTREMELY dangerous! Any impact will trigger a massive explosion...So what are you waiting for?!";
			priceText.Text = "ONLY " + ShopPrices.GetVehiclePrice(7).ToString("N0") + " CREDITS" + AffordItMessage();
			Combo_SkipBuy();
			PlayerPrefs.SetInt("showCanAffordCount", 10);
			break;
		case NotificationType.Shop_Vehicle_Fire_Truck:
			image2.gameObject.active = true;
			title.PlayAnim("FireTruck");
			image2.PlayAnim("FireTruck");
			centerAlignedText.Text = "Fire! Fire! These boys are in a rush and will stop the traffic to get through the lights super-quick.";
			priceText.Text = "ONLY " + ShopPrices.GetVehiclePrice(6).ToString("N0") + " CREDITS" + AffordItMessage();
			Combo_SkipBuy();
			PlayerPrefs.SetInt("showCanAffordCount", 10);
			break;
		case NotificationType.Shop_Vehicle_Das_Sporta:
			image2.gameObject.active = true;
			title.PlayAnim("DasSporta");
			image2.PlayAnim("DasSporta");
			centerAlignedText.Text = "A fine piece of German engingeering. Naturally it's worth more! Earn double damage points for crashing these wheels.";
			priceText.Text = "ONLY " + ShopPrices.GetVehiclePrice(2).ToString("N0") + " CREDITS" + AffordItMessage();
			Combo_SkipBuy();
			PlayerPrefs.SetInt("showCanAffordCount", 10);
			break;
		case NotificationType.Shop_Vehicle_Tank:
			image2.gameObject.active = true;
			title.PlayAnim("Tank");
			image2.PlayAnim("Tank");
			centerAlignedText.Text = "The Tank is a vehicle of MASS DESTRUCTION. It will OBLITERATE whatever it crashes into. Bring on the CARNAGE!";
			priceText.Text = "ONLY " + ShopPrices.GetVehiclePrice(8).ToString("N0") + " CREDITS" + AffordItMessage();
			Combo_SkipBuy();
			PlayerPrefs.SetInt("showCanAffordCount", 10);
			break;
		case NotificationType.Shop_City_Gas_Station:
			image1.gameObject.active = true;
			title.PlayAnim("GasStation");
			image1.PlayAnim("GasStation");
			centerAlignedText.Text = "Gas Stations are notoriously flammable. This one is leaking, so it is a disaster waiting to happen. Tick. Tick. BOOM!";
			priceText.Text = "ONLY " + ShopPrices.GetBuildingPrice(1).ToString("N0") + " CREDITS" + AffordItMessage();
			Combo_SkipBuy();
			PlayerPrefs.SetInt("showCanAffordCount", 10);
			break;
		case NotificationType.Shop_City_Demolition_Site:
			image1.gameObject.active = true;
			title.PlayAnim("DemolitionSite");
			image1.PlayAnim("DemolitionSite");
			centerAlignedText.Text = "It's probably not the best idea to suspend a 10 tonne weight over a busy intersection...Oh well, what is the worst that can happen?";
			priceText.Text = "ONLY " + ShopPrices.GetBuildingPrice(3).ToString("N0") + " CREDITS" + AffordItMessage();
			Combo_SkipBuy();
			PlayerPrefs.SetInt("showCanAffordCount", 10);
			break;
		case NotificationType.Shop_City_Car_Showroom:
			image1.gameObject.active = true;
			title.PlayAnim("CarShowroom");
			image1.PlayAnim("CarShowroom");
			centerAlignedText.Text = "This outrageously expensive (and flimsy) glass showroom looks fantastic! Until a car flies through the window...";
			priceText.Text = "ONLY " + ShopPrices.GetBuildingPrice(2).ToString("N0") + " CREDITS" + AffordItMessage();
			Combo_SkipBuy();
			PlayerPrefs.SetInt("showCanAffordCount", 10);
			break;
		case NotificationType.Shop_City_Fireworks_Factory:
			image1.gameObject.active = true;
			title.PlayAnim("FireworksFactory");
			image1.PlayAnim("FireworksFactory");
			centerAlignedText.Text = "If this factory ever caught fire it would be an incredibly violent display...Want to see some REAL fireworks!";
			priceText.Text = "ONLY " + ShopPrices.GetBuildingPrice(0).ToString("N0") + " CREDITS" + AffordItMessage();
			Combo_SkipBuy();
			PlayerPrefs.SetInt("showCanAffordCount", 10);
			break;
		case NotificationType.HiScore:
			break;
		}
	}

	private string AffordItMessage()
	{
		Vector3 position = priceText.transform.position;
		position.y += 10f;
		priceText.transform.position = position;
		switch (UnityEngine.Random.Range(0, 2))
		{
		case 0:
			return "\nYou've got the coins";
		case 1:
			return "\nSpend your coins.";
		case 2:
			return "\nYou can afford it.";
		default:
			return "\nYou can afford it.";
		}
	}

	private void BuyNotification()
	{
		switch (type)
		{
		case NotificationType.Shop_Vehicle_Taxi:
			if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(0))
			{
				string itemName = "Taxi";
				int buildingPrice = ShopPrices.GetVehiclePrice(0);
				HandlePurchasePopUps(itemName, buildingPrice);
			}
			break;
		case NotificationType.Shop_Vehicle_Trailer:
			if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(1))
			{
				string itemName = "Trailer";
				int buildingPrice = ShopPrices.GetVehiclePrice(1);
				HandlePurchasePopUps(itemName, buildingPrice);
			}
			break;
		case NotificationType.Shop_Vehicle_Das_Sporta:
			if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(2))
			{
				string itemName = "Das Sporta";
				int buildingPrice = ShopPrices.GetVehiclePrice(2);
				HandlePurchasePopUps(itemName, buildingPrice);
			}
			break;
		case NotificationType.Shop_Vehicle_Police_Car:
			if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(3))
			{
				string itemName = "Police Car";
				int buildingPrice = ShopPrices.GetVehiclePrice(3);
				HandlePurchasePopUps(itemName, buildingPrice);
			}
			break;
		case NotificationType.Shop_Vehicle_Ambulance:
			if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(4))
			{
				string itemName = "Ambulance";
				int buildingPrice = ShopPrices.GetVehiclePrice(4);
				HandlePurchasePopUps(itemName, buildingPrice);
			}
			break;
		case NotificationType.Shop_Vehicle_Italiano:
			if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(5))
			{
				string itemName = "Italiano";
				int buildingPrice = ShopPrices.GetVehiclePrice(5);
				HandlePurchasePopUps(itemName, buildingPrice);
			}
			break;
		case NotificationType.Shop_Vehicle_Fire_Truck:
			if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(6))
			{
				string itemName = "Fire Truck";
				int buildingPrice = ShopPrices.GetVehiclePrice(6);
				HandlePurchasePopUps(itemName, buildingPrice);
			}
			break;
		case NotificationType.Shop_Vehicle_Fuel_Tanker:
			if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(7))
			{
				string itemName = "Fuel Tanker";
				int buildingPrice = ShopPrices.GetVehiclePrice(7);
				HandlePurchasePopUps(itemName, buildingPrice);
			}
			break;
		case NotificationType.Shop_Vehicle_Tank:
			if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(8))
			{
				string itemName = "Tank";
				int buildingPrice = ShopPrices.GetVehiclePrice(8);
				HandlePurchasePopUps(itemName, buildingPrice);
			}
			break;
		case NotificationType.Shop_City_Fireworks_Factory:
			if (!SingletonBase<TrafficShopItemHandler>.Instance.IsBuildingUnlocked(0))
			{
				string itemName = "Firework Factory";
				int buildingPrice = ShopPrices.GetBuildingPrice(0);
				HandlePurchasePopUps(itemName, buildingPrice);
			}
			break;
		case NotificationType.Shop_City_Gas_Station:
			if (!SingletonBase<TrafficShopItemHandler>.Instance.IsBuildingUnlocked(1))
			{
				string itemName = "Gas Station";
				int buildingPrice = ShopPrices.GetBuildingPrice(1);
				HandlePurchasePopUps(itemName, buildingPrice);
			}
			break;
		case NotificationType.Shop_City_Car_Showroom:
			if (!SingletonBase<TrafficShopItemHandler>.Instance.IsBuildingUnlocked(2))
			{
				string itemName = "Car Showroom";
				int buildingPrice = ShopPrices.GetBuildingPrice(2);
				HandlePurchasePopUps(itemName, buildingPrice);
			}
			break;
		case NotificationType.Shop_City_Demolition_Site:
			if (!SingletonBase<TrafficShopItemHandler>.Instance.IsBuildingUnlocked(3))
			{
				string itemName = "Demolition Site";
				int buildingPrice = ShopPrices.GetBuildingPrice(3);
				HandlePurchasePopUps(itemName, buildingPrice);
			}
			break;
		case NotificationType.InsuranceFraud:
			Debug.Log("Buy insurance fraud");
			PurchaseManager.Instance.Purchase(PurchaseManager.Instance.productIdentifierInsurance);
			break;
		}
	}

	private void HandlePurchasePopUps(string itemName, int price)
	{
		if (CurrencyManager.CanSpendCurrency(price))
		{
			AlertManager.SetDelegate(Alert_BuyItem);
			AlertManager.ShowAlert("Buy " + itemName, "Would you like to unlock the " + itemName + " for " + price.ToString("N0") + " credits? You currently have " + CurrencyManager.GetCurrency().ToString("N0") + " credits.", "Cancel", "Buy");
		}
		else
		{
			int currency = CurrencyManager.GetCurrency();
			string empty = string.Empty;
			string empty2 = string.Empty;
			empty = string.Format("You need {0} more Credits to unlock the {1}. Earn them by playing the game!", (price - currency).ToString("N0"), itemName);
			AlertManager.ShowAlert("Not enough Credits", empty, "Ok");
		}
	}

	private void BuyItem()
	{
		switch (type)
		{
		case NotificationType.Shop_Vehicle_Taxi:
		{
			string text = "Taxi";
			SingletonBase<ShopController>.Instance.purchasedItem(text).purchased = true;
			FlurryController.ReportPieChartEvent("Shop Purchases", "Vehicle", text);
			UnlockAchievement(text);
			SingletonBase<TrafficShopItemHandler>.Instance.UnlockVehicle(0);
			CurrencyManager.RemoveCurrency(ShopPrices.GetVehiclePrice(0));
			ReloadLevel();
			break;
		}
		case NotificationType.Shop_Vehicle_Trailer:
		{
			string text = "Trailer";
			SingletonBase<ShopController>.Instance.purchasedItem(text).purchased = true;
			FlurryController.ReportPieChartEvent("Shop Purchases", "Vehicle", text);
			UnlockAchievement(text);
			SingletonBase<TrafficShopItemHandler>.Instance.UnlockVehicle(1);
			CurrencyManager.RemoveCurrency(ShopPrices.GetVehiclePrice(1));
			ReloadLevel();
			break;
		}
		case NotificationType.Shop_Vehicle_Das_Sporta:
		{
			string text = "Das Sporta";
			SingletonBase<ShopController>.Instance.purchasedItem(text).purchased = true;
			FlurryController.ReportPieChartEvent("Shop Purchases", "Vehicle", text);
			UnlockAchievement(text);
			SingletonBase<TrafficShopItemHandler>.Instance.UnlockVehicle(2);
			CurrencyManager.RemoveCurrency(ShopPrices.GetVehiclePrice(2));
			ReloadLevel();
			break;
		}
		case NotificationType.Shop_Vehicle_Police_Car:
		{
			string text = "Police Car";
			SingletonBase<ShopController>.Instance.purchasedItem(text).purchased = true;
			FlurryController.ReportPieChartEvent("Shop Purchases", "Vehicle", text);
			UnlockAchievement(text);
			SingletonBase<TrafficShopItemHandler>.Instance.UnlockVehicle(3);
			CurrencyManager.RemoveCurrency(ShopPrices.GetVehiclePrice(3));
			ReloadLevel();
			break;
		}
		case NotificationType.Shop_Vehicle_Ambulance:
		{
			string text = "Ambulance";
			SingletonBase<ShopController>.Instance.purchasedItem(text).purchased = true;
			FlurryController.ReportPieChartEvent("Shop Purchases", "Vehicle", text);
			UnlockAchievement(text);
			SingletonBase<TrafficShopItemHandler>.Instance.UnlockVehicle(4);
			CurrencyManager.RemoveCurrency(ShopPrices.GetVehiclePrice(4));
			ReloadLevel();
			break;
		}
		case NotificationType.Shop_Vehicle_Italiano:
		{
			string text = "Italiano";
			SingletonBase<ShopController>.Instance.purchasedItem(text).purchased = true;
			FlurryController.ReportPieChartEvent("Shop Purchases", "Vehicle", text);
			UnlockAchievement(text);
			SingletonBase<TrafficShopItemHandler>.Instance.UnlockVehicle(5);
			CurrencyManager.RemoveCurrency(ShopPrices.GetVehiclePrice(5));
			ReloadLevel();
			break;
		}
		case NotificationType.Shop_Vehicle_Fire_Truck:
		{
			string text = "Fire Truck";
			SingletonBase<ShopController>.Instance.purchasedItem(text).purchased = true;
			FlurryController.ReportPieChartEvent("Shop Purchases", "Vehicle", text);
			UnlockAchievement(text);
			SingletonBase<TrafficShopItemHandler>.Instance.UnlockVehicle(6);
			CurrencyManager.RemoveCurrency(ShopPrices.GetVehiclePrice(6));
			ReloadLevel();
			break;
		}
		case NotificationType.Shop_Vehicle_Fuel_Tanker:
		{
			string text = "Fuel Tanker";
			SingletonBase<ShopController>.Instance.purchasedItem(text).purchased = true;
			FlurryController.ReportPieChartEvent("Shop Purchases", "Vehicle", text);
			UnlockAchievement(text);
			SingletonBase<TrafficShopItemHandler>.Instance.UnlockVehicle(7);
			CurrencyManager.RemoveCurrency(ShopPrices.GetVehiclePrice(7));
			ReloadLevel();
			break;
		}
		case NotificationType.Shop_Vehicle_Tank:
		{
			string text = "Tank";
			SingletonBase<ShopController>.Instance.purchasedItem(text).purchased = true;
			FlurryController.ReportPieChartEvent("Shop Purchases", "Vehicle", text);
			UnlockAchievement(text);
			SingletonBase<TrafficShopItemHandler>.Instance.UnlockVehicle(8);
			CurrencyManager.RemoveCurrency(ShopPrices.GetVehiclePrice(8));
			ReloadLevel();
			break;
		}
		case NotificationType.Shop_City_Fireworks_Factory:
		{
			string text = "Firework Factory";
			SingletonBase<ShopController>.Instance.purchasedItem(text).purchased = true;
			FlurryController.ReportPieChartEvent("Shop Purchases", "City", text);
			UnlockAchievement(text);
			SingletonBase<TrafficShopItemHandler>.Instance.UnlockBuilding(0);
			CurrencyManager.RemoveCurrency(ShopPrices.GetBuildingPrice(0));
			ReloadLevel();
			break;
		}
		case NotificationType.Shop_City_Gas_Station:
		{
			string text = "Gas Station";
			SingletonBase<ShopController>.Instance.purchasedItem(text).purchased = true;
			FlurryController.ReportPieChartEvent("Shop Purchases", "City", text);
			UnlockAchievement(text);
			SingletonBase<TrafficShopItemHandler>.Instance.UnlockBuilding(1);
			CurrencyManager.RemoveCurrency(ShopPrices.GetBuildingPrice(1));
			ReloadLevel();
			break;
		}
		case NotificationType.Shop_City_Car_Showroom:
		{
			string text = "Car Showroom";
			SingletonBase<ShopController>.Instance.purchasedItem(text).purchased = true;
			FlurryController.ReportPieChartEvent("Shop Purchases", "City", text);
			UnlockAchievement(text);
			SingletonBase<TrafficShopItemHandler>.Instance.UnlockBuilding(2);
			CurrencyManager.RemoveCurrency(ShopPrices.GetBuildingPrice(2));
			ReloadLevel();
			break;
		}
		case NotificationType.Shop_City_Demolition_Site:
		{
			string text = "Demolition Site";
			SingletonBase<ShopController>.Instance.purchasedItem(text).purchased = true;
			FlurryController.ReportPieChartEvent("Shop Purchases", "City", text);
			UnlockAchievement(text);
			SingletonBase<TrafficShopItemHandler>.Instance.UnlockBuilding(3);
			CurrencyManager.RemoveCurrency(ShopPrices.GetBuildingPrice(3));
			ReloadLevel();
			break;
		}
		}
		bannerCurrency.Text = CurrencyManager.GetCurrency().ToString("N0");
		Dismiss();
	}

	private void ReloadLevel()
	{
		Dismiss();
		Application.LoadLevel(Application.loadedLevel);
	}

	private void UnlockAchievement(string name)
	{
		switch (name)
		{
		case "Taxi":
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_Taxi);
			break;
		case "Police Car":
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_Police);
			break;
		case "Italiano":
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_Italiano);
			break;
		case "Trailer":
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_Trailer);
			break;
		case "Ambulance":
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_Ambulance);
			break;
		case "Fuel Tanker":
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_FuelTanker);
			break;
		case "Fire Truck":
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_FireTruck);
			break;
		case "Das Sporta":
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_DasSporta);
			break;
		case "Tank":
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_Tank);
			break;
		case "Gas Station":
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_GasStation);
			break;
		case "Demolition Site":
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_DemolitionSite);
			break;
		case "Car Showroom":
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_CarShowroom);
			break;
		case "Firework Factory":
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_FireworksFactory);
			break;
		}
	}

	// En Unity 3.5 un objeto activo se veía aunque su padre estuviera desactivado; desde Unity 4
	// no. Se ocultan todos los botones pero el contenedor sigue activo, o los que se activan
	// después (Skip, Next, Start...) no aparecerían y el aviso no se podría cerrar.
	private void HideButtons()
	{
		buttonParent.SetActiveRecursively(false);
		buttonParent.SetActive(true);
	}

	private void Combo_ZeroStart()
	{
		HideButtons();
		startButton.gameObject.active = true;
	}

	private void Combo_ZeroContinue()
	{
		HideButtons();
		continueButton.gameObject.active = true;
	}

	private void Combo_SkipNext()
	{
		HideButtons();
		skipButton.gameObject.active = true;
		nextButton.gameObject.active = true;
	}

	private void Combo_SkipLike()
	{
		HideButtons();
		skipButton.gameObject.active = true;
		likeButton.gameObject.active = true;
	}

	private void Combo_SkipBuy()
	{
		HideButtons();
		skipButton.gameObject.active = true;
		buyButton.gameObject.active = true;
		if (type == NotificationType.InsuranceFraud)
		{
			priceText.gameObject.active = true;
		}
	}

	private void Combo_SkipUnlock()
	{
		HideButtons();
		skipButton.gameObject.active = true;
		unlockButton.gameObject.active = true;
		priceText.gameObject.active = true;
	}

	public void Alert_BuyItem(object sender, EventArgs e)
	{
		AlertEventArgs alertEventArgs = (AlertEventArgs)e;
		if (alertEventArgs.buttonIndex == 1)
		{
			BuyItem();
		}
	}

	public void Alert_BuyCoins(object sender, EventArgs e)
	{
		AlertEventArgs alertEventArgs = (AlertEventArgs)e;
		if (alertEventArgs.buttonIndex == 1)
		{
			Debug.Log("Buy coins deligate");
			PurchaseManager.Instance.StartSpinnerCoroutine();
			PurchaseManager.Instance.Purchase(PurchaseManager.Instance.productIdentifierUnlockAll);
		}
	}

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (_instance == null)
		{
			_instance = GetComponent<NotificationManager>();
		}
		MyAwake();
	}
}
