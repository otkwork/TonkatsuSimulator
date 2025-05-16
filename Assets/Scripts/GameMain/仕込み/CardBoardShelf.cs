using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardBoardShelf : MonoBehaviour
{
	[SerializeField] Transform[] m_cardBoardPos;	// �i�{�[����u���|�W�V����
	[SerializeField] GameObject m_cardBoard;        // �i�{�[���̃v���n�u
	[SerializeField] GameObject[] createCardBoard = new GameObject[6];
	[SerializeField] GameObject[] foods;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < createCardBoard.Length; i++)
		{
			createCardBoard[i] = null;
		}
    }

	public bool IsEmpty()
	{
        for (int i = 0; i < createCardBoard.Length; i++)
        {
            // ��ł��󂢂ĂȂ��ꏊ�������
            if (createCardBoard[i] != null)
            {
				return false;
            }
        }
		return true;
    }

	// �����Ă����H�ו��̎�ނ𑗂��Ă������i�{�[���ɓ����
	public void SetCardBoard(FoodType.Food food, int foodAmount)
	{
		int shelfIndex = -1;
		// �󂫂�T��
		for(int i = 0; i < createCardBoard.Length;i++)
		{
			// ��ł��󂫂���������
			if (createCardBoard[i] == null)
			{
				shelfIndex = i;
				break;
			}
		}
		// �󂫂��Ȃ��ꍇreturn
		if (shelfIndex == -1) return;

		// �i�{�[���𐶐�
		GameObject box = Instantiate(m_cardBoard);
		box.transform.TryGetComponent(out CardBoard cardBoard);

		// �i�{�[���ɋl�߂�
		for (int i = 0; i < foodAmount; ++i)
		{
			Instantiate(foods[(int)food], cardBoard.GetPosition(i).position, cardBoard.GetPosition(i).rotation)
				.transform.SetParent(box.transform);
		}
		// �I�ɕ��ׂ�
		PutCardBoard(box, shelfIndex);
	}

	// ������i�{�[����I�ɒu��
	private void PutCardBoard(GameObject box, int shelfIndex)
	{
		box.transform.position = m_cardBoardPos[shelfIndex].position;
		box.transform.rotation = m_cardBoardPos[shelfIndex].rotation;
		createCardBoard[shelfIndex] = box;
		box.transform.SetParent(transform.GetChild(shelfIndex));
	}

	public void GetCardBoard(GameObject cardBoard)
	{
		// �����̒i�{�[�����I�̂ǂ̒i�{�[�����m�F����
		int cardBoardNum = Array.IndexOf(createCardBoard, cardBoard);

		// �w�肵���i�{�[�����I�ɂ������ꍇ
		if (cardBoardNum != -1)
		{
			createCardBoard[cardBoardNum].transform.parent = null;
			createCardBoard[cardBoardNum] = null;
		}
	}
}
