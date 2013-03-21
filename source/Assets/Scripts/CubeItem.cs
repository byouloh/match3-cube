using UnityEngine;

namespace Assets.Scripts
{
	public class CubeItem : MonoBehaviour
	{
		private static bool _isLocked;

		public int X;
		public int Y;
		public int Z;

		private Material _material;
		public Material Material
		{
			get { return _material; }
			set
			{
				_material = value;
				renderer.material = _material;
			}
		}

		public int Value
		{
			get { return IsMole ? "Mole".GetHashCode() : Material.name.GetHashCode(); }
		}

		public bool IsQuestion { get; private set; }
		public bool IsQuestionBlocked { get; set; }
		public bool IsMole { get; private set; }

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

		public void OnMouseOver()
		{
			renderer.material.color = Color.Lerp(Color.white, Color.red, 0.1f);
		}

		public void OnMouseExit()
		{
			renderer.material.color = Color.white;
		}

		public void SetPosition(int x, int y, int z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public static void SwapPosition(CubeItem cube1, CubeItem cube2)
		{
			int x = cube1.X;
			cube1.X = cube2.X;
			cube2.X = x;

			int y = cube1.Y;
			cube1.Y = cube2.Y;
			cube2.Y = y;

			int z = cube1.Z;
			cube1.Z = cube2.Z;
			cube2.Z = z;
		}

		public void UpdatePosition()
		{
			float xOffset = -3.0f*FieldManager.Instance.CubeArea;
			float yOffset = -3.0f*FieldManager.Instance.CubeArea;
			float zOffset = -3.0f*FieldManager.Instance.CubeArea;
			Vector3 position = new Vector3(X*FieldManager.Instance.CubeArea + xOffset,
			                               Y*FieldManager.Instance.CubeArea + yOffset,
			                               Z*FieldManager.Instance.CubeArea + zOffset);
			transform.localPosition = position;
			transform.localRotation = new Quaternion();
		}

		public override string ToString()
		{
			return string.Format("X:{0}, Y:{1}, Z:{2}", X, Y, Z);
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
			renderer.material = Material;
		}

		public bool Contains(Vector2 position)
		{
			Vector3 wordPosition = Camera.main.ScreenToWorldPoint(position);
			return collider.bounds.Contains(wordPosition);
		}
	}
}
