using Assets.Scripts.Common;

namespace Assets.Scripts.Levels
{
	public abstract class LevelBehaviour : MonoBehaviourBase
	{
		public bool IsActive { get; private set; }

		public void Run()
		{
			IsActive = true;
			OnRun();
		}

		public void Stop()
		{
			IsActive = false;
			OnStop();
		}

		public abstract void OnRun();
		public abstract void OnStop();		
	}
}
