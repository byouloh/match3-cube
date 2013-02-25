using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Levels.SnakeLevel
{
	public class SnakePart : MonoBehaviourBase
	{
		public Position Position { get; private set; }

		public SnakePart()
		{
			Position = new Position(0, 0);
		}

		public void SetPosition(Position position)
		{
			Position.Set(position);
			Transform.position = GetPosition();
		}

		private Vector3 GetPosition()
		{
			return new Vector3(
				(Position.X - FieldManager.Instance.X/2)*FieldManager.Instance.CubeArea,
				(Position.Y - FieldManager.Instance.Y/2)*FieldManager.Instance.CubeArea,
				Transform.position.z);
		}

		public void MoveTo(Position position)
		{
			Position.Set(position);
			Transform.position = GetPosition();
		}
	}
}
