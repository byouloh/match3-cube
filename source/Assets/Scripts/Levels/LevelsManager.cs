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
		public const string LEVEL_TOAST_STYLE = "LevelToastStyleKey";

		private readonly int[] _levelPoints = new int[] {100, 150, 200, 250, 300, 350, 400, 450, 500, 550};
		private List<Level> _levels;

		public Level CurrentLevel { get; private set; }

		public static LevelsManager Instance { get; private set; }

		public void Awake()
		{
			Instance = this;

			ToastManager.RegisterStyle(LEVEL_TOAST_STYLE,
			                           new ToastStyle
				                           {
					                           Effect = Effect.Oncoming,
					                           Duration = 2f
				                           });
			GameEvents.StartNewGame.Subscribe(OnStartNewGame);
			GameEvents.GameOver.Subscribe(OnGameOver);
			GameEvents.ScoreAdded.Subscribe(OnScoreAdded);
		}

		public void Start()
		{
			EnsureLevels();
		}

		private void OnStartNewGame(GameEventArgs gameEventArgs)
		{
			ShuffleLevels();
			ChangeLevel(_levels.First());
		}

		private void OnGameOver(GameEventArgs gameEventArgs)
		{
			CurrentLevel.Behaviour.Stop();
			CurrentLevel = null;
		}

		private void ShuffleLevels()
		{
			Level firstLevel = _levels.First();
			_levels.Remove(firstLevel);
			_levels.Shuffle();
			_levels.Insert(0, firstLevel);
			NormalizeLevelNumbers();
		}

		private void NormalizeLevelNumbers()
		{
			int number = 1;
			foreach (Level level in _levels)
			{
				level.Number = number;
				level.Points = _levelPoints[number - 1];
				number++;
			}
		}

		private void EnsureLevels()
		{
			_levels = new List<Level>();
			_levels.Add(new Level
				{
					Title = "Begin",
					Behaviour = EmptyBehaviour.Instance
				});
			_levels.Add(new Level
				{
					Title = "Day and night",
					Behaviour = DayNightBehaviour.Instance
				});
			_levels.Add(new Level
				{
					Title = "Spotlights",
					Behaviour = LightsBehaviour.Instance
				});
			_levels.Add(new Level
				{
					Title = "Wiggle",
					Behaviour = WiggleBehaviour.Instance
				});
			_levels.Add(new Level
				{
					Title = "Snake",
					Behaviour = SnakeBahaviour.Instance
				});
			_levels.Add(new Level
				{
					Title = "Turn",
					Behaviour = TurnBehaviour.Instance
				});
			_levels.Add(new Level
				{
					Title = "Rotation",
					Behaviour = RotationBehaviour.Instance
				});
			_levels.Add(new Level
				{
					Title = "Viruses",
					Behaviour = VirusesBehaviour.Instance
				});
			_levels.Add(new Level
				{
					Title = "Mole",
					Behaviour = MolesBehaviour.Instance
				});
			_levels.Add(new Level
				{
					Title = "Questions",
					Behaviour = QuestionsBehaviour.Instance
				});
			NormalizeLevelNumbers();
		}

		private void OnScoreAdded(GameEventArgs gameEventArgs)
		{
			int score = ScoreManager.Instance.Score;
			foreach (Level level in _levels)
			{
				score -= level.Points;
				if (score < 0)
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
			ToastManager.Push(string.Format("Level {0}\n{1}", CurrentLevel.Number, CurrentLevel.Title), LEVEL_TOAST_STYLE);
			if (level.Number > 1)
			{
				AudioManager.Play(Sound.NextLevel);
				GameEvents.NextLevel.Publish(new GameEventArgs<Level>(level));
			}
		}

		public float GetLevelsProgress()
		{
			float progress = 0;
			int score = ScoreManager.Instance.ViewScore;
			foreach (Level level in _levels)
			{
				if (score > level.Points)
				{
					progress += 1.0f;
					score -= level.Points;
				}
				else
				{
					progress += score*1.0f/level.Points;
					break;
				}
			}
			return progress;
		}
	}
}
