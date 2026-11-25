using UnityEngine;

public class HeadlightFlasher : MonoBehaviour
{
	private const float FLASH_DURATION = 0.5f;

	private const int FLASHES_MAX = 2;

	public GameObject[] headlights;

	private int flashes;

	private float timer;

	private float duration;

	private bool flashHeadlights;

	private bool on;

	private void Start()
	{
		flashHeadlights = false;
		on = false;
		ShowLights(headlights, on);
		timer = 0f;
		duration = 0.5f;
	}

	private void Update()
	{
		if (flashHeadlights)
		{
			timer += Time.deltaTime;
			if (timer > duration && flashes < 2)
			{
				timer = 0f;
				on = !on;
				ShowLights(headlights, on);
				flashes++;
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

	public void FlashHeadlights()
	{
		flashHeadlights = true;
	}
}
