using System.Collections.Generic;
using UnityEngine;

// Avisos del juego con el diálogo nativo de Android. En el original los mostraba el plugin
// Etcetera de prime31 (Java); aquí se crea el AlertDialog directamente desde C#. Al pulsar un
// botón se avisa al juego con su texto, igual que el plugin (EtceteraAndroidManager).
// Fuera de Android (editor) se dibuja un cuadro sencillo con OnGUI.
public class NativeAlert : MonoBehaviour
{
	private static NativeAlert instance;

	private static readonly Queue<string> clicks = new Queue<string>();

	private string editorTitle, editorMessage, editorButton0, editorButton1;

	private bool editorVisible;

	private static NativeAlert Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject go = new GameObject("NativeAlert");
				Object.DontDestroyOnLoad(go);
				instance = go.AddComponent<NativeAlert>();
			}
			return instance;
		}
	}

	public static void Show(string title, string message, string button0, string button1)
	{
		Instance.ShowDialog(title ?? string.Empty, message ?? string.Empty, button0 ?? string.Empty, button1 ?? string.Empty);
	}

	private void ShowDialog(string title, string message, string button0, string button1)
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			editorTitle = title;
			editorMessage = message;
			editorButton0 = button0;
			editorButton1 = button1;
			editorVisible = true;
			return;
		}
		AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		activity.Call("runOnUiThread", new AndroidJavaRunnable(delegate
		{
			AndroidJavaObject builder = new AndroidJavaObject("android.app.AlertDialog$Builder", activity);
			builder.Call<AndroidJavaObject>("setTitle", title);
			builder.Call<AndroidJavaObject>("setMessage", message);
			builder.Call<AndroidJavaObject>("setCancelable", false);
			builder.Call<AndroidJavaObject>("setPositiveButton", button0, new ClickListener(button0));
			if (button1.Length > 0)
			{
				builder.Call<AndroidJavaObject>("setNegativeButton", button1, new ClickListener(button1));
			}
			builder.Call<AndroidJavaObject>("show");
		}));
	}

	private class ClickListener : AndroidJavaProxy
	{
		private readonly string button;

		public ClickListener(string button) : base("android.content.DialogInterface$OnClickListener")
		{
			this.button = button;
		}

		// hilo de la interfaz de Android: el aviso al juego se pasa al hilo de Unity
		public void onClick(AndroidJavaObject dialog, int which)
		{
			lock (clicks)
			{
				clicks.Enqueue(button);
			}
		}
	}

	private void Update()
	{
		for (;;)
		{
			string button;
			lock (clicks)
			{
				if (clicks.Count == 0)
				{
					break;
				}
				button = clicks.Dequeue();
			}
			EtceteraAndroidManager.RaiseAlertButtonClicked(button);
		}
	}

	private void OnGUI()
	{
		if (!editorVisible)
		{
			return;
		}
		Rect r = new Rect(Screen.width * 0.1f, Screen.height * 0.3f, Screen.width * 0.8f, Screen.height * 0.4f);
		GUI.Box(r, editorTitle);
		GUI.Label(new Rect(r.x + 10f, r.y + 30f, r.width - 20f, r.height - 80f), editorMessage);
		float bw = (r.width - 30f) / 2f;
		if (GUI.Button(new Rect(r.x + 10f, r.yMax - 45f, bw, 35f), editorButton0))
		{
			editorVisible = false;
			EtceteraAndroidManager.RaiseAlertButtonClicked(editorButton0);
		}
		if (editorButton1.Length > 0 && GUI.Button(new Rect(r.x + 20f + bw, r.yMax - 45f, bw, 35f), editorButton1))
		{
			editorVisible = false;
			EtceteraAndroidManager.RaiseAlertButtonClicked(editorButton1);
		}
	}
}
