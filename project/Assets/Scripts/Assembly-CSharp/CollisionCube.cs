using UnityEngine;

public class CollisionCube : MonoBehaviour
{
	public string currentlyIn;

	private void OnTriggerEnter(Collider other)
	{
		currentlyIn = other.gameObject.tag;
	}
}
