using UnityEngine;

public class ShopPrices : MonoBehaviour
{
	public static int Vehicle0 = 2500;

	public static int Vehicle1 = 5000;

	public static int Vehicle2 = 4000;

	public static int Vehicle3 = 3000;

	public static int Vehicle4 = 8000;

	public static int Vehicle5 = 6000;

	public static int Vehicle6 = 10000;

	public static int Vehicle7 = 15000;

	public static int Vehicle8 = 250000;

	public static int City0 = 100000;

	public static int City1 = 20000;

	public static int City2 = 50000;

	public static int City3 = 250000;

	public static int UnlockAll = 750000;

	// Port: el seguro se compraba con dinero real; ahora cuesta créditos
	public static int InsuranceFraud = 150000;

	public static int GetVehiclePrice(int index)
	{
		switch (index)
		{
		case 0:
			return Vehicle0;
		case 1:
			return Vehicle1;
		case 2:
			return Vehicle2;
		case 3:
			return Vehicle3;
		case 4:
			return Vehicle4;
		case 5:
			return Vehicle5;
		case 6:
			return Vehicle6;
		case 7:
			return Vehicle7;
		case 8:
			return Vehicle8;
		default:
			return 0;
		}
	}

	public static int GetBuildingPrice(int index)
	{
		switch (index)
		{
		case 0:
			return City0;
		case 1:
			return City1;
		case 2:
			return City2;
		case 3:
			return City3;
		default:
			return 0;
		}
	}
}
