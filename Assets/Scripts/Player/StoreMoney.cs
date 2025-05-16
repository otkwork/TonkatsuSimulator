using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreMoney : MonoBehaviour
{
	[SerializeField] int startMoney;  // 初期金額
	[SerializeField] TextMeshProUGUI m_text;
	private int m_money;  // 売上金
	const int KanstMoney = 99999999;

	// ロース, ささみ, ヒレ, チキン, 唐揚げ
	// 発注価格(1セット価格)
	private int[] orderPrice	= { 130, 200, 320, 210, 250};

	// 提供価格
	private int[] providePrice	= { 670, 750, 930, 770, 830};

    // Start is called before the first frame update
    void Start()
    {
        m_money = startMoney;
    }

    // Update is called once per frame
    void Update()
    {
		// カンストの時の表示
		if (m_money > KanstMoney)
		{
			m_text.text = KanstMoney.ToString();
			return;
		}
        m_text.text = m_money.ToString();
    }

	// 商品提供時の売上
	public void ProductSales(FoodType.Food food)
	{
		m_money += providePrice[(int)food];
	}

	// 発注時の所持金
	public bool OrderBuy(int price)
	{
		// 注文価格が所持金より高い場合return false
		if (m_money < price) return false;

		m_money -= price;
		return true;
	}

	public int AllOrderPrice(FoodType.Food food, int foodAmount)
	{
		return orderPrice[(int)food] * foodAmount;
	}
}
