using NeonPlay;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
	private Transform cachedTrans;

	public bool isUnderLaneControl = true;

	public Material[] materialList;

	public Material materialDestroyed;

	public int maxPoints;

	public int minPoints;

	public float damageMultiplier = 1f;

	private float paulOffset = 10f;

	public float centerOfGravityOffset = -1f;

	public int laneIndex;

	public string accelerateSound;

	public float damage;

	public float explosionDamageThreshold = 500f;

	public float waited;

	public float currentSpeed;

	public float acceleration;

	public float lastPosition;

	public float position;

	public float stopPoint = 99999f;

	public bool isStopping;

	public bool stoppedForLight;

	public bool isGoingForGreen;

	public bool playedAccelerate;

	public bool survived;

	public GameManager gameManager;

	public IndicatorFlasher indicatorFlasher;

	public Exploder exploder;

	public RandomExplosion explosionObject;

	public AudioSource audioSource;

	public bool isOnFire;

	private bool isSmoking;

	public bool isAlarmOn = true;

	public bool hasExploded;

	private SOUND alarmSound;

	public bool caravan;

	public bool trailer;

	public bool tank;

	public bool truck;

	public bool bus;

	public bool fuelTanker;

	public bool police;

	public bool fireTruck;

	public bool taxi;

	public bool ambulance;

	private Vector3 tankStartingVelocity = Vector3.zero;

	private Vector3 spawn = Vector3.zero;

	private Vector3 direction = Vector3.zero;

	private bool crashed;

	private bool collisionLocationUpdated;

	private float tankSlowSpeed = 1f;

	public float stopGapFront;

	public float stopGapBehind;

	public float minimumTimeToExplode = 1.5f;

	public float maximumTimeToExplode = 3f;

	public int percentageToExplode = 50;

	public int force = 3000;

	public int radius = 20;

	private double alarmStartTime;

	public bool AlarmOn
	{
		get
		{
			return isAlarmOn;
		}
		set
		{
			isAlarmOn = value;
		}
	}

	private void Start()
	{
		cachedTrans = base.transform;
		direction = cachedTrans.forward;
		if (base.gameObject.tag != "Vehicle")
		{
			Debug.LogError("Vehicle " + base.name + " not tagged as a vehicle");
		}
		if (base.gameObject.layer == 10)
		{
			Object.Destroy(GetComponent<Rigidbody>());
		}
		Initialise();
	}

	protected virtual void Initialise()
	{
		if (materialList.Length > 0)
		{
			base.GetComponent<Renderer>().material = materialList[Random.Range(0, materialList.Length)];
		}
		GameObject gameObject = GameObject.Find("Game Manager");
		gameManager = gameObject.GetComponent<GameManager>();
		isGoingForGreen = false;
		gameObject = base.gameObject;
		indicatorFlasher = gameObject.GetComponent<IndicatorFlasher>();
		alarmSound = gameManager.GetAlarm();
		audioSource = GetComponent<AudioSource>();
		if (audioSource != null)
		{
			audioSource.clip = gameManager.GetAudioClip(alarmSound);
		}
		gameObject = base.gameObject;
		exploder = gameObject.GetComponent<Exploder>();
		spawn = base.transform.position;
		if (centerOfGravityOffset != 0f)
		{
			float num = 0f;
			BoxCollider component = GetComponent<BoxCollider>();
			if (component != null)
			{
				num = component.center.y;
			}
			base.GetComponent<Rigidbody>().centerOfMass = new Vector3(0f, centerOfGravityOffset + num, 0f);
		}
	}

	private void Update()
	{
		if (!crashed && trailer)
		{
			cachedTrans.forward = direction;
		}
		if (crashed && damage >= explosionDamageThreshold / 5f)
		{
			AddFlames();
		}
		if (audioSource != null && alarmStartTime != 0.0 && audioSource.isPlaying)
		{
			if ((double)Time.timeSinceLevelLoad - alarmStartTime > 3.0)
			{
				audioSource.Stop();
			}
			if (damage > explosionDamageThreshold * 0.5f)
			{
				audioSource.Stop();
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (isStopping)
		{
			Vector3 center = spawn + base.transform.forward * stopPoint;
			Gizmos.color = new Color(1f, 1f, 0f, 1f);
			Gizmos.DrawCube(center, Vector3.one);
		}
		Vector3 zero = Vector3.zero;
		zero.z += GetHalfLength(true);
		Vector3 vector = zero;
		zero.x -= 1.5f;
		vector.x += 1.5f;
		zero = base.transform.TransformPoint(zero);
		vector = base.transform.TransformPoint(vector);
		Vector3 zero2 = Vector3.zero;
		zero2.z -= GetHalfLength(false);
		Vector3 vector2 = zero2;
		zero2.x -= 1.5f;
		vector2.x += 1.5f;
		zero2 = base.transform.TransformPoint(zero2);
		vector2 = base.transform.TransformPoint(vector2);
		if (!isStopping)
		{
			Gizmos.color = new Color(0f, 1f, 0f, 1f);
		}
		else
		{
			Gizmos.color = new Color(1f, 0f, 0f, 1f);
		}
		Gizmos.DrawLine(vector, zero);
		Gizmos.DrawLine(zero, zero2);
		Gizmos.DrawLine(zero2, vector2);
		Gizmos.DrawLine(vector2, vector);
	}

	private void OnCollisionEnter(Collision other)
	{
		if ((other.gameObject.tag == "Vehicle" || other.gameObject.tag == "WreckingBall") && ((other.gameObject.tag == "Vehicle" && (other.gameObject.GetComponent<VehicleController>().laneIndex != laneIndex || gameManager.GameOver)) || other.gameObject.tag == "WreckingBall"))
		{
			if (other.gameObject.tag == "WreckingBall")
			{
				SingletonBase<AchievementStorage>.Instance.CraneHit++;
			}
			crashed = true;
			if (trailer)
			{
				SingletonBase<AchievementStorage>.Instance.CaravanHit++;
				base.transform.parent.GetComponent<VehicleController>().isUnderLaneControl = false;
			}
			if (tank)
			{
				SingletonBase<AchievementStorage>.Instance.TankHit++;
				TankVengence component = GetComponent<TankVengence>();
				if ((bool)component)
				{
					component.SetTarget(other.gameObject.transform);
				}
			}
			isUnderLaneControl = false;
			if (!collisionLocationUpdated)
			{
				collisionLocationUpdated = true;
				SingletonBase<CameraManager>.Instance.CollisionObjects.Add(base.gameObject);
			}
			if (other.gameObject.tag == "Vehicle" && other.contacts.Length >= 1 && base.GetComponent<Rigidbody>() != null && !base.gameObject.name.Contains("Trailer") && !other.gameObject.name.Contains("Trailer"))
			{
				float num = 0.1f * Mathf.Sqrt(base.GetComponent<Rigidbody>().mass) * Random.Range(0.5f * other.relativeVelocity.magnitude, other.relativeVelocity.magnitude);
				base.GetComponent<Rigidbody>().AddForceAtPosition(Vector3.up * num, other.contacts[0].point);
			}
			gameManager.GameOver = true;
			gameManager.PlayCrash(other.relativeVelocity.magnitude);
			AddSmoke();
			if (isAlarmOn)
			{
				FlashIndicators();
				PlayAlarm();
			}
		}
		if (other.gameObject.tag == "ExplosiveObject")
		{
			crashed = true;
			if (!hasExploded)
			{
				AddExplosion();
			}
		}
		float num2 = 0f;
		if (other.gameObject.tag != "Ground")
		{
			num2 = other.relativeVelocity.magnitude;
		}
		if (!gameManager)
		{
			return;
		}
		if (gameManager.GameOver)
		{
			damage += num2;
			if (damage >= explosionDamageThreshold && !hasExploded)
			{
				hasExploded = true;
				RandomExplosion randomExplosion = base.gameObject.AddComponent<RandomExplosion>();
				if (randomExplosion.Detonate(gameManager, base.gameObject, minimumTimeToExplode, maximumTimeToExplode, percentageToExplode, force, radius) && fuelTanker)
				{
					SingletonBase<AchievementStorage>.Instance.FuelTankerHit++;
				}
			}
		}
		gameManager.AwardDamage(num2 * damageMultiplier / paulOffset);
	}

	public void MakeItExplode(bool immediate, float delay)
	{
		hasExploded = true;
		RandomExplosion randomExplosion = base.gameObject.AddComponent<RandomExplosion>();
		if (immediate)
		{
			randomExplosion.Detonate(gameManager, base.gameObject, 0f + delay, 0f + delay, 100, force, radius);
		}
		else
		{
			randomExplosion.Detonate(gameManager, base.gameObject, minimumTimeToExplode + delay, maximumTimeToExplode + delay, percentageToExplode, force, radius);
		}
	}

	public void UpdatePosition()
	{
		lastPosition = position;
		position += currentSpeed * Time.deltaTime;
	}

	public void AddDamageScore(int score)
	{
		if (score > 100000 || score < 0)
		{
			Debug.LogError("Damage problem: " + score + " (" + base.name + ")");
		}
		gameManager.AwardDamage(score);
	}

	private void AddFlames()
	{
		if (!isOnFire)
		{
			SingletonBase<AchievementStorage>.Instance.VehiclesOnFire++;
			isOnFire = gameManager.sfxManager.AddFlames(base.gameObject);
		}
	}

	private void AddSmoke()
	{
		if (!isSmoking)
		{
			isSmoking = gameManager.sfxManager.AddSmoke(base.gameObject);
		}
	}

	private void AddExplosion()
	{
		if (!hasExploded)
		{
			hasExploded = gameManager.sfxManager.AddExplosion(base.gameObject);
		}
	}

	private void FlashIndicators()
	{
		if (!(indicatorFlasher == null))
		{
			indicatorFlasher.FlashIndicators();
		}
	}

	private void PlayAlarm()
	{
		if (audioSource != null && alarmStartTime == 0.0)
		{
			audioSource.volume = 0.15f;
			audioSource.Play();
			alarmStartTime = Time.timeSinceLevelLoad;
		}
	}

	public float GetHalfLength(bool frontHalf)
	{
		BoxCollider component = GetComponent<BoxCollider>();
		float z = component.extents.z;
		if (frontHalf)
		{
			z += stopGapFront;
			z += component.center.z;
		}
		else
		{
			z -= component.center.z;
			z += stopGapBehind;
		}
		return z * base.transform.localScale.z;
	}

	public void Explode()
	{
		if (exploder != null)
		{
			exploder.goBangNow = true;
		}
	}
}
