using System;
using System.Collections;

namespace NeonPlay
{
	public class NPConverter
	{
		public static ArrayList HashTableToArrayList(Hashtable hTable)
		{
			ArrayList arrayList = new ArrayList();
			foreach (int key in hTable.Keys)
			{
				arrayList.Add(hTable[key]);
			}
			return arrayList;
		}

		public static float ConvertToRadians(float angleInDegrees)
		{
			return angleInDegrees / 360f * ((float)Math.PI * 2f);
		}

		public static float ConvertToDegrees(float angleInRadians)
		{
			return angleInRadians * (180f / (float)Math.PI);
		}
	}
}
