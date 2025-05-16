using UnityEngine;

public class ProvideTable : MonoBehaviour
{
    private int m_provideNum = 0;

    // ディスパチが押された時
    public GameObject IsProvide(FoodType.Food foodType, Material material, bool onCabbage, bool isCut)
    {
        for (int i = 0; i < m_provideNum; ++i)
        {
            if (CheckDish(i, foodType, material, onCabbage, isCut))
            {
                // 条件に合った料理があった場合その料理をreturn
                return gameObject.transform.GetChild(i).gameObject;
            }
        }
        return null;
    }

    // 注文に合った物が作れているかどうか
    private bool CheckDish(int provideNum, FoodType.Food foodType, Material material, bool onCabbage, bool isCut)
    {
        gameObject.transform.GetChild(provideNum).TryGetComponent(out Dish dish);

        if (dish.Type != foodType) return false;            // 指定のカツじゃない場合false
        if (dish.GetMaterial() != material) return false;     // 指定の揚げ時間じゃない場合false
        if (dish.OnCabbage() != onCabbage) return false;    // キャベツが載っていない場合false
        if (dish.IsCutKatsu() != isCut) return false;       // 切っていない場合false
        m_provideNum--;
        return true;
    }

    // 提供台に皿を置いた
    public void SetProvide()
    {
        m_provideNum++;
    }

    // 提供台の皿を取った
    public void TakeProvide()
    {
        m_provideNum--;
    }
}
