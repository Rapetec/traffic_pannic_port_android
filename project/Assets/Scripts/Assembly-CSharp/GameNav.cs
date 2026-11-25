using UnityEngine;

public sealed class GameNav : MonoBehaviour
{
	private static volatile GameNav _instance;

	private static object _lock;

	public GameManager gm;

	[SerializeField]
	private UIPanelBase pausePanel;

	[SerializeField]
	private UIPanelBase hiscorePanel;

	[SerializeField]
	private UIPanelBase gameoverPanel;

	[SerializeField]
	private UIPanelBase loadingPanel;

	[SerializeField]
	private PackedSprite gameOverTitle;

	private UIPanelManager pm;

	public UIPanelBase LoadingPanel
	{
		get
		{
			return loadingPanel;
		}
	}

	public static GameNav Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_lock)
				{
					GameObject gameObject = new GameObject();
					gameObject.AddComponent<GameNav>();
					_instance = gameObject.GetComponent<GameNav>();
					gameObject.name = _instance.GetType().ToString();
				}
			}
			return _instance;
		}
	}

	private GameNav()
	{
	}

	static GameNav()
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
		ChartboostManager.Instance.SetShouldDisplayInterstitial(false);
	}

	public void BringInPause()
	{
		if (!gm.GameOver)
		{
			gm.pausebutton.active = false;
			gm.scorebackground.SetActiveRecursively(false);
			pausePanel.gameObject.SetActiveRecursively(true);
			Time.timeScale = 0f;
		}
	}

	public void DismissPause()
	{
		gm.pausebutton.active = true;
		gm.scorebackground.SetActiveRecursively(true);
		pausePanel.gameObject.SetActiveRecursively(false);
		Time.timeScale = 1f;
	}

	public void BringInHiscore(bool score, int scoreValue)
	{
		FlurryController.ReportSingleEvent("High Score Panel");
		hiscorePanel.gameObject.SetActiveRecursively(true);
		gm.scorebackground.SetActiveRecursively(false);
		gm.trafficLights.gameObject.active = false;
		if (score)
		{
			gm.hiscoreMessage.Text = "You set a new hi-score of " + scoreValue.ToString("N0");
		}
		else
		{
			gm.hiscoreMessage.Text = "You set a new damage hi-score of $" + scoreValue.ToString("N0");
		}
	}

	public void DismissHiscore()
	{
		hiscorePanel.gameObject.SetActiveRecursively(false);
		BringInGameOver();
	}

	public void BringInGameOver()
	{
		DismissPause();
		gm.pausebutton.active = false;
		gm.scorebackground.SetActiveRecursively(false);
		gm.trafficLights.gameObject.active = false;
		gameoverPanel.gameObject.SetActiveRecursively(true);
		if (gm.TrafficJam)
		{
			gameOverTitle.PlayAnim(1);
		}
		else
		{
			gameOverTitle.PlayAnim(0);
		}
	}

	public void BringInLoading()
	{
		loadingPanel.BringIn();
	}

	public void Retry()
	{
		Application.LoadLevel("Game");
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
			_instance = GetComponent<GameNav>();
		}
		MyAwake();
	}
}
