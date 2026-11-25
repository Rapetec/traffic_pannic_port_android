using UnityEngine;

public class SizeSpriteToScreen : MonoBehaviour
{
	public bool sizeHorizontally;

	public bool sizeVertically;

	public float widthOffsetPercent = 100f;

	public float heightOffsetPercent = 100f;

	private void Start()
	{
		Sprite component = GetComponent<Sprite>();
		PackedSprite component2 = GetComponent<PackedSprite>();
		UIBtnChangePanel component3 = GetComponent<UIBtnChangePanel>();
		float w;
		float h;
		if (component == null)
		{
			if (component2 == null)
			{
				if (component3 == null)
				{
					return;
				}
				w = component3.width;
				h = component3.height;
			}
			else
			{
				w = component2.width;
				h = component2.height;
			}
		}
		else
		{
			w = component.width;
			h = component.height;
		}
		// Port: el original medía en píxeles de pantalla (Screen.width) y lo corregía con un factor
		// pensado para los móviles de 2012. En pantallas actuales eso agranda el fondo y el logo
		// más del doble. Aquí se mide en unidades de la cámara de la interfaz, que en el iPhone
		// para el que se diseñó (960x640, cámara de tamaño 320) da exactamente lo mismo.
		SpriteRoot root = GetComponent<SpriteRoot>();
		Camera cam = (root != null && root.renderCamera != null) ? root.renderCamera : Camera.main;
		float worldH = (cam != null && cam.orthographic) ? cam.orthographicSize * 2f : (float)Screen.height;
		float aspect = (cam != null) ? cam.aspect : (float)Screen.width / (float)Screen.height;
		float worldW = worldH * aspect;
		if (sizeHorizontally && sizeVertically && widthOffsetPercent >= 100f && heightOffsetPercent >= 100f)
		{
			// Fondo a pantalla completa: se estira como mucho hasta 16:9 y, en pantallas más
			// alargadas, se recorta por arriba y por abajo en vez de deformarse.
			w = worldW;
			h = Mathf.Max(worldH, worldW / 1.7777778f);
		}
		else
		{
			// Elementos con tamaño relativo en las dos direcciones (el logo): se toman respecto a
			// una pantalla 3:2 como la original, para que no se ensanchen en móviles alargados.
			float baseW = (sizeHorizontally && sizeVertically) ? worldH * Mathf.Min(aspect, 1.5f) : worldW;
			if (sizeHorizontally)
			{
				w = baseW / 100f * widthOffsetPercent;
			}
			if (sizeVertically)
			{
				h = worldH / 100f * heightOffsetPercent;
			}
		}
		if (component != null)
		{
			component.Setup(w, h, component.lowerLeftPixel, component.pixelDimensions);
		}
		if (component2 != null)
		{
			component2.Setup(w, h);
		}
		else if (component3 != null)
		{
			component3.Setup(w, h);
		}
	}
}
