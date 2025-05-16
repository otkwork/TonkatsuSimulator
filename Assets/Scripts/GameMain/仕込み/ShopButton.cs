using TMPro;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
	enum ButtonType
	{
		Shop,
		Cart,
		Buy,
	}
	[SerializeField] ButtonType button;

	[SerializeField] FoodType.Food type;
	[SerializeField] int foodAmount;
	Shop shop;

	static int m_buyPrice = 0;

	private void Start()
	{
		shop = transform.parent.parent.GetComponent<Shop>();
		switch (button)
		{
			// 注文メニューのボタン
			case ButtonType.Shop:
				transform.GetChild(0).TryGetComponent(out TextMeshPro shopText);
				shopText.text = ProvideButton.FoodName[(int)type] + "\n" + foodAmount + "個".ToString();
				break;

			// カート内のボタン
			case ButtonType.Cart:

				break;

			// 購入ボタン
			case ButtonType.Buy:

				break;
		}

	}

	private void Update()
	{
		// 購入ボタンの場合
		if (button == ButtonType.Buy)
		{
			transform.GetChild(0).TryGetComponent(out TextMeshPro buyText);
			buyText.text = m_buyPrice.ToString();
		}
	}

	public void BuyFood(GameObject hit)
	{
		switch (button)
		{
			// 注文メニューのボタン
			case ButtonType.Shop:
				m_buyPrice += shop.SetOrder(type, foodAmount);
				break;

			// カート内のボタン
			case ButtonType.Cart:
				m_buyPrice -= shop.DeleteOrder(hit);
				break;

			// 購入ボタン
			case ButtonType.Buy:
				// 一つでも空いてないところがあればbreak
				if (!shop.IsEmpty()) break;
				// 購入物があれば注文の合計価格を0
				if (shop.BuyOrder()) m_buyPrice = 0;
				break;
		}
	}
}
