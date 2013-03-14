
namespace Assets.Scripts.ToastManagement
{
	public class ToastStyle
	{
		public const string EMPTY_CATEGORY = "EmptyCategory";

		public Effect Effect { get; set; }
		public float Duration { get; set; }
		public int BubbleSpeed { get; set; }
		public string Category { get; set; }
		public bool IsRandomColor { get; set; }

		public ToastStyle()
		{
			Effect = Effect.Bubble;
			BubbleSpeed = 30;
			Duration = 3.0f;
			Category = EMPTY_CATEGORY;
		}

		public static ToastStyle CreateDefault()
		{
			return new ToastStyle();
		}
	}
}
