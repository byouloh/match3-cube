using UnityEngine;

namespace Assets.Scripts
{
	public class AppPrefs
	{
		private const string IS_MUSIC_DISABLE_KEY = "IsMusicDisable";
		private const string IS_SOUND_DISABLE_KEY = "IsSoundDisable";

		public bool IsMusicDisable { get; set; }
		public bool IsSoundDisable { get; set; }

		public void Load()
		{
			IsMusicDisable = bool.Parse(PlayerPrefs.GetString(IS_MUSIC_DISABLE_KEY, false.ToString()));
			IsSoundDisable = bool.Parse(PlayerPrefs.GetString(IS_SOUND_DISABLE_KEY, false.ToString()));
		}

		public void Save()
		{
			PlayerPrefs.SetString(IS_MUSIC_DISABLE_KEY, IsMusicDisable.ToString());
			PlayerPrefs.SetString(IS_SOUND_DISABLE_KEY, IsSoundDisable.ToString());
		}
	}
}
