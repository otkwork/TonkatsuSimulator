using UnityEngine;

public class FryButtonType : MonoBehaviour
{
	[SerializeField] Fryer m_fryer;
    private int[] m_fryTime = { 30, 60, 90, 120 };
    
    public enum ButtonType
    {
        ThirtySecond,
        SixtySecond,
		NinetySecond,
        OneHundredTwenty
    }

    public enum FryerPosition    // フライヤーの左右どっちの網か
    {
        Left,
        Right,
    }

    [SerializeField] 
    ButtonType m_buttonType;

    [SerializeField] 
    FryerPosition m_position;

	// ボタンのタイプを返す
    public ButtonType Type
    {
        get { return m_buttonType; }
    }

	public void PutButton(int index)
	{
		m_fryer.PutButton(m_fryTime[index], (int)m_position);
	}
}
