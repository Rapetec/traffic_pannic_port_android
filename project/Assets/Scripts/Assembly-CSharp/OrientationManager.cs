using UnityEngine;

public class OrientationManager : MonoBehaviour
{
	private DeviceOrientation orientation;

	private void Start()
	{
		Object.DontDestroyOnLoad(this);
	}

	private void FixedUpdate()
	{
		if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft && Screen.orientation != ScreenOrientation.LandscapeLeft)
		{
			Screen.orientation = ScreenOrientation.LandscapeLeft;
		}
		if (Input.deviceOrientation == DeviceOrientation.LandscapeRight && Screen.orientation != ScreenOrientation.LandscapeRight)
		{
			Screen.orientation = ScreenOrientation.LandscapeRight;
		}
	}
}
