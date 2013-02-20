using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Levels
{
	public class VirusesBehaviour : LevelBehaviour
	{
		public static VirusesBehaviour Instance;
		
		private List<VirusItem> _viruses;
		private float _lastVirusAddedTime;

		public Transform VirusPrefab;
		public float StepDelay;
		public int MaxVirusesCount;
		public float NextVirusDelay;
		
		public void Awake()
		{
			_viruses = new List<VirusItem>();
			Instance = this;
		}
	
		public void Update ()
		{
			if (IsActive)
			{
				foreach (VirusItem virusItem in _viruses)
				{
					if (virusItem.LastStepTime + StepDelay <= Time.time)
						virusItem.Move();
				}

				if (_lastVirusAddedTime + NextVirusDelay <= Time.time && _viruses.Count < MaxVirusesCount)
				{
					AddVirus();
				}
			}
		}

		public override void OnRun()
		{
			AddVirus();
		}

		private void AddVirus()
		{
			Transform virus = (Transform) Instantiate(VirusPrefab, VirusPrefab.position, VirusPrefab.rotation);
			VirusItem virusItem = virus.GetComponent<VirusItem>();
			virusItem.Init();
			_viruses.Add(virusItem);
			_lastVirusAddedTime = Time.time;
		}

		public override void OnStop()
		{
			foreach (VirusItem virusItem in _viruses)
				Destroy(virusItem.gameObject);
			_viruses.Clear();
		}

		public bool IsCubeInfected(Position position)
		{
			return _viruses.Any(v => v.Position.Equals(position));
		}
	}
}
