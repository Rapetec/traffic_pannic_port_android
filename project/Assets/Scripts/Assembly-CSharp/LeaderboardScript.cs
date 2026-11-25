using NeonPlay;
using UnityEngine;

public class LeaderboardScript : MonoBehaviour
{
	private const string appName = "Traffic Panic 3D";

	private const string id = "385173";

	private const string key = "LK7W8fbUplOMmLDLATnNSQ";

	private const string secret = "dzpHzRkhApBsRgAmFCwLjAVnCbQNzVo6wqGZdeqi1M";

	public static readonly string LeaderIDDamage = "com.neonplay.trafficpanic2.damage";

	public static readonly string LeaderIDScore = "com.neonplay.trafficpanic2.score";

	public static readonly string AndroidLeaderIDScore = "996786";

	public static readonly string AndroidLeaderIDDamage = "996776";

	public GameObject buttonParent;

	public GameObject panelParent;

	public UIScrollList list;

	public GameObject itemObj;

	public SpriteText dialogue;

	public GameObject openFeintButton;

	private int leaderBoardType;

	private bool tableUpdate;

	private string boardName;

	private static OpenFeintFacade openFeint;

	private AndroidJavaObject highscores;

	private bool initilized;

	private void Awake()
	{
		panelParent.transform.position = new Vector3(panelParent.transform.position.x - 130f, panelParent.transform.position.y, panelParent.transform.position.z);
	}

	private void Start()
	{
		leaderBoardType = 0;
		openFeint = new OpenFeintFacade();
		Initialise();
	}

	private void OnDestroy()
	{
	}

	private void Initialise()
	{
		buttonParent.SetActiveRecursively(false);
		if (!openFeint.isUserLoggedIn())
		{
			dialogue.Text = "LOG IN TO OPEN FEINT TO RETRIEVE SCORES";
		}
	}

	private void GetData(int leaderBoard)
	{
		list.ClearList(true);
		switch (leaderBoard)
		{
		case 0:
			boardName = LeaderIDScore;
			break;
		case 1:
			boardName = LeaderIDDamage;
			break;
		}
		if (openFeint.isInitialized() && openFeint.isUserLoggedIn())
		{
			dialogue.Text = SingletonBase<CopyScript>.Instance.GetLeaderboard.retrieving;
			EtceteraAndroid.showProgressDialog("Loading Scores", "Loading the OpenFeint scores...");
			switch (boardName)
			{
			case "com.neonplay.trafficpanic2.damage":
				highscores = openFeint.GetHighScoresFromLeaderBoardId(AndroidLeaderIDDamage);
				break;
			case "com.neonplay.trafficpanic2.score":
				highscores = openFeint.GetHighScoresFromLeaderBoardId(AndroidLeaderIDScore);
				break;
			}
			tableUpdate = true;
			EtceteraAndroid.hideProgressDialog();
		}
	}

	public void ToggleCategory()
	{
		leaderBoardType = Mathf.Abs(leaderBoardType - 1);
		GetData(leaderBoardType);
	}

	public void OpenDashboard()
	{
		openFeint.OpenDashboard();
	}

	public void GetFriendsData()
	{
	}

	public void GetGlobalData()
	{
	}

	public void GetToday()
	{
	}

	public void GetWeekly()
	{
	}

	public void GetAll()
	{
	}

	private void Update()
	{
		if (!initilized && openFeint.isInitialized() && openFeint.isUserLoggedIn())
		{
			initilized = true;
			GetData(leaderBoardType);
		}
		if (!tableUpdate)
		{
			return;
		}
		int num = highscores.Call<int>("size", new object[0]);
		if (num > 0)
		{
			list.ClearList(true);
			dialogue.Text = string.Empty;
			for (int i = 0; i < Mathf.Min(20, num); i++)
			{
				IUIListObject iUIListObject = list.CreateItem(itemObj, string.Empty);
				ScoreItem component = iUIListObject.gameObject.GetComponent<ScoreItem>();
				AndroidJavaObject androidJavaObject = highscores.Call<AndroidJavaObject>("get", new object[1] { i });
				component.rank.Text = (i + 1).ToString();
				string text = androidJavaObject.Get<AndroidJavaObject>("user").Get<string>("name").ToUpper();
				if (text.Length < 25)
				{
					component.alias.Text = text;
				}
				else
				{
					component.alias.Text = text.Substring(0, 24);
				}
				if (boardName == LeaderIDDamage)
				{
					component.score.Text = "$" + androidJavaObject.Get<long>("score");
				}
				else
				{
					component.score.Text = androidJavaObject.Get<long>("score").ToString();
				}
			}
		}
		tableUpdate = false;
	}

	public static void ReportLeaderBoardScores(int score, int damage)
	{
		if (openFeint.isInitialized() && openFeint.isUserLoggedIn())
		{
			openFeint.SubmitScore(AndroidLeaderIDScore, score);
			openFeint.SubmitScore(AndroidLeaderIDDamage, damage);
		}
	}
}
