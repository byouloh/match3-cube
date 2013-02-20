using System;

namespace Assets.Scripts.Common
{
	public static class Extensions
	{
		public static string ToMillisecondsString(this DateTime dateTime)
		{
			return string.Format("{0}:{1}", dateTime.Second, dateTime.Millisecond);
		}
	}
}
