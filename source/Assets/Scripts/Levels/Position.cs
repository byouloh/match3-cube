
namespace Assets.Scripts.Levels
{
	public class Position
	{
		public int X { get; private set; }
		public int Y { get; private set; }

		public Position(int x, int y)
		{
			X = x;
			Y = y;
		}

		public bool Equals(Position other)
		{
			return X == other.X && Y == other.Y;
		}

		public override string ToString()
		{
			return string.Format("[X:{0}, Y:{1}]", X, Y);
		}

		public bool IsInRange(int minX, int maxX, int minY, int maxY)
		{
			return X >= minX && X <= maxX && Y >= minY && Y < maxY;
		}

		public Position Clone()
		{
			return new Position(X, Y);
		}

		public void Set(Position position)
		{
			X = position.X;
			Y = position.Y;
		}

		public static void Swap(Position position1, Position position2)
		{
			int temp = position1.X;
			position1.X = position2.X;
			position2.X = temp;

			temp = position1.Y;
			position1.Y = position2.Y;
			position2.Y = temp;
		}

		public static Position operator -(Position position1, Position position2)
		{
			return new Position(position1.X - position2.X, position1.Y - position2.Y);
		}
	}
}
