using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreMoney : MonoBehaviour
{
	[SerializeField] int startMoney;  // �������z
	[SerializeField] TextMeshProUGUI m_text;
	private int m_money;  // �����
	const int KanstMoney = 99999999;

	// ���[�X, ������, �q��, �`�L��, ���g��
	// �������i(1�Z�b�g���i)
	private int[] orderPrice	= { 130, 200, 320, 210, 250};

	// �񋟉��i
	private int[] providePrice	= { 670, 750, 930, 770, 830};

    // Start is called before the first frame update
    void Start()
    {
        m_money = startMoney;
    }

    // Update is called once per frame
    void Update()
    {
		// �J���X�g�̎��̕\��
		if (m_money > KanstMoney)
		{
			m_text.text = KanstMoney.ToString();
			return;
		}
        m_text.text = m_money.ToString();
    }

	// ���i�񋟎��̔���
	public void ProductSales(FoodType.Food food)
	{
		m_money += providePrice[(int)food];
	}

	// �������̏�����
	public bool OrderBuy(int price)
	{
		// �������i����������荂���ꍇreturn false
		if (m_money < price) return false;

		m_money -= price;
		return true;
	}

	public int AllOrderPrice(FoodType.Food food, int foodAmount)
	{
		return orderPrice[(int)food] * foodAmount;
	}
}
