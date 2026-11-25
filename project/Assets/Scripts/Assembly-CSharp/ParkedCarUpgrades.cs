using System.Collections.Generic;
using NeonPlay;
using UnityEngine;

public class ParkedCarUpgrades : MonoBehaviour
{
	public List<GameObject> UpgradeListOne;

	public List<GameObject> UpgradeListTwo;

	private void Start()
	{
		if (SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(5) && SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleSelected(5))
		{
			foreach (GameObject item in UpgradeListOne)
			{
				item.SetActiveRecursively(false);
				GameObject gameObject = (GameObject)Object.Instantiate(Resources.Load("Vehicles/Italiano"), item.transform.position, Quaternion.identity);
				gameObject.transform.Rotate(0f, 270f, 0f);
			}
		}
		if (!SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(2) || !SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleSelected(2))
		{
			return;
		}
		foreach (GameObject item2 in UpgradeListTwo)
		{
			item2.SetActiveRecursively(false);
			GameObject gameObject2 = (GameObject)Object.Instantiate(Resources.Load("Vehicles/DasSporta"), item2.transform.position, Quaternion.identity);
			gameObject2.transform.Rotate(0f, 270f, 0f);
		}
	}
}
