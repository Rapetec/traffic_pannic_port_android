using NeonPlay;
using UnityEngine;

public sealed class TextManager : SingletonBase<TextManager>
{
	private bool hideContinueText;

	private bool maxAlpha;

	private float alpha = 127.5f;

	private TextController controller;

	public TextController TextController
	{
		set
		{
			controller = value;
		}
	}

	private TextManager()
	{
	}

	public void UpdateContinueText()
	{
		if (!maxAlpha)
		{
			alpha += 4f;
			if (alpha >= 255f)
			{
				maxAlpha = true;
			}
		}
		else
		{
			alpha -= 4f;
			if (alpha <= 0f)
			{
				maxAlpha = false;
			}
		}
		controller.continueText.Color = new Color(1f, 1f, 1f, alpha / 255f);
		if (controller.continueText.Text == string.Empty)
		{
			controller.continueText.Text = "TAP TO CONTINUE";
		}
	}

	public void HideContinueText()
	{
		controller.continueText.Text = string.Empty;
	}

	public void CalculateGameOver(int score, int damage, int cash)
	{
		if (!hideContinueText)
		{
			HideContinueText();
		}
		controller.GOscore.Text = score.ToString("N0");
		controller.GOdamage.Text = "$" + damage.ToString("N0");
		controller.GOcash.Text = cash.ToString("N0");
	}
}
