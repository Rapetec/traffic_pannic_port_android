using NeonPlay;
using UnityEngine;

public class SocialController : MonoBehaviour
{
	public SpriteText twitterCopy;

	public bool menuMode;

	public GameManager gm;

	private UIPanelBase loadingPanel;

	private bool mustPostFacebook;

	private bool mustPostTwitter;

	private bool twitterInitialized;

	private string twitterConsumerKey = "SeUTThLOXLtsh6RclAJWA";

	private string twitterConsumerSecret = "XFqIaYTREI6rgXE09MG1902ihUGZw0Yt0exHgizxtiY";

	private string facebookAppID = "228147233915258";

	private void Start()
	{
		if (menuMode)
		{
			loadingPanel = MenuNav.Instance.LoadingPanel;
		}
		else
		{
			loadingPanel = GameNav.Instance.LoadingPanel;
		}
		TwitterAndroid.init(twitterConsumerKey, twitterConsumerSecret);
		FacebookAndroid.init(facebookAppID);
		if (menuMode)
		{
			twitterCopy.Text = SingletonBase<CopyScript>.Instance.GetTwitter.optionsTweet;
		}
	}

	private void OnDestroy()
	{
	}

	private void twitterLogin()
	{
		ShareTwitter();
	}

	private void twoFieldPromptTextEntered(string userName, string password)
	{
		TwitterBinding.login(userName, password);
	}

	private void twitterLoginFailed(string error)
	{
		AlertManager.ShowAlert("Login Failed", string.Empty + error, "Ok");
	}

	private void twitterPost()
	{
		AlertManager.ShowAlert("Twitter", "Posting Successful", "Ok");
		SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDTwitterPost);
		loadingPanel.Dismiss();
	}

	private void twitterPostFailed(string error)
	{
		AlertManager.ShowAlert("Posting Failed", string.Empty + error, "Ok");
		loadingPanel.Dismiss();
	}

	private void facebookLogin()
	{
		if (mustPostFacebook)
		{
			ShareFacebook();
			mustPostFacebook = false;
		}
	}

	private void facebookLoginFailed(string error)
	{
		AlertManager.ShowAlert("Login Failed", error, "Ok");
	}

	private void facebookPost()
	{
		AlertManager.ShowAlert("Facebook", "Posting Successful", "Ok");
	}

	private void facebookPostFailed(string error)
	{
		AlertManager.ShowAlert("Posting failed", error, "Ok");
	}

	public void ShareFacebook()
	{
		if (!FacebookBinding.isLoggedIn())
		{
			mustPostFacebook = true;
			FacebookBinding.login();
			return;
		}
		if (menuMode)
		{
			FacebookBinding.showPostMessageDialogWithOptions("http://bit.ly/TP-3D", SingletonBase<CopyScript>.Instance.GetFacebook.optionsTitle, "http://scores.neonplay.com/images/trafficpanic3d/trafficpanic3d_icon_facebook_75.png", SingletonBase<CopyScript>.Instance.GetFacebook.optionsPost);
			FlurryController.ReportPieChartEvent("Menu Sharing", "Share Pressed", "Facebook");
		}
		else if (gm.betterScore)
		{
			string text = gm.best.ToString("N0");
			FacebookBinding.showPostMessageDialogWithOptions("http://bit.ly/TP-3D", SingletonBase<CopyScript>.Instance.GetFacebook.pointHiScoreTitle1 + text + SingletonBase<CopyScript>.Instance.GetFacebook.pointHiScoreTitle2, "http://scores.neonplay.com/images/trafficpanic3d/trafficpanic3d_icon_facebook_75.png", SingletonBase<CopyScript>.Instance.GetFacebook.pointHiScore);
			FlurryController.ReportPieChartEvent("Game Sharing", "Share Pressed", "Facebook");
		}
		else
		{
			string text2 = "$" + gm.bestDamage.ToString("N0");
			FacebookBinding.showPostMessageDialogWithOptions("http://bit.ly/TP-3D", SingletonBase<CopyScript>.Instance.GetFacebook.damageHiScoreTitle1 + text2 + SingletonBase<CopyScript>.Instance.GetFacebook.damageHiScoreTitle2, "http://scores.neonplay.com/images/trafficpanic3d/trafficpanic3d_icon_facebook_75.png", SingletonBase<CopyScript>.Instance.GetFacebook.damageHiScore);
			FlurryController.ReportPieChartEvent("Game Sharing", "Share Pressed", "Facebook");
		}
		SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDFacebookPost);
	}

	public void ShareTwitter()
	{
		bool flag = false;
		if (!TwitterAndroid.isLoggedIn())
		{
			mustPostTwitter = true;
			TwitterAndroid.showLoginDialog();
			return;
		}
		if (menuMode)
		{
			FlurryController.ReportPieChartEvent("Menu Sharing", "Share Pressed", "Twitter");
			TwitterAndroid.postUpdate(SingletonBase<CopyScript>.Instance.GetTwitter.optionsTweet);
		}
		else if (gm.betterScore)
		{
			string text = gm.best.ToString("N0");
			FlurryController.ReportPieChartEvent("Game Sharing", "Share Pressed", "Twitter");
			TwitterAndroid.postUpdate(SingletonBase<CopyScript>.Instance.GetTwitter.pointHiScore1 + text + SingletonBase<CopyScript>.Instance.GetTwitter.pointHiScore2);
		}
		else
		{
			FlurryController.ReportPieChartEvent("Game Sharing", "Share Pressed", "Twitter");
			string text2 = "$" + gm.bestDamage.ToString("N0");
			TwitterAndroid.postUpdate(SingletonBase<CopyScript>.Instance.GetTwitter.damageHiScore1 + text2 + SingletonBase<CopyScript>.Instance.GetTwitter.damageHiScore2);
		}
		loadingPanel.BringIn();
	}

	public void TwitterLogout()
	{
		TwitterAndroid.logout();
	}
}
