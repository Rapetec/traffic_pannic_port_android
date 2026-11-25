using NeonPlay;
using UnityEngine;

public class ExplosiveObject : MonoBehaviour
{
	private bool emitting;

	public int extraDamage = 1000;

	public GameObject particleParent;

	public bool fireworks;

	public bool showroom;

	private void Start()
	{
		particleParent.SetActiveRecursively(false);
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Vehicle" && other.gameObject.layer != 11 && !emitting)
		{
			if (fireworks)
			{
				SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShowTime);
				SingletonBase<CameraManager>.Instance.FireworksSetOff();
			}
			if (showroom)
			{
				SingletonBase<AchievementStorage>.Instance.ShowroomHit++;
			}
			emitting = true;
			other.gameObject.GetComponent<VehicleController>().AddDamageScore(extraDamage);
			if ((bool)particleParent)
			{
				particleParent.SetActiveRecursively(true);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Vehicle" && other.gameObject.layer != 11 && !emitting)
		{
			if (fireworks)
			{
				SingletonBase<ChallengeData>.Instance.ReportAchievement(ChallengeData.AchievementIDShowTime);
				SingletonBase<CameraManager>.Instance.FireworksSetOff();
			}
			if (showroom)
			{
				SingletonBase<AchievementStorage>.Instance.ShowroomHit++;
			}
			emitting = true;
			other.gameObject.GetComponent<VehicleController>().AddDamageScore(extraDamage);
			if ((bool)particleParent)
			{
				particleParent.SetActiveRecursively(true);
			}
		}
	}
}
