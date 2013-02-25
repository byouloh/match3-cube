using System;
using Assets.Scripts.Levels;

namespace Assets.Scripts.Common
{
	public class Direction
	{
		public static Direction Left { get; private set; }
		public static Direction Right { get; private set; }
		public static Direction Up { get; private set; }
		public static Direction Down { get; private set; }

		public int DX { get; private set; }
		public int DY { get; private set; }

		static Direction()
		{
			Left = new Direction(-1, 0);
			Right = new Direction(1, 0);
			Up = new Direction(0, -1);
			Down = new Direction(0, 1);
		}

		private Direction(int dx, int dy)
		{
			DX = dx;
			DY = dy;
		}

		public Position Apply(int x, int y)
		{
			return new Position(x + DX, y + DY);
		}

		public Position Apply(Position position)
		{
			return new Position(position.X + DX, position.Y + DY);
		}

		public static Direction Get(Position from, Position to)
		{
			Direction rawDirection = new Direction(to.X - from.X, to.Y - from.Y);
			if (rawDirection.Equals(Left))
				return Left;
			if (rawDirection.Equals(Right))
				return Right;
			if (rawDirection.Equals(Up))
				return Up;
			if (rawDirection.Equals(Down))
				return Down;

			throw new Exception(string.Format("Can't determine direction from position {0} to position {1}", from, to));
		}

		public bool Equals(Direction other)
		{
			return DX == other.DX && DY == other.DY;
		}

		public override string ToString()
		{
			if (this == Up)
				return "Up";
			else if (this == Down)
				return "Down";
			else if (this == Left)
				return "Left";
			else if (this == Right)
				return "Right";

			throw new NotSupportedException();
		}
	}
}
