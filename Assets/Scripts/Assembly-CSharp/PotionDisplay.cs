using UnityEngine;
using UnityEngine.UI;

public class PotionDisplay : MonoBehaviour
{
	[SerializeField]
	protected Item potionItem;

	[SerializeField]
	protected Item potionFlaskItem;

	[SerializeField]
	protected Sprite fullPotionSprite;

	[SerializeField]
	protected Sprite emptyPotionSprite;

	[SerializeField]
	protected Image[] sprites;

	[SerializeField]
	protected Image buttonPrompt;

	private void Update()
	{
		int quantity = potionFlaskItem.Quantity;
		int quantity2 = potionItem.Quantity;
		for (int i = 0; i < sprites.Length; i++)
		{
			if (sprites.Length - i > quantity)
			{
				sprites[i].enabled = false;
			}
			else if (sprites.Length - i > quantity2)
			{
				sprites[i].enabled = true;
				sprites[i].sprite = emptyPotionSprite;
			}
			else
			{
				sprites[i].enabled = true;
				sprites[i].sprite = fullPotionSprite;
			}
		}
		buttonPrompt.enabled = quantity > 0;
	}
}
