using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
	[SerializeField] AudioClip[] musicClip;
	AudioSource musicSource;

	// 今流している音楽の引数
	private int musicIndex;
	// 音楽の流れている時間
	private float musicDelta;

	// 次の曲に入るまでの時間
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
		
		// 音楽が終わったら
		if (musicDelta >= musicClip[musicIndex].length + fadeTime)
		{
			musicIndex++;
			// 最後の曲が終わったら最初に戻る
			if (musicIndex >= musicClip.Length)　musicIndex = 0;
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
