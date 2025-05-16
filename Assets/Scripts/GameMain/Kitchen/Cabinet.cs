using UnityEngine;

public class Cabinet : MonoBehaviour
{
	private const float OpenSpeed = 0.02f;
	private Vector3 m_openPos = new Vector3(0, 0, 0.4f);    // キャビネットのポジション
	private bool m_isOpen = false;

	private Vector3 m_pos = new Vector3(0, 0, 0);
    // Update is called once per frame
    void FixedUpdate()
    {
		transform.localPosition = m_pos;

		if(m_isOpen)
		{
			if (m_pos.z <= m_openPos.z) m_pos.z += OpenSpeed;
		}
		else
		{
            if (m_pos.z >= Vector3.zero.z) m_pos.z -= OpenSpeed;
        }
    }

	public void Drawer()
	{
		m_isOpen = !m_isOpen;
	}
}
