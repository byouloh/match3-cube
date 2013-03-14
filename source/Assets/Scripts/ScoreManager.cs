using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Events;
using Assets.Scripts.GUIs;
using Assets.Scripts.ToastManagement;
using UnityEngine;

namespace Assets.Scripts
{
	public class ScoreManager : MonoBehaviourBase
	{
		public const string SCORE_TOAST_STYLE = "ScoreToastStyleLabel";
		public const string CUBE_SCORE_TOAST_STYLE = "CubeScoreToastStyleKey";

		private bool _isOver;

		public int Score { get; private set; }
		public int ViewScore { get; private set; }

		public int ScoreByCube;

		public static ScoreManager Instance { get; private set; }

		public void Awake()
		{
			Instance = this;
			GameEvents.MatchesRemoved.Subscribe(OnMatchesRemoved);
			GameEvents.VirusGone.Subscribe(OnVirusGone);
			GameEvents.StrawberryRemoved.Subscribe(OnStrawberryRemoved);
			GameEvents.SnakePartRemoved.Subscribe(OnSnakePartsRemoved);
			GameEvents.StartNewGame.Subscribe(OnStartNewGame);
			GameEvents.GameOver.Subscribe(OnGameOver);
			ToastManager.RegisterStyle(SCORE_TOAST_STYLE,
									   new ToastStyle
									   {
										   Effect = Effect.Transparency,
										   Duration = 2f
									   });
			ToastManager.RegisterStyle(CUBE_SCORE_TOAST_STYLE,
				new ToastStyle
				{
					Effect = Effect.Bubble,
					BubbleSpeed = 15,
					Duration = 1.0f,
					IsRandomColor = true
				});
		}

		private void OnGameOver(GameEventArgs gameEventArgs)
		{
			_isOver = true;
		}

		private void OnStartNewGame(GameEventArgs gameEventArgs)
		{
			Score = 0;
			ViewScore = 0;
			_isOver = false;
		}

		private void OnMatchesRemoved(MatchesEventArgs matchesEventArgs)
		{
			List<CubeItem> cubes = matchesEventArgs.Matches.SelectMany(m => m.Cubes).Distinct().ToList();
			int addingScore = cubes.Count*ScoreByCube;
			AddScore(addingScore);

			foreach (CubeItem cubeItem in cubes)
				PushScoreToast(cubeItem.transform.position);
		}

		private void AddScore(int addingScore)
		{
			if (!_isOver)
			{
				ViewScore = Score;
				Score += addingScore;
				iTween.ValueTo(gameObject, iTween.Hash(iT.ValueTo.from, ViewScore, iT.ValueTo.to, Score,
				                                       iT.ValueTo.time, 0.5, iT.ValueTo.onupdate, "OnTweenedScoreUpdate"));
				ToastManager.Push(string.Format("+{0}", addingScore), SCORE_TOAST_STYLE);
				GameEvents.ScoreAdded.Publish(GameEventArgs.Empty);
			}
		}

		private void PushScoreToast(Vector3 position)
		{
			Vector2 screenCoords = Camera.main.WorldToScreenPoint(position);
			ToastManager.Push(string.Format("+{0}", ScoreByCube.ToString()),
			                  new Vector2(screenCoords.x - Screen.width/2, screenCoords.y - Screen.height/2),
			                  CUBE_SCORE_TOAST_STYLE);
		}

		private void OnTweenedScoreUpdate(float score)
		{
			ViewScore = (int) score;
		}

		private void OnVirusGone(GameEventArgs gameEventArgs)
		{
			AddScore(25);
		}

		private void OnStrawberryRemoved(GameEventArgs gameEventArgs)
		{
			AddScore(25);
		}

		private void OnSnakePartsRemoved(GameEventArgs gameEventArgs)
		{
			AddScore(10);
		}

	}
}
