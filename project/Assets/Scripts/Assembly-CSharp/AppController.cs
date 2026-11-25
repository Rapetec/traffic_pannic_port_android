using NeonPlay;
using UnityEngine;

public class AppController : MonoBehaviour
{
	private static volatile AppController _instance;

	private static object _lock;

	private bool enableAds = false;  // sin anuncios

	private bool first;

	private int menuVisits;

	private int appID = 475292152;

	public bool EnableAds
	{
		get
		{
			return enableAds;
		}
	}

	public bool FirstTime
	{
		get
		{
			return first;
		}
		set
		{
			first = value;
		}
	}

	public int AppID
	{
		get
		{
			return appID;
		}
	}

	public int MenuVisits
	{
		get
		{
			return menuVisits;
		}
		set
		{
			menuVisits = value;
			PlayerPrefs.SetInt("MenuVisits", menuVisits);
		}
	}

	public static AppController Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_lock)
				{
					GameObject gameObject = new GameObject();
					gameObject.AddComponent<AppController>();
					_instance = gameObject.GetComponent<AppController>();
					gameObject.name = _instance.GetType().ToString();
				}
			}
			return _instance;
		}
	}

	private AppController()
	{
	}

	static AppController()
	{
		_instance = null;
		_lock = new object();
	}

	private void MyAwake()
	{
		Screen.orientation = ScreenOrientation.AutoRotation;
		bool autorotateToLandscapeLeft = (Screen.autorotateToLandscapeRight = true);
		Screen.autorotateToLandscapeLeft = autorotateToLandscapeLeft;
		autorotateToLandscapeLeft = (Screen.autorotateToPortraitUpsideDown = false);
		Screen.autorotateToPortrait = autorotateToLandscapeLeft;
		if (PlayerPrefs.GetInt("FirstTime") == 0)
		{
			first = true;
		}
		menuVisits = PlayerPrefs.GetInt("MenuVisits");
		int num = PlayerPrefs.GetInt("Version");
		if (num == 0)
		{
			PlayerPrefs.SetInt("Version", 6);
			num = 6;
		}
		if (num >= 3)
		{
			enableAds = false;  // sin anuncios
		}
		else
		{
			enableAds = false;
		}
		int num2 = PlayerPrefs.GetInt("showCanAffordCount");
		if (num2 < 2)
		{
			num2 = 3;
		}
		PlayerPrefs.SetInt("showCanAffordCount", num2);
		int num3 = PlayerPrefs.GetInt("showIAPCount");
		if (num3 <= 1)
		{
			num3 = 20;
		}
		PlayerPrefs.SetInt("showIAPCount", num3);
	}

	private void Start()
	{
		FlurryController.Init("4LS13ZC96C3H5YKW68ZV");
		SingletonBase<ChallengeData>.Instance.LoginOF();
		AchievementStorage instance = SingletonBase<AchievementStorage>.Instance;
	}

	private void OnApplicationFocus(bool focused)
	{
		if (!focused)
		{
			AudioListener.volume = 0f;
		}
		else if (SingletonBase<SoundManager>.Instance.IsSoundOn)
		{
			AudioListener.volume = 1f;
		}
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(this);
		if (_instance != null && _instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		if (_instance == null)
		{
			_instance = GetComponent<AppController>();
		}
		MyAwake();
	}
}
