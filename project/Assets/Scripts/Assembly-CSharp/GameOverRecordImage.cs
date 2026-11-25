using UnityEngine;

public class GameOverRecordImage : MonoBehaviour
{
	public GameManager gm;

	public bool score;

	private void Start()
	{
		if (score)
		{
			if (!gm.betterScore)
			{
				base.gameObject.active = false;
			}
		}
		else if (!gm.betterDamage)
		{
			base.gameObject.active = false;
		}
	}
}
