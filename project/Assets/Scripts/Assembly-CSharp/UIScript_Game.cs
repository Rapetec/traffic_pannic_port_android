using System.Collections;
using NeonPlay;
using UnityEngine;

public class UIScript_Game : MonoBehaviour
{
	private void Pause()
	{
		FlurryController.ReportSingleEvent("Pause Pressed");
		GameNav.Instance.BringInPause();
	}

	private void Resume()
	{
		FlurryController.ReportSingleEvent("Resume Pressed");
		GameNav.Instance.DismissPause();
	}

	private void GameOver()
	{
		GameNav.Instance.BringInGameOver();
	}

	private void Retry()
	{
		FlurryController.ReportSingleEvent("Retry Pressed");
		Time.timeScale = 1f;
		GameNav.Instance.BringInLoading();
		Application.LoadLevelAsync("Game");
	}

	private void Menu()
	{
		FlurryController.ReportSingleEvent("Return to Menu Pressed");
		MenuLogic();
	}

	private void MenuLogic()
	{
		Time.timeScale = 1f;
		GameNav.Instance.BringInLoading();
		SingletonBase<AchievementStorage>.Instance.ReportAccumulations();
		Application.LoadLevelAsync("Menu");
	}

	private void MenuVehicleShop()
	{
		FlurryController.ReportSingleEvent("Return to Shop Pressed");
		InitialMenuPanelManager.shop_vehicle = true;
		MenuLogic();
	}

	private void DismissHiscore()
	{
		GameNav.Instance.DismissHiscore();
	}

	private IEnumerator WaitForScene(string scene, float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		Application.LoadLevel(scene);
	}
}
