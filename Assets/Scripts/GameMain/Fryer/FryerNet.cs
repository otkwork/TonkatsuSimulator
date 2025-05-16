using TMPro;
using UnityEngine;

public class FryerNet : MonoBehaviour
{
	[SerializeField] TextMeshPro m_displayTime;     // m_fryTime�̕\��
    [SerializeField] GameObject[] m_netBasket = new GameObject[FryerAmount];    // �Ԃ̃J�c������ꏊ�̔z��
    GameObject[] m_foodObject = new GameObject[FryerAmount];                    // �Ԃɓ����Ă���J�c�̃I�u�W�F�N�g�z��
    FoodType.Food[] m_fryerFood = new FoodType.Food[FryerAmount];               // �Ԃɓ����Ă���J�c�̏��̔z��
    BoxCollider[] m_boxCol = new BoxCollider[FryerAmount];                      // �Ԃ̔���Cube�̔z��    
	private const int OneMinute = 60;		// 1�� = 60�b
    private const float IsFryPos = 0.45f;	// ���ɓ���Ă���Ƃ��̖Ԃ�Y���W
    private const float NoFryPos = 0.7f;    // ���ɓ���Ă��Ȃ��Ƃ��̖Ԃ�Y���W
	private const float Speed = 0.01f;		// �Ԃ̏㉺���鑬�x
    private const int FryerAmount = 3;      // �t���C���[�̖Ԃ̓����ꏊ�̐�

    private bool m_isFry; // �g���Ă��邩�ǂ���
    private float m_fryTime;  // �g���鎞��

    // Start is called before the first frame update
    void Start()
    {
        // �����ʒu
        gameObject.transform.localPosition = new Vector3(transform.localPosition.x, NoFryPos, transform.localPosition.z);
        m_isFry = false;
        m_fryTime = 0;
        for (int i = 0; i < FryerAmount; ++i) 
        {
            m_fryerFood[i] = FoodType.Food.None;    // �Ԃɓ����Ă�H�ו���None�ɂ���
            m_boxCol[i] = m_netBasket[i].GetComponent<BoxCollider>();   // �R���C�_�[���擾
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �^�C�}�[��0�ɂȂ�����Ԃ��グ��
        if(m_fryTime < 0 )
        {
			m_isFry = false;
			m_fryTime = 0;
        }
		else
		{
			m_fryTime -= Time.deltaTime;
		}

        /*
        // ���Z�b�g�{�^���ŖԂ��グ��
        if(Input.GetKeyDown(KeyCode.Space))
        {
			m_isFry = false;
			m_fryTime = 0;
        }
        */

		int minute = (int)m_fryTime / OneMinute;
		int second = (int)m_fryTime % OneMinute;
		// ��:�b
		m_displayTime.text = minute + ":" + second.ToString("00");
	}

    private void FixedUpdate()
    {
        // �g����
        if (m_isFry)
        {
            // ���ɐZ�����Ă��Ȃ�������
            if (transform.localPosition.y > IsFryPos)
            {
                transform.localPosition -= new Vector3(0, Speed, 0);
            }
        }
        else
        {
            // ���ɐZ�����Ă�����
            if (transform.localPosition.y < NoFryPos)
            {
                transform.localPosition += new Vector3(0, Speed, 0);
            }
        }

        // �t���C���[�ɃJ�c�������Ă���Ȃ画��̃R���C�_�[������
        for(int i = 0;i < FryerAmount; ++i)
        {
            if (m_foodObject[i] == null) m_fryerFood[i] = FoodType.Food.None;
            m_boxCol[i].enabled = m_fryerFood[i] == FoodType.Food.None;
        }
    }

    // �t���C���[�̃^�C�}�[�������̎��Ԃɂ���
    public void SetTimer(int time)
    {
        if (m_isFry) return;  // ���ɓ���Ă���Ƃ��͐V�������Ԏw��ł��Ȃ��悤�ɂ��� 

		m_fryTime = time;
		m_isFry = true;
    }

    // �t���C���[�̒u���ꏊ�ɕ��������Ă��邩�ǂ���
    public bool IsEmptySet(RaycastHit basket, int haveItem, GameObject createFood, float fryTime, bool isCut)
    {
        // �w�肵���Ԃ̏ꏊ���m�F
        for(int i = 0;i < FryerAmount; ++i)
        {
            if (m_netBasket[i] == basket.transform.gameObject)
            {
				// �󂢂Ă��邩�ǂ���
                if (m_fryerFood[i] == FoodType.Food.None)
                {
                    m_fryerFood[i] = (FoodType.Food)haveItem;

                    // �t���C���[�̎w�肵���ꏊ�ɓ���ăt���C���[�̎q�I�u�W�F�N�g�ɂ���
                    m_foodObject[i] = Instantiate(createFood, basket.transform.position, Quaternion.identity);
                    m_foodObject[i].transform.SetParent(basket.transform);
                    m_foodObject[i].transform.GetComponent<Tonkatsu>().SetTime(fryTime);
                    m_foodObject[i].transform.GetComponent<Tonkatsu>().CutKatsu(isCut);
                    return true;
                }
            }
        }
        return false;
    }
}
