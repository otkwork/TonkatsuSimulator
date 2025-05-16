using Unity.VisualScripting;
using UnityEngine;

public class Tonkatsu : MonoBehaviour
{
	[SerializeField] Material[] m_materials = new Material[4];  // マテリアル調理前調理後
	[SerializeField] float fryTime; // 揚げ時間[
	[SerializeField] AudioClip cutSound;
	// 前後のオーバーしてはいけない時間
	[SerializeField] float outTime = 30.0f;
	private GameObject particl;

	[SerializeField] MeshRenderer[] m_meshRenderer;
	int materialIndex;
	private float m_time = 0;   // 油に入っていた時間
	private bool m_cut = false;	// 切ったかどうか
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
		// 半分以上揚げたら
        if (m_time * 2 > fryTime)
        {
			// 揚げ時間よりも時間が短い場合1
			// 揚げ時間+outTiemよりも時間が長い場合3
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
		// 切ったときのサウンド
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
