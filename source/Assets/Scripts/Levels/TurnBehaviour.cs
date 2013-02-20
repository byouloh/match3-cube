using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Levels
{
	public class TurnBehaviour : LevelBehaviour
	{
		private TimeEvent _nextTurnEvent;
		private float _curDegree;

		public static TurnBehaviour Instance { get; private set; }

		public Transform Cubes;
		public float Period;//In seconds

		public void Awake()
		{
			Instance = this;
		}

		public void Update()
		{
			if (IsActive && _nextTurnEvent.PopIsOccurred())
			{
				NextTurn();
			}
		}

		public override void OnRun()
		{
			_nextTurnEvent = new TimeEvent(Period);
		}

		public override void OnStop()
		{
			_curDegree = 0;
			iTween.RotateTo(Cubes.gameObject, Vector3.zero, 1);
		}

		private void NextTurn()
		{
			_curDegree += GameRandom.NextBool() ? 90f : -90f;
			iTween.RotateTo(Cubes.gameObject, new Vector3(0, 0, _curDegree), 0.5f);
			AudioManager.Play(Sound.Turn);
		}

	}
}
