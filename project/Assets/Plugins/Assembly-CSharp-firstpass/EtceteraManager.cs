using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EtceteraManager : MonoBehaviour
{
	public delegate void EceteraTextureDelegate(Texture2D texture);

	public delegate void EceteraTextureFailedDelegate(string error);

	[method: MethodImpl(32)]
	public static event Action imagePickerCancelled;

	[method: MethodImpl(32)]
	public static event Action<string> imagePickerChoseImage;

	[method: MethodImpl(32)]
	public static event Action<string> alertButtonClicked;

	[method: MethodImpl(32)]
	public static event Action promptCancelled;

	[method: MethodImpl(32)]
	public static event Action<string> singleFieldPromptTextEntered;

	[method: MethodImpl(32)]
	public static event Action<string, string> twoFieldPromptTextEntered;

	[method: MethodImpl(32)]
	public static event Action<string> remoteRegistrationSucceeded;

	[method: MethodImpl(32)]
	public static event Action<string> remoteRegistrationFailed;

	[method: MethodImpl(32)]
	public static event Action urbanAirshipRegistrationSucceeded;

	[method: MethodImpl(32)]
	public static event Action<string> urbanAirshipRegistrationFailed;

	[method: MethodImpl(32)]
	public static event Action<string> remoteNotificationReceived;

	[method: MethodImpl(32)]
	public static event Action<string> mailComposerFinished;

	[method: MethodImpl(32)]
	public static event Action<string> smsComposerFinished;

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void imagePickerDidCancel(string empty)
	{
		if (EtceteraManager.imagePickerCancelled != null)
		{
			EtceteraManager.imagePickerCancelled();
		}
	}

	public void imageSavedToDocuments(string filePath)
	{
		if (EtceteraManager.imagePickerChoseImage != null)
		{
			EtceteraManager.imagePickerChoseImage(filePath);
		}
	}

	public static IEnumerator textureFromFileAtPath(string filePath, EceteraTextureDelegate del, EceteraTextureFailedDelegate errorDel)
	{
		// selector de fotos del SDK: desactivado
		yield break;
	}

	public void alertViewClickedButton(string buttonTitle)
	{
		if (EtceteraManager.alertButtonClicked != null)
		{
			EtceteraManager.alertButtonClicked(buttonTitle);
		}
	}

	public void alertPromptCancelled(string empty)
	{
		if (EtceteraManager.promptCancelled != null)
		{
			EtceteraManager.promptCancelled();
		}
	}

	public void alertPromptEnteredText(string text)
	{
		string[] array = text.Split(new string[1] { "|||" }, StringSplitOptions.None);
		if (array.Length == 1 && EtceteraManager.singleFieldPromptTextEntered != null)
		{
			EtceteraManager.singleFieldPromptTextEntered(array[0]);
		}
		if (array.Length == 2 && EtceteraManager.twoFieldPromptTextEntered != null)
		{
			EtceteraManager.twoFieldPromptTextEntered(array[0], array[1]);
		}
	}

	public void remoteRegistrationDidSucceed(string deviceToken)
	{
		if (EtceteraManager.remoteRegistrationSucceeded != null)
		{
			EtceteraManager.remoteRegistrationSucceeded(deviceToken);
		}
	}

	public void remoteRegistrationDidFail(string error)
	{
		if (EtceteraManager.remoteRegistrationFailed != null)
		{
			EtceteraManager.remoteRegistrationFailed(error);
		}
	}

	public void urbanAirshipRegistrationDidSucceed(string empty)
	{
		if (EtceteraManager.urbanAirshipRegistrationSucceeded != null)
		{
			EtceteraManager.urbanAirshipRegistrationSucceeded();
		}
	}

	public void urbanAirshipRegistrationDidFail(string error)
	{
		if (EtceteraManager.urbanAirshipRegistrationFailed != null)
		{
			EtceteraManager.urbanAirshipRegistrationFailed(error);
		}
	}

	public void remoteNotificationWasReceived(string alert)
	{
		if (EtceteraManager.remoteNotificationReceived != null)
		{
			EtceteraManager.remoteNotificationReceived(alert);
		}
	}

	public void mailComposerFinishedWithResult(string result)
	{
		if (EtceteraManager.mailComposerFinished != null)
		{
			EtceteraManager.mailComposerFinished(result);
		}
	}

	public void smsComposerFinishedWithResult(string result)
	{
		if (EtceteraManager.smsComposerFinished != null)
		{
			EtceteraManager.smsComposerFinished(result);
		}
	}
}
