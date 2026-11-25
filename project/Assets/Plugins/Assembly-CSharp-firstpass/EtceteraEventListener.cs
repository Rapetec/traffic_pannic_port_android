using UnityEngine;

public class EtceteraEventListener : MonoBehaviour
{
	private void OnEnable()
	{
		EtceteraManager.imagePickerCancelled += imagePickerCancelled;
		EtceteraManager.imagePickerChoseImage += imagePickerChoseImage;
		EtceteraManager.alertButtonClicked += alertButtonClicked;
		EtceteraManager.promptCancelled += promptCancelled;
		EtceteraManager.singleFieldPromptTextEntered += singleFieldPromptTextEntered;
		EtceteraManager.twoFieldPromptTextEntered += twoFieldPromptTextEntered;
		EtceteraManager.remoteRegistrationSucceeded += remoteRegistrationSucceeded;
		EtceteraManager.remoteRegistrationFailed += remoteRegistrationFailed;
		EtceteraManager.urbanAirshipRegistrationSucceeded += urbanAirshipRegistrationSucceeded;
		EtceteraManager.urbanAirshipRegistrationFailed += urbanAirshipRegistrationFailed;
		EtceteraManager.remoteNotificationReceived += remoteNotificationReceived;
		EtceteraManager.mailComposerFinished += mailComposerFinished;
		EtceteraManager.smsComposerFinished += smsComposerFinished;
	}

	private void OnDisable()
	{
		EtceteraManager.imagePickerCancelled -= imagePickerCancelled;
		EtceteraManager.imagePickerChoseImage -= imagePickerChoseImage;
		EtceteraManager.alertButtonClicked -= alertButtonClicked;
		EtceteraManager.promptCancelled -= promptCancelled;
		EtceteraManager.singleFieldPromptTextEntered -= singleFieldPromptTextEntered;
		EtceteraManager.twoFieldPromptTextEntered -= twoFieldPromptTextEntered;
		EtceteraManager.remoteRegistrationSucceeded -= remoteRegistrationSucceeded;
		EtceteraManager.remoteRegistrationFailed -= remoteRegistrationFailed;
		EtceteraManager.urbanAirshipRegistrationSucceeded -= urbanAirshipRegistrationSucceeded;
		EtceteraManager.urbanAirshipRegistrationFailed -= urbanAirshipRegistrationFailed;
		EtceteraManager.mailComposerFinished -= mailComposerFinished;
		EtceteraManager.smsComposerFinished -= smsComposerFinished;
	}

	private void imagePickerCancelled()
	{
		Debug.Log("imagePickerCancelled");
	}

	private void imagePickerChoseImage(string imagePath)
	{
		Debug.Log("image picker chose image: " + imagePath);
	}

	private void alertButtonClicked(string text)
	{
		Debug.Log("alert button clicked: " + text);
	}

	private void promptCancelled()
	{
		Debug.Log("promptCancelled");
	}

	private void singleFieldPromptTextEntered(string text)
	{
		Debug.Log("field : " + text);
	}

	private void twoFieldPromptTextEntered(string textOne, string textTwo)
	{
		Debug.Log("field one: " + textOne + ", field two: " + textTwo);
	}

	private void remoteRegistrationSucceeded(string deviceToken)
	{
		Debug.Log("remoteRegistrationSucceeded with deviceToken: " + deviceToken);
	}

	private void remoteRegistrationFailed(string error)
	{
		Debug.Log("remoteRegistrationFailed : " + error);
	}

	private void urbanAirshipRegistrationSucceeded()
	{
		Debug.Log("urbanAirshipRegistrationSucceeded");
	}

	private void urbanAirshipRegistrationFailed(string error)
	{
		Debug.Log("urbanAirshipRegistrationFailed : " + error);
	}

	private void remoteNotificationReceived(string alert)
	{
		Debug.Log("remoteNotificationReceived: " + alert);
	}

	private void mailComposerFinished(string result)
	{
		Debug.Log("mailComposerFinished : " + result);
	}

	private void smsComposerFinished(string result)
	{
		Debug.Log("smsComposerFinished : " + result);
	}
}
