using UnityEngine;

[ExecuteInEditMode]
public class SampleBitMask : MonoBehaviour
{
	[BitMask]
	public Topping myToppings;

	private Rect firstLabelPosition = new Rect(100f, 100f, 800f, 50f);

	private Rect secondLabelPosition = new Rect(100f, 200f, 800f, 50f);

	private void OnGUI()
	{
		if ((myToppings & Topping.cherries) == Topping.cherries)
		{
			GUI.Label(firstLabelPosition, "myToppings contains cherries!");
		}
		else
		{
			GUI.Label(firstLabelPosition, "No cherries in myToppings.");
		}
		if ((myToppings & (Topping)36) == (Topping)36)
		{
			GUI.Label(secondLabelPosition, "myToppings contains coconut AND carmel!");
		}
		else
		{
			GUI.Label(secondLabelPosition, "myToppings doesn't contain both coconut and carmel.");
		}
	}
}
