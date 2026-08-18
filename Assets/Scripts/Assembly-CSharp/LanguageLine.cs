using System;
using System.Collections.Generic;
using UnityEngine;

public class LanguageLine : ScriptableObject
{
	[Serializable]
	public class ListWrapper
	{
		public List<MixedTextBuilder.MixedTextSegment> L = new List<MixedTextBuilder.MixedTextSegment>();
	}

	public static readonly int spritePixelSize = 256;

	public static readonly float cameraPadding = 0.125f;

	[SerializeField]
	public ListWrapper[] preparsed;
}
