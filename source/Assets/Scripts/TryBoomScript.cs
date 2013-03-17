using System;
using Assets.Scripts.Events;
using UnityEngine;

namespace Assets.Scripts
{
	public class TryBoomScript : MonoBehaviour
	{
		private const float BUTTON_WIDTH = 80;

		private bool _isDeadEnd;
		private float _deadEndStartTime;
		private bool _isHintShowing;
		private bool _shouldCheckOnDeadEnd;

		public int HintsOnStart;
		public int RemainHints;
		public float NoDefaultHintPeriod;//in seconds
		public GameObject TryBoomButton;

		public static TryBoomScript Instance { get; private set; }

		public void Awake()
		{
			Instance = this;
			GameEvents.StateChanged.Subscribe(args => _shouldCheckOnDeadEnd = true);
			GameEvents.StartNewGame.Subscribe(OnStartNewGame);
		}

		private void OnStartNewGame(GameEventArgs gameEventArgs)
		{
			RemainHints = HintsOnStart;
			_isDeadEnd = false;
			_isHintShowing = false;
			iTween.Stop(TryBoomButton);
		}

		public void Update()
		{
			if (_shouldCheckOnDeadEnd)
			{
				CheckOnDeadEnd();
			}

			if (_isDeadEnd && !_isHintShowing && _deadEndStartTime + NoDefaultHintPeriod <= Time.time)
			{
				ShowHint();
			}

			if (_isHintShowing)
			{
				if (FieldManager.Instance.HasAvailableMoves())
				{
					_isDeadEnd = false;
					_isHintShowing = false;
					iTween.Stop(TryBoomButton);
				}
			}
		}

		private void CheckOnDeadEnd()
		{
			_shouldCheckOnDeadEnd = false;
			bool isDeadEnd = !FieldManager.Instance.HasAvailableMoves();
			if (isDeadEnd && !_isDeadEnd) //dead end just started
			{
				_isDeadEnd = true;
				_deadEndStartTime = Time.time;

				if (RemainHints > 0)
				{
					RemainHints--;
					ShowHint();
				}
			}
		}

		private void ShowHint()
		{
			_isHintShowing = true;
			iTween.ShakePosition(TryBoomButton, new Vector3(0.01f, 0.01f), 100);
		}

		public void TryBoom()
		{
			if (FieldManager.Instance.HasAvailableMoves())
				FailBoom();
			else
				Boom();
		}

		private void FailBoom()
		{
			TimerManager.Instance.AddPenaltySeconds(5);
			FieldManager.Instance.ShowAvailableMove(0.5f);
		}

		private void Boom()
		{
			AudioManager.Play(Sound.Punch);
			FieldManager.Instance.ClearTopLair();
			_isDeadEnd = false;
			_isHintShowing = false;
			iTween.Stop(TryBoomButton);
		}
	}
}
