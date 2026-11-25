using UnityEngine;

public class EnableHelper : MonoBehaviour
{
	public enum Type
	{
		ADReminder = 0
	}

	public Type type;

	private void OnEnable()
	{
		if (type == Type.ADReminder && (PurchaseManager.Instance.PurchaseMade || PlayerPrefs.GetInt("UnlockAll") == 1))
		{
			base.gameObject.active = false;
		}
	}
}
