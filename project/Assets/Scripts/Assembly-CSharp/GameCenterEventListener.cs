using System.Collections.Generic;
using UnityEngine;

public class GameCenterEventListener : MonoBehaviour
{
	private void Start()
	{
		GameCenterManager.loadPlayerDataFailed += loadPlayerDataFailed;
		GameCenterManager.playerDataLoaded += playerDataLoaded;
		GameCenterManager.playerAuthenticated += playerAuthenticated;
		GameCenterManager.playerFailedToAuthenticate += playerFailedToAuthenticate;
		GameCenterManager.playerLoggedOut += playerLoggedOut;
		GameCenterManager.loadCategoryTitlesFailed += loadCategoryTitlesFailed;
		GameCenterManager.categoriesLoaded += categoriesLoaded;
		GameCenterManager.reportScoreFailed += reportScoreFailed;
		GameCenterManager.reportScoreFinished += reportScoreFinished;
		GameCenterManager.retrieveScoresFailed += retrieveScoresFailed;
		GameCenterManager.scoresLoaded += scoresLoaded;
		GameCenterManager.retrieveScoresForPlayerIdFailed += retrieveScoresForPlayerIdFailed;
		GameCenterManager.scoresForPlayerIdLoaded += scoresForPlayerIdLoaded;
		GameCenterManager.reportAchievementFailed += reportAchievementFailed;
		GameCenterManager.reportAchievementFinished += reportAchievementFinished;
		GameCenterManager.loadAchievementsFailed += loadAchievementsFailed;
		GameCenterManager.achievementsLoaded += achievementsLoaded;
		GameCenterManager.resetAchievementsFailed += resetAchievementsFailed;
		GameCenterManager.resetAchievementsFinished += resetAchievementsFinished;
		GameCenterManager.retrieveAchievementMetadataFailed += retrieveAchievementMetadataFailed;
		GameCenterManager.achievementMetadataLoaded += achievementMetadataLoaded;
	}

	private void OnDisable()
	{
		GameCenterManager.loadPlayerDataFailed -= loadPlayerDataFailed;
		GameCenterManager.playerDataLoaded -= playerDataLoaded;
		GameCenterManager.playerAuthenticated -= playerAuthenticated;
		GameCenterManager.playerLoggedOut -= playerLoggedOut;
		GameCenterManager.loadCategoryTitlesFailed -= loadCategoryTitlesFailed;
		GameCenterManager.categoriesLoaded -= categoriesLoaded;
		GameCenterManager.reportScoreFailed -= reportScoreFailed;
		GameCenterManager.reportScoreFinished -= reportScoreFinished;
		GameCenterManager.retrieveScoresFailed -= retrieveScoresFailed;
		GameCenterManager.scoresLoaded -= scoresLoaded;
		GameCenterManager.retrieveScoresForPlayerIdFailed -= retrieveScoresForPlayerIdFailed;
		GameCenterManager.scoresForPlayerIdLoaded -= scoresForPlayerIdLoaded;
		GameCenterManager.reportAchievementFailed -= reportAchievementFailed;
		GameCenterManager.reportAchievementFinished -= reportAchievementFinished;
		GameCenterManager.loadAchievementsFailed -= loadAchievementsFailed;
		GameCenterManager.achievementsLoaded -= achievementsLoaded;
		GameCenterManager.resetAchievementsFailed -= resetAchievementsFailed;
		GameCenterManager.resetAchievementsFinished -= resetAchievementsFinished;
		GameCenterManager.retrieveAchievementMetadataFailed -= retrieveAchievementMetadataFailed;
		GameCenterManager.achievementMetadataLoaded -= achievementMetadataLoaded;
	}

	private void playerAuthenticated()
	{
		Debug.Log("playerAuthenticated");
		if (PlayerPrefs.GetInt("AchievementsReported") == 0)
		{
			ReportAllAchievements();
			PlayerPrefs.SetInt("AchievementsReported", 1);
		}
	}

	private void ReportAllAchievements()
	{
		GameCenterBinding.reportAchievement(ChallengeData.AchievementID911, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDCabby, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDDamage1, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDDamage2, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDDamage3, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDDamage4, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDDamage5, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDDelivery, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDEmail, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDFacebookPost, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDFacebookLike, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDFireman, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDGlasshouse, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDGoGoGo, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDGoGoGo, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDHolidayNightmare, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDKaboom, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDMaster, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDMerciless, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDMoreGames, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDParamedic, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDPendulum, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDPetrolBomb, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDPyromaniac, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDScore1, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDScore2, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDScore3, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDScore4, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDScore5, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDScrapMetal, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDShop_Taxi, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDShop_Police, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDShop_Italiano, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDShop_Trailer, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDShop_Ambulance, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDShop_FuelTanker, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDShop_FireTruck, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDShop_DasSporta, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDShop_Tank, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDShop_GasStation, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDShop_DemolitionSite, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDShop_CarShowroom, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDShop_FireworksFactory, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDShowTime, 0f);
		GameCenterBinding.reportAchievement(ChallengeData.AchievementIDTwitterPost, 0f);
	}

	private void playerFailedToAuthenticate(string error)
	{
		Debug.Log("playerFailedToAuthenticate: " + error);
	}

	private void playerLoggedOut()
	{
		Debug.Log("playerLoggedOut");
	}

	private void playerDataLoaded(List<GameCenterPlayer> players)
	{
		Debug.Log("playerDataLoaded");
		foreach (GameCenterPlayer player in players)
		{
			Debug.Log(player);
		}
	}

	private void loadPlayerDataFailed(string error)
	{
		Debug.Log("loadPlayerDataFailed: " + error);
	}

	private void categoriesLoaded(List<GameCenterLeaderboard> leaderboards)
	{
		Debug.Log("categoriesLoaded");
		foreach (GameCenterLeaderboard leaderboard in leaderboards)
		{
			Debug.Log(leaderboard);
		}
	}

	private void loadCategoryTitlesFailed(string error)
	{
		Debug.Log("loadCategoryTitlesFailed: " + error);
	}

	private void scoresLoaded(List<GameCenterScore> scores)
	{
		Debug.Log("scoresLoaded");
		foreach (GameCenterScore score in scores)
		{
			Debug.Log(score);
		}
	}

	private void retrieveScoresFailed(string error)
	{
		Debug.Log("retrieveScoresFailed: " + error);
	}

	private void retrieveScoresForPlayerIdFailed(string error)
	{
		Debug.Log("retrieveScoresForPlayerIdFailed: " + error);
	}

	private void scoresForPlayerIdLoaded(List<GameCenterScore> scores)
	{
		Debug.Log("scoresForPlayerIdLoaded");
		foreach (GameCenterScore score in scores)
		{
			Debug.Log(score);
		}
	}

	private void reportScoreFinished(string category)
	{
		Debug.Log("reportScoreFinished for category: " + category);
	}

	private void reportScoreFailed(string error)
	{
		Debug.Log("reportScoreFailed: " + error);
	}

	private void achievementMetadataLoaded(List<GameCenterAchievementMetadata> achievementMetadata)
	{
		Debug.Log("achievementMetadatLoaded");
		foreach (GameCenterAchievementMetadata achievementMetadatum in achievementMetadata)
		{
			Debug.Log(achievementMetadatum);
		}
	}

	private void retrieveAchievementMetadataFailed(string error)
	{
		Debug.Log("retrieveAchievementMetadataFailed: " + error);
	}

	private void resetAchievementsFinished()
	{
		Debug.Log("resetAchievmenetsFinished");
	}

	private void resetAchievementsFailed(string error)
	{
		Debug.Log("resetAchievementsFailed: " + error);
	}

	private void achievementsLoaded(List<GameCenterAchievement> achievements)
	{
		Debug.Log("achievementsLoaded");
		foreach (GameCenterAchievement achievement in achievements)
		{
			Debug.Log(achievement);
		}
	}

	private void loadAchievementsFailed(string error)
	{
		Debug.Log("loadAchievementsFailed: " + error);
	}

	private void reportAchievementFinished(string identifier)
	{
		Debug.Log("reportAchievementFinished: " + identifier);
	}

	private void reportAchievementFailed(string error)
	{
		Debug.Log("reportAchievementFailed: " + error);
	}
}
