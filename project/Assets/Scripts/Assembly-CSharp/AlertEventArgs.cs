using System;

public class AlertEventArgs : EventArgs
{
	public string buttonTitle;

	public int buttonIndex;

	public AlertEventArgs(string buttonTitle, int buttonIndex)
	{
		this.buttonTitle = buttonTitle;
		this.buttonIndex = buttonIndex;
	}
}
