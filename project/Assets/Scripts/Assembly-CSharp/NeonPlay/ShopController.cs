using System.Collections;

namespace NeonPlay
{
	public sealed class ShopController : SingletonBase<ShopController>
	{
		public bool loadItemsOnStartUp = true;

		public int numberOfItems = 10;

		private bool populated;

		private Hashtable items;

		private ShopController()
		{
			if (loadItemsOnStartUp)
			{
				populate();
			}
		}

		private void populate()
		{
			if (!populated)
			{
				populated = true;
				items = null;
				if (items == null)
				{
					items = new Hashtable(numberOfItems);
				}
			}
		}

		public PurchasedItem purchasedItem(string value)
		{
			if (!loadItemsOnStartUp)
			{
				populate();
			}
			PurchasedItem purchasedItem = null;
			if (!items.ContainsKey(value))
			{
				purchasedItem = new PurchasedItem(value);
				items.Add(value, purchasedItem);
			}
			else
			{
				purchasedItem = (PurchasedItem)items[value];
			}
			return purchasedItem;
		}
	}
}
