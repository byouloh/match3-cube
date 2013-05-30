using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Tweens
{
	public class GravityTween : MonoBehaviourBase
	{
		private Vector3 _startPosition;
		private float _x;
		private float _y;

		private float _rotationSpeed;
		private float _xSpeed;
		private float _ySpeed;
		private float _yAcceleration;
		private float _remainTime;
		private tk2dSprite _sprite;
		private Color _spriteColor;
	
		public void Start()
		{
			_ySpeed = 100;
			_yAcceleration = -450;
			_startPosition = Transform.position;
			_x = _startPosition.x;
			_y = _startPosition.y;
			_sprite = GetComponent<tk2dSprite>();
			_spriteColor = _sprite.color;
		}

		public void Update()
		{
			if (_remainTime > 0)
			{
				Move();
				Rotate();
				F();
				_remainTime -= Time.deltaTime;
			}
			else
			{
				Destroy(this);
			}
		}

		private void Move()
		{
			_x += Time.deltaTime*_xSpeed;
			_ySpeed = _ySpeed + Time.deltaTime * _yAcceleration;
			_y += Time.deltaTime*_ySpeed;
			Transform.SetPosition(_x, _y);
		}

		private void Rotate()
		{
			Transform.Rotate(Vector3.forward, Time.deltaTime * _rotationSpeed);
		}

		private void F()
		{
			_spriteColor.a -= Time.deltaTime*0.75f;
			_sprite.color = _spriteColor;
		}

		public static void Run(GameObject gameObject, float time)
		{
			GravityTween gravityTween = gameObject.AddComponent<GravityTween>();
			gravityTween._remainTime = time;
			float xSpeed = Random.Range(-100, 100);
			gravityTween._xSpeed = xSpeed;
			gravityTween._rotationSpeed = xSpeed*2;
		}
	}
}
