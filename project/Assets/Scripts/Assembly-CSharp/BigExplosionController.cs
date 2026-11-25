using UnityEngine;

public class BigExplosionController : MonoBehaviour
{
	public float lifeTime = 1f;

	public float age;

	public GameObject flame;

	public GameObject smoke;

	public Exploder exploder;

	private void Start()
	{
		flame = GameObject.Find("expFlame");
		smoke = GameObject.Find("expSmoke");
	}

	private void Update()
	{
		age += Time.deltaTime;
		if (flame == null && smoke == null)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
