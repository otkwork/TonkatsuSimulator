using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardBoardShelf : MonoBehaviour
{
	[SerializeField] Transform[] m_cardBoardPos;	// 段ボールを置くポジション
	[SerializeField] GameObject m_cardBoard;        // 段ボールのプレハブ
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
            // 一つでも空いてない場所があれば
            if (createCardBoard[i] != null)
            {
				return false;
            }
        }
		return true;
    }

	// 送られてきた食べ物の種類を送られてきた数段ボールに入れる
	public void SetCardBoard(FoodType.Food food, int foodAmount)
	{
		int shelfIndex = -1;
		// 空きを探す
		for(int i = 0; i < createCardBoard.Length;i++)
		{
			// 一つでも空きがあったら
			if (createCardBoard[i] == null)
			{
				shelfIndex = i;
				break;
			}
		}
		// 空きがない場合return
		if (shelfIndex == -1) return;

		// 段ボールを生成
		GameObject box = Instantiate(m_cardBoard);
		box.transform.TryGetComponent(out CardBoard cardBoard);

		// 段ボールに詰める
		for (int i = 0; i < foodAmount; ++i)
		{
			Instantiate(foods[(int)food], cardBoard.GetPosition(i).position, cardBoard.GetPosition(i).rotation)
				.transform.SetParent(box.transform);
		}
		// 棚に並べる
		PutCardBoard(box, shelfIndex);
	}

	// 作った段ボールを棚に置く
	private void PutCardBoard(GameObject box, int shelfIndex)
	{
		box.transform.position = m_cardBoardPos[shelfIndex].position;
		box.transform.rotation = m_cardBoardPos[shelfIndex].rotation;
		createCardBoard[shelfIndex] = box;
		box.transform.SetParent(transform.GetChild(shelfIndex));
	}

	public void GetCardBoard(GameObject cardBoard)
	{
		// 引数の段ボールが棚のどの段ボールか確認する
		int cardBoardNum = Array.IndexOf(createCardBoard, cardBoard);

		// 指定した段ボールが棚にあった場合
		if (cardBoardNum != -1)
		{
			createCardBoard[cardBoardNum].transform.parent = null;
			createCardBoard[cardBoardNum] = null;
		}
	}
}
