using System.Runtime.InteropServices;
// (stub: SDK de terceros desactivado; el original llamaba a Java o a la red)
using UnityEngine;

public class GameCenterBinding
{

	public static bool isGameCenterAvailable()
	{
		return default(bool);
	}


	public static void authenticateLocalPlayer()
	{
	}


	public static bool isPlayerAuthenticated()
	{
		return default(bool);
	}


	public static string playerAlias()
	{
		return default(string);
	}


	public static string playerIdentifier()
	{
		return default(string);
	}


	public static bool isUnderage()
	{
		return default(bool);
	}


	public static void retrieveFriends()
	{
	}


	public static void loadPlayerData(string[] playerIdArray)
	{
	}


	public static void loadLeaderboardTitles()
	{
	}


	public static void reportScore(long score, string leaderboardId)
	{
	}


	public static void showLeaderboardWithTimeScope(GameCenterLeaderboardTimeScope timeScope)
	{
	}


	public static void showLeaderboardWithTimeScopeAndLeaderboard(GameCenterLeaderboardTimeScope timeScope, string leaderboardId)
	{
	}


	public static void retrieveScores(bool friendsOnly, GameCenterLeaderboardTimeScope timeScope, int start, int end)
	{
	}


	public static void retrieveScores(bool friendsOnly, GameCenterLeaderboardTimeScope timeScope, int start, int end, string leaderboardId)
	{
	}


	public static void retrieveScoresForPlayerId(string playerId)
	{
	}


	public static void retrieveScoresForPlayerId(string playerId, string leaderboardId)
	{
	}


	public static void reportAchievement(string identifier, float percent)
	{
	}


	public static void getAchievements()
	{
	}


	public static void resetAchievements()
	{
	}


	public static void showAchievements()
	{
	}


	public static void retrieveAchievementMetadata()
	{
	}


	public static void showCompletionBannerForAchievements()
	{
	}
}
