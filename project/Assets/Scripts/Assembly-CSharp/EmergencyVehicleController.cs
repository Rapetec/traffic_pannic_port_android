using UnityEngine;

public class EmergencyVehicleController : VehicleController
{
	public enum EMERGENCY_SERVICE
	{
		POLICE = 0,
		AMBULANCE = 1,
		FIRE = 2
	}

	public EMERGENCY_SERVICE service;

	private SOUND siren;

	private void Start()
	{
		Initialise();
		InitialiseSiren();
		gameManager.PlaySound(siren);
	}

	private void InitialiseSiren()
	{
		switch (service)
		{
		case EMERGENCY_SERVICE.POLICE:
			siren = SOUND.SIREN_POLICE;
			break;
		case EMERGENCY_SERVICE.AMBULANCE:
			siren = SOUND.SIREN_AMBULANCE;
			break;
		case EMERGENCY_SERVICE.FIRE:
			siren = SOUND.SIREN_FIRE;
			break;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "EmergencyCollider")
		{
			gameManager.AddEmergencyVehicleInTriggerZone();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "EmergencyCollider")
		{
			gameManager.RemoveEmergencyVehicleFromTriggerZone();
		}
	}
}
