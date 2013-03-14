using Assets.Scripts.Common;
using Assets.Scripts.Levels;

namespace Assets.Scripts.Events
{
	public class  GameEvents : MonoBehaviourBase
	{
		private static bool _isInitialized;
		
		public static GameEvent<GameEventArgs> StateChanged { get; private set; }
		public static GameEvent<MatchesEventArgs> MatchesRemoved { get; private set; }
		public static GameEvent<GameEventArgs> TopLairCleared { get; private set; }
		public static GameEvent<PositionEventArgs> ColumnMovedUp { get; private set; }
		public static GameEvent<GameEventArgs<Level>> NextLevel { get; private set; }
		public static GameEvent<PositionEventArgs> CubeRemoved { get; private set; }
		public static GameEvent<GameEventArgs> VirusGone { get; private set; }
		public static GameEvent<PositionEventArgs> SnakeMoved { get; private set; }
		public static GameEvent<GameEventArgs> StrawberryRemoved { get; private set; }
		public static GameEvent<GameEventArgs> SnakePartRemoved { get; private set; }
		public static GameEvent<GameEventArgs> StartNewGame { get; private set; } 
		public static GameEvent<GameEventArgs> GameOver { get; private set; }
		public static GameEvent<GameEventArgs> ScoreAdded { get; private set; }
		public static GameEvent<GameEventArgs> GameWin { get; private set; } 

		static GameEvents()
		{
			Initalize();
		}

		private static void Initalize()
		{
			if (!_isInitialized)
			{
				StateChanged = new GameEvent<GameEventArgs>();
				MatchesRemoved = new GameEvent<MatchesEventArgs>();
				TopLairCleared = new GameEvent<GameEventArgs>();
				ColumnMovedUp = new GameEvent<PositionEventArgs>();
				NextLevel = new GameEvent<GameEventArgs<Level>>();
				CubeRemoved = new GameEvent<PositionEventArgs>();
				VirusGone = new GameEvent<GameEventArgs>();
				SnakeMoved = new GameEvent<PositionEventArgs>();
				StrawberryRemoved = new GameEvent<GameEventArgs>();
				SnakePartRemoved = new GameEvent<GameEventArgs>();
				StartNewGame = new GameEvent<GameEventArgs>();
				GameOver = new GameEvent<GameEventArgs>();
				ScoreAdded = new GameEvent<GameEventArgs>();
				GameWin = new GameEvent<GameEventArgs>();
				_isInitialized = true;
			}
		}
	}
}
