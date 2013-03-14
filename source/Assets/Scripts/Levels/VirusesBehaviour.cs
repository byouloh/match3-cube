using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Events;
using UnityEngine;

namespace Assets.Scripts.Levels
{
	public class VirusesBehaviour : LevelBehaviour
	{
		public static VirusesBehaviour Instance;
		
		private List<VirusItem> _viruses;
		private TimeEvent _newVirusEvent;

		public Transform VirusPrefab;
		public float StepDelay;
		public int MaxVirusesCount;
		public float NextVirusDelay;
		
		public void Awake()
		{
			_viruses = new List<VirusItem>();
			Instance = this;
			GameEvents.CubeRemoved.Subscribe(OnCubeRemoved);
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

				if (_newVirusEvent.PopIsOccurred() && _viruses.Count < MaxVirusesCount)
				{
					AddVirus();
				}
			}
		}

		public override void OnRun()
		{
			_newVirusEvent = new TimeEvent(NextVirusDelay);
			AddVirus();
		}

		private void AddVirus()
		{
			Transform virus = (Transform) Instantiate(VirusPrefab, VirusPrefab.position, VirusPrefab.rotation);
			VirusItem virusItem = virus.GetComponent<VirusItem>();
			virusItem.Init();
			_viruses.Add(virusItem);
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

		private void OnCubeRemoved(PositionEventArgs positionEventArgs)
		{
			if (IsActive)
			{
				List<VirusItem> removedViruses = _viruses.Where(v => v.Position.Equals(positionEventArgs.Position)).ToList();
				foreach (VirusItem virus in removedViruses)
				{
					_viruses.Remove(virus);
					Destroy(virus.gameObject);
					AudioManager.Play(Sound.VirusGone);
					GameEvents.VirusGone.Publish(GameEventArgs.Empty);
				}
			}
		}

	}
}
