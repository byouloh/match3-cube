using System;
using Assets.Scripts.Common;

namespace Assets.Scripts.GUIs
{
	public class HUDController : MonoBehaviourBase
	{
		public ButtonHandler MusicButton;
		public ButtonHandler SoundsButton;
		public ButtonHandler TryBoomButton;
		public UILabel ScoreLabel;
		public UILabel TimerLabel;

		public void Start ()
		{
			MusicButton.Click += MusicButtonOnClick;
			SoundsButton.Click+=SoundsButtonOnClick;
			TryBoomButton.Click+=TryBoomButtonOnClick;
		}

		private void TryBoomButtonOnClick(object sender, EventArgs eventArgs)
		{
			TryBoomScript.Instance.TryBoom();
		}

		private void SoundsButtonOnClick(object sender, EventArgs eventArgs)
		{
			AudioManager.SoundSwitchOnOff();
		}

		private void MusicButtonOnClick(object sender, EventArgs eventArgs)
		{
			AudioManager.MusicSwitchOnOff();
		}

		public void Update()
		{
			ScoreLabel.text = string.Format("Score: {0}", ScoreManager.Instance.ViewScore);
			TimerLabel.text = string.Format("Time: {0}", TimerManager.Instance.RemainSeconds);
		}
	}
}
