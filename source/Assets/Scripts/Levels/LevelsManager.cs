using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Events;
using Assets.Scripts.Levels.SnakeLevel;
using Assets.Scripts.ToastManagement;
using UnityEngine;

namespace Assets.Scripts.Levels
{
	public class LevelsManager : MonoBehaviour
	{
		private const string LEVEL_TOAST_STYLE = "LevelToastStyleKey";

		private List<Level> _levels;

		public Texture LevelsTexture;
		public Texture StripTexture;
		public int PixelsPerLevel = 40;
		public GUIStyle ToastStyle;
		public Level CurrentLevel { get; private set; }

		public static LevelsManager Instance { get; private set; }

		public void Awake()
		{
			Instance = this;

			ToastManager.RegisterStyle(LEVEL_TOAST_STYLE,
									   new ToastStyle
									   {
										   Effect = Effect.Oncoming,
										   Duration = 3f,
										   GUIStyle = ToastStyle
									   });
		}

		public void Start()
		{
			EnsureLevels(false);
			GameEvents.NewGameStarted.Subscribe(OnNewGameStarted);
			GameEvents.GameOver.Subscribe(OnGameOver);
			GameEvents.ScoreAdded.Subscribe(OnScoreAdded);
		}

		private void OnNewGameStarted(GameEventArgs gameEventArgs)
		{
			ChangeLevel(_levels.First());
		}

		private void OnGameOver(GameEventArgs gameEventArgs)
		{
			CurrentLevel.Behaviour.Stop();
			CurrentLevel = null;
		}

		private void EnsureLevels(bool isRandom)
		{
			if (_levels == null)
			{
				_levels = new List<Level>();
				LevelBehaviour[] behaviours = new LevelBehaviour[]
					{
						LightsBehaviour.Instance,
						WiggleBehaviour.Instance,
						SnakeBahaviour.Instance,
						TurnBehaviour.Instance,
						RotationBehaviour.Instance,
						VirusesBehaviour.Instance,
						DayNightBehaviour.Instance,
						MolesBehaviour.Instance,					
						QuestionsBehaviour.Instance
						
						//RunAwayBehaviour.Instance,
					};

				if (isRandom)
					GameRandom.ShuffleArray(behaviours);

				_levels.Add(new Level
					{
						Number = 1,
						Title = "First level",
						Points = 100,
						Behaviour = EmptyBehaviour.Instance
						//Behaviour = QuestionsBehaviour.Instance
					});
				_levels.Add(new Level
					{
						Number = 2,
						Title = "Level 2",
						Points = 300,
						Behaviour = behaviours[0]
					});
				_levels.Add(new Level
					{
						Number = 3,
						Title = "Level 3",
						Points = 350,
						Behaviour = behaviours[1]
					});
				_levels.Add(new Level
					{
						Number = 4,
						Title = "Level 4",
						Points = 350,
						Behaviour = behaviours[2]
					});
				_levels.Add(new Level
					{
						Number = 5,
						Title = "Level 5",
						Points = 350,
						Behaviour = behaviours[3]
					});
				_levels.Add(new Level
					{
						Number = 6,
						Title = "Level 6",
						Points = 350,
						Behaviour = behaviours[4]
					});
				_levels.Add(new Level
					{
						Number = 7,
						Title = "Level 7",
						Points = 350,
						Behaviour = behaviours[5]
					});
				_levels.Add(new Level
					{
						Number = 8,
						Title = "Level 8",
						Points = 350,
						Behaviour = behaviours[6]
					});
				_levels.Add(new Level
					{
						Number = 9,
						Title = "Level 9",
						Points = 350,
						Behaviour = behaviours[7]
					});
				_levels.Add(new Level
					{
						Number = 10,
						Title = "Level 10",
						Points = 350,
						Behaviour = behaviours[8]
					});
			}
		}

		#region OnGUI

		public void OnGUI()
		{
			ShowProgressBar();
		}

		private void ShowProgressBar()
		{
			const int width = 59;
			const int height = 426;
			int left = Screen.width - width - 20;
			int top = (Screen.height - height) / 2;
			GUI.DrawTexture(new Rect(left, top, width, height), LevelsTexture);

			int stripHeight = GetStripHeight(ScoreManager.Instance.ViewScore);
			GUI.DrawTexture(new Rect(left, top + height - stripHeight, 20, stripHeight), StripTexture);
		}

		private int GetStripHeight(int score)
		{
			int height = 0;
			foreach (Level level in _levels)
			{
				if (score > level.Points)
				{
					height += PixelsPerLevel;
					score -= level.Points;
				}
				else
				{
					height += PixelsPerLevel*score/level.Points;
					break;
				}
			}

			return height;
		}

		#endregion

		private void OnScoreAdded(GameEventArgs gameEventArgs)
		{
			int score = ScoreManager.Instance.Score;
			foreach (Level level in _levels)
			{
				score -= level.Points;
				if (score <= 0)
				{
					if (level != CurrentLevel)
						ChangeLevel(level);
					return;
				}
			}
			GameEvents.GameWin.Publish(GameEventArgs.Empty);
			OnGameOver(GameEventArgs.Empty);
		}

		private void ChangeLevel(Level level)
		{
			if (CurrentLevel != null)
				CurrentLevel.Behaviour.Stop();

			CurrentLevel = level;
			CurrentLevel.Behaviour.Run();
			ShowCurrentLevel();
			if (level.Number > 1)
			{
				AudioManager.Play(Sound.NextLevel);
				GameEvents.NextLevel.Publish(new GameEventArgs<Level>(level));
			}
		}

		private void ShowCurrentLevel()
		{
			ToastManager.Push(string.Format("Level {0}\n{1}", CurrentLevel.Number, CurrentLevel.Title),
			                  new Vector2(Screen.width/2, Screen.height/2),
			                  LEVEL_TOAST_STYLE);
		}
	}
}
