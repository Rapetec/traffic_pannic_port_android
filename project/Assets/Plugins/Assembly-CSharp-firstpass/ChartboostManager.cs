using UnityEngine;

public class ChartboostManager : MonoBehaviour
{
	private static volatile ChartboostManager _instance;

	private static object _lock;

	public static ChartboostManager Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_lock)
				{
					GameObject gameObject = new GameObject();
					gameObject.AddComponent<ChartboostManager>();
					_instance = gameObject.GetComponent<ChartboostManager>();
					gameObject.name = _instance.GetType().ToString();
				}
			}
			return _instance;
		}
	}

	private ChartboostManager()
	{
	}

	static ChartboostManager()
	{
		_instance = null;
		_lock = new object();
	}

	private void MyAwake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		Init();
		SetShouldRequestInterstitial(true);
	}

	private void DidDismissInterstitial(string nullMsg)
	{
	}

	private void DidCloseInterstitial(string nullMsg)
	{
	}

	private void DidClickInterstitial(string nullMsg)
	{
	}

	private void DidDismissMoreApps(string nullMsg)
	{
	}

	private void DidCloseMoreApps(string nullMsg)
	{
	}

	private void DidClickMoreApps(string nullMsg)
	{
	}

	public void Init()
	{
		ChartboostDelegate.InstallCB();
	}

	public void DisplayInterstitial()
	{
		ChartboostDelegate.DisplayInterstitial(null);
	}

	public void DisplayMoreApps()
	{
		ChartboostDelegate.DisplayMoreApps();
	}

	public void SetShouldRequestInterstitial(bool b)
	{
		ChartboostDelegate.SetShouldRequestInterstitial(b);
	}

	public void SetShouldDisplayInterstitial(bool b)
	{
		ChartboostDelegate.SetShouldDisplayInterstitial(b);
	}

	public void SetShouldDisplayMoreApps(bool b)
	{
		ChartboostDelegate.SetShouldDisplayMoreApps(b);
	}

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		if (_instance == null)
		{
			_instance = GetComponent<ChartboostManager>();
		}
		MyAwake();
	}
}
