using NeonPlay;
using UnityEngine;

public class AppInitialiser : MonoBehaviour
{
	public static bool first;

	public static bool ENABLE_ADS = true;

	private void Start()
	{
		if (PlayerPrefs.GetInt("FirstTime") == 0)
		{
			first = true;
			PlayerPrefs.SetInt("FirstTime", 1);
		}
		SingletonBase<ChallengeData>.Instance.LoginOF();
		int num = PlayerPrefs.GetInt("Version");
		if (num == 0)
		{
			num = 4;
			PlayerPrefs.SetInt("Version", num);
		}
		if (num >= 3)
		{
			ENABLE_ADS = true;
		}
		else
		{
			ENABLE_ADS = false;
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
		AchievementStorage instance = SingletonBase<AchievementStorage>.Instance;
		ChallengeData instance2 = SingletonBase<ChallengeData>.Instance;
	}
}
