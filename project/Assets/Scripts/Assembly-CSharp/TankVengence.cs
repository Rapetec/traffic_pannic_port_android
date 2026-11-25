using System.Collections;
using UnityEngine;

public class TankVengence : MonoBehaviour
{
	public Transform target;

	private bool shootTimesUp;

	private bool canShoot;

	private int shotsToTake;

	public Transform turret;

	public Renderer muzzleFlash;

	private GameManager gameManager;

	private bool targetNear;

	private bool upright;

	private void Start()
	{
		GameObject gameObject = GameObject.Find("Game Manager");
		gameManager = gameObject.GetComponent<GameManager>();
		target = null;
		canShoot = true;
		shootTimesUp = false;
		muzzleFlash.enabled = false;
		targetNear = false;
		shotsToTake = Random.Range(2, 5);
		upright = false;
	}

	private void Update()
	{
		targetNear = false;
		upright = false;
		if (target == null || turret == null || !canShoot)
		{
			base.enabled = false;
			return;
		}
		Vector3 forward = target.position - turret.transform.position;
		float y = forward.y;
		forward.y = 0f;
		float magnitude = forward.magnitude;
		if (magnitude < 10f)
		{
			return;
		}
		targetNear = true;
		if (!(Vector3.Dot(base.transform.up, Vector3.up) < 0.99f))
		{
			upright = true;
			if (Mathf.Abs(Vector3.Dot(turret.forward, forward.normalized)) > 0.99f && shootTimesUp)
			{
				StartCoroutine(Shoot());
				return;
			}
			Quaternion to = Quaternion.LookRotation(forward);
			turret.transform.rotation = Quaternion.RotateTowards(turret.transform.rotation, to, 90f * Time.deltaTime);
		}
	}

	public void SetTarget(Transform newTarget)
	{
		base.enabled = true;
		if (!(target != null))
		{
			target = newTarget;
			StartCoroutine(WaitToShoot());
			Debug.Log("Tank targeting " + target.name);
		}
	}

	public bool IsTargetting()
	{
		if (base.enabled && target != null && (canShoot || shotsToTake > 0) && targetNear && upright)
		{
			return true;
		}
		return false;
	}

	private IEnumerator MuzzleFlash()
	{
		muzzleFlash.enabled = true;
		yield return new WaitForSeconds(0.2f);
		muzzleFlash.enabled = false;
	}

	private IEnumerator WaitToShoot()
	{
		shootTimesUp = false;
		yield return new WaitForSeconds(Random.Range(1f, 2f));
		shootTimesUp = true;
	}

	private IEnumerator Shoot()
	{
		canShoot = false;
		yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
		VehicleController vc = target.gameObject.GetComponent<VehicleController>();
		if ((bool)vc)
		{
			gameManager.PlayTankShoot();
			StartCoroutine(MuzzleFlash());
			vc.MakeItExplode(true, 0.15f);
			shotsToTake--;
			if (shotsToTake > 0)
			{
				StartCoroutine(WaitToShoot());
				canShoot = true;
				base.enabled = true;
			}
		}
	}
}
