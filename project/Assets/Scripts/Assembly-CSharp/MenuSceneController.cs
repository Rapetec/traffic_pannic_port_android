using NeonPlay;
using UnityEngine;

public class MenuSceneController : MonoBehaviour
{
	public SpriteText currencyText;

	private void Start()
	{
		currencyText.Text = CurrencyManager.GetCurrency().ToString("N0");
	}
}
