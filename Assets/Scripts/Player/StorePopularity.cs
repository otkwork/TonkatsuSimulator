using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StorePopularity : MonoBehaviour
{
	[SerializeField] Image popularity;
	private float popularityScale;
	const float UpPopNum = 0.01f;
	const float DownPopNum = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
		popularityScale = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        popularity.fillAmount = popularityScale;
		popularity.color = Color.Lerp(Color.black, Color.yellow, popularityScale);
    }

	public void DownPop()
	{
		popularityScale -= DownPopNum;
	}

	public void UpPop()
	{
		popularityScale += UpPopNum;
	}
}
