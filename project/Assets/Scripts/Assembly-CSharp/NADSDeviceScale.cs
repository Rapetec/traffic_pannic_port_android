using NeonPlay;
using UnityEngine;

public class NADSDeviceScale : MonoBehaviour
{
	private void Start()
	{
		if (NPUtility.IsTablet())
		{
			base.transform.localScale /= 2f;
		}
	}
}
