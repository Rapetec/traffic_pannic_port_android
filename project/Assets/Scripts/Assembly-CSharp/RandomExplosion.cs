using System.Collections;
using NeonPlay;
using UnityEngine;

public class RandomExplosion : MonoBehaviour
{
	public float timeToExplode;

	private int force;

	private int radius;

	public bool explode;

	public bool exploded;

	private GameManager gm;

	private GameObject explosionObject;

	public bool HasExploded()
	{
		return exploded;
	}

	public bool SetToExplode()
	{
		if (exploded)
		{
			return false;
		}
		return explode;
	}

	public bool Detonate(GameManager gameManager, GameObject thisObject, float minimumTimeToExplode, float maximumTimeToExplode, int percentageToExplode, int force, int radius)
	{
		gm = gameManager;
		explosionObject = thisObject;
		this.force = force;
		this.radius = radius;
		int num = Random.Range(0, 101);
		if (num <= percentageToExplode)
		{
			timeToExplode = Random.Range(minimumTimeToExplode, maximumTimeToExplode);
			StartCoroutine(Explode());
			explode = true;
			return true;
		}
		return false;
	}

	private IEnumerator Explode()
	{
		yield return new WaitForSeconds(timeToExplode);
		gm.sfxManager.AddExplosion(explosionObject);
		DoExplode();
	}

	private void DoExplode()
	{
		float num = 0f;
		if (!explode)
		{
			return;
		}
		explode = false;
		exploded = true;
		gm.PlaySound(SOUND.EXPLOSION);
		VehicleController component = GetComponent<VehicleController>();
		if (component != null && (bool)component.materialDestroyed)
		{
			component.GetComponent<Renderer>().material = component.materialDestroyed;
		}
		Vector3 vector = explosionObject.transform.position + Vector3.down * Random.Range(0f, 10f) + Vector3.left * Random.Range(-5f, 5f) + Vector3.forward * Random.Range(-10f, 10f);
		Collider[] array = Physics.OverlapSphere(vector, radius);
		Collider[] array2 = array;
		foreach (Collider collider in array2)
		{
			VehicleController component2 = collider.gameObject.GetComponent<VehicleController>();
			if (component2 != null)
			{
				component2.isUnderLaneControl = false;
			}
			if ((bool)collider.GetComponent<Rigidbody>() && collider.name != "wrecking ball")
			{
				float num2 = 1f;
				float num3 = 0.025f * (float)force * Mathf.Sqrt(collider.GetComponent<Rigidbody>().mass);
				if (collider.gameObject == base.gameObject)
				{
					num2 = 2f;
				}
				collider.GetComponent<Rigidbody>().AddExplosionForce(num3, vector, radius, (float)radius / 4000f * num2, ForceMode.Impulse);
				num += num3;
			}
		}
		float num4 = Mathf.Clamp01(num / 7000f);
		if (num4 > 0.1f && Random.Range(0f, 1f) <= num4 && Random.Range(0f, 1f) < 0.1f)
		{
			SingletonBase<CameraManager>.Instance.MatrixModeOn(base.gameObject);
		}
	}
}
