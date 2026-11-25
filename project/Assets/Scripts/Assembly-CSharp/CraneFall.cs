using NeonPlay;
using UnityEngine;

public class CraneFall : MonoBehaviour
{
	public GameObject wire;

	public HingeJoint joint;

	public GameManager gameManager;

	public float damageFallAmount = 1000f;

	private bool fallen;

	private void Start()
	{
		GameObject gameObject = GameObject.Find("Game Manager");
		gameManager = gameObject.GetComponent<GameManager>();
	}

	private void Update()
	{
		float num = damageFallAmount;
		if (SingletonBase<TrafficShopItemHandler>.Instance.InsuranceFraud)
		{
			num *= 2f;
		}
		if (!fallen && gameManager.damage > num)
		{
			base.GetComponent<Rigidbody>().isKinematic = false;
			joint.breakForce = 0f;
			joint.breakTorque = 0f;
			fallen = true;
			SingletonBase<CameraManager>.Instance.FireworksSetOff();
			base.enabled = false;
			gameManager.AwardDamage(250f);
		}
	}
}
