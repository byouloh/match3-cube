

namespace Assets.Scripts
{
	public class GameLocker
	{
		public static bool IsLocked { get; set; }

		public static void Lock()
		{
			IsLocked = true;
		}

		public static void Unlock()
		{
			IsLocked = false;
		}
	}
}
