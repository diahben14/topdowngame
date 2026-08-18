using System.Collections;
using FMODUnity;
using InControl;
using UnityEngine;

public class PageDisplay : GUIMode
{
	[SerializeField]
	protected Material templateMaterial;

	[SerializeField]
	protected Renderer[] pageRenderers;

	[SerializeField]
	protected int totalLeafCount = 4;

	[SerializeField]
	protected Transform pageSpineRoot;

	[SerializeField]
	protected float leftRightOffset = 1.4f;

	[SerializeField]
	protected float leftRightSmoothTime = 0.5f;

	[SerializeField]
	protected Transform cameraPivotTransform;

	[SerializeField]
	protected float appearDuration = 0.25f;

	[SerializeField]
	protected float dismissDuration = 0.16f;

	[SerializeField]
	protected float pageTurnDuration = 0.5f;

	[SerializeField]
	protected float maxViewTweakDegrees = 15f;

	[SerializeField]
	protected float viewTweakSmoothTime = 2f;

	[SerializeField]
	protected Texture transparentTexture;

	[SerializeField]
	protected Animator pageturnAnimator;

	[SerializeField]
	[EventRef]
	protected string sfx_open;

	[SerializeField]
	[EventRef]
	protected string sfx_close;

	[SerializeField]
	[EventRef]
	protected string sfx_flip;

	protected static PageDisplay instance;

	protected static bool showSpecificPageOnOpen;

	protected static int specificPageToShowOnOpen;

	protected Animator cachedAnimator;

	protected int shownPage;

	protected bool focusedOnRight;

	protected Vector3 viewTweakVelocity;

	protected Vector3 smoothedEuler;

	protected Vector3 leftRightVelocity;

	protected Material[] pageMaterials;

	protected const string kPathToPages = "Pages/leaf_";

	protected bool turningPage;

	protected bool showingLastPage;

	protected bool showingFirstPage;

	protected bool leftGoesToFirstPage;

	protected bool rightGoesToLastPage;

	protected int bookPositionOffset;

	public static bool Open { get; protected set; }

	protected override bool pauseTime
	{
		get
		{
			return true;
		}
	}

	public static void ShowPage(int i)
	{
		showSpecificPageOnOpen = true;
		specificPageToShowOnOpen = i;
		GUIMode.PushMode(instance);
	}

	public static void Show()
	{
		showSpecificPageOnOpen = false;
		GUIMode.PushMode(instance);
	}

	public static void AnyPageUnlocked()
	{
	}

	protected override void guiModeSetup()
	{
		instance = this;
		cachedAnimator = GetComponent<Animator>();
		int num = 6;
		pageMaterials = new Material[num];
		for (int i = 0; i < num; i++)
		{
			pageMaterials[i] = new Material(templateMaterial);
		}
		pageRenderers[0].materials = new Material[2]
		{
			null,
			pageMaterials[0]
		};
		pageRenderers[1].materials = new Material[2]
		{
			pageMaterials[1],
			pageMaterials[2]
		};
		pageRenderers[2].materials = new Material[2]
		{
			pageMaterials[3],
			pageMaterials[4]
		};
		pageRenderers[3].materials = new Material[2]
		{
			pageMaterials[5],
			null
		};
	}

	protected override IEnumerator afterEnable()
	{
		SFX.PlayAudioClip(sfx_open);
		PauseCamera.Render();
		int num = 0;
		if (showSpecificPageOnOpen)
		{
			num = specificPageToShowOnOpen;
			focusedOnRight = true;
		}
		else
		{
			int lastViewedPage = Profile.GetLastViewedPage();
			if (lastViewedPage != -1)
			{
				num = lastViewedPage;
			}
		}
		shownPage = num;
		updateRenderersWithTextures();
		Open = true;
		cachedAnimator.SetBool("show", true);
		for (float f = 0f; f < appearDuration; f += TimeManager.UnscaledDeltaTime)
		{
			yield return 0;
		}
	}

	protected override IEnumerator beforeDisable()
	{
		SFX.PlayAudioClip(sfx_close);
		Profile.SetLastViewedPage(shownPage);
		cachedAnimator.SetBool("show", false);
		for (float f = 0f; f < dismissDuration; f += TimeManager.UnscaledDeltaTime)
		{
			yield return 0;
		}
		Open = false;
	}

	protected void Update()
	{
		if (!base.ReceiveInput)
		{
			return;
		}
		if (!turningPage)
		{
			if (GUIInput.Actions.PreviousPage.WasPressed && !showingFirstPage)
			{
				if (focusedOnRight)
				{
					focusedOnRight = false;
				}
				else
				{
					pageturnAnimator.SetTrigger("turn previous");
					shownPage = getUnlockedLeafWithStep(shownPage, -1);
					bookPositionOffset = (leftGoesToFirstPage ? (-1) : 0);
					focusedOnRight = true;
					StartCoroutine(pageturnSequence());
				}
			}
			if (GUIInput.Actions.NextPage.WasPressed && !showingLastPage)
			{
				if (focusedOnRight)
				{
					pageturnAnimator.SetTrigger("turn next");
					shownPage = getUnlockedLeafWithStep(shownPage, 1);
					bookPositionOffset = (rightGoesToLastPage ? 1 : 0);
					focusedOnRight = false;
					StartCoroutine(pageturnSequence());
				}
				else
				{
					focusedOnRight = true;
				}
			}
		}
		int num = bookPositionOffset;
		if (num == 0)
		{
			num = ((!focusedOnRight) ? 1 : (-1));
		}
		pageSpineRoot.localPosition = Vector3.SmoothDamp(pageSpineRoot.localPosition, leftRightOffset * (float)num * Vector3.right, ref leftRightVelocity, leftRightSmoothTime, float.PositiveInfinity, TimeManager.UnscaledDeltaTime);
		Vector3 target = new Vector3(0f - GUIInput.Actions.Look.Y, GUIInput.Actions.Look.X, 0f) * maxViewTweakDegrees;
		smoothedEuler = Vector3.SmoothDamp(smoothedEuler, target, ref viewTweakVelocity, viewTweakSmoothTime, float.PositiveInfinity, TimeManager.UnscaledDeltaTime);
		cameraPivotTransform.rotation = Quaternion.Euler(smoothedEuler);
		if ((GUIInput.Actions.Cancel.WasPressed || (PlayerInput.Actions != null && PlayerInput.Actions.Manual.WasPressed) || GUIInput.Actions.Confirm.WasPressed) && !turningPage)
		{
			close();
		}
	}

	protected IEnumerator pageturnSequence()
	{
		turningPage = true;
		SFX.PlayAudioClip(sfx_flip);
		for (float f = 0f; f < pageTurnDuration; f += TimeManager.UnscaledDeltaTime)
		{
			yield return 0;
		}
		turningPage = false;
		updateRenderersWithTextures();
	}

	protected void close()
	{
		GUIMode.PopMode(this);
	}

	protected int getUnlockedLeafWithStep(int leafIndex, int step)
	{
		if (step < -1 || step > 1)
		{
			Debug.LogWarningFormat("Step can't be {0}.", step);
		}
		int num = leafIndex + step;
		while (true)
		{
			if (num < 0 || num > totalLeafCount)
			{
				return -1;
			}
			if (Profile.IsPageUnlocked(num) || num == totalLeafCount)
			{
				return num;
			}
			if (step == 0)
			{
				break;
			}
			num += step;
		}
		return -1;
	}

	protected void OnApplicationQuit()
	{
		for (int i = 0; i < pageMaterials.Length; i++)
		{
			pageMaterials[i].mainTexture = null;
		}
	}

	protected void updateRenderersWithTextures()
	{
		int unlockedLeafWithStep = getUnlockedLeafWithStep(shownPage, 0);
		Debug.LogFormat("Current page {0}", unlockedLeafWithStep);
		if (unlockedLeafWithStep == totalLeafCount)
		{
			Debug.Log("looking at back cover");
			showingLastPage = true;
			bookPositionOffset = 1;
			leftGoesToFirstPage = false;
			pageMaterials[3].mainTexture = transparentTexture;
			pageMaterials[4].mainTexture = transparentTexture;
			pageMaterials[5].mainTexture = transparentTexture;
		}
		else
		{
			showingLastPage = false;
			bookPositionOffset = 0;
			pageMaterials[3].mainTexture = getTexture(shownPage, true);
			pageMaterials[4].mainTexture = getTexture(shownPage, false);
			int unlockedLeafWithStep2 = getUnlockedLeafWithStep(shownPage, 1);
			if (unlockedLeafWithStep2 == totalLeafCount)
			{
				Debug.Log("next -> is the back cover");
				pageMaterials[5].mainTexture = transparentTexture;
				rightGoesToLastPage = true;
			}
			else
			{
				Debug.Log("there is a next page...");
				pageMaterials[5].mainTexture = getTexture(unlockedLeafWithStep2, true);
				rightGoesToLastPage = false;
			}
		}
		int unlockedLeafWithStep3 = getUnlockedLeafWithStep(shownPage, -1);
		if (unlockedLeafWithStep3 == -1)
		{
			Debug.Log("looking at front cover");
			showingFirstPage = true;
			leftGoesToFirstPage = false;
			bookPositionOffset = -1;
			pageMaterials[0].mainTexture = transparentTexture;
			pageMaterials[1].mainTexture = transparentTexture;
			pageMaterials[2].mainTexture = transparentTexture;
			return;
		}
		showingFirstPage = false;
		pageMaterials[1].mainTexture = getTexture(unlockedLeafWithStep3, true);
		pageMaterials[2].mainTexture = getTexture(unlockedLeafWithStep3, false);
		int unlockedLeafWithStep4 = getUnlockedLeafWithStep(unlockedLeafWithStep3, -1);
		if (unlockedLeafWithStep4 == -1)
		{
			Debug.Log("prev <- is the front cover");
			leftGoesToFirstPage = true;
			pageMaterials[0].mainTexture = transparentTexture;
		}
		else
		{
			Debug.Log("there is a previous page...");
			leftGoesToFirstPage = false;
			pageMaterials[0].mainTexture = getTexture(unlockedLeafWithStep4, false);
		}
	}

	protected Texture getTexture(int page, bool front)
	{
		string pathToPage = getPathToPage(page, front);
		Texture texture = Resources.Load(pathToPage) as Texture;
		if ((bool)texture)
		{
			return texture;
		}
		Debug.LogFormat("Couldn't find page texture at path {0}", pathToPage);
		return null;
	}

	protected string getPathToPage(int i, bool front)
	{
		if (i == 2 && front && PlatformSprite.Platform == PlatformType.XBOX1)
		{
			return "Pages/leaf_2r_alt";
		}
		if (((i == 28 && !front) || (i == 29 && front)) && PlayerInput.Actions.LastDeviceClass != InputDeviceClass.Controller)
		{
			return "Pages/leaf_" + i + (front ? "r" : "v") + "_kb";
		}
		return "Pages/leaf_" + i + (front ? "r" : "v");
	}
}
