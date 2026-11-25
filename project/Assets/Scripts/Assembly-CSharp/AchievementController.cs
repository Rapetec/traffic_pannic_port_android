using System;
using NeonPlay;
using UnityEngine;

public class AchievementController : MonoBehaviour
{
	private const string appName = "Traffic Panic 3D";

	private const string id = "385173";

	private const string key = "LK7W8fbUplOMmLDLATnNSQ";

	private const string secret = "dzpHzRkhApBsRgAmFCwLjAVnCbQNzVo6wqGZdeqi1M";

	public static bool iPhone = true;

	public static string AchievementIDScore1 = "com.neonplay.trafficpanic2.score.1000points";

	public static string AchievementIDScore2 = "com.neonplay.trafficpanic2.score.5000points";

	public static string AchievementIDScore3 = "com.neonplay.trafficpanic2.score.15000points";

	public static string AchievementIDScore4 = "com.neonplay.trafficpanic2.score.50000points";

	public static string AchievementIDScore5 = "com.neonplay.trafficpanic2.score.100000points";

	public static string AchievementIDFacebookPost = "com.neonplay.trafficpanic3d.facebook";

	public static string AchievementIDFacebookLike = "com.neonplay.trafficpanic3d.facebook.like";

	public static string AchievementIDTwitterPost = "com.neonplay.trafficpanic3d.twitter";

	public static string AchievementIDEmail = "com.neonplay.trafficpanic3d.email";

	public static string AchievementIDMoreGames = "com.neonplay.trafficpanic3d.moregames";

	public static string AchievementIDShop_GasStation = "com.neonplay.trafficpanic2.refuel";

	public static string AchievementIDShop_DemolitionSite = "com.neonplay.trafficpanic2.crazycrane";

	public static string AchievementIDShop_CarShowroom = "com.neonplay.trafficpanic2.carsalesman";

	public static string AchievementIDShop_FireworksFactory = "com.neonplay.trafficpanic2.rocketpower";

	public static string AchievementIDPyromaniac = "com.neonplay.trafficpanic2.pyromaniac";

	public static string AchievementIDShowTime = "com.neonplay.trafficpanic2.showtime";

	public static string AchievementIDKaboom = "com.neonplay.trafficpanic2.kaboom";

	public static string AchievementIDMerciless = "com.neonplay.trafficpanic2.merciless";

	public static string AchievementIDPendulum = "com.neonplay.trafficpanic2.pendulum";

	public static string AchievementIDGlasshouse = "com.neonplay.trafficpanic2.glasshouse";

	public static string AchievementIDPetrolBomb = "com.neonplay.trafficpanic2.petrolbomb";

	public static string AchievementIDHolidayNightmare = "com.neonplay.trafficpanic2.holidaynightmare";

	public static string AchievementIDScrapMetal = "com.neonplay.trafficpanic2.scrapmetal";

	public static string AchievementIDDelivery = "com.neonplay.trafficpanic2.delivery";

	public static string AchievementID911 = "com.neonplay.trafficpanic2.911";

	public static string AchievementIDFireman = "com.neonplay.trafficpanic2.fireman";

	public static string AchievementIDCabby = "com.neonplay.trafficpanic2.cabby";

	public static string AchievementIDParamedic = "com.neonplay.trafficpanic2.paramedic";

	public static string AchievementIDGoGoGo = "com.neonplay.trafficpanic2.gogogo";

	public static string AchievementIDMaster = "com.neonplay.trafficpanic2.trafficmaster";

	public GameObject openFeintButton;

	public UIScrollList list;

	public GameObject itemObj;

	public SpriteText dialogue;

	private IUIListObject newItem;

	private bool tableUpdate;

	private int maxAchievements = 43;

	private int completedA;

	private int incompleteA;

	private int totalA;

	private bool doneInitialisation;

	private static OpenFeintFacade openFeint;

	private bool displayed;

	public static string AchievementIDDamage1
	{
		get
		{
			return (!iPhone) ? "1234" : "com.neonplay.trafficpanic3d.damage.1";
		}
	}

	public static string AchievementIDDamage2
	{
		get
		{
			return (!iPhone) ? "1235" : "com.neonplay.trafficpanic3d.damage.2";
		}
	}

	public static string AchievementIDDamage3
	{
		get
		{
			return (!iPhone) ? "1236" : "com.neonplay.trafficpanic3d.damage.3";
		}
	}

	public static string AchievementIDDamage4
	{
		get
		{
			return (!iPhone) ? "1237" : "com.neonplay.trafficpanic3d.damage.4";
		}
	}

	public static string AchievementIDDamage5
	{
		get
		{
			return (!iPhone) ? "1238" : "com.neonplay.trafficpanic3d.damage.5";
		}
	}

	public static string AchievementIDShop_Taxi
	{
		get
		{
			return (!iPhone) ? "1384632" : "com.neonplay.trafficpanic2.shoptaxi";
		}
	}

	public static string AchievementIDShop_Police
	{
		get
		{
			return (!iPhone) ? "1388382" : "com.neonplay.trafficpanic2.shoppolice";
		}
	}

	public static string AchievementIDShop_Italiano
	{
		get
		{
			return (!iPhone) ? "1388392" : "com.neonplay.trafficpanic2.shopitaliano";
		}
	}

	public static string AchievementIDShop_Trailer
	{
		get
		{
			return (!iPhone) ? "1388402" : "com.neonplay.trafficpanic2.shoptrailer";
		}
	}

	public static string AchievementIDShop_Ambulance
	{
		get
		{
			return (!iPhone) ? "1388412" : "com.neonplay.trafficpanic2.shopambulance";
		}
	}

	public static string AchievementIDShop_FuelTanker
	{
		get
		{
			return (!iPhone) ? "1388422" : "com.neonplay.trafficpanic2.shopfueltanker";
		}
	}

	public static string AchievementIDShop_FireTruck
	{
		get
		{
			return (!iPhone) ? "1388432" : "com.neonplay.trafficpanic2.shopfiretruck";
		}
	}

	public static string AchievementIDShop_DasSporta
	{
		get
		{
			return (!iPhone) ? "1388442" : "com.neonplay.trafficpanic2.shopdassporta";
		}
	}

	public static string AchievementIDShop_Tank
	{
		get
		{
			return (!iPhone) ? "1388452" : "com.neonplay.trafficpanic2.shoptank";
		}
	}

	private void OnDestroy()
	{
	}

	private void Awake()
	{
		iPhone = false;
	}

	private void Start()
	{
		openFeint = new OpenFeintFacade();
		Initialise();
	}

	private void Initialise()
	{
		if (!doneInitialisation)
		{
			doneInitialisation = true;
			MonoBehaviour.print("Initialised Achievements controller");
			totalA = 0;
			incompleteA = 0;
			completedA = 0;
			if (!openFeint.isUserLoggedIn())
			{
				dialogue.Text = "LOG IN TO OPEN FEINT TO RETRIEVE ACHIEVEMENTS";
			}
		}
	}

	private void GetData()
	{
		list.ClearList(true);
		dialogue.Text = SingletonBase<CopyScript>.Instance.GetAchievements.retrieving;
	}

	private void Update()
	{
		if (tableUpdate)
		{
			tableUpdate = false;
			dialogue.Text = string.Empty;
			AddListItem();
		}
		else if (openFeint.isUserLoggedIn() && !displayed)
		{
			displayed = true;
			tableUpdate = true;
		}
	}

	private void AddListItem()
	{
		AndroidJavaObject achievements = openFeint.GetAchievements();
		if (achievements == null)
		{
			return;
		}
		totalA = achievements.Call<int>("size", new object[0]);
		for (int i = 0; i < totalA; i++)
		{
			AndroidJavaObject androidJavaObject = achievements.Call<AndroidJavaObject>("get", new object[1] { i });
			newItem = list.CreateItem(itemObj, string.Empty);
			AchievementItem component = newItem.gameObject.GetComponent<AchievementItem>();
			component.title.Text = androidJavaObject.Get<string>("title");
			component.description.Text = androidJavaObject.Get<string>("description");
			if (androidJavaObject.Get<bool>("isUnlocked"))
			{
				completedA++;
				iPhone = true;
				iPhone = false;
				Texture2D texture2D = (Texture2D)Resources.Load("AchievementIcons/LOCKED");
				if (texture2D != null)
				{
					component.image.SetTexture(texture2D);
					continue;
				}
				texture2D = (Texture2D)Resources.Load("AchievementIcons/LOCKED");
				if (texture2D != null)
				{
					component.image.SetTexture(texture2D);
				}
			}
			else
			{
				incompleteA++;
				Texture2D texture2D2 = (Texture2D)Resources.Load("AchievementIcons/LOCKED");
				if (texture2D2 != null)
				{
					component.image.SetTexture(texture2D2);
				}
				component.title.color = new Color(1f, 1f, 1f, 0.5f);
				component.description.color = new Color(1f, 1f, 1f, 0.5f);
			}
		}
	}

	private void OpenAchievements()
	{
		openFeint.LaunchAchievements();
	}

	public static void ReportAchievement(string AchievementID, float percent)
	{
		int achievementID = Convert.ToInt32(AchievementID);
		openFeint.UnlockAchievement(achievementID);
	}
}
