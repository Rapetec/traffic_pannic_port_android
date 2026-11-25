using UnityEngine;

namespace NeonPlay
{
	public sealed class TrafficShopItemHandler : SingletonBase<TrafficShopItemHandler>
	{
		private int maxVehicles = 9;

		private int maxBuildings = 4;

		private string unlockedVehicles;

		private string unlockedBuildings;

		private string selectedVehicles;

		private string selectedBuildings;

		private bool unlockAll;

		private bool unlockAllSelected;

		private bool insuranceFraud;

		public bool InsuranceFraud
		{
			get
			{
				return insuranceFraud;
			}
			set
			{
				insuranceFraud = value;
				PlayerPrefs.SetInt("InsuranceFraud", value ? 1 : 0);
				if (Application.loadedLevelName == "Game")
				{
					EtceteraBinding.hideActivityView();
					NotificationManager.Instance.Dismiss();
					Application.LoadLevel(Application.loadedLevel);
				}
			}
		}

		public bool UnlockAll
		{
			get
			{
				return unlockAll;
			}
			set
			{
				unlockAll = value;
				PlayerPrefs.SetInt("UnlockAll", value ? 1 : 0);
			}
		}

		public bool UnlockAllSelected
		{
			get
			{
				return unlockAllSelected;
			}
			set
			{
				unlockAllSelected = value;
				PlayerPrefs.SetInt("UnlockAllSelected", value ? 1 : 0);
			}
		}

		public string UnlockedVehicles
		{
			get
			{
				return unlockedVehicles;
			}
		}

		public string UnlockedBuildings
		{
			get
			{
				return unlockedBuildings;
			}
		}

		private TrafficShopItemHandler()
		{
			unlockedVehicles = PlayerPrefs.GetString("UnlockedVehicles");
			unlockedBuildings = PlayerPrefs.GetString("UnlockedBuildings");
			selectedVehicles = PlayerPrefs.GetString("SelectedVehicles");
			selectedBuildings = PlayerPrefs.GetString("SelectedBuildings");
			if (unlockedVehicles == string.Empty)
			{
				unlockedVehicles = "000000000";
			}
			if (unlockedBuildings == string.Empty)
			{
				unlockedBuildings = "0000";
			}
			if (selectedVehicles == string.Empty)
			{
				selectedVehicles = "000000000";
			}
			if (selectedBuildings == string.Empty)
			{
				selectedBuildings = "0000";
			}
			if (unlockedVehicles.Length != maxVehicles)
			{
				for (int i = unlockedVehicles.Length; i < maxVehicles; i++)
				{
					unlockedVehicles += '0';
				}
			}
			if (unlockedBuildings.Length != maxBuildings)
			{
				for (int j = unlockedBuildings.Length; j < maxBuildings; j++)
				{
					unlockedBuildings += '0';
				}
			}
			if (selectedVehicles.Length != maxVehicles)
			{
				for (int k = selectedVehicles.Length; k < maxVehicles; k++)
				{
					selectedVehicles += '0';
				}
			}
			if (selectedBuildings.Length != maxBuildings)
			{
				for (int l = selectedBuildings.Length; l < maxBuildings; l++)
				{
					selectedBuildings += '0';
				}
			}
			int @int = PlayerPrefs.GetInt("UnlockAll");
			unlockAll = ((@int != 0) ? true : false);
			int int2 = PlayerPrefs.GetInt("UnlockAllSelected");
			unlockAllSelected = ((int2 != 0) ? true : false);
			int int3 = PlayerPrefs.GetInt("InsuranceFraud");
			InsuranceFraud = ((int3 != 0) ? true : false);
		}

		public void UnlockVehicle(int index)
		{
			char[] array = unlockedVehicles.ToCharArray();
			array[index] = '1';
			unlockedVehicles = new string(array);
			PlayerPrefs.SetString("UnlockedVehicles", unlockedVehicles);
			SelectVehicle(index, true);
		}

		public void UnlockBuilding(int index)
		{
			char[] array = unlockedBuildings.ToCharArray();
			array[index] = '1';
			unlockedBuildings = new string(array);
			PlayerPrefs.SetString("UnlockedBuildings", unlockedBuildings);
			SelectBuilding(index, true);
		}

		public void SelectVehicle(int index, bool selected)
		{
			char[] array = selectedVehicles.ToCharArray();
			Debug.Log("Selected vehicles length: " + selectedVehicles.Length);
			if (selected)
			{
				array[index] = '1';
			}
			else
			{
				array[index] = '0';
			}
			selectedVehicles = new string(array);
			PlayerPrefs.SetString("SelectedVehicles", selectedVehicles);
		}

		public void SelectBuilding(int index, bool selected)
		{
			char[] array = selectedBuildings.ToCharArray();
			if (selected)
			{
				array[index] = '1';
			}
			else
			{
				array[index] = '0';
			}
			selectedBuildings = new string(array);
			PlayerPrefs.SetString("SelectedBuildings", selectedBuildings);
		}

		public void ToggleSelectedVehicle(int index)
		{
			char[] array = selectedVehicles.ToCharArray();
			if (array[index] == '1')
			{
				array[index] = '0';
			}
			else
			{
				array[index] = '1';
			}
			selectedVehicles = new string(array);
			PlayerPrefs.SetString("SelectedVehicles", selectedVehicles);
		}

		public void ToggleSelectedBuilding(int index)
		{
			char[] array = selectedBuildings.ToCharArray();
			if (array[index] == '1')
			{
				array[index] = '0';
			}
			else
			{
				array[index] = '1';
			}
			selectedBuildings = new string(array);
			PlayerPrefs.SetString("SelectedBuildings", selectedBuildings);
		}

		public void ToggleSelectedUnlockAll()
		{
			char[] array = selectedVehicles.ToCharArray();
			char[] array2 = selectedBuildings.ToCharArray();
			UnlockAllSelected = !unlockAllSelected;
			if (!unlockAllSelected)
			{
				for (int i = 0; i < maxVehicles; i++)
				{
					array[i] = '0';
				}
				for (int j = 0; j < maxBuildings; j++)
				{
					array2[j] = '0';
				}
			}
			else
			{
				for (int k = 0; k < maxVehicles; k++)
				{
					array[k] = '1';
				}
				for (int l = 0; l < maxBuildings; l++)
				{
					array2[l] = '1';
				}
			}
			selectedVehicles = new string(array);
			selectedBuildings = new string(array2);
			PlayerPrefs.SetString("SelectedVehicles", selectedVehicles);
			PlayerPrefs.SetString("SelectedBuildings", selectedBuildings);
			Debug.Log("Selected buildings: " + selectedBuildings.ToString());
		}

		public bool IsVehicleUnlocked(int index)
		{
			if (unlockedVehicles[index] == '1' || unlockAll)
			{
				return true;
			}
			return false;
		}

		public bool IsBuildingUnlocked(int index)
		{
			if (unlockedBuildings[index] == '1' || unlockAll)
			{
				return true;
			}
			return false;
		}

		public bool IsVehicleSelected(int index)
		{
			if (selectedVehicles[index] == '1')
			{
				return true;
			}
			return false;
		}

		public bool IsBuildingSelected(int index)
		{
			if (selectedBuildings[index] == '1')
			{
				return true;
			}
			return false;
		}

		public void UnlockEverything()
		{
			Debug.Log("Unlock everything!!");
			char[] array = selectedVehicles.ToCharArray();
			char[] array2 = selectedBuildings.ToCharArray();
			Debug.Log("vStr before: " + new string(array) + " bStr before: " + new string(array2));
			UnlockAll = true;
			UnlockAllSelected = true;
			for (int i = 0; i < maxVehicles; i++)
			{
				array[i] = '1';
			}
			for (int j = 0; j < maxBuildings; j++)
			{
				array2[j] = '1';
			}
			selectedVehicles = new string(array);
			selectedBuildings = new string(array2);
			Debug.Log("vStr after: " + selectedVehicles + " bStr after: " + selectedBuildings);
			PlayerPrefs.SetString("SelectedVehicles", selectedVehicles);
			PlayerPrefs.SetString("SelectedBuildings", selectedBuildings);
			if (Application.loadedLevelName == "Game")
			{
				Debug.Log("Loaded level == game");
				EtceteraBinding.hideActivityView();
				NotificationManager.Instance.Dismiss();
				Application.LoadLevel(Application.loadedLevel);
			}
			else
			{
				Debug.Log("Application loaded level != game");
			}
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_Taxi);
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_Police);
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_Italiano);
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_Trailer);
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_Ambulance);
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_FuelTanker);
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_FireTruck);
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_DasSporta);
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_Tank);
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_GasStation);
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_DemolitionSite);
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_CarShowroom);
			SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShop_FireworksFactory);
		}
	}
}
