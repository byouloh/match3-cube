using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;

namespace Assets.Scripts.Levels
{
	public class Snake
	{
		public List<Position> Parts { get; private set; }

		public Position Head { get { return Parts[0]; } }

		public Snake()
		{
			Parts = new List<Position>();
			Parts.Add(new Position(3, 3));
			Parts.Add(new Position(2, 3));
		}

		public void Move(Direction direction)
		{
			Parts.Insert(0, direction.Apply(Parts[0]));
			Parts.RemoveAt(Parts.Count - 1);
		}

		public void Eat(Direction direction)
		{
			Parts.Insert(0, direction.Apply(Parts[0]));
		}

		public bool Contains(Position position)
		{
			return Parts.Any(position.Equals);
		}
	}
}
