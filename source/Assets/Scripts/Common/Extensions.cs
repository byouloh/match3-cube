using System;
using UnityEngine;

namespace Assets.Scripts.Common
{
	public static class Extensions
	{
		public static string ToMillisecondsString(this DateTime dateTime)
		{
			return string.Format("{0}:{1}", dateTime.Second, dateTime.Millisecond);
		}

		#region Position

		public static void SetPosition(this Transform transform, float x, float y, float z)
		{
			transform.position = new Vector3(x, y, z);
		}

		public static void SetPosition(this Transform transform, float x, float y)
		{
			transform.position = new Vector3(x, y, transform.position.z);
		}

		public static void SetPosition(this Transform transform, Vector2 point, float z)
		{
			transform.position = new Vector3(point.x, point.y, z);
		}

		public static Vector2 Rotate(this Vector2 vector, float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.forward) * vector;
		}

		public static Vector2 Position2(this Transform transform)
		{
			return transform.position;
		}

		public static void SetZ(this Transform transform, float z)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, z);
		}

		#endregion

	}
}
