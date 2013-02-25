using System;
using Assets.Scripts.Common;
using Assets.Scripts.Events;

namespace Assets.Scripts.GUIs
{
	public class GameOverController : MonoBehaviourBase
	{
		public ButtonHandler PlayAgainButton;
		public UILabel BestScoreLabel;
		public UILabel ScoreLabel;
		public UILabel NewRecordLabel;
		public UILabel WinLabel;
		public UILabel GameOverLabel;
		public UILabel TimeIsUpLabel;

		public void Start()
		{
			GameEvents.GameOver.Subscribe(OnGameOver);
			GameEvents.GameWin.Subscribe(OnGameWin);
			gameObject.SetActive(false);
			PlayAgainButton.Click += PlayAgainButtonOnClick;
		}

		private void PlayAgainButtonOnClick(object sender, EventArgs eventArgs)
		{
			GameEvents.StartNewGame.Publish(GameEventArgs.Empty);
			gameObject.SetActive(false);
			ButtonHandler.UnblockButtons();
			CubeItem.UnLock();
		}

		private void OnGameOver(GameEventArgs gameEventArgs)
		{
			GameOver(false);
		}

		private void OnGameWin(GameEventArgs gameEventArgs)
		{
			GameOver(true);
		}

		private void GameOver(bool isGameWin)
		{
			WinLabel.gameObject.SetActive(isGameWin);
			GameOverLabel.gameObject.SetActive(!isGameWin);
			TimeIsUpLabel.gameObject.SetActive(!isGameWin);

			ButtonHandler.BlockButtons();
			ButtonHandler.UnblockButton(PlayAgainButton);
			CubeItem.Lock();

			ScoreLabel.text = string.Format("Score: {0}", ScoreManager.Instance.Score);

			NewRecordLabel.gameObject.SetActive(false);
			if (ScoreManager.Instance.Score > AppPrefs.Instance.BestScore)
			{
				AppPrefs.Instance.BestScore = ScoreManager.Instance.Score;
				AppPrefs.Instance.Save();
				NewRecordLabel.gameObject.SetActive(true);
			}

			BestScoreLabel.text = string.Format("Best score: {0}", AppPrefs.Instance.BestScore);
			gameObject.SetActive(true);
		}


	}
}
