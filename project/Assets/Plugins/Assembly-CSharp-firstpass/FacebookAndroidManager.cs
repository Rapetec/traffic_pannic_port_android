using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FacebookAndroidManager : MonoBehaviour
{
	[method: MethodImpl(32)]
	public static event Action authDidSucceedEvent;

	[method: MethodImpl(32)]
	public static event Action<string> authDidFailEvent;

	[method: MethodImpl(32)]
	public static event Action dialogDidSucceedEvent;

	[method: MethodImpl(32)]
	public static event Action dialogWasCancelledEvent;

	[method: MethodImpl(32)]
	public static event Action<string> dialogDidFailEvent;

	[method: MethodImpl(32)]
	public static event Action<object> graphRequestDidSucceedEvent;

	[method: MethodImpl(32)]
	public static event Action<string> graphRequestDidFailEvent;

	[method: MethodImpl(32)]
	public static event Action<object> restRequestDidSucceedEvent;

	[method: MethodImpl(32)]
	public static event Action<string> restRequestDidFailEvent;

	[method: MethodImpl(32)]
	public static event Action<Hashtable> imageUploadDidSucceedEvent;

	[method: MethodImpl(32)]
	public static event Action<string> imageUploadDidFailEvent;

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void authDidSucceed(string empty)
	{
		if (FacebookAndroidManager.authDidSucceedEvent != null)
		{
			FacebookAndroidManager.authDidSucceedEvent();
		}
	}

	public void authDidFail(string error)
	{
		if (FacebookAndroidManager.authDidFailEvent != null)
		{
			FacebookAndroidManager.authDidFailEvent(error);
		}
	}

	public void dialogDidSucceed(string empty)
	{
		if (FacebookAndroidManager.dialogDidSucceedEvent != null)
		{
			FacebookAndroidManager.dialogDidSucceedEvent();
		}
	}

	public void dialogWasCancelled(string empty)
	{
		if (FacebookAndroidManager.dialogWasCancelledEvent != null)
		{
			FacebookAndroidManager.dialogWasCancelledEvent();
		}
	}

	public void dialogDidFail(string error)
	{
		if (FacebookAndroidManager.dialogDidFailEvent != null)
		{
			FacebookAndroidManager.dialogDidFailEvent(error);
		}
	}

	public void graphRequestDidSucceed(string response)
	{
		if (FacebookAndroidManager.graphRequestDidSucceedEvent != null)
		{
			FacebookAndroidManager.graphRequestDidSucceedEvent(MiniJSON.jsonDecode(response));
		}
	}

	public void graphRequestDidFail(string error)
	{
		if (FacebookAndroidManager.graphRequestDidFailEvent != null)
		{
			FacebookAndroidManager.graphRequestDidFailEvent(error);
		}
	}

	public void restRequestDidSucceed(string response)
	{
		if (FacebookAndroidManager.restRequestDidSucceedEvent != null)
		{
			FacebookAndroidManager.restRequestDidSucceedEvent(MiniJSON.jsonDecode(response));
		}
	}

	public void restRequestDidFail(string error)
	{
		if (FacebookAndroidManager.restRequestDidFailEvent != null)
		{
			FacebookAndroidManager.restRequestDidFailEvent(error);
		}
	}

	public void imageUploadDidSucceed(string response)
	{
		if (FacebookAndroidManager.imageUploadDidSucceedEvent != null)
		{
			FacebookAndroidManager.imageUploadDidSucceedEvent(response.hashtableFromJson());
		}
	}

	public void imageUploadDidFail(string error)
	{
		if (FacebookAndroidManager.imageUploadDidFailEvent != null)
		{
			FacebookAndroidManager.imageUploadDidFailEvent(error);
		}
	}
}
