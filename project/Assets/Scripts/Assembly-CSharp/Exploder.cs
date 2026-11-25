using UnityEngine;

public class Exploder : MonoBehaviour
{
	public bool goBangNow;

	public float radius;

	public float power = 100f;

	public float upAmount = 1f;

	private GameManager gameManager;

	private void Start()
	{
		GameObject gameObject = GameObject.Find("Game Manager");
		gameManager = gameObject.GetComponent<GameManager>();
		base.transform.rotation = Quaternion.identity;
	}

	private void Update()
	{
		if (!goBangNow)
		{
			return;
		}
		goBangNow = false;
		gameManager.PlaySound(SOUND.EXPLOSION);
		Vector3 vector = base.transform.position + Vector3.up * 5f;
		Collider[] array = Physics.OverlapSphere(vector, radius);
		Collider[] array2 = array;
		foreach (Collider collider in array2)
		{
			if (!(collider.gameObject == base.gameObject))
			{
				VehicleController component = collider.gameObject.GetComponent<VehicleController>();
				if (component != null)
				{
					component.isUnderLaneControl = false;
				}
				if ((bool)collider.GetComponent<Rigidbody>())
				{
					float explosionForce = power * collider.GetComponent<Rigidbody>().mass;
					collider.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, vector, radius, upAmount * radius / 1000f);
				}
			}
		}
	}
}
