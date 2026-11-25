using UnityEngine;

public class ChallengeController : MonoBehaviour
{
	private static OpenFeintFacade openFeint;

	public void OpenLeaderboards()
	{
		if (NetworkReachability.NotReachable /* sin red: la app no pide permisos de red */ == NetworkReachability.NotReachable)
		{
			AlertManager.ShowAlert("Not Connected", "Connect to the internet to retrieve leaderboard information.", "Okay");
			return;
		}
		FlurryController.ReportSingleEvent("Leaderboards Pressed");
		if (!openFeint.isUserLoggedIn())
		{
			openFeint.OpenDashboard();
		}
		else
		{
			openFeint.LaunchLeaderboards();
		}
	}

	public void OpenAchievements()
	{
		if (NetworkReachability.NotReachable /* sin red: la app no pide permisos de red */ == NetworkReachability.NotReachable)
		{
			AlertManager.ShowAlert("Not Connected", "Connect to the internet to retrieve leaderboard information.", "Okay");
			return;
		}
		FlurryController.ReportSingleEvent("Achievements Pressed");
		if (!openFeint.isUserLoggedIn())
		{
			openFeint.OpenDashboard();
		}
		else
		{
			openFeint.LaunchAchievements();
		}
	}

	private void Start()
	{
		openFeint = new OpenFeintFacade();
	}

	private void OnDestroy()
	{
	}
}
