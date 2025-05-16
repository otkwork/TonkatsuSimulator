using UnityEngine;

public class CardBoard : MonoBehaviour
{
	[SerializeField] Transform[] setPos;
	// Start is called before the first frame update
	Animator m_anim;
	private bool open = false;

	[SerializeField] BoxCollider coverBoxCol;
	[SerializeField] BoxCollider[] aroundBoxCol;

	private void Start()
	{
		m_anim = GetComponent<Animator>();
	}

	public bool Open()
	{
		if (!open)
		{
			m_anim.SetBool("Open", true);
			open = true;
			coverBoxCol.enabled = false;
			return true;
		}
		else
		{
			return false;
		}
	}

	public void SetCollider(bool Active)
	{
		// �i�{�[���̂ӂ��ȊO�̃R���C�_�[
		for (int i = 0; i < aroundBoxCol.Length; i++)
		{
			aroundBoxCol[i].enabled = Active;
		}

		// �ӂ��̕����̃R���C�_�[
		if(Active)
		{
			coverBoxCol.enabled = !open;
		}
		else
		{
			coverBoxCol.enabled = false;
		}
	}

	public Transform GetPosition(int index) { return setPos[index].transform; }
}
