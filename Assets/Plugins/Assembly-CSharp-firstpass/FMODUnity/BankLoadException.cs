using System;
using FMOD;

namespace FMODUnity
{
	public class BankLoadException : Exception
	{
		public string Path;

		public RESULT Result;

		public BankLoadException(string path, RESULT result)
			: base(string.Format("[FMOD] Could not load bank '{0}' : {1} : {2}", path, result.ToString(), Error.String(result)))
		{
			Path = path;
			Result = result;
		}

		public BankLoadException(string path, string error)
			: base(string.Format("[FMOD] Could not load bank '{0}' : {1}", path, error))
		{
			Path = path;
			Result = RESULT.ERR_INTERNAL;
		}
	}
}
