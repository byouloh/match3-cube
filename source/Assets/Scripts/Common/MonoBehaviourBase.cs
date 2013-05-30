using UnityEngine;

namespace Assets.Scripts.Common
{
	public abstract class MonoBehaviourBase : MonoBehaviour
	{
		private Transform _transform;
		public Transform Transform
		{
			get
			{
				if (_transform == null)
					_transform = transform;
				return _transform;
			}
		}

		private Camera _camera;
		public Camera Camera
		{
			get
			{
				if (_camera == null)
					_camera = camera;
				return _camera;
			}
		}

		protected T Instantiate<T>(Object obj) where T : Object
		{
			return (T)Instantiate(obj);
		}
	}

}
