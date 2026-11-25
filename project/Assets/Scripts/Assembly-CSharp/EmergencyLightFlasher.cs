using UnityEngine;

public class EmergencyLightFlasher : MonoBehaviour
{
	private enum LIGHT_STATE
	{
		START = 0,
		RED_ONCE = 1,
		PAUSE_1 = 2,
		BLUE_TWICE_1 = 3,
		PAUSE_2 = 4,
		BLUE_TWICE_2 = 5,
		PAUSE_3 = 6,
		RED_TWICE_1 = 7,
		PAUSE_4 = 8,
		RED_TWICE_2 = 9,
		PAUSE_5 = 10,
		BLUE_ONCE = 11,
		PAUSE_6 = 12,
		FINISH = 13
	}

	private enum LIGHT_MODE
	{
		RED = 0,
		BLUE = 1,
		OFF = 2
	}

	public const float FLASH_DURATION = 0.1f;

	public const float PAUSE_DURATION = 0.1f;

	public GameObject[] redLights;

	public GameObject[] blueLights;

	private float timer;

	private LIGHT_STATE lightState = LIGHT_STATE.RED_ONCE;

	private void Start()
	{
		ShowLights(redLights, false);
		ShowLights(blueLights, false);
		timer = 0f;
	}

	private void Update()
	{
		timer += Time.deltaTime;
		UpdateLightState();
		switch (GetLightMode())
		{
		case LIGHT_MODE.RED:
			ShowLights(redLights, true);
			ShowLights(blueLights, false);
			break;
		case LIGHT_MODE.BLUE:
			ShowLights(redLights, false);
			ShowLights(blueLights, true);
			break;
		default:
			ShowLights(redLights, false);
			ShowLights(blueLights, false);
			break;
		}
	}

	private void UpdateLightState()
	{
		switch (lightState)
		{
		case LIGHT_STATE.RED_ONCE:
		case LIGHT_STATE.BLUE_TWICE_1:
		case LIGHT_STATE.BLUE_TWICE_2:
		case LIGHT_STATE.RED_TWICE_1:
		case LIGHT_STATE.RED_TWICE_2:
		case LIGHT_STATE.BLUE_ONCE:
			if (timer >= 0.1f)
			{
				NextState();
			}
			break;
		case LIGHT_STATE.START:
		case LIGHT_STATE.FINISH:
			NextState();
			break;
		default:
			if (timer >= 0.1f)
			{
				NextState();
			}
			break;
		}
	}

	private void NextState()
	{
		if (lightState == LIGHT_STATE.FINISH)
		{
			lightState = LIGHT_STATE.START;
		}
		else
		{
			lightState++;
		}
		timer = 0f;
	}

	private LIGHT_MODE GetLightMode()
	{
		switch (lightState)
		{
		case LIGHT_STATE.RED_ONCE:
		case LIGHT_STATE.RED_TWICE_1:
		case LIGHT_STATE.RED_TWICE_2:
			return LIGHT_MODE.RED;
		case LIGHT_STATE.BLUE_TWICE_1:
		case LIGHT_STATE.BLUE_TWICE_2:
		case LIGHT_STATE.BLUE_ONCE:
			return LIGHT_MODE.BLUE;
		default:
			return LIGHT_MODE.OFF;
		}
	}

	private void ShowLights(GameObject[] lights, bool on)
	{
		for (int i = 0; i < lights.Length; i++)
		{
			lights[i].GetComponent<Renderer>().enabled = on;
		}
	}
}
