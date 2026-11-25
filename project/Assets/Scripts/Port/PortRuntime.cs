using UnityEngine;

// Ajustes de motor que en Unity 3.5 eran el comportamiento por defecto.
public static class PortRuntime
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Init()
	{
		// Unity 3.5 actualizaba los colliders en cuanto se movía un Transform. Desde 2018.3 solo
		// lo hace en el paso de física, que no corre con Time.timeScale = 0. Los botones de EZ GUI
		// se mueven con los paneles (y el menú de pausa sale con el tiempo parado), así que sin
		// esto los toques irían a donde estaba el botón antes.
		Physics.autoSyncTransforms = true;
		// Los colliders de EZ GUI son triggers: los rayos tienen que verlos.
		Physics.queriesHitTriggers = true;
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
	}

	// En Unity 3.5 el 3D de un sonido lo decidía el clip, y todos los de este juego son 2D. Desde
	// Unity 5 lo decide el AudioSource, y al convertir la escena se quedaban en 3D con una curva
	// de distancia a cero: los choques y explosiones no se oían. Las escenas y prefabs ya vienen
	// corregidos; esto cubre cualquier AudioSource que se haya escapado.
	private static void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
	{
		foreach (AudioSource s in Object.FindObjectsOfType<AudioSource>())
		{
			s.spatialBlend = 0f;
		}
	}
}
