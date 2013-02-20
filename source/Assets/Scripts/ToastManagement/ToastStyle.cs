using UnityEngine;

namespace Assets.Scripts.ToastManagement
{
	public class ToastStyle
	{
		public Effect Effect { get; set; }
		public float Duration { get; set; }
		public int BubbleSpeed { get; set; }
		public GUIStyle GUIStyle { get; set; }

		public ToastStyle()
		{
			Effect = Effect.Bubble;
			BubbleSpeed = 30;
			Duration = 3.0f;
		}

		public static ToastStyle CreateDefault()
		{
			return new ToastStyle();
		}
	}
}
