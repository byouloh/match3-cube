
namespace Assets.Scripts.Levels
{
	public class EmptyBehaviour : LevelBehaviour
	{
		public static EmptyBehaviour Instance { protected set; get; }

		public void Awake()
		{
			Instance = this;
		}

		public override void OnRun(){}

		public override void OnStop(){}
	}
}
