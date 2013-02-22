using System;
using Assets.Scripts.Levels;
using Random = UnityEngine.Random;

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

		public static void ShuffleArray<T>(T[] array)
		{
			T[] source = new T[array.Length];
			Array.Copy(array, source, array.Length);
			for (int i = 1; i < array.Length; i++)
			{
				int indRnd = Random.Range(0, i + 1);
				array[i] = array[indRnd];
				array[indRnd] = source[i];
			}
		}

	}
}
