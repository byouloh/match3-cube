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

		public float HintPauseDelay;
		public float OneHitPeriodDelay;

		public void Awake()
		{
			Instance = this;
		}

		public void Start()
		{
			GameEvents.StateChanged.Subscribe(OnStateChanged);
		}

		private void OnStateChanged(GameEventArgs gameEventArgs)
		{
			_lastTimeStateChanged = Time.time;
			_lastTimeHintShowed = Time.time;
		}

		public void Update()
		{
			if (_lastTimeStateChanged + HintPauseDelay <= Time.time &&
			    _lastTimeHintShowed + OneHitPeriodDelay <= Time.time)
			{
				ShowHint();
			}
		}

		private void ShowHint()
		{
			if (FieldManager.Instance.HasAvailableMoves())
			{
				FieldManager.Instance.ShowAvailableMove();
				_lastTimeHintShowed = Time.time;
			}
		}
	}
}
