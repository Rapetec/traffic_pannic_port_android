using System.Collections;
using NeonPlay;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{
	private static volatile PurchaseManager _instance;

	private static object _lock;

	private bool IABavailable;

	private bool purchaseMade;

	private bool purchaseSuccess;

	private bool inPurchase;

	public readonly string productIdentifier50ThousandDollars = "com.neonplay.trafficpanic2.50000dollars";

	public readonly string productIdentifier150ThousandDollars = "com.neonplay.trafficpanic2.150000dollars";

	public readonly string productIdentifier1MillionDollars = "com.neonplay.trafficpanic2.1000000dollars";

	public readonly string productIdentifier5MillionDollars = "com.neonplay.trafficpanic2.5000000dollars";

	public readonly string productIdentifierUnlockAll = "com.neonplay.trafficpanic2.unlockall";

	public readonly string productIdentifierInsurance = "com.neonplay.trafficpanic2.insurance";

	public bool PurchaseMade
	{
		get
		{
			return purchaseMade;
		}
	}

	public bool PurchaseSuccess
	{
		get
		{
			return purchaseSuccess;
		}
		set
		{
			purchaseSuccess = value;
		}
	}

	public bool InPurchase
	{
		get
		{
			return inPurchase;
		}
		set
		{
			inPurchase = value;
		}
	}

	public static PurchaseManager Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_lock)
				{
					GameObject gameObject = new GameObject();
					gameObject.AddComponent<PurchaseManager>();
					_instance = gameObject.GetComponent<PurchaseManager>();
					gameObject.name = _instance.GetType().ToString();
				}
			}
			return _instance;
		}
	}

	private PurchaseManager()
	{
	}

	static PurchaseManager()
	{
		_instance = null;
		_lock = new object();
	}

	private void RetrieveProducts()
	{
	}

	public void StartSpinnerCoroutine()
	{
		StartCoroutine("ShowSpinner");
	}

	private IEnumerator ShowSpinner()
	{
		yield return new WaitForSeconds(0.2f);
	}

	private void purchaseSuccessfuliOS(string productIdentifier, string receipt, int quantity)
	{
		PurchaseSuccessful(productIdentifier, receipt, quantity);
	}

	private void purchaseSuccessfulAndroid(string productIdentifier)
	{
		PurchaseSuccessful(productIdentifier, string.Empty, 0);
	}

	private void PurchaseSuccessful(string productIdentifier, string receipt, int quantity)
	{
		if (productIdentifier == productIdentifier50ThousandDollars)
		{
			CurrencyManager.AddCurrency(50000);
		}
		else if (productIdentifier == productIdentifier150ThousandDollars)
		{
			CurrencyManager.AddCurrency(150000);
		}
		else if (productIdentifier == productIdentifier1MillionDollars)
		{
			CurrencyManager.AddCurrency(1000000);
		}
		else if (productIdentifier == productIdentifier5MillionDollars)
		{
			CurrencyManager.AddCurrency(5000000);
		}
		else if (productIdentifier == productIdentifierUnlockAll)
		{
			SingletonBase<TrafficShopItemHandler>.Instance.UnlockEverything();
		}
		else if (productIdentifier == productIdentifierInsurance)
		{
			SingletonBase<TrafficShopItemHandler>.Instance.InsuranceFraud = true;
		}
		if (!purchaseMade)
		{
			PlayerPrefs.SetInt("PurchaseMade", 1);
			purchaseMade = true;
		}
		PlayerPrefs.Save();
		purchaseSuccess = true;
		ADManager.SetHidden(true);
	}

	private void purchaseFailed(string error)
	{
		string text = "Unsuccessful purchase";
		text = "Unsuccessful purchase : " + error;
		AlertManager.ShowAlert("Purchase failed", text, "Ok");
	}

	private void purchaseCancelled(string error)
	{
	}

	private void Start()
	{
		IABavailable = true;
		IABAndroid.init("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA3kZ7gShUEEPXP3OYNPo2kizqDiC0vq9awKLOb17YBhfoNN8ngQUVFFyjg/ljKh1N29PZVZFQELqkKG1mzJk+DZU3GmLdaJb7PUOnRHkMxixVj3D8x4IfLQXvU9tfOis8dVjlfbV5haivye4PqW2x597ZYe8W8DHdm41oNOthqNKoKxQAicaGoF1AfloNM0ovEcaYvwc4wWVcxdaO71beCt3lHzMwYGEegPPedEhJhINLcKnoj3lIIiK+7w1iP+OQJe0c2qmKrgU6v7t/LooGsBXqpCd9oHYf3W9LvPI/38fMfdI54CU8WM7yY3GahfeN0spfBZFxH1Onio1bKmwIjQIDAQAB");
		IABAndroid.startCheckBillingAvailableRequest();
		IABAndroidManager.purchaseSucceededEvent += purchaseSuccessfulAndroid;
		IABAndroidManager.purchaseFailedEvent += purchaseFailed;
		IABAndroidManager.purchaseCancelledEvent += purchaseCancelled;
		IABAndroidManager.transactionsRestoredEvent += restoreTransactionsFinished;
		IABAndroidManager.transactionRestoreFailedEvent += restoreTransactionsFailed;
		IABAndroidManager.billingSupportedEvent += billingAvailable;
	}

	private void Update()
	{
	}

	// Port: precio en créditos de lo que antes se compraba con dinero real (0 = no aplica)
	private int CreditPrice(string productIdentifier)
	{
		if (productIdentifier == productIdentifierUnlockAll)
		{
			return ShopPrices.UnlockAll;
		}
		if (productIdentifier == productIdentifierInsurance)
		{
			return ShopPrices.InsuranceFraud;
		}
		return 0;
	}

	private string ProductName(string productIdentifier)
	{
		return (productIdentifier == productIdentifierUnlockAll) ? "Unlock All" : "Insurance Fraud";
	}

	public string Price(string productIdentifier)
	{
		return PriceWithCurrencySymbol(productIdentifier);
	}

	public string PriceWithCurrencySymbol(string productIdentifier)
	{
		int price = CreditPrice(productIdentifier);
		return (price > 0) ? (price.ToString("N0") + " CREDITS") : "TAP FOR INFO";
	}

	private string pendingProduct;

	private int pendingPrice;

	// La tienda de Google Play de 2012 ya no existe. «Unlock All» y el seguro («Insurance
	// Fraud») se pagan ahora con los créditos que se ganan jugando, con la misma confirmación
	// que el resto de la tienda.
	public void Purchase(string productIdentifier)
	{
		int price = CreditPrice(productIdentifier);
		if (price <= 0)
		{
			PurchaseSuccessful(productIdentifier, string.Empty, 0);
			return;
		}
		string name = ProductName(productIdentifier);
		int have = CurrencyManager.GetCurrency();
		if (CurrencyManager.CanSpendCurrency(price))
		{
			pendingProduct = productIdentifier;
			pendingPrice = price;
			AlertManager.SetDelegate(ConfirmCreditPurchase);
			AlertManager.ShowAlert("Buy " + name, "Would you like to buy " + name + " for " + price.ToString("N0") + " credits? You currently have " + have.ToString("N0") + " credits.", "Cancel", "Buy");
		}
		else
		{
			AlertManager.ShowAlert("Not enough Credits", name + " costs " + price.ToString("N0") + " credits and you have " + have.ToString("N0") + ". Earn more by playing the game!", "Ok");
		}
	}

	private void ConfirmCreditPurchase(object sender, System.EventArgs e)
	{
		AlertEventArgs args = (AlertEventArgs)e;
		if (args.buttonIndex != 1 || pendingProduct == null || !CurrencyManager.CanSpendCurrency(pendingPrice))
		{
			pendingProduct = null;
			return;
		}
		string product = pendingProduct;
		pendingProduct = null;
		CurrencyManager.RemoveCurrency(pendingPrice);
		UpdateCurrencyBanner();
		PurchaseSuccessful(product, string.Empty, 0);
	}

	private static void UpdateCurrencyBanner()
	{
		try
		{
			GameObject banner = GameObject.FindWithTag("BannerCurrency");
			if (banner != null)
			{
				SpriteText text = banner.GetComponent<SpriteText>();
				if (text != null)
				{
					text.Text = CurrencyManager.GetCurrency().ToString("N0");
				}
			}
		}
		catch (UnityException)
		{
		}
	}

	public void RestoreTransactions()
	{
		restoreTransactionsFinished();
	}

	private void billingAvailable(bool availability)
	{
		IABavailable = true;
	}

	private void restoreTransactionsFailed(string error)
	{
		AlertManager.ShowAlert("Failed", "Failed to retrieve transactions: " + error, "Ok");
	}

	private void restoreTransactionsFinished()
	{
		AlertManager.ShowAlert("Success", "Purchased items have been successfully restored.", "Ok");
	}

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		if (_instance == null)
		{
			_instance = GetComponent<PurchaseManager>();
		}
		Object.DontDestroyOnLoad(this);
		purchaseMade = PlayerPrefs.GetInt("PurchaseMade") == 1;
	}
}
