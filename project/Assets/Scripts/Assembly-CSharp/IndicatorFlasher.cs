using UnityEngine;

public class IndicatorFlasher : MonoBehaviour
{
	private const float FLASH_DURATION_MIN = 0.2f;

	private const float FLASH_DURATION_MAX = 0.4f;

	public GameObject[] indicatorLights;

	private float timer;

	private float duration;

	private bool flashIndicators;

	private bool on;

	private void Start()
	{
		flashIndicators = false;
		on = false;
		ShowLights(indicatorLights, on);
		timer = Random.Range(0f, 0.4f);
		duration = Random.Range(0.2f, 0.4f);
	}

	private void Update()
	{
		if (flashIndicators)
		{
			timer += Time.deltaTime;
			if (timer > duration)
			{
				timer = 0f;
				on = !on;
				ShowLights(indicatorLights, on);
			}
		}
	}

	private void ShowLights(GameObject[] lights, bool on)
	{
		for (int i = 0; i < lights.Length; i++)
		{
			lights[i].GetComponent<Renderer>().enabled = on;
		}
	}

	public void FlashIndicators()
	{
		flashIndicators = true;
	}
}
