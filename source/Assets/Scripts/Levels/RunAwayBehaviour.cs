using UnityEngine;

namespace Assets.Scripts.Levels
{
	public class RunAwayBehaviour : LevelBehaviour
	{
		private Vector3 _defaultPosition;

		public static RunAwayBehaviour Instance { get; private set; }

		public void Awake()
		{
			Instance = this;
		}

		public override void OnRun()
		{
			_defaultPosition = Camera.mainCamera.transform.position;
			iTween.MoveTo(Camera.mainCamera.gameObject,
			              iTween.Hash(iT.MoveTo.position,
			                          new Vector3(_defaultPosition.x, _defaultPosition.y, _defaultPosition.z - 50),
			                          iT.MoveTo.time, 25,
			                          iT.MoveTo.easetype, iTween.EaseType.easeInOutCubic,
			                          iT.MoveTo.looptype, iTween.LoopType.pingPong));
		}

		public override void OnStop()
		{
			Camera.mainCamera.transform.position = _defaultPosition;
		}
	}
}
