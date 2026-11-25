using System.Collections;
using NeonPlay;
using UnityEngine;

public class LaneController : MonoBehaviour
{
	private const float MIN_VEHICLE_GAP = 6f;

	private const float VEHICLE_ACCEL = 500f;

	private const float MINIMUM_PARKING_SEPARATION = 0f;

	private const float MINIMUM_TRAFFIC_LIGHT_SEPARATION = 7.5f;

	public const float DEFAULT_STOP_POINT = 99999f;

	private const float EMERGENCY_STOP_POINT_BUFFER = 10f;

	private ArrayList allowedVehicles;

	private ArrayList allowedVehiclesNoEmergency;

	public VehicleController[] excludedVehicles;

	public bool playerControlled;

	public float laneStopPoint;

	public float truckSpawnTime = 5f;

	private bool spawnTrucks;

	private float emergencyStopPoint;

	public float topSpeed = 30f;

	public float minimumSpawnTime = 2f;

	public float maximumSpawnTime = 5f;

	public float nextSpawnTime;

	public Vector3 spawnPoint;

	public Vector3 direction;

	public float laneLength;

	public ArrayList vehicles;

	public int maxVehicles = 10;

	public int laneIndex;

	public GameManager gameManager;

	public float brakeReactionTime = 1.5f;

	private float minEmergencyTime = 10f;

	private float lastEmergencyTime;

	private void Start()
	{
		GameObject gameObject = GameObject.Find("Game Manager");
		gameManager = gameObject.GetComponent<GameManager>();
		spawnPoint = base.transform.TransformPoint(new Vector3(-0.5f, -2f, 0f));
		laneLength = base.transform.localScale.x;
		direction = base.transform.TransformDirection(new Vector3(1f, 0f, 0f));
		emergencyStopPoint = laneStopPoint - gameManager.stoppingDistance - 10f;
		base.GetComponent<Renderer>().enabled = false;
		BuildAllowedVehiclesList(ref gameManager);
		vehicles = new ArrayList();
		nextSpawnTime = Random.Range(0f, minimumSpawnTime * 0.5f);
		StartCoroutine(SpawnTrucks());
	}

	private void Update()
	{
		if (gameManager.GameOver)
		{
			brakeReactionTime -= Time.deltaTime;
			if (brakeReactionTime < 0f)
			{
				topSpeed -= Time.deltaTime * 50f;
				if (topSpeed < 0f)
				{
					topSpeed = 0f;
				}
			}
		}
		nextSpawnTime -= Time.deltaTime;
		if (nextSpawnTime < 0f)
		{
			SpawnVehicle();
			float num = 0f;
			if (!playerControlled)
			{
				num = Mathf.Clamp01((15f - Time.timeSinceLevelLoad) / 15f) * 0.5f;
				if (Time.timeSinceLevelLoad > 60f)
				{
					float num2 = 1f - Mathf.Clamp01((60f - (Time.timeSinceLevelLoad - 60f)) / 60f * 1f);
					num -= 0.5f * num2;
				}
			}
			else
			{
				num = Mathf.Clamp01((15f - Time.timeSinceLevelLoad) / 15f) * 0.2f;
			}
			nextSpawnTime = Random.Range(minimumSpawnTime + num, maximumSpawnTime + num);
		}
		if (vehicles == null || vehicles.Count <= 0)
		{
			return;
		}
		VehicleController vehicleController = null;
		float num3 = 0f;
		for (int num4 = vehicles.Count - 1; num4 >= 0; num4--)
		{
			VehicleController vehicle = (VehicleController)vehicles[num4];
			vehicleController = ((num4 <= 0) ? null : ((VehicleController)vehicles[num4 - 1]));
			if (vehicle.isUnderLaneControl)
			{
				num3 = vehicle.lastPosition;
				if (vehicle.position > laneLength && !gameManager.GameOver)
				{
					vehicles.RemoveAt(num4);
					Object.Destroy(vehicle.gameObject);
				}
				else
				{
					if (vehicle.position > laneLength / 1.4f && !vehicle.survived)
					{
						vehicle.survived = true;
						VehicleSurvived(ref vehicle);
					}
					bool flag = gameManager.ShouldStop(this);
					if (vehicle.isStopping && gameManager.GameOver)
					{
						flag = true;
					}
					if (!vehicle.stoppedForLight)
					{
						vehicle.isStopping = false;
					}
					if (flag)
					{
						float num5 = laneStopPoint - gameManager.stoppingDistance - vehicle.GetHalfLength(true);
						if (vehicle.position < num5 && vehicle.position + vehicle.currentSpeed * Time.deltaTime >= num5)
						{
							vehicle.isStopping = true;
							if (!vehicle.stoppedForLight)
							{
								vehicle.stoppedForLight = true;
								gameManager.PlaySkid();
							}
						}
					}
					else if (vehicle.stoppedForLight)
					{
						vehicle.isStopping = false;
						vehicle.stoppedForLight = false;
						if (vehicle.currentSpeed == 0f)
						{
							gameManager.PlayAccelerate(vehicle.accelerateSound);
						}
					}
					if (vehicleController != null && vehicleController.currentSpeed <= vehicle.currentSpeed * 0.5f)
					{
						float num6 = Mathf.Abs(vehicle.position - vehicleController.position);
						float num7 = 6f + (vehicleController.GetHalfLength(false) + vehicle.GetHalfLength(true));
						if (num6 < num7)
						{
							vehicle.isStopping = true;
						}
						if (num6 < num7 - 3f)
						{
							vehicle.currentSpeed -= 500f * Time.deltaTime;
							if (vehicle.currentSpeed < 0f)
							{
								vehicle.currentSpeed = 0f;
							}
						}
					}
					if (!vehicle.isStopping && vehicle.currentSpeed != topSpeed)
					{
						vehicle.currentSpeed += 500f * Time.deltaTime;
						if (vehicle.currentSpeed > topSpeed)
						{
							vehicle.currentSpeed = topSpeed;
						}
					}
					if (vehicle.isStopping && vehicle.currentSpeed != 0f)
					{
						vehicle.currentSpeed -= 500f * Time.deltaTime;
						if (vehicle.currentSpeed < 0f)
						{
							vehicle.currentSpeed = 0f;
						}
					}
					if (vehicle.currentSpeed < topSpeed)
					{
						vehicle.waited += Time.deltaTime;
					}
					vehicle.UpdatePosition();
					vehicle.transform.position = spawnPoint + direction * vehicle.position;
					vehicle.transform.forward = direction;
					vehicle.GetComponent<Rigidbody>().velocity = direction * vehicle.currentSpeed;
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(0f, 1f, 0f, 1f);
		Gizmos.DrawLine(spawnPoint, spawnPoint + base.transform.up * 10f);
		Gizmos.color = new Color(1f, 0f, 0f, 1f);
		Vector3 vector = spawnPoint + base.transform.right * laneStopPoint;
		Gizmos.DrawLine(vector, vector + base.transform.up * 10f);
		Gizmos.color = new Color(0f, 1f, 1f, 1f);
		Vector3 vector2 = spawnPoint + base.transform.right * emergencyStopPoint;
		Gizmos.DrawLine(vector2, vector2 + base.transform.up * 10f);
	}

	private void VehicleSurvived(ref VehicleController vehicle)
	{
		if (!playerControlled)
		{
			return;
		}
		float num = 1f - Mathf.Min(gameManager.maxWaitTime, vehicle.waited) / gameManager.maxWaitTime;
		float num2 = num * (float)vehicle.maxPoints;
		if (num2 < (float)vehicle.minPoints)
		{
			num2 = vehicle.minPoints;
		}
		bool hi = false;
		if (num == 1f)
		{
			num2 *= 2f;
			hi = true;
			if (vehicle.truck)
			{
				SingletonBase<AchievementStorage>.Instance.LorryNoStop++;
			}
			if (vehicle.police)
			{
				SingletonBase<AchievementStorage>.Instance.PoliceNoStop++;
			}
			if (vehicle.fireTruck)
			{
				SingletonBase<AchievementStorage>.Instance.FireTruckNoStop++;
			}
			if (vehicle.taxi)
			{
				SingletonBase<AchievementStorage>.Instance.TaxiNoStop++;
			}
			if (vehicle.ambulance)
			{
				SingletonBase<AchievementStorage>.Instance.AmbulanceNoStop++;
			}
			SingletonBase<AchievementStorage>.Instance.TotalNoStop++;
		}
		gameManager.AwardPoints((int)num2, laneIndex, hi);
		if (vehicle.truck)
		{
			gameManager.playerTruck = false;
		}
		if (vehicle.bus)
		{
			gameManager.playerBus = false;
		}
	}

	private bool SpawnVehicle()
	{
		if (gameManager.GameOver || vehicles == null || vehicles.Count >= maxVehicles)
		{
			return false;
		}
		VehicleController proposedVehicle = GetVehicle();
		if (proposedVehicle == null)
		{
			return false;
		}
		if (!CanSpawn(ref proposedVehicle))
		{
			if (playerControlled)
			{
				gameManager.TrafficJam = true;
				gameManager.GameOver = true;
			}
			return false;
		}
		VehicleController vehicleController = (VehicleController)Object.Instantiate(proposedVehicle, spawnPoint, Quaternion.LookRotation(direction));
		vehicleController.laneIndex = laneIndex;
		vehicleController.currentSpeed = topSpeed;
		vehicleController.isStopping = false;
		vehicleController.stoppedForLight = false;
		vehicleController.AlarmOn = false;
		vehicles.Add(vehicleController);
		vehicleController.gameManager = gameManager;
		if (vehicleController.caravan)
		{
			foreach (Transform item in vehicleController.transform)
			{
				if ((bool)item.gameObject.GetComponent<VehicleController>())
				{
					item.gameObject.GetComponent<VehicleController>().laneIndex = laneIndex;
				}
			}
		}
		if (vehicleController.police || vehicleController.ambulance || vehicleController.fireTruck)
		{
			lastEmergencyTime = Time.timeSinceLevelLoad;
		}
		return true;
	}

	private IEnumerator SpawnTrucks()
	{
		yield return new WaitForSeconds(truckSpawnTime);
		spawnTrucks = true;
	}

	private bool IsEmergencyVehicle(ref VehicleController vehicle)
	{
		return vehicle.Equals(gameManager.policeCar);
	}

	private VehicleController GetVehicle()
	{
		ArrayList arrayList = allowedVehiclesNoEmergency;
		if (Time.timeSinceLevelLoad - lastEmergencyTime > minEmergencyTime)
		{
			arrayList = allowedVehicles;
		}
		if (arrayList == null || arrayList.Count == 0)
		{
			return null;
		}
		int num = 0;
		VehicleController result;
		if (spawnTrucks)
		{
			num = Random.Range(0, arrayList.Count);
			result = (VehicleController)arrayList[num];
		}
		else
		{
			num = Random.Range(0, arrayList.Count - 2);
			result = (VehicleController)arrayList[num];
		}
		if (playerControlled)
		{
			if (num == arrayList.Count - 1)
			{
				if (!gameManager.playerBus)
				{
					gameManager.playerBus = true;
				}
				else
				{
					result = (VehicleController)arrayList[num - 1];
				}
			}
			else if (num == arrayList.Count)
			{
				if (!gameManager.playerTruck)
				{
					gameManager.playerTruck = true;
				}
				else
				{
					result = (VehicleController)arrayList[num - 2];
				}
			}
		}
		return result;
	}

	private void BuildAllowedVehiclesList(ref GameManager gameManager)
	{
		allowedVehicles = new ArrayList();
		for (int i = 0; i < gameManager.allowedVehicles.Length; i++)
		{
			if (!IsVehicleExcluded(ref gameManager.allowedVehicles[i]))
			{
				allowedVehicles.Add(gameManager.allowedVehicles[i]);
			}
		}
		allowedVehiclesNoEmergency = allowedVehicles;
		AddPaidForVehicles();
	}

	private void AddPaidForVehicles()
	{
		int num = 0;
		if (gameManager.taxi != null && !IsVehicleExcluded(ref gameManager.taxi) && SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(0) && SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleSelected(0))
		{
			allowedVehicles.Add(gameManager.taxi);
			allowedVehiclesNoEmergency.Add(gameManager.taxi);
		}
		if (gameManager.trailer != null && !IsVehicleExcluded(ref gameManager.trailer) && SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(1) && SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleSelected(1))
		{
			allowedVehicles.Add(gameManager.trailer);
			allowedVehiclesNoEmergency.Add(gameManager.trailer);
		}
		if (gameManager.dassporta != null && !IsVehicleExcluded(ref gameManager.dassporta) && SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(2) && SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleSelected(2))
		{
			allowedVehicles.Add(gameManager.dassporta);
			allowedVehiclesNoEmergency.Add(gameManager.dassporta);
		}
		if (gameManager.policeCar != null && !IsVehicleExcluded(ref gameManager.policeCar) && SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(3) && SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleSelected(3))
		{
			allowedVehicles.Add(gameManager.policeCar);
			num++;
		}
		if (gameManager.ambulance != null && !IsVehicleExcluded(ref gameManager.ambulance) && SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(4) && SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleSelected(4))
		{
			allowedVehicles.Add(gameManager.ambulance);
			num++;
		}
		if (gameManager.italiano != null && !IsVehicleExcluded(ref gameManager.italiano) && SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(5) && SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleSelected(5))
		{
			allowedVehicles.Add(gameManager.italiano);
			allowedVehiclesNoEmergency.Add(gameManager.italiano);
		}
		if (gameManager.fireTruck != null && !IsVehicleExcluded(ref gameManager.fireTruck) && SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(6) && SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleSelected(6))
		{
			allowedVehicles.Add(gameManager.fireTruck);
			num++;
		}
		if (gameManager.fuelTanker != null && !IsVehicleExcluded(ref gameManager.fuelTanker) && SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(7) && SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleSelected(7))
		{
			allowedVehicles.Add(gameManager.fuelTanker);
			allowedVehicles.Add(gameManager.fuelTanker);
			allowedVehiclesNoEmergency.Add(gameManager.fuelTanker);
			allowedVehiclesNoEmergency.Add(gameManager.fuelTanker);
		}
		if (gameManager.tank != null && !IsVehicleExcluded(ref gameManager.tank) && SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleUnlocked(8) && SingletonBase<TrafficShopItemHandler>.Instance.IsVehicleSelected(8))
		{
			allowedVehicles.Add(gameManager.tank);
			allowedVehiclesNoEmergency.Add(gameManager.tank);
		}
		minEmergencyTime = 6f - (float)num;
	}

	private bool IsVehicleExcluded(ref VehicleController vehicle)
	{
		VehicleController[] array = excludedVehicles;
		foreach (VehicleController vehicleController in array)
		{
			if (vehicleController.Equals(vehicle))
			{
				return true;
			}
		}
		return false;
	}

	private bool CanSpawn(ref VehicleController proposedVehicle)
	{
		if (vehicles.Count == 0)
		{
			return true;
		}
		float num = proposedVehicle.GetHalfLength(true) + gameManager.stoppingDistance;
		VehicleController vehicleController = (VehicleController)vehicles[vehicles.Count - 1];
		float num2 = vehicleController.position - vehicleController.GetHalfLength(false);
		if (num < num2)
		{
			return true;
		}
		return false;
	}
}
