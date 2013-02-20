using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Levels
{
	public class VirusItem : MonoBehaviour
	{
		public Position Position;
		public float LastStepTime;

		public void SetPosition(Position position, bool isUseTween)
		{
			Position = position;
			Vector2 cubePosition = FieldManager.Instance.GetCubePosition(Position.X, Position.Y);
			Vector3 virusPosition = new Vector3(cubePosition.x, cubePosition.y, transform.position.z);
			if (isUseTween)
				iTween.MoveTo(gameObject, virusPosition, 2.0f);
			else
				transform.position = virusPosition;
			LastStepTime = Time.time;
		}

		public void Init()
		{
			Position position = GetRandomPosition();
			SetPosition(position, false);
		}

		private Position GetRandomPosition()
		{
			Position position;
			do
			{
				position = GameRandom.NextPosition();
			} while (VirusesBehaviour.Instance.IsCubeInfected(position));
			return position;
		}

		public void Move()
		{
			Direction direction = GameRandom.NextDirection();
			Position newPosition = direction.Apply(Position);
			if (newPosition.IsInRange(0, FieldManager.Instance.X - 1, 0, FieldManager.Instance.Y - 1))
			{
				SetPosition(newPosition, true);
			}
		}
	}
}
