

namespace Assets.Scripts.Levels
{
	public class QuestionsBehaviour : LevelBehaviour
	{
		public static QuestionsBehaviour Instance { get; private set; }

		public void Awake()
		{
			Instance = this;
		}

		public override void OnRun()
		{
		}

		public override void OnStop()
		{
			foreach (CubeItem cube in FieldManager.Instance.GetTopLairCubes())
				cube.TransformToDefault();
		}

		public override void OnMoveUpColumn(int x, int y)
		{
			FieldManager.Instance.GetCube(x, y).TransformToQuestion();
		}
	}
}
