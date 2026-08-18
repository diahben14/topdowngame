using UnityEngine;

namespace Lib.Build
{
	[CreateAssetMenu]
	public class VersionPreset : ScriptableObject
	{
		[TextArea(5, 10)]
		public string notes;

		public string version;

		public string overrideAppIdentifier;
	}
}
