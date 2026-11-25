using NeonPlay;
using UnityEngine;

public class MainMenuPanelDelegate : MonoBehaviour
{
	public PackedSprite banner;

	public PackedSprite NADcover;

	public UIPanelManager panelManager;

	private bool tablet;

	private void Start()
	{
		tablet = NPUtility.IsTablet();
		if (panelManager.initialPanel == GetComponent<UIPanel>())
		{
			PanelWillAppear();
		}
	}

	public void PanelWillDisappear()
	{
		if (!PurchaseManager.Instance.PurchaseMade)
		{
			ADManager.SetHidden(false);
		}
	}

	public void PanelWillAppear()
	{
		if (!PurchaseManager.Instance.PurchaseMade)
		{
			ADManager.SetHidden(true);
		}
	}

	private void Update()
	{
		ADManager.SetHidden(true);
	}
}
