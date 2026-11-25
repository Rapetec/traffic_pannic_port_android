using System;
using System.Collections;
using NeonPlay;
using UnityEngine;

public class UIScript_Menu : MonoBehaviour
{
	private static bool vehicleShopVisit;

	public void BringInMenu()
	{
		MenuNav.Instance.BringInMenu();
	}

	public void BringInOptions()
	{
		FlurryController.ReportSingleEvent("Options Pressed");
		MenuNav.Instance.BringInOptions();
	}

	public void BringInShop()
	{
		FlurryController.ReportSingleEvent("Menu Shop Pressed");
		MenuNav.Instance.BringInVehicle();
	}

	public void BringInVehicle()
	{
		FlurryController.ReportSingleEvent("Vehicle Shop Pressed");
		MenuNav.Instance.BringInVehicle();
	}

	public void BringInCity()
	{
		FlurryController.ReportSingleEvent("City Shop Pressed");
		MenuNav.Instance.BringInCity();
	}

	public void BringInTwitter()
	{
		MenuNav.Instance.BringInTwitter();
	}

	public void EnterCreditsFromVehicle()
	{
		vehicleShopVisit = true;
	}

	public void EnterCreditsFromCity()
	{
		vehicleShopVisit = false;
	}

	public void BuyMayor()
	{
		FlurryController.ReportSingleEvent("Unlock All Pressed");
		string empty = string.Empty;
		string empty2 = string.Empty;
		empty = "Would you like to unlock every vehicle and building in the game? (Price and confirmation is on the next screen)";
		empty2 = "Next";
		AlertManager.SetDelegate(Unlock_All);
		AlertManager.ShowAlert("Unlock All", empty, "Cancel", empty2);
	}

	public void InsuranceFraudBuy()
	{
		FlurryController.ReportSingleEvent("Insurance Fraud Pressed");
		if (!SingletonBase<TrafficShopItemHandler>.Instance.InsuranceFraud)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			empty = "Double your score and earn more credits with the Insurance Fraud! (Price and confirmation is on the next screen)";
			empty2 = "Next";
			AlertManager.SetDelegate(Insurance_Fraud);
			AlertManager.ShowAlert("Insurance Fraud", empty, "Cancel", empty2);
		}
	}

	public void PlayGame()
	{
		MenuNav.Instance.LoadingPanel.BringIn();
		FlurryController.ReportSingleEvent("Play Game Pressed");
		FlurryAndroid.onEndSession();
		Application.LoadLevelAsync("Game");
	}

	public void StartMenu()
	{
		MenuNav.Instance.BringInMenu();
		FlurryController.ReportSingleEvent("Start Pressed");
		if (!PurchaseManager.Instance.PurchaseMade)
		{
			ADManager.InitializeAds();
		}
	}

	public void Feedback()
	{
		FlurryController.ReportSingleEvent("Feedback Pressed");
		if (!PurchaseManager.Instance.PurchaseMade)
		{
			ADManager.SetHidden(true);
		}
		EtceteraAndroid.showEmailComposer(SingletonBase<CopyScript>.Instance.GetFeedback.mailAddress, SingletonBase<CopyScript>.Instance.GetFeedback.subject, SingletonBase<CopyScript>.Instance.GetFeedback.body, false);
	}

	public void Review()
	{
		FlurryController.ReportSingleEvent("Review Pressed");
		EtceteraAndroid.showWebView("Traffic Panic 3D - Review", "https://market.android.com/details?id=com.neonplay.trafficpanic2");
	}

	public void ShareEmail()
	{
		SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDEmail);
		FlurryController.ReportSingleEvent("Menu Email Share Pressed");
		EtceteraAndroid.showEmailComposer(SingletonBase<CopyScript>.Instance.GetEmail.mailAddress, SingletonBase<CopyScript>.Instance.GetEmail.subject, SingletonBase<CopyScript>.Instance.GetEmail.body, false);
		if (!PurchaseManager.Instance.PurchaseMade)
		{
			ADManager.SetHidden(true);
		}
	}

	public void RestoreTransactions()
	{
		PurchaseManager.Instance.RestoreTransactions();
		FlurryController.ReportSingleEvent("Restore Transactions Pressed");
	}

	public void MoreGames()
	{
		SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDMoreGames);
		NADSBanner.LaunchMoreGames("OPTIONS SCREEN CUSTOM IMAGE");
	}

	public void mailComposerFinished(string result)
	{
		EtceteraManager.mailComposerFinished -= mailComposerFinished;
		if (!PurchaseManager.Instance.PurchaseMade)
		{
			ADManager.SetHidden(false);
		}
	}

	public void Unlock_All(object sender, EventArgs e)
	{
		AlertEventArgs alertEventArgs = (AlertEventArgs)e;
		if (alertEventArgs.buttonIndex == 1)
		{
			StartCoroutine("ShowActivity");
			PurchaseManager.Instance.Purchase(PurchaseManager.Instance.productIdentifierUnlockAll);
		}
	}

	public void Insurance_Fraud(object sender, EventArgs e)
	{
		AlertEventArgs alertEventArgs = (AlertEventArgs)e;
		if (alertEventArgs.buttonIndex == 1)
		{
			StartCoroutine("ShowActivity");
			PurchaseManager.Instance.Purchase(PurchaseManager.Instance.productIdentifierInsurance);
		}
	}

	private IEnumerator ShowActivity()
	{
		yield return new WaitForSeconds(0.2f);
	}
}
