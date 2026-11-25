using System.Collections;
using UnityEngine;

public class FireworksTrail : MonoBehaviour
{
	private enum FireState
	{
		None = 0,
		Reset = 1,
		Trail = 2,
		Explode = 3
	}

	public Vector3 target;

	public float waitAtStart;

	private Transform cachedTrans;

	private Vector3 start;

	private float timer;

	private float timeToReset = 5f;

	private float waitToFire = 0.5f;

	private float speed = 1f;

	private ParticleEmitter emitter;

	private bool sounded;

	private GameManager gameManager;

	private FireState state;

	private void Start()
	{
		GameObject gameObject = GameObject.Find("Game Manager");
		gameManager = gameObject.GetComponent<GameManager>();
		cachedTrans = base.transform;
		emitter = GetComponent<ParticleEmitter>();
		start = cachedTrans.position;
		timeToReset = GetComponentInChildren<ParticleEmitter>().maxEnergy * 2f;
		EnableTrail();
		sounded = false;
	}

	private IEnumerator WaitToStart()
	{
		yield return new WaitForSeconds(waitAtStart);
		state = FireState.Trail;
		sounded = false;
	}

	private void Update()
	{
		switch (state)
		{
		case FireState.None:
			StartCoroutine(WaitToStart());
			break;
		case FireState.Reset:
			cachedTrans.position = start;
			speed = Random.Range(1f, 2f);
			waitToFire = Random.Range(0.7f, 1.5f);
			EnableTrail();
			sounded = false;
			break;
		case FireState.Trail:
			if (Vector3.Distance(cachedTrans.localPosition, target) > 5f)
			{
				speed *= 1.01f;
				cachedTrans.localPosition = Vector3.Lerp(cachedTrans.localPosition, target, Time.deltaTime * speed);
				break;
			}
			DisableTrail();
			state = FireState.Explode;
			if (!sounded)
			{
				gameManager.PlayFireworks();
				sounded = true;
			}
			break;
		case FireState.Explode:
			timer += Time.deltaTime;
			if (timer >= timeToReset)
			{
				timer = 0f;
				state = FireState.Reset;
			}
			break;
		}
	}

	private void EnableTrail()
	{
		foreach (Transform cachedTran in cachedTrans)
		{
			cachedTran.gameObject.active = false;
		}
		StartCoroutine(WaitToEnable(waitToFire));
	}

	private IEnumerator WaitToEnable(float time)
	{
		yield return new WaitForSeconds(time);
		emitter.emit = true;
		state = FireState.Trail;
	}

	private void DisableTrail()
	{
		foreach (Transform cachedTran in cachedTrans)
		{
			cachedTran.gameObject.active = true;
		}
		emitter.emit = false;
	}
}
