
namespace Assets.Scripts.Events
{
	public class GameEventArgs
	{
		private static GameEventArgs _empty;
		public static GameEventArgs Empty
		{
			get
			{
				if (_empty == null)
					_empty = new GameEventArgs();
				return _empty;
			}
		}
	}

	public class GameEventArgs<T> : GameEventArgs
	{
		public T Object { get; private set; }

		public GameEventArgs(T obj)
		{
			Object = obj;
		}
	}
}
