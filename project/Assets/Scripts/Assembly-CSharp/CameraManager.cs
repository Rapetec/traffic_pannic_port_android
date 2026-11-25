using System.Collections.Generic;
using NeonPlay;
using UnityEngine;

public sealed class CameraManager : SingletonBase<CameraManager>
{
	private const double INIT_CAM_TIME = -10.0;

	public const float matrixCamSpeed = 0.15f;

	public const float matrixCamTime = 0.4f;

	public const float matrixCamRadius = 20f;

	public const float matrixCamHeight = 7f;

	public const float matrixCamAngleSpeed = 500f;

	private CameraController controller;

	private List<GameObject> collisionObjects;

	private float interestVolume;

	private bool useWide;

	private bool useReallyWide;

	private double useFireworksStart = -10.0;

	private double useGasStationStart = -10.0;

	private int pumpsSetOff;

	private bool matrixModeOn;

	private GameObject matrixObject;

	private float matrixModeTime;

	private bool matrixModeDone;

	public bool somethingToSee;

	public CameraController Controller
	{
		get
		{
			return controller;
		}
		set
		{
			controller = value;
		}
	}

	public List<GameObject> CollisionObjects
	{
		get
		{
			return collisionObjects;
		}
	}

	private CameraManager()
	{
		collisionObjects = new List<GameObject>();
	}

	public void Reset()
	{
		useWide = false;
		useReallyWide = false;
		useFireworksStart = -10.0;
		useGasStationStart = -10.0;
		somethingToSee = false;
		pumpsSetOff = 0;
		matrixModeDone = false;
	}

	public bool SetActiveCamera(int index)
	{
		if (controller.cameraList[index].enabled)
		{
			return false;
		}
		for (int i = 0; i < controller.cameraList.Count; i++)
		{
			if (i != index)
			{
				controller.cameraList[i].enabled = false;
				continue;
			}
			controller.cameraList[i].enabled = true;
			controller.currentCamera = controller.cameraList[i];
		}
		return true;
	}

	public void UpdateAverageCollisionLocation()
	{
		Vector3 position = Vector3.zero;
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		zero2.y = 1f;
		somethingToSee = false;
		if (collisionObjects.Count <= 0)
		{
			position = new Vector3(0f, 0f, -50f);
			interestVolume = 0f;
		}
		else
		{
			float num = 0f;
			for (int i = 0; i < collisionObjects.Count; i++)
			{
				Vector3 position2 = collisionObjects[i].transform.position;
				float num2 = 0.25f;
				VehicleController component = collisionObjects[i].GetComponent<VehicleController>();
				if ((bool)component)
				{
					if (component.damage > 0f)
					{
						num2 += 0.75f;
					}
					RandomExplosion component2 = component.gameObject.GetComponent<RandomExplosion>();
					if (component2 != null && component2.SetToExplode())
					{
						num2 += 100f;
						somethingToSee = true;
					}
					if (component.isOnFire)
					{
						num2 += 5f;
					}
					TankVengence component3 = component.gameObject.GetComponent<TankVengence>();
					if (component3 != null && component3.IsTargetting())
					{
						num2 += 200f;
						if ((bool)component3.target)
						{
							position += component3.target.position * 150f;
							num2 += 150f;
						}
						somethingToSee = true;
					}
				}
				position += position2 * num2;
				if (position2.x < zero.x)
				{
					zero.x = position2.x;
				}
				if (position2.z < zero.z)
				{
					zero.z = position2.z;
				}
				if (position2.x > zero2.x)
				{
					zero2.x = position2.x;
				}
				if (position2.z > zero2.z)
				{
					zero2.z = position2.z;
				}
				num += num2;
			}
			position /= num;
			interestVolume = (zero2.x - zero.x) * (zero2.z - zero.z);
			useReallyWide = interestVolume > 11000f;
			if (interestVolume > 5000f)
			{
				useWide = true;
			}
		}
		position.y = Mathf.Clamp(position.y, 1f, 15f);
		controller.collisionCube.transform.position = position;
	}

	public bool IsMatrixMode()
	{
		return matrixModeOn;
	}

	public bool IsWideAngle()
	{
		return useWide;
	}

	public bool IsReallyWide()
	{
		return useReallyWide;
	}

	public void FireworksSetOff()
	{
		if (useFireworksStart == -10.0)
		{
			useFireworksStart = Time.timeSinceLevelLoad;
		}
	}

	public bool IsFireworks()
	{
		if (somethingToSee)
		{
			return false;
		}
		return (double)Time.timeSinceLevelLoad - useFireworksStart < 7.0 || (!somethingToSee && useFireworksStart != -10.0);
	}

	public void GasStationSetOff()
	{
		pumpsSetOff++;
		if (pumpsSetOff >= 2 && useGasStationStart == -10.0)
		{
			useGasStationStart = Time.timeSinceLevelLoad;
		}
	}

	public bool IsGasStation()
	{
		if (useGasStationStart == -10.0)
		{
			return false;
		}
		if (somethingToSee)
		{
			return false;
		}
		return (double)Time.timeSinceLevelLoad - useGasStationStart < 7.0 || (!somethingToSee && useGasStationStart != -10.0);
	}

	public void Shake()
	{
		if (!(controller.currentCamera == null))
		{
			CameraShake component = controller.currentCamera.GetComponent<CameraShake>();
			if (component != null)
			{
				component.Shake();
			}
		}
	}

	public void MatrixModeOn(GameObject trackObject)
	{
		if (!matrixModeOn && !matrixModeDone)
		{
			matrixObject = trackObject;
			matrixModeOn = true;
			matrixModeTime = 0f;
			Time.timeScale = 0.15f;
			Time.fixedDeltaTime = 0.003f;
			matrixModeDone = true;
		}
	}

	public GameObject MatrixModeObject()
	{
		if (!matrixModeOn)
		{
			return null;
		}
		return matrixObject;
	}

	public bool MatrixModeUpdate()
	{
		if (!matrixModeOn)
		{
			return false;
		}
		matrixModeTime += Time.deltaTime;
		if (matrixModeTime > 0.4f)
		{
			matrixModeOn = false;
			matrixModeTime = 0f;
			Time.timeScale = 1f;
			Time.fixedDeltaTime = 0.02f;
			return true;
		}
		return false;
	}
}
