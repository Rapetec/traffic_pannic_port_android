using System;
using System.Collections;
using NeonPlay;
using UnityEngine;

public class UIScript_ShopItem : MonoBehaviour
{
	private int price;

	public bool unlocked;

	public bool vehicle;

	public bool building;

	public bool unlockAll;

	public PackedSprite lockSprite;

	public SpriteText priceText;

	public PackedSprite selectionSprite;

	public string itemName = "Un-assigned item name";

	private int index;

	private AudioSource purchaseSource;

	private SpriteText bannerCurrency;

	private void Start()
	{
		char[] array = base.transform.parent.name.ToCharArray();
		string text = string.Empty;
		for (int i = 0; i < array.Length && char.IsNumber(array[i]); i++)
		{
			text += array[i];
		}
		if (!unlockAll)
		{
			index = Convert.ToInt32(text);
		}
		if (vehicle)
		{
			price = ShopPrices.GetVehiclePrice(index);
		}
		else if (building)
		{
			price = ShopPrices.GetBuildingPrice(index);
		}
		else if (unlockAll)
		{
			price = ShopPrices.UnlockAll;
		}
		if (SingletonBase<TrafficShopItemHandler>.Instance.UnlockAll)
		{
			SingletonBase<ShopController>.Instance.purchasedItem(itemName).purchased = true;
		}
		if (!unlocked)
		{
			unlocked = SingletonBase<ShopController>.Instance.purchasedItem(itemName).purchased;
		}
		if (unlocked)
		{
			lockSprite.PlayAnim(1);
			priceText.Text = string.Empty;
		}
		else
		{
			priceText.Text = price.ToString("N0") + " CREDITS";
			selectionSprite.PlayAnim(0);
		}
		bannerCurrency = GameObject.FindWithTag("BannerCurrency").GetComponent<SpriteText>();
		purchaseSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (!unlocked)
		{
			if (SingletonBase<TrafficShopItemHandler>.Instance.UnlockAll)
			{
				unlocked = true;
				if (vehicle)
				{
					SingletonBase<TrafficShopItemHandler>.Instance.UnlockVehicle(index);
				}
				else if (building)
				{
					SingletonBase<TrafficShopItemHandler>.Instance.UnlockBuilding(index);
				}
				SingletonBase<ShopController>.Instance.purchasedItem(itemName).purchased = true;
			}
		}
		else
		{
			lockSprite.PlayAnim(1);
			priceText.Text = string.Empty;
			ChangeSelectedSprite();
		}
	}

	private void Buy()
	{
		if (!unlocked)
		{
			if (CurrencyManager.CanSpendCurrency(price))
			{
				AlertManager.SetDelegate(PopUp_BuyItems);
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
		else if (vehicle)
		{
			SingletonBase<TrafficShopItemHandler>.Instance.ToggleSelectedVehicle(index);
		}
		else if (building)
		{
			SingletonBase<TrafficShopItemHandler>.Instance.ToggleSelectedBuilding(index);
		}
		else if (unlockAll)
		{
			SingletonBase<TrafficShopItemHandler>.Instance.ToggleSelectedUnlockAll();
		}
	}

	public void SetBought()
	{
		FlurryController.ReportPieChartEvent("Shop Purchases", (!vehicle) ? "City" : "Vehicle", itemName);
		CurrencyManager.RemoveCurrency(price);
		bannerCurrency.Text = CurrencyManager.GetCurrency().ToString("N0");
		lockSprite.PlayAnim(1);
		priceText.Text = string.Empty;
		SingletonBase<ShopController>.Instance.purchasedItem(itemName).purchased = true;
		unlocked = true;
		PlayAudio(true);
		SetUnlocked();
		UnlockAchievement();
	}

	private void UnlockAchievement()
	{
		switch (itemName)
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

	private void SetUnlocked()
	{
		if (vehicle)
		{
			SingletonBase<TrafficShopItemHandler>.Instance.UnlockVehicle(index);
		}
		else if (building)
		{
			SingletonBase<TrafficShopItemHandler>.Instance.UnlockBuilding(index);
		}
		else if (unlockAll)
		{
			SingletonBase<TrafficShopItemHandler>.Instance.UnlockAll = true;
			SingletonBase<TrafficShopItemHandler>.Instance.ToggleSelectedUnlockAll();
		}
	}

	private void PlayAudio(bool bought)
	{
		if (bought)
		{
			purchaseSource.clip = (AudioClip)Resources.Load("Menu/Success");
		}
		else
		{
			purchaseSource.clip = (AudioClip)Resources.Load("Menu/Fail");
		}
		purchaseSource.Play();
	}

	private void ChangeSelectedSprite()
	{
		if (vehicle)
		{
			if (SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleSelected(index))
			{
				selectionSprite.PlayAnim(1);
			}
			else
			{
				selectionSprite.PlayAnim(2);
			}
		}
		else if (building)
		{
			if (SingletonBase<TrafficShopItemHandler>.Instance.IsBuildingSelected(index))
			{
				selectionSprite.PlayAnim(1);
			}
			else
			{
				selectionSprite.PlayAnim(2);
			}
		}
		else if (unlockAll)
		{
			if (SingletonBase<TrafficShopItemHandler>.Instance.UnlockAllSelected)
			{
				selectionSprite.PlayAnim(1);
			}
			else
			{
				selectionSprite.PlayAnim(2);
			}
		}
	}

	public void PopUp_BuyItems(object sender, EventArgs e)
	{
		AlertEventArgs alertEventArgs = (AlertEventArgs)e;
		if (alertEventArgs.buttonIndex == 0)
		{
			PlayAudio(false);
		}
		else if (alertEventArgs.buttonIndex == 1)
		{
			Debug.Log("BOUGHT");
			SetBought();
		}
	}

	public void PopUp_BuyCoins(object sender, EventArgs e)
	{
		AlertEventArgs alertEventArgs = (AlertEventArgs)e;
		if (alertEventArgs.buttonIndex == 0)
		{
			PlayAudio(false);
		}
		else if (alertEventArgs.buttonIndex == 1)
		{
			StartCoroutine("ShowActivity");
			PurchaseManager.Instance.Purchase(PurchaseManager.Instance.productIdentifierUnlockAll);
		}
	}

	private IEnumerator ShowActivity()
	{
		yield return new WaitForSeconds(0.2f);
		EtceteraBinding.showBezelActivityViewWithLabel("Please wait. Connecting to the App Store.");
	}
}
