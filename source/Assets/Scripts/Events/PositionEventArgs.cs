using Assets.Scripts.Levels;

namespace Assets.Scripts.Events
{
	public class PositionEventArgs : GameEventArgs
	{
		public Position Position { get; private set; }

		public PositionEventArgs(Position position)
		{
			Position = position;
		}
	}
}
