using NeonPlay;
using UnityEngine;

public class ResizeScrollView : MonoBehaviour
{
	private void Start()
	{
		if (NPUtility.IsTablet())
		{
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z);
		}
	}
}
