using UnityEngine;

public class OrderPC : MonoBehaviour
{
	[SerializeField] GameObject cameraPos;
	[SerializeField] Material fade;
	[SerializeField] float fadeSpeed;
	[SerializeField] GameObject[] pcPage;
	BoxCollider boxCol;
	bool setCol = false;
	bool openShop = false;

	bool isFadeIn;	// trueならFadeIn falseならFadeOut

    // Start is called before the first frame update
    void Start()
    {
		isFadeIn = true;
		boxCol = GetComponent<BoxCollider>();
		fade.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
		if (setCol)
		{
			boxCol.enabled = true;
			setCol = false;
		}
		
		if (isFadeIn) FadeIn();
		else FadeOut();

		// ショップを開く
		if (openShop)
		{
			if (isFadeIn && FadeIn())
			{
				pcPage[(int)PCPage.Page.Start].SetActive(false);
				pcPage[(int)PCPage.Page.Shop].SetActive(true);
				isFadeIn = false;
			}
		}
		// ショップを閉じる
		else
		{
			if (isFadeIn && FadeIn())
			{
				pcPage[(int)PCPage.Page.Start].SetActive(true);
				pcPage[(int)PCPage.Page.Shop].SetActive(false);
			}
		}
    }

	public GameObject GetPos()
	{
		return cameraPos;
	}

	public void UsePC()
	{
		isFadeIn = false;
		boxCol.enabled = false;
	}

	public void ShutDownPC()
	{
		isFadeIn = true;
		setCol = true;
		openShop = false;
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

	private bool FadeIn()
	{
		fade.color += Color.black * Time.deltaTime * fadeSpeed;
		if (fade.color.a >= 1)
		{
			fade.color = Color.black;
			return true;
		}
		return false;
	}

	public void OpenShop()
	{
		isFadeIn = true;
		openShop = true;
	}
}
