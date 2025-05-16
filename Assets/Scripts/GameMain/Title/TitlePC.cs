using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitlePC : MonoBehaviour
{
	[SerializeField] Material fade;		// フェードするオブジェクトのマテリアル
	[SerializeField] float fadeSpeed;	// フェードの速度

	private float m_elapsedTime;    // PC起動してからの時間

	private bool isFadeIn;  // trueならFadeIn falseならFadeOut
	private bool changeGame;    // ゲームシーンに遷移

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
			// 暗くする
			if (FadeIn())
			{
				if (changeGame) SceneManager.LoadScene("Game");
				// 真っ暗にした後に引数分の時間待つ
				LoadingTime(1f);
			}
		}
		else
		{

			// 明るくする
			FadeOut();
		}		
	}

	// アプリ的なのを開いたとき
	public void ChangeScene(TitleApp.Application app)
	{
		switch (app)
		{
			// ゲームスタート
			case TitleApp.Application.Start:
				isFadeIn = true;
				changeGame = true;
				break;

			// 終了
			case TitleApp.Application.Quit:
				EndGame();
				break;
		}
	}

	// ロード
	private bool LoadingTime(float loadTime)
	{
		// ロード時間
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

	//ゲーム終了
	private void EndGame()
	{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
	}
}
