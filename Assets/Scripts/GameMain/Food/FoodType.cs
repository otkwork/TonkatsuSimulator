using UnityEngine;

public class FoodType : MonoBehaviour
{
    [SerializeField] Food type;

    public enum Food
    {
        None = -1,
        Tonkatsu,	// ���[�X����
		Sasami,		// �����݃J�c
		Hire,		// �q���J�c
		Chicken,	// �`�L���J�c
		Karaage,	// ���g��

        Length
    }

    public Food Type
    {
        get { return type; }
    }
}
