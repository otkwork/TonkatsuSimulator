using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
	[SerializeField] AudioClip[] musicClip;
	AudioSource musicSource;

	// �������Ă��鉹�y�̈���
	private int musicIndex;
	// ���y�̗���Ă��鎞��
	private float musicDelta;

	// ���̋Ȃɓ���܂ł̎���
	private const float fadeTime = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
		musicIndex = 0;
		musicSource = GetComponent<AudioSource>();
		musicDelta = 0;
		Music();
    }

    // Update is called once per frame
    void Update()
    {
		musicDelta += Time.deltaTime;
		
		// ���y���I�������
		if (musicDelta >= musicClip[musicIndex].length + fadeTime)
		{
			musicIndex++;
			// �Ō�̋Ȃ��I�������ŏ��ɖ߂�
			if (musicIndex >= musicClip.Length)�@musicIndex = 0;
			musicDelta = 0;
			Music();
		}
    }

	private void Music()
	{
		musicSource.clip = musicClip[musicIndex];
		musicSource.Play();
	}
}
