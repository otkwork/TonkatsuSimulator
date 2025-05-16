using UnityEngine;

public class ProvideTable : MonoBehaviour
{
    private int m_provideNum = 0;

    // �f�B�X�p�`�������ꂽ��
    public GameObject IsProvide(FoodType.Food foodType, Material material, bool onCabbage, bool isCut)
    {
        for (int i = 0; i < m_provideNum; ++i)
        {
            if (CheckDish(i, foodType, material, onCabbage, isCut))
            {
                // �����ɍ������������������ꍇ���̗�����return
                return gameObject.transform.GetChild(i).gameObject;
            }
        }
        return null;
    }

    // �����ɍ������������Ă��邩�ǂ���
    private bool CheckDish(int provideNum, FoodType.Food foodType, Material material, bool onCabbage, bool isCut)
    {
        gameObject.transform.GetChild(provideNum).TryGetComponent(out Dish dish);

        if (dish.Type != foodType) return false;            // �w��̃J�c����Ȃ��ꍇfalse
        if (dish.GetMaterial() != material) return false;     // �w��̗g�����Ԃ���Ȃ��ꍇfalse
        if (dish.OnCabbage() != onCabbage) return false;    // �L���x�c���ڂ��Ă��Ȃ��ꍇfalse
        if (dish.IsCutKatsu() != isCut) return false;       // �؂��Ă��Ȃ��ꍇfalse
        m_provideNum--;
        return true;
    }

    // �񋟑�ɎM��u����
    public void SetProvide()
    {
        m_provideNum++;
    }

    // �񋟑�̎M�������
    public void TakeProvide()
    {
        m_provideNum--;
    }
}
