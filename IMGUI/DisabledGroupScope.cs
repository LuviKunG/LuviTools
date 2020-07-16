using UnityEngine;

namespace LuviKunG.IMGUI
{
	public class DisabledGroupScope : GUI.Scope
	{
		private bool cached;

		public DisabledGroupScope(bool enabled)
		{
			cached = GUI.enabled;
			GUI.enabled = enabled;
		}

		protected override void CloseScope()
		{
			GUI.enabled = cached;
		}
	}
}