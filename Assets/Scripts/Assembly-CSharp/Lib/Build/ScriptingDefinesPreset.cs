using UnityEngine;

namespace Lib.Build
{
	[CreateAssetMenu]
	public class ScriptingDefinesPreset : ScriptableObject
	{
		[TextArea(5, 10)]
		public string notes;

		public string[] defines;
	}
}
