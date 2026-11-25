using NeonPlay;
using UnityEngine;

public class ResizeCamera : MonoBehaviour
{
	public bool Landscape;

	private void Awake()
	{
		if (NPUtility.IsTablet() && Landscape)
		{
			GetComponent<Camera>().orthographicSize *= 1.125f;
		}
	}
}
