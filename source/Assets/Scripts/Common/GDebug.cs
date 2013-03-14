using System;
using UnityEngine;

namespace Assets.Scripts.Common
{
	public class GDebug
	{
		private static int _counter;
		private static readonly System.Object _lockObject = new System.Object();

		public static void Log(object message, bool isShowTime = false)
		{
			lock (_lockObject)
			{
				_counter++;
				string time = isShowTime ? string.Format("({0})", DateTime.Now.ToMillisecondsString()) : string.Empty;
				Debug.Log(string.Format("{0}{1}:{2}", _counter, time, message));
			}
		}

		public static void Log(bool condition, object message)
		{
			if (condition)
				Log(message);
		}

	}
}
