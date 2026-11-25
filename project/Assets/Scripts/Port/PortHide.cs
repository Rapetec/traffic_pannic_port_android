using UnityEngine;

// Oculta un objeto de la interfaz para siempre (se usa en la publicidad de otros juegos de
// Neonplay). No basta con desactivarlo: los paneles de EZ GUI reactivan a sus hijos y los
// botones vuelven a encender su renderer al cambiar de estado, así que se apaga cada frame.
public class PortHide : MonoBehaviour
{
	private void OnEnable()
	{
		Hide();
	}

	private void LateUpdate()
	{
		Hide();
	}

	private void Hide()
	{
		foreach (Renderer r in GetComponentsInChildren<Renderer>(true))
		{
			r.enabled = false;
		}
		foreach (Collider c in GetComponentsInChildren<Collider>(true))
		{
			c.enabled = false;
		}
	}
}
