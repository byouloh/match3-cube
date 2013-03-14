using Assets.Scripts.Common;
using Assets.Scripts.Events;
using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.Analytics
{
	public class GameAnalytics : MonoBehaviourBase
	{
		public void Awake()
		{
			GameEvents.StartNewGame.Subscribe(OnStartNewGame);
			GameEvents.NextLevel.Subscribe(OnNextLevel);
			GameEvents.GameOver.Subscribe(OnGameOver);
			GameEvents.GameWin.Subscribe(OnGameWin);
			Application.RegisterLogCallback(Log);
		}

		public void Start()
		{
			if (Application.isWebPlayer)
			{
				GA.API.Quality.NewEvent("Web player source", Application.absoluteURL);
			}
		}

		private void Log(string condition, string stackTrace, LogType type)
		{
			if (type == LogType.Error)
				GA.API.Debugging.HandleLog("Error", stackTrace, type);
			else if (type == LogType.Exception)
				GA.API.Debugging.HandleLog("Exception", stackTrace, type);
		}

		private void OnStartNewGame(GameEventArgs gameEventArgs)
		{
			GA.API.Design.NewEvent("StartNewGame");
		}

		private void OnNextLevel(GameEventArgs<Level> gameEventArgs)
		{
			GA.API.Design.NewEvent(string.Format("Level.{0}", gameEventArgs.Object.Number));
		}

		private void OnGameWin(GameEventArgs gameEventArgs)
		{
			GA.API.Design.NewEvent("GameWin");
		}

		private void OnGameOver(GameEventArgs gameEventArgs)
		{
			GA.API.Design.NewEvent("GameOver");
		}

		
	}
}
