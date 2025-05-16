using UnityEngine;

public class PCPage : MonoBehaviour
{
	PlayerRay playerRay;
	const float RayDistance = 2;
	Transform hitParent;
	
	public enum Page
	{
		Start,
		Shop,
	}

	[SerializeField] Page page;
	// Start is called before the first frame update
	void Start()
    {
        playerRay = GameObject.FindWithTag("Player").GetComponent<PlayerRay>();
    }

    // Update is called once per frame
    void Update()
    {
		if (Cursor.visible == false) return;

        if (Input.GetMouseButtonDown(0))
		{
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out var hit))
			{
				// 当たったオブジェクトを入れる
				hitParent = hit.transform;
				while (true)
				{
					// オブジェクトの親がいるなら
					if (hitParent.transform.parent != null)
					{
						if (hitParent.TryGetComponent(out OrderPC pc))
						{
							switch (page)
							{
								case Page.Start:
									pc.OpenShop();
									break;

								case Page.Shop:
									if (hit.transform.TryGetComponent(out ShopButton shop))
									{
										shop.BuyFood(hit.transform.gameObject);
									}
									break;
							}
							break;
						}
						// 親を入れる
						hitParent = hitParent.parent;
					}
					else
					{
						playerRay.ReturnCamera();
						break;
					}
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			playerRay.ReturnCamera();
		}
    }
}
