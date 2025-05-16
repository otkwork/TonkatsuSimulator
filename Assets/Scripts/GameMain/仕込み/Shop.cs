using TMPro;
using UnityEngine;
using static FoodType;

public class Shop : MonoBehaviour
{
	[SerializeField] private FoodType.Food[] foods = new FoodType.Food[OrderAmount];
	[SerializeField] private int[] foodAmounts = new int[OrderAmount];
	[SerializeField] GameObject[] order = new GameObject[OrderAmount];
	[SerializeField] CardBoardShelf shelf;  // 段ボールの棚
	[SerializeField] StoreMoney m_storeMoney;

	[SerializeField] AudioClip buySound;

	TextMeshPro[] m_text = new TextMeshPro[OrderAmount];

	private const int OrderAmount = 6;
	
	private void Start()
	{
		// テキストとnullのセット
		for(int i = 0; i < OrderAmount; i++)
		{
			foods[i] = FoodType.Food.None;
			foodAmounts[i] = (int)FoodType.Food.None;
			m_text[i] = order[i].transform.GetChild(0).GetComponent<TextMeshPro>();
		}
	}

	// 選択した注文をかごに入れる
	public int SetOrder(FoodType.Food food, int foodAmount)
	{
		int price = 0;
		for(int i = 0; i < OrderAmount; i++)
		{
			// 注文かごに入っていない
			if (foods[i] == FoodType.Food.None)
			{
				foods[i] = food;
				foodAmounts[i] = foodAmount;
				m_text[i].text = ProvideButton.FoodName[(int)food] + "\n" + foodAmount + "個".ToString();
				// 購入物の合計金額をreturn
				price = m_storeMoney.AllOrderPrice(food, foodAmount);
				break;
			}
		}
		return price;
	}

	// 注文かごで押したボタンの商品を消して前詰め
	public int DeleteOrder(GameObject button)
	{
		int price = 0;
		for (int index = 0; index < OrderAmount; ++index)
		{
			// 押したボタンがどれか探す
			if (button == order[index])
			{
				price = m_storeMoney.AllOrderPrice(foods[index], foodAmounts[index]);
				// 押したボタンの注文を削除
				foods[index] = FoodType.Food.None;
				foodAmounts[index] = (int)FoodType.Food.None;

				// 並びを詰める
				for (int j = index; j < OrderAmount - 1; ++j)
				{
					foods[j] = foods[j + 1];
					foodAmounts[j] = foodAmounts[j + 1];
				}
				foods[OrderAmount - 1] = Food.None;
				foodAmounts[OrderAmount - 1] = (int)Food.None;

				for (int j = 0; j < OrderAmount; ++j)
				{
					if (foods[j] == Food.None)
						m_text[j].text = null;
					else
						m_text[j].text = ProvideButton.FoodName[(int)foods[j]] + "\n" + foodAmounts[j] + "個".ToString();
				}
				break;
			}
		}
		return price;
	}
	
	public bool BuyOrder()
	{
		int allPrice = 0;
		for (int i = 0; i < OrderAmount; ++i)
		{
			if (foods[i] == Food.None) break;	// 注文の最後尾まで回す
			allPrice += m_storeMoney.AllOrderPrice(foods[i], foodAmounts[i]);

		}

		// 金がないならreturn
		if (!m_storeMoney.OrderBuy(allPrice)) return false;

		// 何か一つでも買っているなら音を出す
		if (allPrice != 0) SoundEffect.Play3D(buySound, transform.position);

		for (int i = 0; i < OrderAmount; ++i)
		{
			// かごの最後尾までを棚に送る
			if (foods[i] == Food.None) return true;
			shelf.SetCardBoard(foods[i], foodAmounts[i]);

            foods[i] = FoodType.Food.None;
            foodAmounts[i] = (int)FoodType.Food.None;
            m_text[i].text = null;
        }

		return true;
	}

	public bool IsEmpty()
	{
		return shelf.IsEmpty();
	}
}
