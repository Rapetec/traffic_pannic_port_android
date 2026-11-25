using System;
using System.Collections.Generic;
using NeonPlay;
using UnityEngine;

public class ChallengeData : SingletonBase<ChallengeData>
{
	public struct Achievement
	{
		public string mainKey;

		public string GCidentifier;

		public string OFidentifier;

		public string imageFilePath;
	}

	public static bool initialized;

	public static string AchievementIDDamage1 = "Damage1";

	public static string AchievementIDDamage2 = "Damage2";

	public static string AchievementIDDamage3 = "Damage3";

	public static string AchievementIDDamage4 = "Damage4";

	public static string AchievementIDDamage5 = "Damage5";

	public static string AchievementIDScore1 = "Score1";

	public static string AchievementIDScore2 = "Score2";

	public static string AchievementIDScore3 = "Score3";

	public static string AchievementIDScore4 = "Score4";

	public static string AchievementIDScore5 = "Score5";

	public static string AchievementIDFacebookPost = "FacebookPost";

	public static string AchievementIDFacebookLike = "FacebookLike";

	public static string AchievementIDTwitterPost = "TwitterPost";

	public static string AchievementIDEmail = "Email";

	public static string AchievementIDMoreGames = "MoreGames";

	public static string AchievementIDShop_Taxi = "ShopTaxi";

	public static string AchievementIDShop_Police = "ShopPolice";

	public static string AchievementIDShop_Italiano = "ShopItaliano";

	public static string AchievementIDShop_Trailer = "ShopTrailer";

	public static string AchievementIDShop_Ambulance = "ShopAmbulance";

	public static string AchievementIDShop_FuelTanker = "ShopFuelTanker";

	public static string AchievementIDShop_FireTruck = "ShopFireTruck";

	public static string AchievementIDShop_DasSporta = "ShopDasSporta";

	public static string AchievementIDShop_Tank = "ShopTank";

	public static string AchievementIDShop_GasStation = "ShopGasStation";

	public static string AchievementIDShop_DemolitionSite = "ShopDemolitionSite";

	public static string AchievementIDShop_CarShowroom = "ShopCarShowroom";

	public static string AchievementIDShop_FireworksFactory = "ShopFireworksFactory";

	public static string AchievementIDPyromaniac = "Pyromaniac";

	public static string AchievementIDShowTime = "ShowTime";

	public static string AchievementIDKaboom = "Kaboom";

	public static string AchievementIDMerciless = "Merciless";

	public static string AchievementIDPendulum = "Pendulum";

	public static string AchievementIDGlasshouse = "GlassHouse";

	public static string AchievementIDPetrolBomb = "PetrolBomb";

	public static string AchievementIDHolidayNightmare = "HolidayNightmare";

	public static string AchievementIDScrapMetal = "ScrapMetal";

	public static string AchievementIDDelivery = "Delivery";

	public static string AchievementID911 = "911";

	public static string AchievementIDFireman = "Fireman";

	public static string AchievementIDCabby = "Cabby";

	public static string AchievementIDParamedic = "Paramedic";

	public static string AchievementIDGoGoGo = "GoGoGo";

	public static string AchievementIDMaster = "TrafficMaster";

	public int maxAchievements = 40;

	private Dictionary<string, Achievement> structFromKey;

	private static OpenFeintFacade openFeint;

	public static readonly string appName = "Traffic Panic 3D";

	public static readonly string id = "385173";

	public static readonly string key = "LK7W8fbUplOMmLDLATnNSQ";

	public static readonly string secret = "dzpHzRkhApBsRgAmFCwLjAVnCbQNzVo6wqGZdeqi1M";

	public static readonly string AndroidLeaderIDScore = "996786";

	public static readonly string AndroidLeaderIDDamage = "996776";

	private ChallengeData()
	{
		if (!initialized)
		{
			initialized = true;
			openFeint = new OpenFeintFacade();
		}
		structFromKey = new Dictionary<string, Achievement>();
		SetUpAchievement(AchievementIDDamage1, "com.neonplay.trafficpanic3d.damage.1", "1392892", "AchievementIcons/" + AchievementIDDamage1);
		SetUpAchievement(AchievementIDDamage2, "com.neonplay.trafficpanic3d.damage.2", "1393012", "AchievementIcons/" + AchievementIDDamage2);
		SetUpAchievement(AchievementIDDamage3, "com.neonplay.trafficpanic3d.damage.3", "1395102", "AchievementIcons/" + AchievementIDDamage3);
		SetUpAchievement(AchievementIDDamage4, "com.neonplay.trafficpanic3d.damage.4", "1392932", "AchievementIcons/" + AchievementIDDamage4);
		SetUpAchievement(AchievementIDDamage5, "com.neonplay.trafficpanic3d.damage.5", "1392902", "AchievementIcons/" + AchievementIDDamage5);
		SetUpAchievement(AchievementIDScore1, "com.neonplay.trafficpanic2.score.1000points", "1393022", "AchievementIcons/" + AchievementIDScore1);
		SetUpAchievement(AchievementIDScore2, "com.neonplay.trafficpanic2.score.5000points", "1393042", "AchievementIcons/" + AchievementIDScore2);
		SetUpAchievement(AchievementIDScore3, "com.neonplay.trafficpanic2.score.15000points", "1393072", "AchievementIcons/" + AchievementIDScore3);
		SetUpAchievement(AchievementIDScore4, "com.neonplay.trafficpanic2.score.50000points", "1393092", "AchievementIcons/" + AchievementIDScore4);
		SetUpAchievement(AchievementIDScore5, "com.neonplay.trafficpanic2.score.100000points", "1393112", "AchievementIcons/" + AchievementIDScore5);
		SetUpAchievement(AchievementIDFacebookPost, "com.neonplay.trafficpanic2.facebook", "1393132", "AchievementIcons/" + AchievementIDFacebookPost);
		SetUpAchievement(AchievementIDFacebookLike, "com.neonplay.trafficpanic2.facebook.like", "1393152", "AchievementIcons/" + AchievementIDFacebookLike);
		SetUpAchievement(AchievementIDTwitterPost, "com.neonplay.trafficpanic2.twitter", "1393162", "AchievementIcons/" + AchievementIDTwitterPost);
		SetUpAchievement(AchievementIDEmail, "com.neonplay.trafficpanic2.email", "1393202", "AchievementIcons/" + AchievementIDEmail);
		SetUpAchievement(AchievementIDMoreGames, "com.neonplay.trafficpanic2.moregames", "1393222", "AchievementIcons/" + AchievementIDMoreGames);
		SetUpAchievement(AchievementIDShop_Taxi, "com.neonplay.trafficpanic2.shoptaxi", "1384632", "AchievementIcons/" + AchievementIDShop_Taxi);
		SetUpAchievement(AchievementIDShop_Police, "com.neonplay.trafficpanic2.shoppolice", "1388382", "AchievementIcons/" + AchievementIDShop_Police);
		SetUpAchievement(AchievementIDShop_Italiano, "com.neonplay.trafficpanic2.shopitaliano", "1388392", "AchievementIcons/" + AchievementIDShop_Italiano);
		SetUpAchievement(AchievementIDShop_Trailer, "com.neonplay.trafficpanic2.shoptrailer", "1388402", "AchievementIcons/" + AchievementIDShop_Trailer);
		SetUpAchievement(AchievementIDShop_Ambulance, "com.neonplay.trafficpanic2.shopambulance", "1388412", "AchievementIcons/" + AchievementIDShop_Ambulance);
		SetUpAchievement(AchievementIDShop_FuelTanker, "com.neonplay.trafficpanic2.shopfueltanker", "1388422", "AchievementIcons/" + AchievementIDShop_FuelTanker);
		SetUpAchievement(AchievementIDShop_FireTruck, "com.neonplay.trafficpanic2.shopfiretruck", "1388432", "AchievementIcons/" + AchievementIDShop_FireTruck);
		SetUpAchievement(AchievementIDShop_DasSporta, "com.neonplay.trafficpanic2.shopdassporta", "1388442", "AchievementIcons/" + AchievementIDShop_DasSporta);
		SetUpAchievement(AchievementIDShop_Tank, "com.neonplay.trafficpanic2.shoptank", "1388452", "AchievementIcons/" + AchievementIDShop_Tank);
		SetUpAchievement(AchievementIDShop_GasStation, "com.neonplay.trafficpanic2.refuel", "1392822", "AchievementIcons/" + AchievementIDShop_GasStation);
		SetUpAchievement(AchievementIDShop_DemolitionSite, "com.neonplay.trafficpanic2.crazycrane", "1392832", "AchievementIcons/" + AchievementIDShop_DemolitionSite);
		SetUpAchievement(AchievementIDShop_CarShowroom, "com.neonplay.trafficpanic2.carsalesman", "1392872", "AchievementIcons/" + AchievementIDShop_CarShowroom);
		SetUpAchievement(AchievementIDShop_FireworksFactory, "com.neonplay.trafficpanic2.rocketpower", "1392882", "AchievementIcons/" + AchievementIDShop_FireworksFactory);
		SetUpAchievement(AchievementIDPyromaniac, "com.neonplay.trafficpanic2.pyromaniac", "1393242", "AchievementIcons/" + AchievementIDPyromaniac);
		SetUpAchievement(AchievementIDShowTime, "com.neonplay.trafficpanic2.showtime", "1393262", "AchievementIcons/" + AchievementIDShowTime);
		SetUpAchievement(AchievementIDKaboom, "com.neonplay.trafficpanic2.kaboom", "1393292", "AchievementIcons/" + AchievementIDKaboom);
		SetUpAchievement(AchievementIDMerciless, "com.neonplay.trafficpanic2.merciless", "1393302", "AchievementIcons/" + AchievementIDMerciless);
		SetUpAchievement(AchievementIDPendulum, "com.neonplay.trafficpanic2.pendulum", "1393312", "AchievementIcons/" + AchievementIDPendulum);
		SetUpAchievement(AchievementIDGlasshouse, "com.neonplay.trafficpanic2.glasshouse", "1393332", "AchievementIcons/" + AchievementIDGlasshouse);
		SetUpAchievement(AchievementIDPetrolBomb, "com.neonplay.trafficpanic2.petrolbomb", "1393372", "AchievementIcons/" + AchievementIDPetrolBomb);
		SetUpAchievement(AchievementIDHolidayNightmare, "com.neonplay.trafficpanic2.holidaynightmare", "1393382", "AchievementIcons/" + AchievementIDHolidayNightmare);
		SetUpAchievement(AchievementIDScrapMetal, "com.neonplay.trafficpanic2.scrapmetal", "1393392", "AchievementIcons/" + AchievementIDScrapMetal);
		SetUpAchievement(AchievementIDDelivery, "com.neonplay.trafficpanic2.delivery", "1393402", "AchievementIcons/" + AchievementIDDelivery);
		SetUpAchievement(AchievementID911, "com.neonplay.trafficpanic2.911", "1393412", "AchievementIcons/" + AchievementID911);
		SetUpAchievement(AchievementIDFireman, "com.neonplay.trafficpanic2.fireman", "1393422", "AchievementIcons/" + AchievementIDFireman);
		SetUpAchievement(AchievementIDCabby, "com.neonplay.trafficpanic2.cabby", "1393432", "AchievementIcons/" + AchievementIDCabby);
		SetUpAchievement(AchievementIDParamedic, "com.neonplay.trafficpanic2.paramedic", "1393442", "AchievementIcons/" + AchievementIDParamedic);
		SetUpAchievement(AchievementIDGoGoGo, "com.neonplay.trafficpanic2.gogogo", "1393452", "AchievementIcons/" + AchievementIDGoGoGo);
		SetUpAchievement(AchievementIDMaster, "com.neonplay.trafficpanic2.trafficmaster", "1393462", "AchievementIcons/" + AchievementIDMaster);
	}

	private void SetUpAchievement(string mainKey, string GCIdentifier, string OFIdentifier, string imageFilePath)
	{
		Achievement achievement = default(Achievement);
		achievement.mainKey = mainKey;
		achievement.GCidentifier = GCIdentifier;
		achievement.OFidentifier = OFIdentifier;
		achievement.imageFilePath = imageFilePath;
		AddAchievementToStruct(achievement);
	}

	private void AddAchievementToStruct(Achievement achievement)
	{
		structFromKey.Add(achievement.mainKey, achievement);
	}

	public Achievement RetrieveStructsWithKey(string key)
	{
		return structFromKey[key];
	}

	public void ReportAchievement(string dataClassID)
	{
		if (openFeint.isInitialized() && openFeint.isUserLoggedIn())
		{
			string oFidentifier = RetrieveStructsWithKey(dataClassID).OFidentifier;
			int achievementID = int.Parse(oFidentifier);
			openFeint.UnlockAchievement(achievementID);
		}
	}

	public void ReportLeaderBoardScores(int score, int damage)
	{
		if (openFeint.isInitialized() && openFeint.isUserLoggedIn())
		{
			openFeint.SubmitScore(AndroidLeaderIDScore, score);
			openFeint.SubmitScore(AndroidLeaderIDDamage, damage);
		}
	}

	public void AskLoginOF()
	{
		AlertManager.SetDelegate(PopUp_OF);
		AlertManager.ShowAlert("Enable OpenFeint", "You're just one click away from gaming with your friends!", "No thanks. I don't like fun.", "Enable & Start playing!");
	}

	public void LoginOF()
	{
		if (!openFeint.isUserLoggedIn() && !openFeint.isInitialized())
		{
			openFeint.Init(appName, key, secret, id, 1);
		}
	}

	public void PopUp_OF(object sender, EventArgs e)
	{
		AlertEventArgs alertEventArgs = (AlertEventArgs)e;
		if (alertEventArgs.buttonIndex == 1)
		{
			PlayerPrefs.SetInt("OFEnabled", 1);
			LoginOF();
		}
	}
}
