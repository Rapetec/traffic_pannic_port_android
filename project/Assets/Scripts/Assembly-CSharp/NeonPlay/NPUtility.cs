using UnityEngine;

namespace NeonPlay
{
	public class NPUtility : MonoBehaviour
	{
		// En el original, iOS preguntaba a una función nativa (npIsIPad) y Android miraba si la
		// pantalla medía 1024 píxeles o más. Los móviles actuales pasan de 1024 y saldría la
		// interfaz de tableta, así que se decide por la proporción: las tabletas son casi
		// cuadradas (4:3) y los móviles alargados.
		public static bool IsTablet()
		{
			float w = Mathf.Max(Screen.width, Screen.height);
			float h = Mathf.Max(1f, Mathf.Min(Screen.width, Screen.height));
			return w / h < 1.45f;
		}

		// Escala retina de iOS (función nativa npScreenScale); el juego no la usa en Android
		public static float ScreenScale()
		{
			return 1f;
		}
	}
}
