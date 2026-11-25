// Solo para comprobar el código con el compilador de Mono contra el UnityEngine 3.5: tipos de
// Unity 2019 que el port usa y que 3.5 no tenía. No va en el proyecto.
namespace UnityEngine
{
	public class AndroidJavaProxy
	{
		public AndroidJavaProxy(string javaInterface) { }
	}
}

namespace UnityEngine.SceneManagement
{
	public struct Scene
	{
		public int buildIndex { get { return 0; } }
	}

	public enum LoadSceneMode { Single, Additive }

	public static class SceneManager
	{
		public static event System.Action<Scene, LoadSceneMode> sceneLoaded;
	}
}
