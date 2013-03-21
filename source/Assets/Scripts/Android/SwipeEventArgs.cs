using System;
using UnityEngine;

namespace Assets.Scripts.Android
{
	public class SwipeEventArgs : EventArgs
	{
		public Vector2 BeginPosition { get; private set; }
		public Vector2 EndPosition { get; private set; }

		public SwipeEventArgs(Vector2 beginPosition, Vector2 endPosition)
		{
			BeginPosition = beginPosition;
			EndPosition = endPosition;
		}
	}
}
