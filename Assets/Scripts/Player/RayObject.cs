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
		// Ray.pointを起点に設置
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
		// 当たったオブジェクトが持っているポジション
		// 皿のカツ、キャベツとバット
		else
		{
            if (active)
            {
                for (int i = 0; i < m_renderer.Length; ++i)
                {
					// 表示
                    m_renderer[i].enabled = true;

					// 皿に乗っている場合
					if (rayDish.enabled && rayDish.TryGetComponent(out RayObject ray))
					{
						// カツとキャベツの表示を皿と同じ色にする
						m_materialNum = ray.GetMaterialNum();
						m_renderer[i].material = m_material[m_materialNum];
					}
					else
					{
						// 常に青
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
        // 青
        m_OnTrigger = true;
    }

    private void OnTriggerStay(Collider other)
	{
		// 赤
		m_OnTrigger = false;
	}

	private void OnTriggerExit(Collider other)
	{
		// 青
		m_OnTrigger = true;
	}

	public int GetMaterialNum()
	{
		return m_materialNum;
	}
}
