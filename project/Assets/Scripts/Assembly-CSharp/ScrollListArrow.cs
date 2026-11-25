using UnityEngine;

public class ScrollListArrow : MonoBehaviour
{
	private enum Disable
	{
		Left = 0,
		Right = 1,
		None = 2
	}

	public UIScrollList scrollList;

	public float scrollSpeed = 0.75f;

	public bool leftArrow;

	public bool rightArrow;

	private Disable disable = Disable.None;

	public void Start()
	{
		scrollList.ScrollToItem(1, 0f);
	}

	public void Update()
	{
		if (scrollList.SnappedItem != null)
		{
			if (scrollList.SnappedItem.Index == 0)
			{
				disable = Disable.Left;
			}
			else if (scrollList.SnappedItem.Index == scrollList.Count - 1)
			{
				disable = Disable.Right;
			}
			else
			{
				disable = Disable.None;
			}
			switch (disable)
			{
			case Disable.Left:
				if (leftArrow)
				{
					base.GetComponent<Renderer>().enabled = false;
				}
				else
				{
					base.GetComponent<Renderer>().enabled = true;
				}
				break;
			case Disable.Right:
				if (rightArrow)
				{
					base.GetComponent<Renderer>().enabled = false;
				}
				else
				{
					base.GetComponent<Renderer>().enabled = true;
				}
				break;
			case Disable.None:
				base.GetComponent<Renderer>().enabled = true;
				break;
			}
		}
		else
		{
			scrollList.ScrollToItem(1, 0f);
		}
	}

	public void Scroll()
	{
		IUIListObject snappedItem = scrollList.SnappedItem;
		if (snappedItem == null)
		{
			return;
		}
		if (leftArrow)
		{
			int num = snappedItem.Index - 1;
			if (num >= 0)
			{
				scrollList.ScrollToItem(num, scrollSpeed);
			}
		}
		else
		{
			int num2 = snappedItem.Index + 1;
			if (num2 < scrollList.Count)
			{
				scrollList.ScrollToItem(num2, scrollSpeed);
			}
		}
	}
}
