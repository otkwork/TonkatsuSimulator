using System;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;

public class ProvideButton : MonoBehaviour
{
	public static readonly ReadOnlyCollection<string> FoodName = 
		Array.AsReadOnly(new string[] 
		{
			"���[�X�J�c",
			"�����݃J�c",
			"�q���J�c",
			"�`�L���J�c",
			"���g��"
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
	private const float TimeUp = 420.0f;	// ����
	private bool m_timeUp = false;	// ���ԂɂȂ�����]����������
    private const int OneMinute = 60;
    void Update()
    {
        m_time += Time.deltaTime;
        m_nameText.text = FoodName[(int)m_food].ToString();
        if (m_material.name != "�ǂ��g������") m_fryText.text = "�悭�Ă�".ToString();
        if (!m_cabbage) m_cabbageText.text = "�L���x�c�Ȃ�".ToString();
        if (m_food != FoodType.Food.Hire && m_food != FoodType.Food.Karaage && !m_cut) m_cutText.text = "�J�b�g�Ȃ�".ToString();

        int minute = (int)m_time / OneMinute;
        int second = (int)m_time % OneMinute;
        // ��:�b
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
		// ���g���ƃq���̓J�b�g�Ȃ�
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
