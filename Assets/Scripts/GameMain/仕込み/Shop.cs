using TMPro;
using UnityEngine;
using static FoodType;

public class Shop : MonoBehaviour
{
	[SerializeField] private FoodType.Food[] foods = new FoodType.Food[OrderAmount];
	[SerializeField] private int[] foodAmounts = new int[OrderAmount];
	[SerializeField] GameObject[] order = new GameObject[OrderAmount];
	[SerializeField] CardBoardShelf shelf;  // �i�{�[���̒I
	[SerializeField] StoreMoney m_storeMoney;

	[SerializeField] AudioClip buySound;

	TextMeshPro[] m_text = new TextMeshPro[OrderAmount];

	private const int OrderAmount = 6;
	
	private void Start()
	{
		// �e�L�X�g��null�̃Z�b�g
		for(int i = 0; i < OrderAmount; i++)
		{
			foods[i] = FoodType.Food.None;
			foodAmounts[i] = (int)FoodType.Food.None;
			m_text[i] = order[i].transform.GetChild(0).GetComponent<TextMeshPro>();
		}
	}

	// �I�����������������ɓ����
	public int SetOrder(FoodType.Food food, int foodAmount)
	{
		int price = 0;
		for(int i = 0; i < OrderAmount; i++)
		{
			// ���������ɓ����Ă��Ȃ�
			if (foods[i] == FoodType.Food.None)
			{
				foods[i] = food;
				foodAmounts[i] = foodAmount;
				m_text[i].text = ProvideButton.FoodName[(int)food] + "\n" + foodAmount + "��".ToString();
				// �w�����̍��v���z��return
				price = m_storeMoney.AllOrderPrice(food, foodAmount);
				break;
			}
		}
		return price;
	}

	// ���������ŉ������{�^���̏��i�������đO�l��
	public int DeleteOrder(GameObject button)
	{
		int price = 0;
		for (int index = 0; index < OrderAmount; ++index)
		{
			// �������{�^�����ǂꂩ�T��
			if (button == order[index])
			{
				price = m_storeMoney.AllOrderPrice(foods[index], foodAmounts[index]);
				// �������{�^���̒������폜
				foods[index] = FoodType.Food.None;
				foodAmounts[index] = (int)FoodType.Food.None;

				// ���т��l�߂�
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
						m_text[j].text = ProvideButton.FoodName[(int)foods[j]] + "\n" + foodAmounts[j] + "��".ToString();
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
			if (foods[i] == Food.None) break;	// �����̍Ō���܂ŉ�
			allPrice += m_storeMoney.AllOrderPrice(foods[i], foodAmounts[i]);

		}

		// �����Ȃ��Ȃ�return
		if (!m_storeMoney.OrderBuy(allPrice)) return false;

		// ������ł������Ă���Ȃ特���o��
		if (allPrice != 0) SoundEffect.Play3D(buySound, transform.position);

		for (int i = 0; i < OrderAmount; ++i)
		{
			// �����̍Ō���܂ł�I�ɑ���
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
