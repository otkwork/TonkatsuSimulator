using Unity.VisualScripting;
using UnityEngine;

public class Tonkatsu : MonoBehaviour
{
	[SerializeField] Material[] m_materials = new Material[4];  // �}�e���A�������O������
	[SerializeField] float fryTime; // �g������[
	[SerializeField] AudioClip cutSound;
	// �O��̃I�[�o�[���Ă͂����Ȃ�����
	[SerializeField] float outTime = 30.0f;
	private GameObject particl;

	[SerializeField] MeshRenderer[] m_meshRenderer;
	int materialIndex;
	private float m_time = 0;   // ���ɓ����Ă�������
	private bool m_cut = false;	// �؂������ǂ���
    // Start is called before the first frame update
    void Start()
    {
		particl = transform.GetChild(2).gameObject;
		particl.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
		materialIndex = 0;
		// �����ȏ�g������
        if (m_time * 2 > fryTime)
        {
			// �g�����Ԃ������Ԃ��Z���ꍇ1
			// �g������+outTiem�������Ԃ������ꍇ3
            materialIndex = m_time < fryTime ? 1 : 
				m_time < fryTime + outTime ? 2 : 3;
        }
		for (int i = 0; i < m_meshRenderer.Length; ++i)
		{
			m_meshRenderer[i].material = m_materials[materialIndex];
		}
    }

    private void OnTriggerStay(Collider other)
	{
		if(other.gameObject.CompareTag("Oil"))
		{
			m_time += Time.deltaTime;
			particl.SetActive(true);
        }
	}

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Oil"))
		{
			particl.SetActive(false);
		}
    }

    public void CutKatsu(bool isCut, bool isSound = false)
	{
		// �؂����Ƃ��̃T�E���h
		if (!transform.GetChild(1).gameObject.activeSelf && isCut && isSound)
		{
			SoundEffect.Play3D(cutSound, transform.position);
		}
		
		transform.GetChild(0).gameObject.SetActive(!isCut);
		transform.GetChild(1).gameObject.SetActive(isCut);
		
		m_cut = isCut;
	}

	public bool IsCutKatsu()
	{
		return m_cut;
	}

    public float GetFryTime()
    {
        return m_time;
    }

	public void SetTime(float time)
	{
		m_time = time;
	}

	public Material GetMaterial()
	{
		return m_materials[materialIndex];
    }
}
