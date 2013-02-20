using System.Collections.Generic;

namespace Assets.Scripts.Events
{
	public class MatchesEventArgs : GameEventArgs
	{
		public List<Match> Matches { get; private set; }

		public MatchesEventArgs(List<Match> match)
		{
			Matches = match;
		}
	}
}
