using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts
{
	public class AppPrefs : MonoBehaviourBase
	{
		public static AppPrefs Instance { get; private set; }

		private const string IS_MUSIC_DISABLE_KEY = "IsMusicDisable";
		private const string IS_SOUND_DISABLE_KEY = "IsSoundDisable";
		private const string BEST_SCORE_KEY = "BestScore";

		public bool IsMusicDisable { get; set; }
		public bool IsSoundDisable { get; set; }
		public int BestScore { get; set; }

		public void Awake()
		{
			Instance = this;
			Load();
		}

		private void Load()
		{
			IsMusicDisable = bool.Parse(PlayerPrefs.GetString(IS_MUSIC_DISABLE_KEY, false.ToString()));
			IsSoundDisable = bool.Parse(PlayerPrefs.GetString(IS_SOUND_DISABLE_KEY, false.ToString()));
			BestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);
		}

		public void Save()
		{
			PlayerPrefs.SetString(IS_MUSIC_DISABLE_KEY, IsMusicDisable.ToString());
			PlayerPrefs.SetString(IS_SOUND_DISABLE_KEY, IsSoundDisable.ToString());
			PlayerPrefs.SetInt(BEST_SCORE_KEY, BestScore);
		}
	}
}
