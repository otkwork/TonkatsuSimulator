using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitlePC : MonoBehaviour
{
	[SerializeField] Material fade;		// �t�F�[�h����I�u�W�F�N�g�̃}�e���A��
	[SerializeField] float fadeSpeed;	// �t�F�[�h�̑��x

	private float m_elapsedTime;    // PC�N�����Ă���̎���

	private bool isFadeIn;  // true�Ȃ�FadeIn false�Ȃ�FadeOut
	private bool changeGame;    // �Q�[���V�[���ɑJ��

	// Start is called before the first frame update
	void Start()
	{
		m_elapsedTime = 0;
		isFadeIn = true;
		changeGame = false;
		fade.color = Color.black;
	}

	// Update is called once per frame
	void Update()
	{
		if (isFadeIn)
		{
			// �Â�����
			if (FadeIn())
			{
				if (changeGame) SceneManager.LoadScene("Game");
				// �^���Âɂ�����Ɉ������̎��ԑ҂�
				LoadingTime(1f);
			}
		}
		else
		{

			// ���邭����
			FadeOut();
		}		
	}

	// �A�v���I�Ȃ̂��J�����Ƃ�
	public void ChangeScene(TitleApp.Application app)
	{
		switch (app)
		{
			// �Q�[���X�^�[�g
			case TitleApp.Application.Start:
				isFadeIn = true;
				changeGame = true;
				break;

			// �I��
			case TitleApp.Application.Quit:
				EndGame();
				break;
		}
	}

	// ���[�h
	private bool LoadingTime(float loadTime)
	{
		// ���[�h����
		m_elapsedTime += Time.deltaTime;
		if (m_elapsedTime > loadTime)
		{
			m_elapsedTime = 0;
			isFadeIn = false;
			return true;
		}
		return false;
	}

	private bool FadeOut()
	{
		fade.color -= Color.black * Time.deltaTime * fadeSpeed;
		if (fade.color.a <= 0)
		{
			fade.color = Color.clear;
			return true;
		}
		return false;
	}

	private bool FadeIn(bool option = false)
	{
		fade.color += Color.black * Time.deltaTime * fadeSpeed;
		if (fade.color.a >= 1)
		{
			fade.color = Color.black;
			return true;
		}
		return false;
	}

	//�Q�[���I��
	private void EndGame()
	{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
	}
}
