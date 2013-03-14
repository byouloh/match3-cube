using System;
using Assets.Scripts.Common;
using Assets.Scripts.Events;
using UnityEngine;

namespace Assets.Scripts
{
	public class HintsManager : MonoBehaviourBase
	{
		public HintsManager Instance { get; private set; }

		private float _lastTimeStateChanged;
		private float _lastTimeHintShowed;
		private bool _hasStartGameHint;

		public float HintPauseDelay;
		public float OneHitPeriodDelay;

		public void Awake()
		{
			Instance = this;
			GameEvents.StateChanged.Subscribe(OnStateChanged);
			GameEvents.StartNewGame.Subscribe(OnStartNewGame);
		}

		public void Update()
		{
			if (_lastTimeStateChanged + HintPauseDelay <= Time.time &&
			    _lastTimeHintShowed + OneHitPeriodDelay <= Time.time &&
			    !FieldManager.Instance.HasMatches())
			{
				ShowHint(0.5f);
			}

			if (_hasStartGameHint && !GameLocker.IsLocked && !FieldManager.Instance.HasMatches())
			{
				_hasStartGameHint = false;
				ShowHint(3);
			}
		}

		private void OnStateChanged(GameEventArgs gameEventArgs)
		{
			_lastTimeStateChanged = Time.time;
			_lastTimeHintShowed = Time.time;
		}

		private void OnStartNewGame(GameEventArgs gameEventArgs)
		{
			_hasStartGameHint = true;
		}		

		private void ShowHint(float duration)
		{
			if (FieldManager.Instance.HasAvailableMoves())
			{
				FieldManager.Instance.ShowAvailableMove(duration);
				_lastTimeHintShowed = Time.time;
			}
		}
	}
}
