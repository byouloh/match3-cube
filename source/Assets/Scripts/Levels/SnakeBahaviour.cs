using System;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Levels
{
	public class SnakeBahaviour : LevelBehaviour
	{
		public static SnakeBahaviour Instance { get; private set; }

		private Snake _snake;
		private float _lastMoveTime;
		private Position _strawberryPosition;

		public Texture2D Body;
		public Texture2D HeadLeft;
		public Texture2D HeadRight;
		public Texture2D HeadUp;
		public Texture2D HeadDown;
		public Texture2D Strawberry;

		public int PartSize;
		public float MoveDelay;
		public float DX;
		public float DY;
		public int FieldWidth;
		public int FieldHeight;

		public void Awake()
		{
			Instance = this;
		}

		public void OnGUI()
		{
			if (IsActive)
			{
				GUIStyle style = new GUIStyle();
				style.normal.background = Body;
				for (int i = 1; i < _snake.Parts.Count; i++)
				{
					Position snakePart = _snake.Parts[i];
					ShowPart(snakePart, style);
				}

				style.normal.background = GetDirectedHead(_snake.Head, _snake.Parts[1]);
				ShowPart(_snake.Head, style);

				style.normal.background = Strawberry;
				ShowPart(_strawberryPosition, style);
			}
		}

		private void ShowPart(Position position, GUIStyle style)
		{
			GUI.Box(new Rect(position.X * PartSize + DX, position.Y * PartSize + DY, PartSize, PartSize),
							GUIContent.none, style);
		}

		private Texture2D GetDirectedHead(Position head, Position firstBody)
		{
			Direction direction = Direction.Get(firstBody, head);
			if (direction == Direction.Up)
				return HeadUp;
			else if (direction == Direction.Down)
				return HeadDown;
			else if (direction == Direction.Left)
				return HeadLeft;
			else if (direction == Direction.Right)
				return HeadRight;

			throw new Exception();
		}
	
		public void Update()
		{
			if (IsActive)
			{
				if (_lastMoveTime + MoveDelay <= Time.time)
				{
					_lastMoveTime = Time.time;
					MoveSnake();
				}
			}
		}

		private void MoveSnake()
		{
			Direction direction = GetToStrawberryDirection();
			if (direction.Apply(_snake.Head).Equals(_strawberryPosition))
			{
				_snake.Eat(direction);
				PutNextStrawberry();
			}
			else
				_snake.Move(direction);
		}

		private Direction GetToStrawberryDirection()
		{
			int dx = _strawberryPosition.X - _snake.Head.X;
			int dy = _strawberryPosition.Y - _snake.Head.Y;
			if (Math.Abs(dx) >= Math.Abs(dy))
			{
				return dx > 0 ? Direction.Right : Direction.Left;
			}
			else
			{
				return dy > 0 ? Direction.Down : Direction.Up;
			}
		}

		private void PutNextStrawberry()
		{
			Position nextPosition;
			int count = 0;
			for (;;)
			{
				count++;
				nextPosition = GameRandom.NextPosition(FieldWidth, FieldHeight);
				
				if (nextPosition == _snake.Head)
					continue;
		
				if (!_snake.Contains(nextPosition) || count >= 1000)
					break;
			}
			_strawberryPosition = nextPosition;
		}

		public override void OnRun()
		{
			_snake = new Snake();
			PutNextStrawberry();
		}

		public override void OnStop()
		{
		}
	}
}
