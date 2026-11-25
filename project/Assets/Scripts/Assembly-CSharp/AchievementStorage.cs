using NeonPlay;
using UnityEngine;

public sealed class AchievementStorage : SingletonBase<AchievementStorage>
{
	private int STARTvehiclesOnFire;

	private int STARTfuelTankersHit;

	private int STARTtanksHit;

	private int STARTcraneHit;

	private int STARTshowroomHit;

	private int STARTstationHit;

	private int STARTcaravanHit;

	private int STARTgameOvers;

	private int STARTlorryNoStop;

	private int STARTpoliceNoStop;

	private int STARTfireTruckNoStop;

	private int STARTtaxiNoStop;

	private int STARTambulanceNoStop;

	private int STARTtotalNoStop;

	private int vehiclesOnFire;

	private int fuelTankersHit;

	private int tanksHit;

	private int craneHit;

	private int showroomHit;

	private int stationHit;

	private int caravanHit;

	private int gameOvers;

	private int lorryNoStop;

	private int policeNoStop;

	private int fireTruckNoStop;

	private int taxiNoStop;

	private int ambulanceNoStop;

	private int totalNoStop;

	public int VehiclesOnFire
	{
		get
		{
			return vehiclesOnFire;
		}
		set
		{
			vehiclesOnFire = value;
		}
	}

	public int FuelTankerHit
	{
		get
		{
			return fuelTankersHit;
		}
		set
		{
			fuelTankersHit = value;
		}
	}

	public int TankHit
	{
		get
		{
			return tanksHit;
		}
		set
		{
			tanksHit = value;
		}
	}

	public int CraneHit
	{
		get
		{
			return craneHit;
		}
		set
		{
			craneHit = value;
		}
	}

	public int ShowroomHit
	{
		get
		{
			return showroomHit;
		}
		set
		{
			showroomHit = value;
		}
	}

	public int StationHit
	{
		get
		{
			return stationHit;
		}
		set
		{
			stationHit = value;
		}
	}

	public int CaravanHit
	{
		get
		{
			return caravanHit;
		}
		set
		{
			caravanHit = value;
		}
	}

	public int GameOvers
	{
		get
		{
			return gameOvers;
		}
		set
		{
			gameOvers = value;
		}
	}

	public int LorryNoStop
	{
		get
		{
			return lorryNoStop;
		}
		set
		{
			lorryNoStop = value;
		}
	}

	public int PoliceNoStop
	{
		get
		{
			return policeNoStop;
		}
		set
		{
			policeNoStop = value;
		}
	}

	public int FireTruckNoStop
	{
		get
		{
			return fireTruckNoStop;
		}
		set
		{
			fireTruckNoStop = value;
		}
	}

	public int TaxiNoStop
	{
		get
		{
			return taxiNoStop;
		}
		set
		{
			taxiNoStop = value;
		}
	}

	public int AmbulanceNoStop
	{
		get
		{
			return ambulanceNoStop;
		}
		set
		{
			ambulanceNoStop = value;
		}
	}

	public int TotalNoStop
	{
		get
		{
			return totalNoStop;
		}
		set
		{
			totalNoStop = value;
		}
	}

	private AchievementStorage()
	{
		STARTvehiclesOnFire = (vehiclesOnFire = PlayerPrefs.GetInt("VehiclesOnFire"));
		STARTfuelTankersHit = (fuelTankersHit = PlayerPrefs.GetInt("TankerHit"));
		STARTtanksHit = (tanksHit = PlayerPrefs.GetInt("TankHit"));
		STARTcraneHit = (craneHit = PlayerPrefs.GetInt("CraneHit"));
		STARTshowroomHit = (showroomHit = PlayerPrefs.GetInt("ShowroomHit"));
		STARTstationHit = (stationHit = PlayerPrefs.GetInt("StationHit"));
		STARTcaravanHit = (caravanHit = PlayerPrefs.GetInt("CaravanHit"));
		STARTgameOvers = (gameOvers = PlayerPrefs.GetInt("GameOvers"));
		STARTlorryNoStop = (lorryNoStop = PlayerPrefs.GetInt("LorryNoStop"));
		STARTpoliceNoStop = (policeNoStop = PlayerPrefs.GetInt("PoliceNoStop"));
		STARTfireTruckNoStop = (fireTruckNoStop = PlayerPrefs.GetInt("FireTruckNoStop"));
		STARTtaxiNoStop = (taxiNoStop = PlayerPrefs.GetInt("TaxiNoStop"));
		STARTambulanceNoStop = (ambulanceNoStop = PlayerPrefs.GetInt("AmbulanceNoStop"));
		STARTtotalNoStop = (totalNoStop = PlayerPrefs.GetInt("TotalNoStop"));
	}

	public void ReportAccumulations()
	{
		if (vehiclesOnFire > STARTvehiclesOnFire)
		{
			PlayerPrefs.SetInt("VehiclesOnFire", vehiclesOnFire);
			if (vehiclesOnFire >= 100)
			{
				SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDPyromaniac);
			}
		}
		if (fuelTankersHit > STARTfuelTankersHit)
		{
			PlayerPrefs.SetInt("TankerHit", fuelTankersHit);
			if (fuelTankersHit >= 100)
			{
				SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDKaboom);
			}
		}
		if (tanksHit > STARTtanksHit)
		{
			PlayerPrefs.SetInt("TankHit", tanksHit);
			if (tanksHit >= 10)
			{
				SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDMerciless);
			}
		}
		if (craneHit > STARTcraneHit)
		{
			PlayerPrefs.SetInt("CraneHit", craneHit);
			if (craneHit >= 15)
			{
				SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDPendulum);
			}
		}
		if (showroomHit > STARTshowroomHit)
		{
			PlayerPrefs.SetInt("ShowroomHit", showroomHit);
			if (showroomHit >= 5)
			{
				SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDGlasshouse);
			}
		}
		if (stationHit > STARTstationHit)
		{
			PlayerPrefs.SetInt("StationHit", stationHit);
			if (stationHit >= 7)
			{
				SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDPetrolBomb);
			}
		}
		if (caravanHit > STARTcaravanHit)
		{
			PlayerPrefs.SetInt("CaravanHit", caravanHit);
			if (caravanHit >= 50)
			{
				SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDHolidayNightmare);
			}
		}
		if (gameOvers > STARTgameOvers)
		{
			PlayerPrefs.SetInt("GameOvers", gameOvers);
			if (gameOvers >= 500)
			{
				SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDScrapMetal);
			}
		}
		if (lorryNoStop > STARTlorryNoStop)
		{
			PlayerPrefs.SetInt("LorryNoStop", lorryNoStop);
			if (lorryNoStop >= 25)
			{
				SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDDelivery);
			}
		}
		if (policeNoStop > STARTpoliceNoStop)
		{
			PlayerPrefs.SetInt("PoliceNoStop", policeNoStop);
			if (policeNoStop >= 100)
			{
				SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementID911);
			}
		}
		if (fireTruckNoStop > STARTfireTruckNoStop)
		{
			PlayerPrefs.SetInt("FireTruckNoStop", fireTruckNoStop);
			if (fireTruckNoStop >= 50)
			{
				SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDFireman);
			}
		}
		if (taxiNoStop > STARTtaxiNoStop)
		{
			PlayerPrefs.SetInt("TaxiNoStop", taxiNoStop);
			if (taxiNoStop >= 75)
			{
				SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDCabby);
			}
		}
		if (ambulanceNoStop > STARTambulanceNoStop)
		{
			PlayerPrefs.SetInt("AmbulanceNoStop", ambulanceNoStop);
			if (ambulanceNoStop >= 25)
			{
				SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDParamedic);
			}
		}
		if (totalNoStop > STARTtotalNoStop)
		{
			PlayerPrefs.SetInt("TotalNoStop", totalNoStop);
			if (totalNoStop >= 500)
			{
				SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDGoGoGo);
			}
		}
	}
}
