using UnityEngine;
using static FoodType;

public class Dish : MonoBehaviour
{
    GameObject m_cabbage;		// �M�ɐ���L���x�c
    GameObject m_fryFood;		// �M�ɐ���J�c
	[SerializeField] GameObject m_cabbagePos;   // �M�ɐ���L���x�c�̃|�W�V����
	[SerializeField] GameObject m_fryFoodPos;	// �M�ɐ���J�c�̃|�W�V����

    FoodType.Food fryFoodType = FoodType.Food.None;            // �M�ɐ���J�c�̎��
    private float m_time = 0;
	private bool m_cut = false;
   
    // �M�ɃJ�c������Ă��邩�ǂ���
    public bool SetKatsu(GameObject dish, int haveItem, GameObject createFood, float fryTime, bool isCut)
    {
        // �M�ɃJ�c���̂��Ă��邩�ǂ���
        if (m_fryFood == null)    // �̂��Ă��Ȃ�
        {
            fryFoodType = (FoodType.Food)haveItem;

			// �M�ɒu���ĎM�̎q�I�u�W�F�N�g�ɂ���
			m_fryFood = Instantiate(createFood, m_fryFoodPos.transform.position, 
				Quaternion.Euler(m_fryFoodPos.transform.eulerAngles));
			m_fryFood.transform.SetParent(dish.transform);
            // �g�����Ԏw��
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
		// �u���ꏊ�ɉ����Ȃ��Ȃ�true
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
		// �L���x�c���Ȃ�������
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
