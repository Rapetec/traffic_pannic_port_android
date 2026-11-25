using NeonPlay;
using UnityEngine;

public class SpecialEffectsManager : MonoBehaviour
{
	private const int NUMBER_FLAME_EFFECTS = 5;

	private const int NUMBER_SMOKE_EFFECTS = 5;

	private const int NUMBER_EXPLOSIONS = 2;

	public Object flameEffect;

	public Object smokeEffect;

	public Object explosionEffect;

	public GameObject damageItems;

	private GameObject[] flamePool;

	private int flameInstances;

	private GameObject[] smokePool;

	private int smokeInstances;

	public bool makeExplosion;

	private int explosionInstances;

	private bool craneAvailable;

	private bool craneStart;

	private HingeJoint craneJoint;

	private CameraShake cameraShake;

	private GameManager gameManager;

	private void Start()
	{
		GameObject gameObject = GameObject.Find("Game Manager");
		gameManager = gameObject.GetComponent<GameManager>();
		InitialisePool(ref flamePool, 5, flameEffect);
		flameInstances = 0;
		InitialisePool(ref smokePool, 5, smokeEffect);
		smokeInstances = 0;
	}

	private void InitialisePool(ref GameObject[] pool, int numberInPool, Object template)
	{
		pool = new GameObject[numberInPool];
		for (int i = 0; i < numberInPool; i++)
		{
			GameObject gameObject = (GameObject)Object.Instantiate(template, base.transform.position, Quaternion.identity);
			gameObject.GetComponent<ParticleEmitter>().emit = false;
			pool[i] = gameObject;
		}
	}

	public bool AddFlames(GameObject parentObject)
	{
		return AddEffect(ref parentObject, ref flamePool, ref flameInstances);
	}

	public bool AddSmoke(GameObject parentObject)
	{
		return AddEffect(ref parentObject, ref smokePool, ref smokeInstances);
	}

	public bool AddExplosion(GameObject parentObject)
	{
		if (craneAvailable && !craneStart)
		{
			craneStart = true;
			JointLimits limits = default(JointLimits);
			limits.max = 180f;
			craneJoint.limits = limits;
		}
		SingletonBase<CameraManager>.Instance.Shake();
		gameManager.DoneExplosion();
		Vector3 position = new Vector3(parentObject.transform.position.x, 3f, parentObject.transform.position.z);
		Object.Instantiate(damageItems, position, Quaternion.identity);
		position = new Vector3(parentObject.transform.position.x, 10f, parentObject.transform.position.z);
		Quaternion quaternion = new Quaternion(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360), 1f);
		Object.Instantiate(damageItems, position, Quaternion.identity);
		Object.Instantiate(explosionEffect, parentObject.transform.position, Quaternion.identity);
		explosionInstances++;
		return true;
	}

	private bool AddEffect(ref GameObject parentObject, ref GameObject[] pool, ref int noInstances)
	{
		if (noInstances < pool.Length)
		{
			pool[noInstances].transform.parent = parentObject.transform;
			pool[noInstances].transform.localPosition = Vector3.zero;
			pool[noInstances].GetComponent<ParticleEmitter>().emit = true;
			noInstances++;
			return true;
		}
		return false;
	}

	private void Update()
	{
		if (SingletonBase<TrafficShopItemHandler>.Instance.IsBuildingUnlocked(3) && SingletonBase<TrafficShopItemHandler>.Instance.IsBuildingSelected(3) && !craneAvailable)
		{
			craneAvailable = true;
			craneJoint = GameObject.FindWithTag("Crane").GetComponent<HingeJoint>();
		}
		if (makeExplosion)
		{
			Object.Instantiate(explosionEffect, base.transform.position, Quaternion.identity);
			makeExplosion = false;
		}
	}
}
