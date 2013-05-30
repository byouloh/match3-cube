using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts
{
	public class BackgroundScript : MonoBehaviourBase
	{
		private float _cachedHeight;
		private float _cachedWidth;

		public void Update()
		{
			if (_cachedHeight != Screen.height || _cachedWidth != Screen.width)
			{
				_cachedHeight = Screen.height;
				_cachedWidth = Screen.width;
				Transform.localScale = new Vector3(_cachedWidth, _cachedHeight, 1);
			}
		}
	}
}
