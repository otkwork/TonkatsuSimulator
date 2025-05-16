using TMPro;
using UnityEngine;

public class FryerNet : MonoBehaviour
{
	[SerializeField] TextMeshPro m_displayTime;     // m_fryTimeの表示
    [SerializeField] GameObject[] m_netBasket = new GameObject[FryerAmount];    // 網のカツを入れる場所の配列
    GameObject[] m_foodObject = new GameObject[FryerAmount];                    // 網に入っているカツのオブジェクト配列
    FoodType.Food[] m_fryerFood = new FoodType.Food[FryerAmount];               // 網に入っているカツの情報の配列
    BoxCollider[] m_boxCol = new BoxCollider[FryerAmount];                      // 網の判定Cubeの配列    
	private const int OneMinute = 60;		// 1分 = 60秒
    private const float IsFryPos = 0.45f;	// 油に入れているときの網のY座標
    private const float NoFryPos = 0.7f;    // 油に入れていないときの網のY座標
	private const float Speed = 0.01f;		// 網の上下する速度
    private const int FryerAmount = 3;      // フライヤーの網の入れる場所の数

    private bool m_isFry; // 揚げているかどうか
    private float m_fryTime;  // 揚げる時間

    // Start is called before the first frame update
    void Start()
    {
        // 初期位置
        gameObject.transform.localPosition = new Vector3(transform.localPosition.x, NoFryPos, transform.localPosition.z);
        m_isFry = false;
        m_fryTime = 0;
        for (int i = 0; i < FryerAmount; ++i) 
        {
            m_fryerFood[i] = FoodType.Food.None;    // 網に入ってる食べ物をNoneにする
            m_boxCol[i] = m_netBasket[i].GetComponent<BoxCollider>();   // コライダーを取得
        }
    }

    // Update is called once per frame
    void Update()
    {
        // タイマーが0になったら網を上げる
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
        // リセットボタンで網を上げる
        if(Input.GetKeyDown(KeyCode.Space))
        {
			m_isFry = false;
			m_fryTime = 0;
        }
        */

		int minute = (int)m_fryTime / OneMinute;
		int second = (int)m_fryTime % OneMinute;
		// 分:秒
		m_displayTime.text = minute + ":" + second.ToString("00");
	}

    private void FixedUpdate()
    {
        // 揚げ中
        if (m_isFry)
        {
            // 油に浸かっていなかったら
            if (transform.localPosition.y > IsFryPos)
            {
                transform.localPosition -= new Vector3(0, Speed, 0);
            }
        }
        else
        {
            // 油に浸かっていたら
            if (transform.localPosition.y < NoFryPos)
            {
                transform.localPosition += new Vector3(0, Speed, 0);
            }
        }

        // フライヤーにカツが入っているなら判定のコライダーを消す
        for(int i = 0;i < FryerAmount; ++i)
        {
            if (m_foodObject[i] == null) m_fryerFood[i] = FoodType.Food.None;
            m_boxCol[i].enabled = m_fryerFood[i] == FoodType.Food.None;
        }
    }

    // フライヤーのタイマーを引数の時間にする
    public void SetTimer(int time)
    {
        if (m_isFry) return;  // 油に入れているときは新しく時間指定できないようにする 

		m_fryTime = time;
		m_isFry = true;
    }

    // フライヤーの置く場所に物が入っているかどうか
    public bool IsEmptySet(RaycastHit basket, int haveItem, GameObject createFood, float fryTime, bool isCut)
    {
        // 指定した網の場所を確認
        for(int i = 0;i < FryerAmount; ++i)
        {
            if (m_netBasket[i] == basket.transform.gameObject)
            {
				// 空いているかどうか
                if (m_fryerFood[i] == FoodType.Food.None)
                {
                    m_fryerFood[i] = (FoodType.Food)haveItem;

                    // フライヤーの指定した場所に入れてフライヤーの子オブジェクトにする
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
