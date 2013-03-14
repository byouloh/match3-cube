using Assets.Scripts.Events;

namespace Assets.Scripts.Levels
{
	public class QuestionsBehaviour : LevelBehaviour
	{
		public static QuestionsBehaviour Instance { get; private set; }

		public void Awake()
		{
			Instance = this;
			GameEvents.ColumnMovedUp.Subscribe(OnColumnMovedUp);
		}

		public override void OnRun()
		{
		}

		public override void OnStop()
		{
			foreach (CubeItem cube in FieldManager.Instance.GetTopLairCubes())
				cube.TransformToDefault();
		}

		private void OnColumnMovedUp(PositionEventArgs positionEventArgs)
		{
			if (IsActive)
			{
				FieldManager.Instance
				            .GetCube(positionEventArgs.Position.X, positionEventArgs.Position.Y)
				            .TransformToQuestion();
			}
		}
	}
}
