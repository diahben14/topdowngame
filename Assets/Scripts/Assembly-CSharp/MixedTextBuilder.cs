using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MixedTextBuilder : MonoBehaviour
{
	[Serializable]
	public class MixedTextSegment
	{
		public string text;

		public bool isLocalizable;

		public List<Parser.ParserSymbol> parserSymbols;
	}

	[SerializeField]
	protected LanguageLine inputLanguageLine;

	[SerializeField]
	protected int charByCharClip = -1;

	[SerializeField]
	protected float normalizedClip = -1f;

	protected const float scaleFactor = 25f;

	[HideInInspector]
	[SerializeField]
	protected HorizontalLayoutGroup[] hlgs;

	protected List<SpriteBuilder> spriteBuilders = new List<SpriteBuilder>();

	protected int lastCharByCharValue = int.MaxValue;

	public LanguageLine InputLanguageLine
	{
		get
		{
			return inputLanguageLine;
		}
		set
		{
			inputLanguageLine = value;
			buildFromScriptableObject();
		}
	}

	public int CharByCharClip
	{
		get
		{
			return charByCharClip;
		}
		set
		{
			charByCharClip = value;
			updateCharByCharClip();
		}
	}

	public int CharLength { get; protected set; }

	public float Width
	{
		get
		{
			return 1f;
		}
	}

	public float Height
	{
		get
		{
			return 1f;
		}
	}

	private void Awake()
	{
		buildFromScriptableObject();
	}

	private void buildFromScriptableObject()
	{
		if (inputLanguageLine != null)
		{
			buildFromPreparsed(inputLanguageLine.preparsed);
		}
	}

	private void clean()
	{
		spriteBuilders.Clear();
		if (hlgs == null)
		{
			return;
		}
		for (int i = 0; i < hlgs.Length; i++)
		{
			HorizontalLayoutGroup horizontalLayoutGroup = hlgs[i];
			if (horizontalLayoutGroup != null)
			{
				UnityEngine.Object.Destroy(horizontalLayoutGroup.gameObject);
			}
		}
		hlgs = null;
	}

	protected void buildFromPreparsed(LanguageLine.ListWrapper[] lines)
	{
		clean();
		if (GetComponent<VerticalLayoutGroup>() == null)
		{
			VerticalLayoutGroup verticalLayoutGroup = base.gameObject.AddComponent<VerticalLayoutGroup>();
			bool flag2 = (verticalLayoutGroup.childForceExpandWidth = false);
			bool flag4 = (verticalLayoutGroup.childForceExpandHeight = flag2);
			bool childControlHeight = (verticalLayoutGroup.childControlWidth = flag4);
			verticalLayoutGroup.childControlHeight = childControlHeight;
		}
		hlgs = new HorizontalLayoutGroup[lines.Length];
		for (int i = 0; i < hlgs.Length; i++)
		{
			GameObject gameObject = new GameObject("line " + i);
			gameObject.hideFlags = HideFlags.HideInHierarchy;
			hlgs[i] = gameObject.AddComponent<HorizontalLayoutGroup>();
			hlgs[i].transform.SetParent(base.transform, false);
			HorizontalLayoutGroup obj = hlgs[i];
			HorizontalLayoutGroup obj2 = hlgs[i];
			HorizontalLayoutGroup obj3 = hlgs[i];
			bool flag2 = (hlgs[i].childForceExpandWidth = false);
			bool flag4 = (obj3.childForceExpandHeight = flag2);
			bool childControlHeight = (obj2.childControlWidth = flag4);
			obj.childControlHeight = childControlHeight;
		}
		int num = 0;
		for (int j = 0; j < lines.Length; j++)
		{
			float num2 = 0f;
			LanguageLine.ListWrapper listWrapper = lines[j];
			for (int k = 0; k < listWrapper.L.Count; k++)
			{
				GameObject obj4 = new GameObject("segment " + k);
				obj4.layer = base.gameObject.layer;
				obj4.AddComponent<RectTransform>().SetParent(hlgs[j].transform, false);
				SpriteBuilder spriteBuilder = obj4.AddComponent<SpriteBuilder>();
				spriteBuilders.Add(spriteBuilder);
				spriteBuilder.Scale = 25f;
				if (listWrapper.L[k].isLocalizable)
				{
					spriteBuilder.LocalizedText = listWrapper.L[k].text;
				}
				else
				{
					spriteBuilder.PreparsedParserSymbols = listWrapper.L[k].parserSymbols;
				}
				num2 += spriteBuilder.GetComponent<RectTransform>().sizeDelta.x;
				num += spriteBuilder.CharLength;
			}
			hlgs[j].GetComponent<RectTransform>().sizeDelta = new Vector2(num2, 25f);
		}
		CharLength = num;
	}

	protected void OnEnable()
	{
		updateCharByCharClip();
	}

	protected void Update()
	{
		updateCharByCharClip();
	}

	protected void updateCharByCharClip()
	{
		if (normalizedClip != -1f)
		{
			charByCharClip = Mathf.RoundToInt(normalizedClip * (float)CharLength);
		}
		if (CharByCharClip == -1)
		{
			for (int i = 0; i < spriteBuilders.Count; i++)
			{
				spriteBuilders[i].CharByCharClip = int.MaxValue;
			}
			return;
		}
		int num = CharByCharClip;
		for (int j = 0; j < spriteBuilders.Count; j++)
		{
			spriteBuilders[j].CharByCharClip = num;
			num -= spriteBuilders[j].CharLength;
		}
	}
}
