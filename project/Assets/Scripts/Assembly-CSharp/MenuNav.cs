using UnityEngine;

public class MenuNav : MonoBehaviour
{
	private static volatile MenuNav _instance;

	private static object _lock;

	private UIPanelManager pm;

	[SerializeField]
	private UIPanelBase menuPanel;

	[SerializeField]
	private UIPanelBase optionsPanel;

	[SerializeField]
	private UIPanelBase vehiclePanel;

	[SerializeField]
	private UIPanelBase cityPanel;

	[SerializeField]
	private UIPanelBase twitterPanel;

	[SerializeField]
	private UIPanelBase loadingPanel;

	public UIPanelBase LoadingPanel
	{
		get
		{
			return loadingPanel;
		}
	}

	public static MenuNav Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_lock)
				{
					GameObject gameObject = new GameObject();
					gameObject.AddComponent<MenuNav>();
					_instance = gameObject.GetComponent<MenuNav>();
					gameObject.name = _instance.GetType().ToString();
				}
			}
			return _instance;
		}
	}

	private MenuNav()
	{
	}

	static MenuNav()
	{
		_instance = null;
		_lock = new object();
	}

	private void MyAwake()
	{
		pm = GetComponent<UIPanelManager>();
	}

	private void Start()
	{
		if (AppController.Instance.MenuVisits >= 3)
		{
			ChartboostManager.Instance.SetShouldDisplayInterstitial(true);
			ChartboostManager.Instance.DisplayInterstitial();
		}
		else
		{
			AppController.Instance.MenuVisits++;
		}
	}

	private void Update()
	{
	}

	public void BringInMenu()
	{
		pm.BringInImmediate(menuPanel);
	}

	public void BringInOptions()
	{
		pm.BringInImmediate(optionsPanel);
	}

	public void BringInVehicle()
	{
		pm.BringInImmediate(vehiclePanel);
	}

	public void BringInCity()
	{
		pm.BringInImmediate(cityPanel);
	}

	public void BringInTwitter()
	{
		pm.BringInImmediate(twitterPanel);
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
			_instance = GetComponent<MenuNav>();
		}
		MyAwake();
	}
}
