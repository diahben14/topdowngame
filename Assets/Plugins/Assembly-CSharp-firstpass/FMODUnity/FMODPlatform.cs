using System;

namespace FMODUnity
{
	[Serializable]
	public enum FMODPlatform
	{
		None = 0,
		PlayInEditor = 1,
		Default = 2,
		Desktop = 3,
		Mobile = 4,
		MobileHigh = 5,
		MobileLow = 6,
		Console = 7,
		Windows = 8,
		Mac = 9,
		Linux = 10,
		iOS = 11,
		Android = 12,
		Deprecated_1 = 13,
		XboxOne = 14,
		PS4 = 15,
		Deprecated_2 = 16,
		Deprecated_3 = 17,
		AppleTV = 18,
		UWP = 19,
		Switch = 20,
		WebGL = 21,
		Stadia = 22,
		Reserved_1 = 23,
		Reserved_2 = 24,
		Count = 25
	}
}
