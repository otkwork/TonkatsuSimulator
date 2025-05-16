using UnityEngine;

public class Vat : MonoBehaviour
{
	[SerializeField] GameObject prefabKatsu;	// 初期設定でつける入れておくカツ
    [SerializeField] GameObject[] m_katsuPos;
    private bool[] m_isChild;
	private const int FirstSetKatsu = 5;

    [SerializeField] FoodType.Food m_foodType;
    // Start is called before the first frame update
    void Start()
    {
        m_isChild = new bool[m_katsuPos.Length];

		for (int i = 0; i < FirstSetKatsu; ++i)
		{
			GameObject katsu = Instantiate(prefabKatsu);
			katsu.transform.SetParent(m_katsuPos[i].transform);
			katsu.transform.position = m_katsuPos[i].transform.position;
			katsu.transform.rotation = m_katsuPos[i].transform.rotation;

			m_isChild[i] = true;
		}
    }

    // カツを取る
    public FoodType.Food GetKatsu()
    {
        for (int i = m_katsuPos.Length - 1; i >= 0; --i)
        {
            //　カツがある場所を探す
            if (m_isChild[i])
            {
                Destroy(m_katsuPos[i].transform.GetChild(0).gameObject);
                m_isChild[i] = false;
                return m_foodType;
            }
        }
        return FoodType.Food.None;
    }

    // カツを戻す
    public bool SetKatsu(GameObject katsu)
    {
        for (int i = 0; i < m_katsuPos.Length; ++i)
        {
            //　カツがない場所を探す
            if (!m_isChild[i])
            {
                GameObject newKatsu = Instantiate(katsu, m_katsuPos[i].transform.position, m_katsuPos[i].transform.localRotation);
                newKatsu.transform.SetParent(m_katsuPos[i].transform);
                m_isChild[i] = true;
                return true;
            }
        }
        return false;
    }

    public FoodType.Food GetFoodType()
    {
        return m_foodType;
    }

    public GameObject GetPos()
    {
        for (int i = 0; i < m_katsuPos.Length; ++i)
        {
            //　カツがない場所を探す
            if (!m_isChild[i])
            {
                return m_katsuPos[i];
            }
        }
        return null;
    }
}
