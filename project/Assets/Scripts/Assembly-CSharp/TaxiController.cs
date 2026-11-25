using UnityEngine;

public class TaxiController : VehicleController
{
	private const float TAXI_PATIENCE_TIME = 0.5f;

	public HeadlightFlasher headlightFlasher;

	public bool headlightsFlashed;

	private void Start()
	{
		Initialise();
		GameObject gameObject = base.gameObject;
		headlightFlasher = gameObject.GetComponent<HeadlightFlasher>();
		gameManager.PlaySound(SOUND.BEEP_BEEP);
	}

	private void Update()
	{
		if (!isUnderLaneControl)
		{
			Vector3 vector = base.transform.position;
			Vector3 velocity = base.GetComponent<Rigidbody>().velocity;
			if (vector.y >= 10f && velocity.y > 0f)
			{
				velocity.y *= 0.5f;
				base.GetComponent<Rigidbody>().velocity = velocity;
			}
		}
		else if (waited > 0.5f && !headlightsFlashed)
		{
			gameManager.PlaySound(SOUND.BEEEEEEEP);
			headlightFlasher.FlashHeadlights();
			headlightsFlashed = true;
		}
	}
}
