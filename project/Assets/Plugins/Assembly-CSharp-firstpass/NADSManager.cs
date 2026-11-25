using System;
using System.Collections;
using UnityEngine;

public class NADSManager : MonoBehaviour
{
	private static volatile NADSManager _instance;

	private static object _lock;

	public Texture2D defaultNADSTexture;

	public string appleNadsIdentifier = "475292152";

	public string androidNadsIdentifier = "com.neonplay.trafficpanic2";

	public bool usingDefaultNAD = true;

	private string result;

	public string nadsIdentifier = "475292152";

	private NADSItem defaultAd;

	public NADSItem[] ads;

	public static NADSManager instance
	{
		get
		{
			if (_instance == null)
			{
				MonoBehaviour.print("NADSManager does not yet exist, creating...");
				lock (_lock)
				{
					GameObject gameObject = new GameObject();
					gameObject.AddComponent<NADSManager>();
					_instance = gameObject.GetComponent<NADSManager>();
					gameObject.name = _instance.GetType().ToString();
				}
			}
			return _instance;
		}
	}

	private NADSManager()
	{
	}

	static NADSManager()
	{
		_instance = null;
		_lock = new object();
	}

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (_instance == null)
		{
			_instance = GetComponent<NADSManager>();
		}
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Anuncios propios de Neonplay (nads.neonplay.com): desactivados, no se descarga nada
	private void Start()
	{
		result = "desactivado";
		ads = new NADSItem[0];
	}

	public NADSItem getAd()
	{
		usingDefaultNAD = false;
		return null;
	}
}
