using UnityEngine;

namespace NeonPlay
{
	public static class CurrencyManager
	{
		private static int accumulatedCurrencyAmount;

		private static int currencyAmount;

		private static bool retrievedCurrency;

		private static void GetCurrencyFromDataSource()
		{
			if (!retrievedCurrency)
			{
				currencyAmount = PlayerPrefs.GetInt("CurrencyManager_Currency");
				if (currencyAmount < 0)
				{
					currencyAmount = 0;
				}
				accumulatedCurrencyAmount = PlayerPrefs.GetInt("CurrencyManager_AccumulatedCurrency");
				if (accumulatedCurrencyAmount < 0)
				{
					accumulatedCurrencyAmount = 0;
				}
				retrievedCurrency = true;
			}
		}

		public static void AddCurrency(int currency)
		{
			GetCurrencyFromDataSource();
			currencyAmount += currency;
			accumulatedCurrencyAmount += currency;
			int num = 2100000000;
			if (currencyAmount > num)
			{
				currencyAmount = num;
			}
			if (accumulatedCurrencyAmount > num)
			{
				accumulatedCurrencyAmount = num;
			}
			PlayerPrefs.SetInt("CurrencyManager_Currency", currencyAmount);
			PlayerPrefs.SetInt("CurrencyManager_AccumulatedCurrency", accumulatedCurrencyAmount);
		}

		public static void RemoveCurrency(int currency)
		{
			GetCurrencyFromDataSource();
			if (currencyAmount - currency >= 0)
			{
				currencyAmount -= currency;
				PlayerPrefs.SetInt("CurrencyManager_Currency", currencyAmount);
			}
		}

		public static int GetCurrency()
		{
			GetCurrencyFromDataSource();
			return currencyAmount;
		}

		public static int GetAccumulatedCurrency()
		{
			GetCurrencyFromDataSource();
			return accumulatedCurrencyAmount;
		}

		public static bool CanSpendCurrency(int currency)
		{
			GetCurrencyFromDataSource();
			if (currencyAmount - currency >= 0)
			{
				return true;
			}
			return false;
		}
	}
}
