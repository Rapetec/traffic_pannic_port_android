using System;
// (stub: SDK de terceros desactivado; el original llamaba a Java o a la red)
using UnityEngine;

public class OpenFeintFacade : IDisposable
{
	private bool calledInit;

	private AndroidJavaClass cls_OpenFeintFacade;

	private AndroidJavaClass cls_OpenFeint;

	private AndroidJavaClass cls_Dashboard;

	public void Init(string name, string key, string secret, string id, int orientation)
	{
	}

	public bool isInitialized()
	{
		return default(bool);
	}

	public bool isUserLoggedIn()
	{
		return default(bool);
	}

	public void OpenDashboard()
	{
	}

	public void LaunchDashboard()
	{
	}

	public void LaunchLeaderboards()
	{
	}

	public void LaunchAchievements()
	{
	}

	public void SubmitHighScore(long highscore, string displayText, string leaderBoardId, bool silent)
	{
	}

	public void LaunchDashboardWithLeaderboardID(string leaderboard)
	{
	}

	public void UnlockAchievement(int achievementID)
	{
	}

	public void UpdateAchievement(int achievementID, float percentage, bool showNotification)
	{
	}

	public void SendSocialNotification(string text, string imageName)
	{
	}

	public void InGameNotification(string text, string imageName)
	{
	}

	public void SubmitScore(string leaderboard, int score)
	{
	}

	public void SubmitAchievement(int achievementID)
	{
	}

	public AndroidJavaObject GetHighScoresFromLeaderBoardId(string leaderboard)
	{
		return default(AndroidJavaObject);
	}

	public AndroidJavaObject GetAchievementById(string achievementId)
	{
		return default(AndroidJavaObject);
	}

	public bool IsAchievementUnlocked(string achievementId)
	{
		return default(bool);
	}

	public AndroidJavaObject GetAchievements()
	{
		return default(AndroidJavaObject);
	}

	public void Dispose()
	{
	}
}
