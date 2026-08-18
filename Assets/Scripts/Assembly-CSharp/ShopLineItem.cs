using UnityEngine;
using UnityEngine.UI;

public class ShopLineItem : MonoBehaviour
{
	[SerializeField]
	public Image ItemIcon;

	[SerializeField]
	public Text PriceText;

	[SerializeField]
	public Button ButtonSubtract;

	[SerializeField]
	public Button ButtonAdd;

	[SerializeField]
	public Text PurchaseQuantityText;

	[SerializeField]
	public Text PurchaseCostText;

	[SerializeField]
	public Text HeldText;

	[SerializeField]
	public Text HeldMaxText;

	[SerializeField]
	public Animator OverCapacityAnimator;

	public WellSupplyItem WellSupplyItem { get; set; }
}
