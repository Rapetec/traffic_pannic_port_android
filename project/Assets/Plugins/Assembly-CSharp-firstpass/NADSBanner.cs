using UnityEngine;

public class NADSBanner : MonoBehaviour
{
	public float refreshTime = 30f;

	private float nextRefresh;

	public int nadWidth = 320;

	public int nadHeight = 70;

	private NADSManager controller;

	private NADSItem ad;

	// Banner de anuncios de Neonplay: se queda oculto. Los paneles de EZ GUI reactivan a sus
	// hijos con SetActiveRecursively, así que no basta con desactivarlo una vez: se esconde cada
	// vez que se activa y no hace nada más.
	private void OnEnable()
	{
		Hide();
	}

	private void Start()
	{
		Hide();
	}

	private void Hide()
	{
		Renderer r = GetComponent<Renderer>();
		if (r != null)
		{
			r.enabled = false;
		}
		foreach (Collider c in GetComponents<Collider>())
		{
			c.enabled = false;
		}
		enabled = false;
	}

	private void Update()
	{
		nextRefresh -= Time.deltaTime;
		if (nextRefresh < 0f || ad == null)
		{
			ad = controller.getAd();
			if (controller.usingDefaultNAD)
			{
				nextRefresh = 1f;
			}
			else
			{
				nextRefresh = refreshTime;
			}
			if (ad != null)
			{
				base.gameObject.GetComponent<Renderer>().material.mainTexture = ad.texture;
				Sprite component = base.gameObject.GetComponent<Sprite>();
				component.Setup(component.width, component.height, new Vector2(ad.rect.x * (float)ad.texture.width, ad.rect.y * (float)ad.texture.height), new Vector2(ad.rect.width * (float)ad.texture.width, ad.rect.height * (float)ad.texture.height));
			}
		}
		if (!Input.GetMouseButtonDown(0))
		{
			return;
		}
		Sprite component2 = base.gameObject.GetComponent<Sprite>();
		Ray ray = component2.renderCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo = default(RaycastHit);
		Physics.Raycast(ray, out hitInfo);
		if (hitInfo.collider != null && hitInfo.collider.tag == "NAD")
		{
			if (NADSManager.instance.usingDefaultNAD)
			{
				LaunchMoreGames("NAD");
			}
			else if (ad != null)
			{
				EtceteraAndroid.showWebView("Neon Play", ad.click_url);
			}
		}
	}

	public static void LaunchMoreGames(string type)
	{
		// "More Games": tienda desaparecida
	}
}
