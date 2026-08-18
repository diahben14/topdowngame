using System;
using UnityEngine;

public static class Parser
{
	[Serializable]
	public class ParserSymbol
	{
		public int spriteCode;

		public Color colour;

		public bool isNewline;

		public ParserSymbol()
		{
		}

		public ParserSymbol(int code, Color colour)
		{
			spriteCode = code;
			this.colour = colour;
		}
	}

	public static int symbolCount
	{
		get
		{
			return 105;
		}
	}

	public static int accentIndex
	{
		get
		{
			return 90;
		}
	}

	public static bool isTypeA(int i)
	{
		return getType(i) == GlyphType.TypeA;
	}

	public static bool isTypeB(int i)
	{
		return getType(i) == GlyphType.TypeB;
	}

	public static bool isTypeP(int i)
	{
		return getType(i) == GlyphType.TypeP;
	}

	public static bool isTypeN(int i)
	{
		return getType(i) == GlyphType.TypeN;
	}

	public static bool isTypeR(int i)
	{
		return getType(i) == GlyphType.TypeR;
	}

	public static bool isTypeC(int i)
	{
		return getType(i) == GlyphType.TypeC;
	}

	public static GlyphType getType(int i)
	{
		i -= 18;
		if (i < 0)
		{
			return GlyphType.TypeA;
		}
		i -= 24;
		if (i < 0)
		{
			return GlyphType.TypeB;
		}
		i -= 12;
		if (i < 0)
		{
			return GlyphType.TypeP;
		}
		i -= 10;
		if (i < 0)
		{
			return GlyphType.TypeN;
		}
		i -= 26;
		if (i < 0)
		{
			return GlyphType.TypeR;
		}
		i--;
		if (i < 0)
		{
			Debug.LogError("getType on placeholder for accent");
			return GlyphType.UNKNOWN;
		}
		i -= 14;
		if (i < 0)
		{
			return GlyphType.TypeC;
		}
		Debug.LogError("Unknown code " + i);
		return GlyphType.UNKNOWN;
	}
}
