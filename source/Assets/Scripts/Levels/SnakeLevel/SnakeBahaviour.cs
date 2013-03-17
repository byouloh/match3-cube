using System;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Events;
using UnityEngine;

namespace Assets.Scripts.Levels.SnakeLevel
{
	public class SnakeBahaviour : LevelBehaviour
	{
		public static SnakeBahaviour Instance { get; private set; }

		private Snake _snake;
		private TimeEvent _nextMoveEvent;
		private Position _strawberryPosition;
		private Vector3 _strawberryDefaultScale;

		public Transform SnakePrefab;
		public Transform Strawberry;
		public float MoveDelay;

		public void Awake()
		{
			Instance = this;
			GameEvents.CubeRemoved.Subscribe(OnCubeRemoved);
		}

		public void Update()
		{
			if (IsActive && _nextMoveEvent.PopIsOccurred())
			{
				MoveSnake();
			}
		}

		private void MoveSnake()
		{
			Direction direction = GetToStrawberryDirection();
			Position nextPosition = direction.Apply(_snake.HeadPosition);

			if (_snake.Contains(nextPosition))
				_snake.Cut(nextPosition);

			if (nextPosition.Equals(_strawberryPosition))
			{
				_snake.Eat(direction);
				SetStrawberryNextPosition();
				AnimateStrawberry();
			}
			else
				_snake.Move(direction);
			GameEvents.SnakeMoved.Publish(new PositionEventArgs(_snake.HeadPosition));
			
		}

		private Direction GetToStrawberryDirection()
		{
			List<Direction> priorDirections = GetPriorDirections();
			foreach (Direction direction in priorDirections)
			{
				Position nextPosition = direction.Apply(_snake.HeadPosition);
				if (!_snake.Contains(nextPosition) && FieldManager.Instance.Contains(nextPosition))
					return direction;
			}
			return priorDirections[0];
		}

		private List<Direction> GetPriorDirections()
		{
			List<Direction> priorDirections = new List<Direction>();
			Position dp = _strawberryPosition - _snake.HeadPosition;
			if (Math.Abs(dp.X) >= Math.Abs(dp.Y))
			{
				if (dp.X > 0)
				{
					priorDirections.Add(Direction.Right);
					if (dp.Y > 0)
					{
						priorDirections.Add(Direction.Down);
						priorDirections.Add(Direction.Up);
						priorDirections.Add(Direction.Left);
					}
					else
					{
						priorDirections.Add(Direction.Up);
						priorDirections.Add(Direction.Down);
						priorDirections.Add(Direction.Left);
					}
				}
				else
				{
					priorDirections.Add(Direction.Left);
					if (dp.Y > 0)
					{
						priorDirections.Add(Direction.Down);
						priorDirections.Add(Direction.Up);
						priorDirections.Add(Direction.Right);
					}
					else
					{
						priorDirections.Add(Direction.Up);
						priorDirections.Add(Direction.Down);
						priorDirections.Add(Direction.Right);
					}
				}
			}
			else
			{
				if (dp.Y > 0)
				{
					priorDirections.Add(Direction.Down);
					if (dp.X > 0)
					{
						priorDirections.Add(Direction.Right);
						priorDirections.Add(Direction.Left);
						priorDirections.Add(Direction.Up);
					}
					else
					{
						priorDirections.Add(Direction.Left);
						priorDirections.Add(Direction.Right);
						priorDirections.Add(Direction.Up);
					}
				}
				else
				{
					priorDirections.Add(Direction.Up);
					if (dp.X > 0)
					{
						priorDirections.Add(Direction.Right);
						priorDirections.Add(Direction.Left);
						priorDirections.Add(Direction.Down);
					}
					else
					{
						priorDirections.Add(Direction.Left);
						priorDirections.Add(Direction.Right);
						priorDirections.Add(Direction.Down);
					}
				}
			}
			return priorDirections;
		}

		private void SetStrawberryNextPosition()
		{
			Position nextPosition;
			int count = 0;
			for (;;)
			{
				count++;
				nextPosition = GameRandom.NextPosition(FieldManager.Instance.X, FieldManager.Instance.Y);

				if (nextPosition.Equals(_snake.HeadPosition))
					continue;

				if (!_snake.Contains(nextPosition) || count >= 1000)
					break;
			}
			_strawberryPosition = nextPosition;
		}

		private void AnimateStrawberry()
		{
			iTween.Stop(Strawberry.gameObject);
			iTween.ScaleTo(Strawberry.gameObject, new Vector3(0, 0, 0), 0.5f);
			iTween.ScaleTo(Strawberry.gameObject,
						   iTween.Hash(iT.ScaleTo.scale, _strawberryDefaultScale,
			                           iT.ScaleTo.time, 0.5f,
			                           iT.ScaleTo.delay, 0.5f));

			Vector3 newPosition = GetStrawberryPosition();
			iTween.MoveTo(Strawberry.gameObject,
			              iTween.Hash(iT.MoveTo.position, newPosition,
			                          iT.MoveTo.time, 0.1f,
			                          iT.MoveTo.delay, 0.5f));
		}

		private Vector3 GetStrawberryPosition()
		{
			return new Vector3(
				(_strawberryPosition.X - FieldManager.Instance.X / 2) * FieldManager.Instance.CubeArea,
				(_strawberryPosition.Y - FieldManager.Instance.Y / 2) * FieldManager.Instance.CubeArea,
				Strawberry.position.z);
		}

		public override void OnRun()
		{
			_strawberryDefaultScale = Strawberry.localScale;
			_snake = Instantiate<Transform>(SnakePrefab).GetComponent<Snake>();
			_snake.Init();
			_nextMoveEvent = new TimeEvent(MoveDelay);			
			SetStrawberryNextPosition();
			Strawberry.position = GetStrawberryPosition();
			Strawberry.gameObject.SetActive(true);
		}

		public override void OnStop()
		{
			_snake.Destroy();
			Strawberry.gameObject.SetActive(false);
		}

		private void OnCubeRemoved(PositionEventArgs positionEventArgs)
		{
			if (IsActive)
			{
				if (positionEventArgs.Position.Equals(_strawberryPosition))
				{
					SetStrawberryNextPosition();
					AnimateStrawberry();
					GameEvents.StrawberryRemoved.Publish(GameEventArgs.Empty);
					AudioManager.Play(Sound.MoveAway);
				}

				if (_snake.Contains(positionEventArgs.Position))
					_snake.Cut(positionEventArgs.Position);
			}
		}

	}
}
