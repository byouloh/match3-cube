using System;
using Assets.Scripts.Common;
using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.GUIs
{
	public class HUDController : MonoBehaviourBase
	{
		private Vector3 _levelArrowDefaultPosition;

		public CheckBoxHandler MusicCheckBox;
		public CheckBoxHandler SoundsCheckBox;
		public ButtonHandler TryBoomButton;
		public UILabel ScoreLabel;
		public UILabel TimerLabel;
		public Transform LevelArrow;
		public int LevelPartHeight;

		public void Start ()
		{
			AppPrefs appPrefs = AppPrefs.Instance;

			MusicCheckBox.Click += MusicCheckBoxOnClick;
			MusicCheckBox.IsChecked = appPrefs.IsMusicDisable;
	
			SoundsCheckBox.Click += SoundsCheckBoxOnClick;
			SoundsCheckBox.IsChecked = appPrefs.IsSoundDisable;

			TryBoomButton.Click+=TryBoomButtonOnClick;
			_levelArrowDefaultPosition = LevelArrow.localPosition;
		}

		private void TryBoomButtonOnClick(object sender, EventArgs eventArgs)
		{
			TryBoomScript.Instance.TryBoom();
		}

		private void SoundsCheckBoxOnClick(object sender, EventArgs eventArgs)
		{
			AudioManager.SoundSwitchOnOff();
		}

		private void MusicCheckBoxOnClick(object sender, EventArgs eventArgs)
		{
			AudioManager.MusicSwitchOnOff();
		}

		public void Update()
		{
			ScoreLabel.text = ScoreManager.Instance.ViewScore.ToString("000");
			TimerLabel.text = TimerManager.Instance.RemainSeconds.ToString("000");

			float yOffset = LevelsManager.Instance.GetLevelsProgress()*LevelPartHeight;
			LevelArrow.localPosition = _levelArrowDefaultPosition + new Vector3(0, yOffset, 0);
		}		

	}
}
