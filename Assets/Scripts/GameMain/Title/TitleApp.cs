using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleApp : MonoBehaviour
{
	[SerializeField] TitlePC pc;
	private Material clickColor;
	public enum Application
	{
		Start,
		Option,
		Quit,
	}

	[SerializeField] Application app;

	const float DoubleClickTime = 0.2f; // �_�u���N���b�N�𔻒肷�鎞��
	const float ClickColor = 0.25f;		// �N���b�N�����Ƃ��ɐF����

	private float clickTime;    // ���ڃN���b�N���Ă���̎���
	private bool click;         // ���N���b�N����

	private bool onMouse;		// �}�E�X���̂��Ă��邩�ǂ���
	// Start is called before the first frame update
	void Start()
    {
		clickColor = GetComponent<MeshRenderer>().material;
		clickColor.color = Color.clear;
        click = false;
		clickTime = 0;
		onMouse = false;
    }

    // Update is called once per frame
    void Update()
    {
		// �}�E�X���̂��Ă���Ƃ�
        if (onMouse)
		{
			if (click)
			{
				// �����N���b�N���Ă���̉��Z����
				clickTime += Time.deltaTime;

				// �_�u���N���b�N����Ȃ��ƒʂ�Ȃ�
				if (clickTime < DoubleClickTime)
				{
					if (Input.GetMouseButtonDown(0))
					{
						click = false;
						clickTime = 0;
						pc.ChangeScene(app);
					}
				}
				else
				{
					click = false;
				}
			}

			if (Input.GetMouseButtonDown(0))
			{
				click = true;
				clickTime = 0;
				clickColor.color = Color.black * ClickColor;
			}
		}
		// �}�E�X������Ă��Ȃ��Ƃ�
		else
		{
			if (Input.GetMouseButtonDown (0))
			{
				clickColor.color = Color.clear;
			}
		}
    }

	// �}�E�X������Ă���Ƃ�
	private void OnMouseOver()
	{
		onMouse = true;
	}

	private void OnMouseExit()
	{
		onMouse = false;
	}
}
