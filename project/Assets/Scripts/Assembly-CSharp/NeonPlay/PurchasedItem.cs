using UnityEngine;

namespace NeonPlay
{
	public class PurchasedItem
	{
		private string purchasedItemkey;

		private bool _dirty;

		private float _quantity;

		private bool _purchased;

		private bool _selected;

		private int _id;

		public bool dirty
		{
			get
			{
				return _dirty;
			}
			set
			{
			}
		}

		public float quantity
		{
			get
			{
				return _quantity;
			}
			set
			{
				_quantity = value;
				_dirty = true;
			}
		}

		public bool purchased
		{
			get
			{
				return _purchased;
			}
			set
			{
				_purchased = value;
				int value2 = (_purchased ? 1 : 0);
				PlayerPrefs.SetInt(string.Format("{0}_Purchased", purchasedItemkey), value2);
			}
		}

		public bool selected
		{
			get
			{
				return _selected;
			}
			set
			{
				_selected = value;
				int value2 = (_selected ? 1 : 0);
				PlayerPrefs.SetInt(string.Format("{0}_Selected", purchasedItemkey), value2);
			}
		}

		public int id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
				PlayerPrefs.SetInt(string.Format("{0}_Id", purchasedItemkey), _id);
			}
		}

		public PurchasedItem(string key)
		{
			purchasedItemkey = string.Format("ShopControllerItem_{0}", key);
			_quantity = PlayerPrefs.GetFloat(string.Format("{0}_Quantity", purchasedItemkey));
			_purchased = ((PlayerPrefs.GetInt(string.Format("{0}_Purchased", purchasedItemkey)) > 0) ? true : false);
			_selected = ((PlayerPrefs.GetInt(string.Format("{0}_Selected", purchasedItemkey)) > 0) ? true : false);
			_id = PlayerPrefs.GetInt(string.Format("{0}_Id", purchasedItemkey));
		}

		public void SaveQuantity()
		{
			PlayerPrefs.SetFloat(string.Format("{0}_Quantity", purchasedItemkey), _quantity);
			_dirty = false;
		}
	}
}
