using UnityEngine;

namespace NeonPlay
{
	public class NPFileManager : MonoBehaviour
	{
		public static string privateDirectory()
		{
			return Application.dataPath.ToString();
		}

		public static string GetiPhoneDocumentsPath()
		{
			string text = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
			text = text.Substring(0, text.LastIndexOf('/'));
			return text + "/Documents";
		}
	}
}
