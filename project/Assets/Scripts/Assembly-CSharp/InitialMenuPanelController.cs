using UnityEngine;

public class InitialMenuPanelController : MonoBehaviour
{
	public UIPanel defaultPanel;

	public UIPanel leaderboardPanel;

	public UIPanel vehicleShopPanel;

	public UIPanel cityShopPanel;

	public UIPanel creditsPanel;

	public static bool firstLaunch = true;

	private void OnEnable()
	{
		if (!firstLaunch)
		{
			UIPanelBase initialPanel;
			if (InitialMenuPanelManager.leaderboards)
			{
				initialPanel = leaderboardPanel;
				InitialMenuPanelManager.leaderboards = false;
			}
			else if (InitialMenuPanelManager.shop_city)
			{
				initialPanel = cityShopPanel;
				InitialMenuPanelManager.shop_city = false;
			}
			else if (InitialMenuPanelManager.shop_vehicle)
			{
				initialPanel = vehicleShopPanel;
				InitialMenuPanelManager.shop_vehicle = false;
			}
			else if (InitialMenuPanelManager.shop_credits)
			{
				initialPanel = creditsPanel;
				InitialMenuPanelManager.shop_credits = false;
			}
			else
			{
				initialPanel = defaultPanel;
			}
			GetComponent<UIPanelManager>().initialPanel = initialPanel;
		}
		else
		{
			firstLaunch = false;
		}
	}
}
