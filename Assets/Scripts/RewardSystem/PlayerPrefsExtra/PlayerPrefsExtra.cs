using System.Collections.Generic;
using UnityEngine;

namespace RewardSystem.PlayerPrefsExtra
{
	public static class PlayerPrefsExtra
	{
		#region List <T>
		public class ListWrapper<T>
		{
			public List<T> list = new List<T> ();
		}

		public static List<T> GetList <T> (string key)
		{
			return Get<ListWrapper<T>> (key, new ListWrapper<T> ()).list;
		}

		public static List<T> GetList <T> (string key, List<T> defaultValue)
		{
			return Get<ListWrapper<T>> (key, new ListWrapper<T> { list = defaultValue }).list;
		}

		public static void SetList <T> (string key, List<T> value)
		{
			Set (key, new ListWrapper<T> { list = value });
		}

		#endregion
		
		static T Get<T> (string key, T defaultValue)
		{
			return JsonUtility.FromJson <T> (PlayerPrefs.GetString (key, JsonUtility.ToJson (defaultValue)));
		}

		static void Set<T> (string key, T value)
		{
			PlayerPrefs.SetString (key, JsonUtility.ToJson (value));
		}

	}
}
