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
		private SnakePart _headLeft;
		private SnakePart _headRight;

		public Transform BodyPrefab;
		public Transform HeadLeftPrefab;
		public Transform HeadRightPrefab;

		public Position HeadPosition { get { return _parts[0].Position; } }

		public Snake()
		{
			_parts = new List<SnakePart>();
		}

		public void Init()
		{
			AddSnakeHeader(new Position(3, 3));
			AddSnakeBody(new Position(2, 3));
			SetHeadDirection(Direction.Right);
		}

		private void AddSnakeHeader(Position position)
		{
			_headLeft = Instantiate<Transform>(HeadLeftPrefab).GetComponent<SnakePart>();
			_headLeft.Transform.parent = Transform;
			_headLeft.SetPosition(position);
			_headLeft.gameObject.SetActive(false);			

			_headRight = Instantiate<Transform>(HeadRightPrefab).GetComponent<SnakePart>();
			_headRight.Transform.parent = Transform;
			_headRight.SetPosition(position);
			_parts.Add(_headRight);
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
			for (int i = 0; i < _parts.Count; i++)
			{
				SnakePart snakePart = _parts[i];
				Position temp = snakePart.Position.Clone();
				if (i == 0)
				{
					_headLeft.MoveTo(nextPartNewPosition);
					_headRight.MoveTo(nextPartNewPosition);
				}
				else
				{
					snakePart.MoveTo(nextPartNewPosition);
				}
				nextPartNewPosition = temp;
			}
			SetHeadDirection(direction);
			AudioManager.Play(Sound.SnakeMove);
		}

		public void Eat(Direction direction)
		{
			Position tailPosition = _parts[_parts.Count - 1].Position.Clone();
			Move(direction);
			AddSnakeBody(tailPosition);
			SetHeadDirection(direction);
			AudioManager.Play(Sound.SnakeEat);
		}

		public bool Contains(Position position)
		{
			return _parts.Any(sp => sp.Position.Equals(position));
		}

		public void Destroy()
		{
			_parts.RemoveAt(0);
			_parts.ForEach(p => Destroy(p.gameObject));
			Destroy(_headRight.gameObject);
			Destroy(_headLeft.gameObject);
			Destroy(gameObject);
		}

		private void SetHeadDirection(Direction direction)
		{
			SnakePart head;
			if (direction == Direction.Right)
			{
				head = _headLeft;
				_headRight.gameObject.SetActive(false);
			}
			else
			{
				head = _headRight;
				_headLeft.gameObject.SetActive(false);
			}
			head.gameObject.SetActive(true);
			head.Transform.rotation = new Quaternion(0, 0, 0, 0);
			_parts[0] = head;

			if (direction == Direction.Up)
				head.transform.RotateAroundLocal(Vector3.forward, 90);
			else if (direction == Direction.Down)
				head.transform.RotateAroundLocal(Vector3.forward, 180);
			else if (direction == Direction.Left)
				head.transform.RotateAroundLocal(Vector3.forward, 0);
			else if (direction == Direction.Right)
				head.transform.RotateAroundLocal(Vector3.forward, 0);
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
			AudioManager.Play(Sound.MoveAway);
		}
	}
}
