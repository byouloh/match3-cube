using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Events;
using UnityEngine;

namespace Assets.Scripts.Levels.SnakeLevel
{
	public class Snake : MonoBehaviourBase
	{
		private readonly List<SnakePart> _parts;

		public Transform BodyPrefab;
		public Transform HeadPrefab;

		public Position HeadPosition { get { return _parts[0].Position; } }

		public Snake()
		{
			_parts = new List<SnakePart>();
		}

		public void Init()
		{
			AddSnakeHeader(new Position(3, 3));
			AddSnakeBody(new Position(2, 3));
			SetHeadDirection(Direction.Left);
		}

		private void AddSnakeHeader(Position position)
		{
			SnakePart head = Instantiate<Transform>(HeadPrefab).GetComponent<SnakePart>();
			head.Transform.parent = Transform;
			head.SetPosition(position);
			_parts.Add(head);
		}

		private void AddSnakeBody(Position position)
		{
			SnakePart body = Instantiate<Transform>(BodyPrefab).GetComponent<SnakePart>();
			body.Transform.parent = Transform;
			body.SetPosition(position);
			_parts.Add(body);
		}

		public void Move(Direction direction)
		{
			Position nextPartNewPosition = direction.Apply(_parts[0].Position);
			foreach (SnakePart snakePart in _parts)
			{
				Position temp = snakePart.Position.Clone();
				snakePart.MoveTo(nextPartNewPosition);
				nextPartNewPosition = temp;
			}
			SetHeadDirection(direction);
		}

		public void Eat(Direction direction)
		{
			Position tailPosition = _parts[_parts.Count - 1].Position.Clone();
			Move(direction);
			AddSnakeBody(tailPosition);
			SetHeadDirection(direction);
		}

		public bool Contains(Position position)
		{
			return _parts.Any(sp => sp.Position.Equals(position));
		}

		public void Destroy()
		{
			_parts.ForEach(p => Destroy(p.gameObject));
			Destroy(gameObject);
		}

		private void SetHeadDirection(Direction direction)
		{
			SnakePart head = _parts[0];
			head.Transform.rotation = new Quaternion(0, 0, 0, 0);
			if (direction == Direction.Up)
				head.transform.RotateAroundLocal(Vector3.forward, 90);//ok
			else if (direction == Direction.Down)
				head.transform.RotateAroundLocal(Vector3.forward, 180);
			else if (direction == Direction.Left)
				head.transform.RotateAroundLocal(Vector3.forward, 0);//ok
			else if (direction == Direction.Right)
				head.transform.RotateAroundLocal(Vector3.forward, 180);
		}

		public void Cut(Position position)
		{
			int ind = _parts.FindIndex(sp => sp.Position.Equals(position));
			
			if (ind == 0)
				ind = 1;//can't remove head

			while (_parts.Count - 1 >= ind)
			{
				iTween.ScaleTo(_parts[ind].gameObject, new Vector3(0, 0, 0), 1.5f);
				Destroy(_parts[ind].gameObject, 1.5f);
				_parts.Remove(_parts[ind]);
				GameEvents.SnakePartRemoved.Publish(GameEventArgs.Empty);
			}
		}
	}
}
