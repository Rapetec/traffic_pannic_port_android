using System;
using System.Diagnostics;
using UnityEngine;

public class AndroidMenuBack : MonoBehaviour
{
	private UIPanelManager pm;

	private void Start()
	{
		pm = GetComponent<UIPanelManager>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (pm.CurrentPanel.name == "Menu")
			{
				AlertManager.SetDelegate(Alert_AndroidBack);
				AlertManager.ShowAlert("Quit Traffic Panic 3D", "Are you sure you want to quit?", "No", "Yes");
			}
			else
			{
				MenuNav.Instance.BringInMenu();
			}
		}
	}

	private void Alert_AndroidBack(object sender, EventArgs e)
	{
		AlertEventArgs alertEventArgs = (AlertEventArgs)e;
		if (alertEventArgs.buttonIndex == 1)
		{
			PlayerPrefs.Save();
			Process.GetCurrentProcess().Kill();
		}
	}
}
