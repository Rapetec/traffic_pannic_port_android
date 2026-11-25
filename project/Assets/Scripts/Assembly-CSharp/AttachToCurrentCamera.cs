using UnityEngine;

public class AttachToCurrentCamera : MonoBehaviour
{
	private void OnPreRender()
	{
		base.transform.position = Camera.main.transform.position;
	}
}
