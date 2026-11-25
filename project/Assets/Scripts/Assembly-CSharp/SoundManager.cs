using NeonPlay;
using UnityEngine;

public sealed class SoundManager : SingletonBase<SoundManager>
{
	private int soundStatus;

	public bool IsSoundOn
	{
		get
		{
			return soundStatus == 0;
		}
	}

	private SoundManager()
	{
		soundStatus = PlayerPrefs.GetInt("Sound");
		SetSound();
	}

	public void ToggleSound()
	{
		soundStatus = Mathf.Abs(soundStatus - 1);
		PlayerPrefs.SetInt("Sound", soundStatus);
		FlurryController.ReportPieChartEvent("Sound", "Pressed", (!IsSoundOn) ? "Off" : "On");
		SetSound();
	}

	private void SetSound()
	{
		AudioListener.volume = (IsSoundOn ? 1 : 0);
	}
}
