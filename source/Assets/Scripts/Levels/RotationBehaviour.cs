using UnityEngine;

namespace Assets.Scripts.Levels
{
	public class RotationBehaviour : LevelBehaviour
	{
		public Vector3 Axis;
		public float Angel;
		public Transform Cubes;

		public static RotationBehaviour Instance { get; private set; }

		public void Awake()
		{
			Instance = this;
		}

		public void Update()
		{
			if (IsActive)
			{
				Cubes.RotateAroundLocal(Axis, Angel * Time.deltaTime);
			}
		}

		public override void OnRun()
		{
			Angel = 0.05f;
		}

		public override void OnStop()
		{
			iTween.RotateTo(Cubes.gameObject, Vector3.zero, 1);
		}

		public void Reset()
		{
			Stop();
		}
	}
}
