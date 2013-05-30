using Assets.Scripts.Levels;
using UnityEngine;

namespace Assets.Scripts
{
	public class CubeItem : MonoBehaviour
	{
		private static bool _isLocked;

		public Position Position { get; set; }
		public CubeType OriginalValue;

		public CubeType Value
		{
			get { return IsMole ? CubeType.Mole : OriginalValue; }
		}

		public bool IsQuestion { get; private set; }
		public bool IsQuestionBlocked { get; set; }
		public bool IsMole { get; private set; }

		public tk2dSprite Sprite { get { return GetComponent<tk2dSprite>(); } }

		public void OnMouseOver()
		{
			Sprite.color = Color.Lerp(Color.white, Color.red, 0.1f);
		}

		public void OnMouseExit()
		{
			Sprite.color = Color.white;
		}

		public void OnMouseDown()
		{
			if (!_isLocked)
			{
				AudioManager.Play(Sound.Click);
				FieldManager.Instance.OnCubeSelect(this);
			}
		}

		public static void Lock()
		{
			_isLocked = true;
		}

		public static void UnLock()
		{
			_isLocked = false;
		}

		public void SetPosition(int x, int y)
		{
			Position = new Position(x, y);
		}

		public void UpdatePosition()
		{
			float xOffset = -3.0f*FieldManager.Instance.CubeArea;
			float yOffset = -3.0f*FieldManager.Instance.CubeArea;
			transform.localPosition = new Vector2(Position.X*FieldManager.Instance.CubeArea + xOffset,
			                                      Position.Y*FieldManager.Instance.CubeArea + yOffset);
			transform.localRotation = new Quaternion();
		}

		public override string ToString()
		{
			return string.Format("Position:" + Position);
		}

		public void TransformToQuestion()
		{
			if (!IsQuestionBlocked)
			{
				IsQuestion = true;
				renderer.material = FieldManager.Instance.QuestionMaterial;
			}
		}

		public void TransformToMole()
		{
			IsMole = true;
			renderer.material = FieldManager.Instance.MoleMaterial;
		}

		public void TransformToDefault()
		{
			IsQuestion = false;
			IsMole = false;
			//renderer.material = Material;
		}

		public bool Contains(Vector2 position)
		{
			Vector3 wordPosition = Camera.main.ScreenToWorldPoint(position);
			return collider.bounds.Contains(wordPosition);
		}

		public static bool IsNeighbors(CubeItem cube1, CubeItem cube2)
		{
			return Mathf.Abs(cube1.Position.X - cube2.Position.X) + Mathf.Abs(cube1.Position.Y - cube2.Position.Y) == 1;
		}

		public void MarkSelected()
		{
			iTween.ScaleTo(Sprite.gameObject, new Vector3(1.1f, 1.1f, 1.0f), 0.5f);
			Sprite.transform.position = new Vector3(Sprite.transform.position.x, Sprite.transform.position.y, -1);
		}

		public void MarkUnSelected()
		{
			iTween.ScaleTo(Sprite.gameObject, new Vector3(1, 1, 1), 0.5f);
			Sprite.transform.position = new Vector3(Sprite.transform.position.x, Sprite.transform.position.y, 0);
		}
	}
}
