using UnityEngine;

public class PagePickup : Pickup
{
	[SerializeField]
	protected string pageName;

	protected static readonly string[] pageNames = new string[30]
	{
		"final", "FT_Island", "tablet", "spidercave", "cathedral", "first", "forest shortcut", "forest", "town_upper", "town_well",
		"stonehenge", "skullcave", "temple", "archipelagos dash", "overworld post-forest", "archipelagos", "atoll", "sewer", "fortress", "frog cave",
		"library1", "town filigree", "overworld dash", "archipelagos_night", "stonehenge_reward", "library2", "--- darkwoods filigree", "waterfall", "demo_intro", "demo_sewer"
	};

	protected override bool destroyOnPickup
	{
		get
		{
			return false;
		}
	}

	public static int LeafNameToLeafNumber(string s)
	{
		for (int i = 0; i < pageNames.Length; i++)
		{
			if (s.ToLower() == pageNames[i].ToLower())
			{
				return i;
			}
		}
		return -1;
	}

	protected override bool alreadyPickedUp()
	{
		return Profile.IsPageUnlocked(LeafNameToLeafNumber(pageName));
	}

	protected override void onGetIt()
	{
		Profile.UnlockPage(LeafNameToLeafNumber(pageName));
		PageDisplay.ShowPage(LeafNameToLeafNumber(pageName));
	}
}
