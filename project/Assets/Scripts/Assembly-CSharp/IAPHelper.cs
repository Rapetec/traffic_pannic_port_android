using NeonPlay;
using UnityEngine;

public class IAPHelper : MonoBehaviour
{
	private PackedSprite tickSprite;

	private UIButton unlockButton;

	public bool unlockAllButton;

	private GameObject priceCover;

	private void Start()
	{
		if (unlockAllButton)
		{
			unlockButton = GetComponent<UIButton>();
			if (PlayerPrefs.GetInt("UnlockAll") == 1)
			{
				unlockButton.gameObject.active = false;
			}
			return;
		}
		tickSprite = GetComponent<PackedSprite>();
		tickSprite.PlayAnim(0);
		foreach (Transform item in base.gameObject.transform.parent.transform)
		{
			if (item.name == "LockedImage")
			{
				priceCover = item.gameObject;
				break;
			}
		}
	}

	private void Update()
	{
		if (unlockAllButton)
		{
			if (PlayerPrefs.GetInt("UnlockAll") == 1)
			{
				unlockButton.gameObject.active = false;
			}
		}
		else if (SingletonBase<TrafficShopItemHandler>.Instance.InsuranceFraud)
		{
			tickSprite.PlayAnim(1);
			priceCover.active = false;
			priceCover.transform.GetChild(0).gameObject.active = false;
		}
	}
}
