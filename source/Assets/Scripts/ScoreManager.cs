using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Events;
using Assets.Scripts.ToastManagement;
using UnityEngine;

namespace Assets.Scripts
{
	public class ScoreManager : MonoBehaviourBase
	{
		private const string SCORE_TOAST_STYLE = "ScoreToastStyleLabel";

		public int Score { get; private set; }
		public int ViewScore { get; private set; }

		public int ScoreByCube;
		public GUIStyle ToastStyle;

		public static ScoreManager Instance { get; private set; }

		public void Awake()
		{
			Instance = this;
		}

		public void Start()
		{
			ToastManager.RegisterStyle(SCORE_TOAST_STYLE,
									   new ToastStyle
									   {
										   Effect = Effect.Transparency,
										   Duration = 2f,
										   GUIStyle = ToastStyle
									   });
			GameEvents.MatchesRemoved.Subscribe(OnMatchesRemoved);
			GameEvents.VirusGone.Subscribe(OnVirusGone);
			GameEvents.StrawberryRemoved.Subscribe(OnStrawberryRemoved);
			GameEvents.SnakePartRemoved.Subscribe(OnSnakePartsRemoved);
		}

		public static void Reset()
		{
			Instance.Score = 0;
			Instance.ViewScore = 0;
		}

		private void OnMatchesRemoved(MatchesEventArgs matchesEventArgs)
		{
			int addingScore = matchesEventArgs.Matches.Sum(m => ScoreByCube*(2*m.Cubes.Count - 3));
			AddScore(addingScore);

			foreach (CubeItem cubeItem in matchesEventArgs.Matches.SelectMany(m => m.Cubes))
				PushScoreToast(cubeItem.transform.position);
		}

		private void AddScore(int addingScore)
		{
			ViewScore = Score;
			Score += addingScore;
			iTween.ValueTo(gameObject, iTween.Hash(iT.ValueTo.from, ViewScore, iT.ValueTo.to, Score,
												   iT.ValueTo.time, 0.5, iT.ValueTo.onupdate, "OnTweenedScoreUpdate"));
			ToastManager.Push(string.Format("+{0}", addingScore),
							  new Vector2(Screen.width / 2, 40),
							  SCORE_TOAST_STYLE);
			GameEvents.ScoreAdded.Publish(GameEventArgs.Empty);
		}

		private void PushScoreToast(Vector3 position)
		{
			Vector2 screenCoords = Camera.main.WorldToScreenPoint(position);
			ToastManager.Push(ScoreByCube.ToString(),
							  new Vector2(screenCoords.x, Screen.height - screenCoords.y),
							  SCORE_TOAST_STYLE);
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
