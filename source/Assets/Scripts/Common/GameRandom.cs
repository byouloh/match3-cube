using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts.Common
{
	public class GameRandom
	{
		public static bool NextBool()
		{
			return Random.Range(0, 2) == 1;
		}

		public static Position NextPosition()
		{
			return new Position(Random.Range(0, FieldManager.Instance.X),
			                    Random.Range(0, FieldManager.Instance.Y));
		}

		public static Position NextPosition(int maxX, int maxY)
		{
			return new Position(Random.Range(0, maxX), Random.Range(0, maxY));
		}

		public static Direction NextDirection()
		{
			int dir = Random.Range(0, 4);
			switch (dir)
			{
				case 0:
					return Direction.Down;
				case 1:
					return Direction.Left;
				case 2:
					return Direction.Right;
				case 3:
				default:
					return Direction.Up;
			}
		}

		public static void MixUpArray<T>(T[] array)
		{
			for (int i = 0; i < array.Length*5; i++)
			{
				int ind1 = Random.Range(0, array.Length);
				int ind2 = Random.Range(0, array.Length);
				T temp = array[ind1];
				array[ind1] = array[ind2];
				array[ind2] = temp;
			}
		}

	}
}
