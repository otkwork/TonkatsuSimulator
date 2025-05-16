using UnityEngine;
using static FoodType;

public class Dish : MonoBehaviour
{
    GameObject m_cabbage;		// 皿に盛るキャベツ
    GameObject m_fryFood;		// 皿に盛るカツ
	[SerializeField] GameObject m_cabbagePos;   // 皿に盛るキャベツのポジション
	[SerializeField] GameObject m_fryFoodPos;	// 皿に盛るカツのポジション

    FoodType.Food fryFoodType = FoodType.Food.None;            // 皿に盛るカツの種類
    private float m_time = 0;
	private bool m_cut = false;
   
    // 皿にカツが乗っているかどうか
    public bool SetKatsu(GameObject dish, int haveItem, GameObject createFood, float fryTime, bool isCut)
    {
        // 皿にカツがのっているかどうか
        if (m_fryFood == null)    // のっていない
        {
            fryFoodType = (FoodType.Food)haveItem;

			// 皿に置いて皿の子オブジェクトにする
			m_fryFood = Instantiate(createFood, m_fryFoodPos.transform.position, 
				Quaternion.Euler(m_fryFoodPos.transform.eulerAngles));
			m_fryFood.transform.SetParent(dish.transform);
            // 揚げ時間指定
            m_time = fryTime;
			m_cut = isCut;
			m_fryFood.transform.GetComponent<Tonkatsu>().SetTime(m_time);
			m_fryFood.transform.GetComponent<Tonkatsu>().CutKatsu(m_cut);
            return true;
        }
        return false;
    }

	public bool ActiveRayObject(bool isKatsu)
	{
		// 置く場所に何もないならtrue
		if (isKatsu)
		{
			return m_fryFood == null;
		}
		else
		{
			return m_cabbage == null;
		}
	}

	public Transform GetRayPos(bool isKatsu)
	{
		if (isKatsu)
		{
			return m_fryFoodPos.transform;
		}
		else
		{
			return m_cabbagePos.transform;
		}
	}

	public bool SetCabbage(GameObject dish, GameObject cabbage)
	{
		// キャベツがなかったら
		if(m_cabbage == null)
		{
			cabbage.SetActive(true);
			m_cabbage = Instantiate(cabbage, m_cabbagePos.transform.position, 
				Quaternion.Euler(0,0,0));
			m_cabbage.transform.SetParent(dish.transform);
			return true;
		}
		return false;
	}

    public void SetFood(GameObject food)
    {
		m_fryFood = food;
    }

    public void SetType(int haveItem)
    {
        fryFoodType = (Food)haveItem;
    }

    public void SetTime(float time)
    {
        m_time = time;
    }

	public bool IsCutKatsu()
	{
		return m_cut;
	}

	public bool OnCabbage()
	{
		return m_cabbage != null;
	}

    public Food Type
    {
        get { return fryFoodType; }
    }

    public float Time
    {
        get { return m_time; }
    }

    public Material GetMaterial()
    {
		if (m_fryFood == null) return null;
		if (m_fryFood.TryGetComponent(out Tonkatsu tonkatsu))
		{
			return tonkatsu.GetMaterial();
		}
		return null;
    }
}
