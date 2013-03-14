using System;
using System.Collections.Generic;

namespace Assets.Scripts.Common
{
	public static class Extensions
	{
		public static string ToMillisecondsString(this DateTime dateTime)
		{
			return string.Format("{0}:{1}", dateTime.Second, dateTime.Millisecond);
		}

		public static void Shuffle<T>(this List<T> list)
		{
			T[] array = list.ToArray();
			GameRandom.ShuffleArray(array);
			list.Clear();
			list.AddRange(array);
		}
	}
}
