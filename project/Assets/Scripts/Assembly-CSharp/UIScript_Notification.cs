using UnityEngine;

public class UIScript_Notification : MonoBehaviour
{
	private void Skip()
	{
		NotificationManager.Instance.Dismiss();
	}

	private void Continue()
	{
		NotificationManager.Instance.Dismiss();
		if (NotificationManager.Instance.gameOverNotification)
		{
			GameNav.Instance.BringInGameOver();
		}
	}

	private void UnlockAll()
	{
		Debug.Log("Pressed unlock all button");
		PurchaseManager.Instance.Purchase(PurchaseManager.Instance.productIdentifierUnlockAll);
	}

	private void StartGame()
	{
		NotificationManager.Instance.Dismiss();
		NotificationManager.Instance.BringInTrafficTip();
	}

	private void DismissTrafficTip()
	{
		NotificationManager.Instance.DismissTrafficTip();
	}

	private void Next()
	{
		NotificationManager.Instance.Next();
	}

	private void Like()
	{
		NotificationManager.Instance.Like();
	}

	private void Buy()
	{
		NotificationManager.Instance.Buy();
	}
}
