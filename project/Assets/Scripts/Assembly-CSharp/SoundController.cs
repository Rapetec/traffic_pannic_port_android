using NeonPlay;
using UnityEngine;

public class SoundController : MonoBehaviour
{
	public GameObject soundOn;

	public GameObject soundOff;

	private bool started;

	private void OnEnable()
	{
		if (started)
		{
			SetButtons();
		}
	}

	private void Start()
	{
		started = true;
	}

	private void ToggleSound()
	{
		SingletonBase<SoundManager>.Instance.ToggleSound();
	}

	private void Update()
	{
		SetButtons();
	}

	private void SetButtons()
	{
		if (SingletonBase<SoundManager>.Instance.IsSoundOn)
		{
			soundOn.active = true;
			soundOff.active = false;
		}
		else
		{
			soundOn.active = false;
			soundOff.active = true;
		}
	}
}
