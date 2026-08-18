using InControl;
using UnityEngine;
using UnityEngine.UI;

public class PlatformSprite : MonoBehaviour
{
	protected static PlatformType _platform = PlatformType.XBOX1;

	[SerializeField]
	protected Sprite[] sprites;

	[SerializeField]
	protected bool IsPlayerCharacterAction;

	[SerializeField]
	protected string actionName;

	[SerializeField]
	protected KeySpriteStyle keySpriteStyle;

	[SerializeField]
	protected bool overrideKeyboardSprite;

	[SerializeField]
	protected Sprite keyboardSprite;

	[SerializeField]
	protected bool disableIfNullImage;

	protected Image cachedImage;

	protected SpriteRenderer cachedSpriteRenderer;

	private PlayerActionSet actionSet;

	private PlayerAction action;

	private bool firstUpdate = true;

	public static PlatformType Platform
	{
		get
		{
			return _platform;
		}
		set
		{
			_platform = value;
		}
	}

	protected void Start()
	{
		_platform = PlatformType.PC;
		cachedImage = GetComponent<Image>();
		cachedSpriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void SetSprite(Sprite sprite)
	{
		if (cachedImage != null)
		{
			cachedImage.sprite = sprite;
			if (disableIfNullImage)
			{
				cachedImage.enabled = sprite != null;
			}
		}
		else
		{
			cachedSpriteRenderer.sprite = sprite;
		}
	}

	protected void Update()
	{
		if (firstUpdate)
		{
			firstUpdate = false;
			if (IsPlayerCharacterAction)
			{
				actionSet = PlayerInput.Actions;
			}
			else
			{
				actionSet = GUIInput.Actions;
			}
			action = actionSet.GetPlayerActionByName(actionName);
		}
		if (Platform == PlatformType.PC)
		{
			if (actionSet.LastDeviceClass == InputDeviceClass.Controller)
			{
				SetSprite(sprites[1]);
			}
			else if (overrideKeyboardSprite)
			{
				SetSprite(keyboardSprite);
			}
			else if (action != null)
			{
				KeyBindingSource keyBindingSource = null;
				foreach (BindingSource binding in action.Bindings)
				{
					if (binding.BindingSourceType == BindingSourceType.KeyBindingSource)
					{
						keyBindingSource = binding as KeyBindingSource;
					}
				}
				if (keyBindingSource != null)
				{
					Key include = keyBindingSource.Control.GetInclude(0);
					SetSprite(PlatformHelper.KeycodeToSprite(include, keySpriteStyle));
				}
			}
			else
			{
				Debug.LogWarning("Null action on " + base.gameObject.name);
			}
		}
		else
		{
			SetSprite(sprites[(int)Platform]);
		}
	}
}
