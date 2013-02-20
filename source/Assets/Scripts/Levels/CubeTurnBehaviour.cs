using System;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Levels
{
	public class CubeTurnBehaviour : LevelBehaviour
	{
		public static CubeTurnBehaviour Instance { get; private set; }

		private TimeEvent _nextTurnEvent;
		private float _xAngle;
		private float _yAngle;

		public Transform Cubes;
		public float Period;	//In seconds

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
			_xAngle = 0;
			_yAngle = 0;
		}

		public override void OnStop()
		{
			iTween.RotateTo(Cubes.gameObject, new Vector3(0, 0, 0), 0.5f);
			AudioManager.Play(Sound.Turn);
		}

		private void NextTurn()
		{
			if (GameRandom.NextBool())
			{
				if (GameRandom.NextBool())
				{
					_xAngle += 90.0f;
					FieldManager.Instance.RotateUp();
				}
				else
				{
					_xAngle += -90.0f;
					FieldManager.Instance.RotateDown();
				}
			}
			else
			{
				if (GameRandom.NextBool())
				{
					_yAngle += 90.0f;
					FieldManager.Instance.RotateLeft();
				}
				else
				{
					_yAngle += -90.0f;
					FieldManager.Instance.RotateRight();
				}
			}

			iTween.RotateTo(Cubes.gameObject, new Vector3(_xAngle, _yAngle, 0), 0.5f);
			AudioManager.Play(Sound.Turn);
		}
	}
}
