using System;

[Serializable]
public class ParserException : Exception
{
	public int lineIndex { get; set; }

	public int charIndex { get; set; }

	public ParserException(int errorOnLine, int charInLine)
	{
		lineIndex = lineIndex;
		charIndex = charInLine;
	}

	public override string ToString()
	{
		return string.Format("Parser error on line {0}, character {1}", lineIndex, charIndex);
	}
}
