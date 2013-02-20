using System;

namespace Assets.Scripts.ToastManagement
{
	[Flags]
	public enum Effect
	{
		Empty = 0,
		Bubble = 1,
		Transparency = 2,
		Oncoming = 4,
	}
}
