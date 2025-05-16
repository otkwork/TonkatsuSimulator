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
			// �������j���[�̃{�^��
			case ButtonType.Shop:
				transform.GetChild(0).TryGetComponent(out TextMeshPro shopText);
				shopText.text = ProvideButton.FoodName[(int)type] + "\n" + foodAmount + "��".ToString();
				break;

			// �J�[�g���̃{�^��
			case ButtonType.Cart:

				break;

			// �w���{�^��
			case ButtonType.Buy:

				break;
		}

	}

	private void Update()
	{
		// �w���{�^���̏ꍇ
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
			// �������j���[�̃{�^��
			case ButtonType.Shop:
				m_buyPrice += shop.SetOrder(type, foodAmount);
				break;

			// �J�[�g���̃{�^��
			case ButtonType.Cart:
				m_buyPrice -= shop.DeleteOrder(hit);
				break;

			// �w���{�^��
			case ButtonType.Buy:
				// ��ł��󂢂ĂȂ��Ƃ��낪�����break
				if (!shop.IsEmpty()) break;
				// �w����������Β����̍��v���i��0
				if (shop.BuyOrder()) m_buyPrice = 0;
				break;
		}
	}
}
