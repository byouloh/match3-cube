
namespace Assets.Scripts.Events
{
	public class GameEvents
	{
		private static bool _isInitialized;
		
		public static GameEvent<GameEventArgs> StateChanged { get; private set; }
		public static GameEvent<MatchesEventArgs> MatchesRemoved { get; private set; }
		public static GameEvent<GameEventArgs> TopLairCleared { get; private set; } 

		public static void Initalize()
		{
			if (!_isInitialized)
			{
				StateChanged = new GameEvent<GameEventArgs>();
				MatchesRemoved = new GameEvent<MatchesEventArgs>();
				TopLairCleared = new GameEvent<GameEventArgs>();
				_isInitialized = true;
			}
		}
	}
}
