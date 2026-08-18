using UnityEngine;

namespace Lib.Build
{
	[CreateAssetMenu]
	public class BuildInfo : ScriptableObject
	{
		public string commit;

		public string version;

		public int buildCounter;

		public int buildTimestamp;

		public string[] buildDefines;
	}
}
