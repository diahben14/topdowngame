using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
[SelectionBase]
public class SpriteBuilder : MonoBehaviour
{
	[FormerlySerializedAs("text")]
	[SerializeField]
	protected float scale = 25f;

	[SerializeField]
	protected bool center;

	[SerializeField]
	protected Color color = Color.white;

	[Range(0f, 1f)]
	[SerializeField]
	protected float animate = 0.5f;

	[Range(0f, 0.5f)]
	[SerializeField]
	protected float normalizedAppearDuration = 0.1f;

	protected int _charByCharClip = int.MaxValue;

	protected const float Kerning = 0.446f;

	protected const float PunctationKerning = 0.3f;

	protected const float CartoucheKerning = 0.6f;

	protected const float CartouchePreKerning = 0.125f;

	protected const float LineHeight = 1f;

	protected static Material _glyphMaterial;

	protected string _parsableSymbolText;

	public List<Parser.ParserSymbol> _preparsedParserSymbols;

	protected string _localizedText;

	protected bool modeIsLocalizedText;

	protected bool localizedTextObjectIsSetUp;

	protected TMP_Text localizedTextObject;

	[HideInInspector]
	[SerializeField]
	protected List<GameObject> gameObjects = new List<GameObject>();

	[HideInInspector]
	[SerializeField]
	protected RectTransform scalerTransform;

	protected static Sprite[] spriteResources;

	protected static readonly string kPathToAlphabet = "Alphabet/Alphabet New";

	protected static readonly string kPathToLocalizedTextObject = "LocalizedTextObject";

	protected static readonly string kPathToLocalizedTextGUIObject = "LocalizedTextObject_UGUI";

	public int CharByCharClip
	{
		get
		{
			return _charByCharClip;
		}
		set
		{
			_charByCharClip = value;
			updateCharByCharClip();
		}
	}

	public float Scale
	{
		get
		{
			return scale;
		}
		set
		{
			scale = value;
		}
	}

	protected static Material glyphMaterial
	{
		get
		{
			if (_glyphMaterial == null)
			{
				_glyphMaterial = Resources.Load<Material>("Glyph Material");
			}
			return _glyphMaterial;
		}
	}

	public string ParsableSymbolText
	{
		get
		{
			return _parsableSymbolText;
		}
		set
		{
			_parsableSymbolText = value;
			modeIsLocalizedText = false;
			rebuild();
		}
	}

	public List<Parser.ParserSymbol> PreparsedParserSymbols
	{
		get
		{
			return _preparsedParserSymbols;
		}
		set
		{
			_preparsedParserSymbols = value;
			modeIsLocalizedText = false;
			rebuild();
		}
	}

	public string LocalizedText
	{
		set
		{
			if (!localizedTextObjectIsSetUp)
			{
				if (value == "")
				{
					return;
				}
				setupLocalizedTextObject();
			}
			modeIsLocalizedText = true;
			_localizedText = value;
			localizedTextObject.text = _localizedText;
			RectTransform component = GetComponent<RectTransform>();
			if (component != null)
			{
				component.sizeDelta = localizedTextObject.GetPreferredValues();
			}
		}
	}

	public int CharLength
	{
		get
		{
			if (!modeIsLocalizedText)
			{
				return gameObjects.Count;
			}
			return _localizedText.Length;
		}
	}

	protected bool isInUGUI
	{
		get
		{
			return false;
		}
	}

	protected void setupLocalizedTextObject()
	{
		GameObject gameObject = Object.Instantiate(isInUGUI ? (Resources.Load(kPathToLocalizedTextGUIObject) as GameObject) : (Resources.Load(kPathToLocalizedTextObject) as GameObject), base.transform);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		gameObject.layer = base.gameObject.layer;
		component.anchoredPosition = Vector3.zero;
		localizedTextObject = gameObject.GetComponent<TMP_Text>();
		localizedTextObjectIsSetUp = true;
	}

	protected void updateCharByCharClip()
	{
		if (modeIsLocalizedText)
		{
			localizedTextObject.maxVisibleCharacters = CharByCharClip;
			return;
		}
		for (int i = 0; i < gameObjects.Count; i++)
		{
			gameObjects[i].SetActive(CharByCharClip > i);
		}
	}

	protected void rebuild()
	{
		staticSetup();
		clean();
		if (ParsableSymbolText == "")
		{
			return;
		}
		GameObject gameObject = new GameObject("Scaler");
		scalerTransform = gameObject.AddComponent<RectTransform>();
		scalerTransform.SetParent(base.transform, false);
		RectTransform rectTransform = scalerTransform;
		Vector2 anchorMin = (scalerTransform.anchorMax = new Vector2(0f, 0.5f));
		rectTransform.anchorMin = anchorMin;
		scalerTransform.anchoredPosition = new Vector2(Scale / 4f, 0f);
		scalerTransform.transform.localRotation = Quaternion.identity;
		scalerTransform.transform.localScale = Vector3.one * scale;
		List<Parser.ParserSymbol> preparsedParserSymbols = PreparsedParserSymbols;
		if (preparsedParserSymbols.Count == 0)
		{
			return;
		}
		float num = 1f;
		float num2 = 0f;
		float num3 = 0f;
		GlyphType glyphType = GlyphType.UNKNOWN;
		int num4 = 0;
		Transform transform = null;
		Transform parent = null;
		bool flag = false;
		for (int i = 0; i <= preparsedParserSymbols.Count; i++)
		{
			if (i == 0 || i == preparsedParserSymbols.Count || preparsedParserSymbols[i].isNewline)
			{
				if (transform != null && center)
				{
					transform.localPosition += Vector3.right * (0f - (num - 0.446f)) / 2f;
				}
				if (num > num3)
				{
					num3 = num;
				}
				if (i < preparsedParserSymbols.Count)
				{
					num = 0f;
					transform = new GameObject("Line Parent").transform;
					transform.transform.parent = scalerTransform;
					transform.transform.localScale = Vector3.one;
					transform.transform.localPosition = Vector3.down * num2;
					num2 += 1f;
				}
				if (i != 0)
				{
					continue;
				}
			}
			Parser.ParserSymbol parserSymbol = preparsedParserSymbols[i];
			if ((Parser.getType(parserSymbol.spriteCode) == GlyphType.TypeA && glyphType == GlyphType.TypeB && !flag) || (Parser.getType(parserSymbol.spriteCode) == GlyphType.TypeB && glyphType == GlyphType.TypeA && !flag))
			{
				flag = true;
				if (Parser.isTypeB(parserSymbol.spriteCode))
				{
					GameObject obj = new GameObject("subglyph, special marker ");
					obj.transform.parent = parent;
					obj.transform.localScale = Vector3.one;
					obj.transform.localRotation = Quaternion.identity;
					obj.transform.localPosition = Vector3.zero;
					obj.layer = base.gameObject.layer;
					SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>();
					spriteRenderer.material = glyphMaterial;
					spriteRenderer.sprite = spriteResources[Parser.accentIndex];
					spriteRenderer.color = color * parserSymbol.colour;
				}
			}
			else
			{
				GameObject gameObject2 = new GameObject("glyph " + num4++);
				float num5 = (Parser.isTypeC(parserSymbol.spriteCode) ? 0.125f : 0f);
				num += num5;
				gameObject2.transform.parent = transform;
				gameObject2.transform.localScale = Vector3.one;
				gameObject2.transform.localRotation = Quaternion.identity;
				gameObject2.transform.localPosition = Vector3.right * num;
				gameObject2.layer = base.gameObject.layer;
				parent = gameObject2.transform;
				float num6 = (Parser.isTypeC(parserSymbol.spriteCode) ? 0.6f : (Parser.isTypeP(parserSymbol.spriteCode) ? 0.3f : 0.446f));
				num += num6;
				gameObjects.Add(gameObject2);
				flag = false;
			}
			GameObject obj2 = new GameObject("subglyph, code " + parserSymbol.spriteCode);
			obj2.transform.parent = parent;
			obj2.transform.localScale = Vector3.one;
			obj2.transform.localRotation = Quaternion.identity;
			obj2.transform.localPosition = Vector3.zero;
			obj2.layer = base.gameObject.layer;
			SpriteRenderer spriteRenderer2 = obj2.AddComponent<SpriteRenderer>();
			spriteRenderer2.material = glyphMaterial;
			spriteRenderer2.sprite = spriteResources[parserSymbol.spriteCode];
			spriteRenderer2.color = color * parserSymbol.colour;
			glyphType = Parser.getType(parserSymbol.spriteCode);
		}
		updateCharByCharClip();
		RectTransform component = GetComponent<RectTransform>();
		if (component != null)
		{
			component.sizeDelta = new Vector2(num3 * scale, num2 * scale);
		}
	}

	protected void clean()
	{
		if (scalerTransform != null)
		{
			Object.Destroy(scalerTransform.gameObject);
			gameObjects.Clear();
		}
	}

	protected static void staticSetup()
	{
		if (spriteResources == null)
		{
			spriteResources = new Sprite[Parser.symbolCount];
			Sprite[] array = Resources.LoadAll<Sprite>(kPathToAlphabet);
			int num = 0;
			for (int i = 0; i < spriteResources.Length; i++)
			{
				spriteResources[num++] = array[i];
			}
		}
	}
}
