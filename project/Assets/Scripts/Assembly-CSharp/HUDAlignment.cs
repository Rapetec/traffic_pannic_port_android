using UnityEngine;

public class HUDAlignment : MonoBehaviour
{
	public Transform LeftElements;

	public Transform RightElements;

	private void Start()
	{
		float num = (float)Screen.width * 640f / (float)Screen.height;
		LeftElements.localPosition = new Vector3((0f - num) * 0.5f, LeftElements.localPosition.y, 0f);
		RightElements.localPosition = new Vector3(num * 0.5f, RightElements.localPosition.y, 0f);
	}
}
