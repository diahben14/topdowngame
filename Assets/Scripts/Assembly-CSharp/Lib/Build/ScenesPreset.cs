using UnityEngine;

namespace Lib.Build
{
	[CreateAssetMenu]
	public class ScenesPreset : ScriptableObject
	{
		[TextArea(5, 10)]
		public string notes;

		public SceneReference[] scenes;
	}
}
