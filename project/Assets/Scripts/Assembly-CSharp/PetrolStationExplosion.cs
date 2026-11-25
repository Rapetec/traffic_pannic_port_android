using System.Collections;
using NeonPlay;
using UnityEngine;

public class PetrolStationExplosion : MonoBehaviour
{
	public GameObject explosion;

	public GameObject smoke;

	public GameObject fire;

	public float minSmokeTime;

	public float maxSmokeTime = 0.5f;

	public float minFireTime = 0.5f;

	public float maxFireTime = 1f;

	public float minExplosionTime = 1f;

	public float maxExplosionTime = 1.5f;

	private CameraShake cameraShake;

	private Transform cachedTrans;

	public bool hit;

	private void Start()
	{
		cachedTrans = base.transform;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (!hit)
		{
			hit = true;
			StartCoroutine(Effects());
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!hit)
		{
			hit = true;
			StartCoroutine(Effects());
		}
	}

	private IEnumerator Effects()
	{
		float smokeTime = Random.Range(minSmokeTime, maxSmokeTime);
		yield return new WaitForSeconds(smokeTime);
		Object.Instantiate(smoke, cachedTrans.position, Quaternion.identity);
		float fireTime = Random.Range(minFireTime, maxFireTime);
		SingletonBase<CameraManager>.Instance.GasStationSetOff();
		yield return new WaitForSeconds(fireTime);
		Object.Instantiate(fire, cachedTrans.position, Quaternion.identity);
		float explosionTime = Random.Range(minExplosionTime, maxExplosionTime);
		yield return new WaitForSeconds(explosionTime);
		SingletonBase<CameraManager>.Instance.Shake();
		GameObject explosionPrefab = (GameObject)Object.Instantiate(explosion, cachedTrans.position, Quaternion.identity);
		explosionPrefab.GetComponent<Exploder>().enabled = true;
		SingletonBase<AchievementStorage>.Instance.StationHit++;
	}
}
