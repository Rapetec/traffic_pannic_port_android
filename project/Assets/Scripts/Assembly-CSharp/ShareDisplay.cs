using UnityEngine;

public class ShareDisplay : MonoBehaviour
{
	public GameObject twitterButton;

	public GameObject facebookButton;

	private void Start()
	{
		twitterButton.active = false;
		facebookButton.active = false;
	}

	private void Update()
	{
		twitterButton.active = false;
		facebookButton.active = false;
	}
}
