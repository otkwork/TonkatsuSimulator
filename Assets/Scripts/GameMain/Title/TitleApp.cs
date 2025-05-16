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

	const float DoubleClickTime = 0.2f; // ダブルクリックを判定する時間
	const float ClickColor = 0.25f;		// クリックしたときに色がつく

	private float clickTime;    // 一回目クリックしてからの時間
	private bool click;         // 一回クリックした

	private bool onMouse;		// マウスがのっているかどうか
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
		// マウスがのっているとき
        if (onMouse)
		{
			if (click)
			{
				// ワンクリックしてからの加算時間
				clickTime += Time.deltaTime;

				// ダブルクリックじゃないと通らない
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
		// マウスが乗っていないとき
		else
		{
			if (Input.GetMouseButtonDown (0))
			{
				clickColor.color = Color.clear;
			}
		}
    }

	// マウスが乗っているとき
	private void OnMouseOver()
	{
		onMouse = true;
	}

	private void OnMouseExit()
	{
		onMouse = false;
	}
}
