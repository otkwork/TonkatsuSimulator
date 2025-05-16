using System;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;

public class ProvideButton : MonoBehaviour
{
	public static readonly ReadOnlyCollection<string> FoodName = 
		Array.AsReadOnly(new string[] 
		{
			"ロースカツ",
			"ささみカツ",
			"ヒレカツ",
			"チキンカツ",
			"唐揚げ"
		});

    private FoodType.Food m_food;
    private Material m_material;
    private bool m_cabbage;
    private bool m_cut;


    [SerializeField] TextMeshPro m_nameText;
    [SerializeField] TextMeshPro m_fryText;
    [SerializeField] TextMeshPro m_cabbageText;
    [SerializeField] TextMeshPro m_cutText;

    [SerializeField] TextMeshPro m_timeText;

    private float m_time = 0;
	private const float TimeUp = 420.0f;	// 七分
	private bool m_timeUp = false;	// 時間になったら評判を下げる
    private const int OneMinute = 60;
    void Update()
    {
        m_time += Time.deltaTime;
        m_nameText.text = FoodName[(int)m_food].ToString();
        if (m_material.name != "良い揚げ時間") m_fryText.text = "よく焼き".ToString();
        if (!m_cabbage) m_cabbageText.text = "キャベツなし".ToString();
        if (m_food != FoodType.Food.Hire && m_food != FoodType.Food.Karaage && !m_cut) m_cutText.text = "カットなし".ToString();

        int minute = (int)m_time / OneMinute;
        int second = (int)m_time % OneMinute;
        // 分:秒
        m_timeText.text = minute + ":" + second.ToString("00");

		if (m_time > TimeUp && !m_timeUp)
		{
			m_timeUp = true;
		}
    }

    public FoodType.Food GetFood
    {
        get { return m_food; }
    }

    public Material GetFry
    {
        get { return m_material; }
    }

    public bool GetCabbage
    {
        get { return m_cabbage; }
    }

    public bool GetCut
    {
        get { return m_cut; }
    }

	public bool GetTimeUp
	{
		get { return m_timeUp; }
	}

	public void SetInfo(FoodType.Food food, Material material, bool cabbage, bool cut)
    {
        m_food = food;
        m_material = material;
        m_cabbage = cabbage;
		// 唐揚げとヒレはカットなし
		if (m_food == FoodType.Food.Hire || m_food == FoodType.Food.Karaage)
		{
			m_cut = false;
		}
        else
        {
			m_cut = cut;
        }
    }
}
