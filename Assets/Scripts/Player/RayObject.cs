using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.UI.Image;

public class RayObject : MonoBehaviour
{
	[SerializeField] MeshRenderer[] m_renderer;
	[SerializeField] MeshRenderer rayDish;

	private bool m_OnTrigger;
	[SerializeField] Material[] m_material;
	int m_materialNum;

	enum RayColor
	{
		Red,
		Blue,
	}
	// Start is called before the first frame update
	void Start()
    {
		m_materialNum = 0;
		for (int i = 0; i < m_renderer.Length; ++i)
		{
			m_renderer[i].material = m_material[(int)RayColor.Blue];
		}
	}

	public void SetInvisible(bool active, bool rayPoint = false)
	{
		// Ray.point���N�_�ɐݒu
		if (rayPoint)
		{
			if (active)
			{
				if (m_OnTrigger) m_materialNum = (int)RayColor.Blue;
				else m_materialNum = (int)RayColor.Red;

				for (int i = 0; i < m_renderer.Length; ++i)
				{
					m_renderer[i].enabled = true;
					m_renderer[i].material = m_material[m_materialNum];
				}
			}
			else
			{
				for (int i = 0; i < m_renderer.Length; ++i)
				{
					m_renderer[i].material = m_material[(int)RayColor.Blue];
					m_renderer[i].enabled = false;
				}
			}
		}
		// ���������I�u�W�F�N�g�������Ă���|�W�V����
		// �M�̃J�c�A�L���x�c�ƃo�b�g
		else
		{
            if (active)
            {
                for (int i = 0; i < m_renderer.Length; ++i)
                {
					// �\��
                    m_renderer[i].enabled = true;

					// �M�ɏ���Ă���ꍇ
					if (rayDish.enabled && rayDish.TryGetComponent(out RayObject ray))
					{
						// �J�c�ƃL���x�c�̕\�����M�Ɠ����F�ɂ���
						m_materialNum = ray.GetMaterialNum();
						m_renderer[i].material = m_material[m_materialNum];
					}
					else
					{
						// ��ɐ�
						m_renderer[i].material = m_material[(int)RayColor.Blue];
					}
				}
            }
            else
            {
                for (int i = 0; i < m_renderer.Length; ++i)
                {
                    m_renderer[i].enabled = false;
                }
            }
        }
	}

	public bool GetCanSet()
	{
		return m_renderer[0].enabled && m_materialNum == (int)RayColor.Blue;
	}

    private void FixedUpdate()
    {
        // ��
        m_OnTrigger = true;
    }

    private void OnTriggerStay(Collider other)
	{
		// ��
		m_OnTrigger = false;
	}

	private void OnTriggerExit(Collider other)
	{
		// ��
		m_OnTrigger = true;
	}

	public int GetMaterialNum()
	{
		return m_materialNum;
	}
}
