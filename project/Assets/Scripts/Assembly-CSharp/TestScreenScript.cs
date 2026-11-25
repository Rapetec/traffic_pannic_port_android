using UnityEngine;

public class TestScreenScript : MonoBehaviour
{
	public bool checkBoxOn = true;

	public void Button_Clicked()
	{
		Debug.Log("Button clicked");
	}

	public void Checked()
	{
		checkBoxOn = !checkBoxOn;
		if (checkBoxOn)
		{
			Debug.Log("Checked");
		}
		else
		{
			Debug.Log("Unchecked");
		}
	}
}
