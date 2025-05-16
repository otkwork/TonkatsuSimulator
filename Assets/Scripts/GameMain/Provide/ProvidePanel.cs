using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProvidePanel : MonoBehaviour
{
    [SerializeField] ProvideTable m_provideTable;
	[SerializeField] StorePopularity m_storePop;
    [SerializeField] GameObject m_provideButton;
    [SerializeField] GameObject[] buttonPos;
    [SerializeField] Material[] m_material;

    [SerializeField] int maxTime;   // 注文が来るまでの最長時間
    [SerializeField] int minTime;   // 注文が来るまでの最短時間

	[SerializeField] AudioClip provideSound;
	[SerializeField] AudioClip orderSound;
    private float upTime;           // 時間経過で注文が来る時間を早くする
    private const int UpTime = 30;

	private const int ProvideNum = 20;
    private const int ProvideMinNum = 1;

	[SerializeField] int rosePercent = 4;

	[SerializeField] GameObject[] m_button = new GameObject[ProvideNum];
    private int m_provideNum = -1;
    private int m_provideTime;
    private int m_provideRandom = ProvideMinNum;

    private float delta;

    private void Awake()
    {
		m_provideTime = Random.Range(minTime, maxTime);
		delta = m_provideTime;
    }

    private void Update()
    {
        delta += Time.deltaTime;
        upTime += Time.deltaTime;
        // 一定時間で注文が来る時間を短くする
        if (upTime > UpTime)
        {
            upTime = 0;
            maxTime -= 1;
			// 指定した時間よりは短くならない
			if (maxTime <= minTime) maxTime = minTime;
        }
        if (delta >= m_provideTime && m_provideNum < ProvideNum)
        {
            SetProvide(RandomProvideNum());
			m_provideTime = Random.Range(minTime, maxTime);
            delta = 0;
        }
    }

    // ディスパチが押された時
    public GameObject Provide(FoodType.Food foodType, Material material, bool onCabbage, bool isCut, bool isTimeUp)
    {
		GameObject provide = m_provideTable.IsProvide(foodType, material, onCabbage, isCut);
		
		// 提供可能
        if (provide != null)
        {
			SoundEffect.Play3D(provideSound, transform.position, 1, 1.5f);
			if (isTimeUp) m_storePop.DownPop();
			else m_storePop.UpPop();
		}
		return provide;
    }

	// 注文を生成
    private void SetProvide(int provideNum)
    {
        for (int i = 0; i < provideNum; ++i)
        {
			// 注文がいっぱい
			if (m_provideNum >= ProvideNum - 1)
			{
				FillProvide();
				return;
			}
            m_provideNum++;

			// 内容を設定
			RandomFood();
            FoodType.Food foodType = RandomFood();
            Material material = m_material[Random.Range(0, 11) != 0 ? 0 : 1];   // 10%でよく焼き
            bool cabbage = Random.Range(0, 11) != 0;    // 10%でキャベツなし
            bool isCut = Random.Range(0, 11) != 0;  // 10%でカットなし

            m_button[m_provideNum] =
                Instantiate(m_provideButton, buttonPos[m_provideNum].transform.position, buttonPos[m_provideNum].transform.rotation);
            m_button[m_provideNum].transform.SetParent(gameObject.transform);
            m_button[m_provideNum].GetComponent<ProvideButton>().SetInfo(foodType, material, cabbage, isCut);
        }
		SoundEffect.Play3D(orderSound, transform.position);
    }

	private void FillProvide()
	{
		m_storePop.DownPop();
	}

	private FoodType.Food RandomFood()
	{
		FoodType.Food food;
		// ロースの確立を上げる
		if (Random.Range(1, 11) <= rosePercent)
		{
			food = FoodType.Food.Tonkatsu;
		}
		else
		{
			// ロース以外
			food = (FoodType.Food)Random.Range(1, (int)FoodType.Food.Length);
		}

		return food;
	}

	// 提供時に後ろの注文を前詰め
    public void DecNum(GameObject putButton)
    {
		for (int index = 0; index < m_button.Length; ++index)
		{
			if (m_button[index] == putButton)
			{
				for (int j = m_provideNum; j > index; --j)
				{
					m_button[j].transform.position = buttonPos[j - 1].transform.position;
				}
				for (int j = index; j < m_provideNum; ++j)
				{
					m_button[j] = m_button[j + 1];
				}
                m_button[m_provideNum] = null;
				m_provideNum--;
				break;
			}
		}
    }

    // 一度に来る注文のランダムテーブル
    private int RandomProvideNum()
    {
        int num;
        // 前回が1人の場合ランダムでもう一回
        if (m_provideRandom == ProvideMinNum)
        {
            num = Random.Range(ProvideMinNum, 21);
            if (num <= 10) m_provideRandom = 1;
            else if (num <= 15) m_provideRandom = 2;
            else if (num <= 18) m_provideRandom = 3;
            else m_provideRandom = 4;

            return m_provideRandom;
        }
        // 前回が複数人だった場合
        else
        {
            return ProvideMinNum;
        }
    }
}
