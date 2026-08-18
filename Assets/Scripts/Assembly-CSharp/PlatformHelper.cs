using InControl;
using UnityEngine;

public class PlatformHelper : MonoBehaviour
{
	[SerializeField]
	protected Sprite[] keyboardGlyphSprites;

	[SerializeField]
	protected Sprite[] LightKeyboardGlyphSprites;

	protected static PlatformHelper instance;

	private static bool setupDone;

	private static int[] keycodeToSprite;

	private static readonly int[] spriteToKeyCode = new int[138]
	{
		65, 78, 66, 104, 72, 0, 13, 76, 87, 88,
		89, 90, 91, 92, 93, 94, 95, 96, 103, 98,
		99, 100, 101, 102, 105, 85, 86, 84, 83, 77,
		79, 80, 81, 82, 14, 15, 16, 17, 18, 19,
		20, 21, 22, 23, 24, 25, 106, 107, 108, 26,
		27, 28, 29, 30, 31, 32, 33, 34, 35, 111,
		128, 114, 115, 116, 118, 71, 120, 121, 119, 123,
		73, 63, 74, 75, 127, 70, 129, 64, 130, 131,
		113, 67, 69, 68, 117, 122, 62, 36, 37, 38,
		39, 40, 41, 42, 43, 44, 45, 46, 47, 48,
		49, 50, 51, 52, 53, 54, 55, 56, 57, 58,
		59, 60, 61, 124, 126, 68, 112, 97, 110, 0,
		9, 5, 12, 8, 10, 6, 7, 0, 0, 11,
		0, 0, 109, 0, 0, 0, 0, 0
	};

	protected void Awake()
	{
		instance = this;
	}

	public static Sprite KeycodeToSprite(Key keycode, KeySpriteStyle style = KeySpriteStyle.Dark)
	{
		if (!setupDone)
		{
			doSetup();
		}
		if (style == KeySpriteStyle.Light)
		{
			return instance.LightKeyboardGlyphSprites[keycodeToSprite[(int)keycode]];
		}
		return instance.keyboardGlyphSprites[keycodeToSprite[(int)keycode]];
	}

	private static void doSetup()
	{
		int num = 0;
		for (int i = 0; i < spriteToKeyCode.Length; i++)
		{
			num = Mathf.Max(spriteToKeyCode[i], num);
		}
		keycodeToSprite = new int[num];
		for (int j = 0; j < num; j++)
		{
			keycodeToSprite[j] = 0;
			for (int k = 0; k < spriteToKeyCode.Length; k++)
			{
				if (spriteToKeyCode[k] == j)
				{
					keycodeToSprite[j] = k;
				}
			}
		}
		setupDone = true;
	}
}
