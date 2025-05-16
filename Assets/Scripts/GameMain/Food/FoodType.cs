using UnityEngine;

public class FoodType : MonoBehaviour
{
    [SerializeField] Food type;

    public enum Food
    {
        None = -1,
        Tonkatsu,	// ロースかつ
		Sasami,		// ささみカツ
		Hire,		// ヒレカツ
		Chicken,	// チキンカツ
		Karaage,	// 唐揚げ

        Length
    }

    public Food Type
    {
        get { return type; }
    }
}
