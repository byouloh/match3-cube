using System.Collections.Generic;

namespace Assets.Scripts
{
	public class Match
	{
		public List<CubeItem> Cubes { get; private set; }

		public Match()
		{
			Cubes = new List<CubeItem>();
		}

		public void Add(CubeItem cubeItem)
		{
			Cubes.Add(cubeItem);
		}

		public override string ToString()
		{
			string str = string.Empty;
			foreach (CubeItem cubeItem in Cubes)
				str += "Position:" + cubeItem.Position;
			return str;
		}
	}
}
