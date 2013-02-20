
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
	}
}
