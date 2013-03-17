using Assets.Scripts.Common;
using Assets.Scripts.Events;

namespace Assets.Scripts.Levels
{
	public class MolesBehaviour : LevelBehaviour
	{
		public static MolesBehaviour Instance { get; private set; }

		private CubeItem _mole;
		private TimeEvent _nextMoveEvent;

		public float MoveDelay;

		public void Awake()
		{
			Instance = this;
			GameEvents.TopLairCleared.Subscribe(OnTopLairCleared);
		}

		private void OnTopLairCleared(GameEventArgs gameEventArgs)
		{
			if (IsActive)
			{
				SelectMole();
			}
		}

		public void Update()
		{
			if (IsActive && _nextMoveEvent.PopIsOccurred())
			{
				Move();
			}
		}

		private void Move()
		{
			Position newPosition;
			do
			{
				Direction direction = GameRandom.NextDirection();
				newPosition = direction.Apply(_mole.X, _mole.Y);
			} while (!newPosition.IsInRange(0, FieldManager.Instance.X - 1, 0, FieldManager.Instance.Y - 1));

			CubeItem otherCube = FieldManager.Instance.GetCube(newPosition.X, newPosition.Y);
			StartCoroutine(FieldManager.Instance.Swap(_mole, otherCube));
			AudioManager.Play(Sound.SnakeMove);
		}

		public override void OnRun()
		{
			_nextMoveEvent = new TimeEvent(MoveDelay);
			SelectMole();
		}

		private void SelectMole()
		{
			Position position = GameRandom.NextPosition();
			_mole = FieldManager.Instance.GetCube(position.X, position.Y);
			_mole.TransformToMole();
		}

		public override void OnStop()
		{
			_mole.TransformToDefault();
		}
	}
}
