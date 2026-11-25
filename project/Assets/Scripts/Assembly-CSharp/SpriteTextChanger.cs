using UnityEngine;

public class SpriteTextChanger : MonoBehaviour
{
	public enum Text_Type
	{
		Insurance_Price = 0,
		Unlock_Price = 1
	}

	public Text_Type textType;

	private SpriteText actualFont;

	private bool fontSet;

	private void Start()
	{
		actualFont = GetComponent<SpriteText>();
		switch (textType)
		{
		case Text_Type.Insurance_Price:
			if (PurchaseManager.Instance.PriceWithCurrencySymbol(PurchaseManager.Instance.productIdentifierInsurance) != null)
			{
				actualFont.Text = PurchaseManager.Instance.PriceWithCurrencySymbol(PurchaseManager.Instance.productIdentifierInsurance);
				fontSet = true;
			}
			else
			{
				actualFont.Text = "TAP FOR INFO";
			}
			break;
		case Text_Type.Unlock_Price:
			if (PurchaseManager.Instance.PriceWithCurrencySymbol(PurchaseManager.Instance.productIdentifierUnlockAll) != null)
			{
				actualFont.Text = PurchaseManager.Instance.PriceWithCurrencySymbol(PurchaseManager.Instance.productIdentifierUnlockAll);
				fontSet = true;
			}
			else
			{
				actualFont.Text = string.Empty;
			}
			break;
		}
	}

	private void Update()
	{
		if (fontSet)
		{
			return;
		}
		switch (textType)
		{
		case Text_Type.Insurance_Price:
			if (PurchaseManager.Instance.PriceWithCurrencySymbol(PurchaseManager.Instance.productIdentifierInsurance) != null)
			{
				actualFont.Text = PurchaseManager.Instance.PriceWithCurrencySymbol(PurchaseManager.Instance.productIdentifierInsurance);
				fontSet = true;
			}
			else
			{
				actualFont.Text = "TAP FOR INFO";
			}
			break;
		case Text_Type.Unlock_Price:
			if (PurchaseManager.Instance.PriceWithCurrencySymbol(PurchaseManager.Instance.productIdentifierUnlockAll) != null)
			{
				actualFont.Text = PurchaseManager.Instance.PriceWithCurrencySymbol(PurchaseManager.Instance.productIdentifierUnlockAll);
				fontSet = true;
			}
			else
			{
				actualFont.Text = string.Empty;
			}
			break;
		}
	}
}
