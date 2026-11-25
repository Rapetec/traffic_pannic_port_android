using System.Collections;
using UnityEngine;

public class LoadGameScript : MonoBehaviour
{
	public static string sceneToLoad;

	private void Start()
	{
		if (sceneToLoad == null)
		{
			sceneToLoad = "Menu";
		}
		StartCoroutine(LoadLevelWithProgress());
	}

	private IEnumerator LoadLevelWithProgress()
	{
		AsyncOperation async = Application.LoadLevelAsync(sceneToLoad);
		while (!async.isDone)
		{
			yield return null;
		}
		Object.Destroy(this);
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(this);
	}
}
