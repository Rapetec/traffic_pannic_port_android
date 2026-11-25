using System;
using System.Runtime.CompilerServices;
using NeonPlay;

public sealed class AlertManager : SingletonBase<AlertManager>
{
	public string actionTitle;

	[method: MethodImpl(32)]
	public event EventHandler alertButtonClicked;

	private AlertManager()
	{
	}

	public static void SetDelegate(EventHandler ehandler)
	{
		AlertManager instance = SingletonBase<AlertManager>.Instance;
		instance.alertButtonClicked = (EventHandler)Delegate.Combine(instance.alertButtonClicked, ehandler);
	}

	public static void ShowAlert(string title, string message, string buttonTitle1)
	{
		SingletonBase<AlertManager>.Instance.actionTitle = null;
		EtceteraAndroidManager.alertButtonClickedEvent += SingletonBase<AlertManager>.Instance.alertViewClickedButton;
		EtceteraAndroid.showAlert(title, message, buttonTitle1);
	}

	public static void ShowAlert(string title, string message, string buttonTitle0, string buttonTitle1)
	{
		SingletonBase<AlertManager>.Instance.actionTitle = buttonTitle1;
		EtceteraAndroidManager.alertButtonClickedEvent += SingletonBase<AlertManager>.Instance.alertViewClickedButton;
		EtceteraAndroid.showAlert(title, message, buttonTitle0, buttonTitle1);
	}

	public void alertViewClickedButton(string buttonTitle)
	{
		// Port: se desengancha antes de avisar, para que el aviso pueda abrir otro diálogo
		// (p. ej. «Unlock All» > confirmar el precio en créditos) sin que se pierda.
		EtceteraAndroidManager.alertButtonClickedEvent -= alertViewClickedButton;
		EventHandler handler = this.alertButtonClicked;
		this.alertButtonClicked = null;
		if (handler != null)
		{
			int buttonIndex = 0;
			if (actionTitle != null && actionTitle == buttonTitle)
			{
				buttonIndex = 1;
			}
			AlertEventArgs e = new AlertEventArgs(buttonTitle, buttonIndex);
			handler(this, e);
		}
	}
}
