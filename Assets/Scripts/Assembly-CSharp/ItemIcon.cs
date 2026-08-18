using UnityEngine;
using UnityEngine.UI;

public class ItemIcon : MonoBehaviour
{
	[SerializeField]
	public Text Quantity;

	[SerializeField]
	public Text Denominator;

	[SerializeField]
	public Image Image;

	[SerializeField]
	public RectTransform RectTransform;

	[SerializeField]
	public Button button;

	[SerializeField]
	public GameObject Upgrade_PanelRoot;

	[SerializeField]
	public MixedTextBuilder Upgrade_StatLabel;

	[SerializeField]
	public Text Upgrade_CurrentLevel;

	[SerializeField]
	public Text Upgrade_CurrentLevelForComparison;

	[SerializeField]
	public Text Upgrade_NextLevelForComparison;

	[SerializeField]
	public Image Upgrade_ComparisonArrow;

	[SerializeField]
	public Image Upgrade_X_OutImage;

	[SerializeField]
	public Image Upgrade_Star;

	public Item Item { get; set; }
}
