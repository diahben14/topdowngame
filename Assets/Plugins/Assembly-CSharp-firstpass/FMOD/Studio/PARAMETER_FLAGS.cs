using System;

namespace FMOD.Studio
{
	[Flags]
	public enum PARAMETER_FLAGS : uint
	{
		READONLY = 1u,
		AUTOMATIC = 2u,
		GLOBAL = 4u
	}
}
