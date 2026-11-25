using UnityEngine;

public class MonoBehaviourSingleton : MonoBehaviour
{
	private static MonoBehaviourSingleton mInstance = null;

	private static object _lock = new object();

	public static MonoBehaviourSingleton Instance
	{
		get
		{
			if (mInstance == null)
			{
				lock (_lock)
				{
					mInstance = new GameObject("MonoBehaviourSingleton_ClassContainer").AddComponent<MonoBehaviourSingleton>();
				}
			}
			return mInstance;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(this);
	}
}
