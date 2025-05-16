using System.Collections.Generic;
using UnityEngine;
using static FoodType;

public class PlayerRay : MonoBehaviour
{
	[SerializeField] StoreMoney m_storeMoney;	// 金
	[SerializeField] float rayDistance = 7; // Rayの長さ
	[SerializeField] GameObject head;		// 頭
	OrderPC m_orderPC;
	PlayerController m_playerCon;
	bool m_setCamera = false;
	float m_cameraPos = 0;

	[SerializeField] GameObject[] m_fryFood;// 手に持つカツ
	[SerializeField] GameObject m_dish;     // 手に持つ皿
	[SerializeField] GameObject m_cabbage;  // 手に持つキャベツ
	[SerializeField] GameObject m_cardBoardPos; // 段ボールを持つポジション
	private GameObject m_cardBoard;			// 手に持つ段ボール
	private const int NonefryFood = -1; // 何も持っていない
	private const int PlayerLayer = 3;  // プレイヤーのレイヤ
	private const int RayObjectLayer = 7;	// Rayのオブジェクトのレイヤー

	private int m_haveFoodNum;      // カツの種類
	private float m_haveFoodTime;   // 手に持っているカツの揚げた時間
	private bool m_cut = false;             // 切っているかどうか

	private int layerMask = 1 << PlayerLayer | 1 << RayObjectLayer;

	[SerializeField] GameObject[] m_rayObject;  // Rayが当たったところに出すオブジェクト
	List<RayObject> m_rayMesh = new List<RayObject>();
	Dish rayDish;
	// RayObjectの引数
	const int DishIndex = 5;
	const int CabbageIndex = 6;
	const int CardBoardIndex = 7;

	const float ObjectOffset = 0.05f; 
	const float CardBoardOffset = 0.2f; 
	// Use this for initialization
	void Start()
	{
		m_haveFoodNum = -1;
		m_haveFoodTime = 0;
		rayDish = m_rayObject[DishIndex].GetComponent<Dish>();
		m_playerCon = GetComponent<PlayerController>();
		for (int i = 0; i < m_rayObject.Length; ++i)
		{
			m_rayMesh.Add(m_rayObject[i].GetComponent<RayObject>());
		}
	}

	// Update is called once per frame
	void Update()
	{
		RaycastHit hit;
		// レイを飛ばす
		if (Physics.Raycast(head.transform.position, head.transform.forward, out hit, rayDistance, ~layerMask))
		{
			// Rayが当たった場所に持っているオブジェクトを表示
			SetRayObject(hit);

			// 左クリック
			if (Input.GetMouseButtonDown(0))
			{
				//========================================//
				// Hit

				// 手に何か持っている場合は通らない
				if (m_cardBoard == null && m_haveFoodNum == NonefryFood &&
					!m_dish.activeSelf && !m_cabbage.activeSelf)
				{
					if (HitCardBoard(hit))	return; // 段ボールにインタラクトした場合

					if (HitPC(hit))			return; // PCにインタラクトした場合

					if (HitKatsu(hit))		return; // カツにインタラクトした場合
				}

				// 段ボールを持っている場合は通らない
				if (m_cardBoard == null)
				{
					if (HitDish(hit))		return; // 皿にインタラクトした場合
				}

				// キャベツ以外を持っている場合通らない
				if (m_cardBoard == null && m_haveFoodNum == NonefryFood && !m_dish.activeSelf)
				{
					if (HitCabbage(hit))	return; // キャベツにインタラクトした場合
				}

				// カツ以外を持っている場合通らない
				if (m_cardBoard == null && !m_dish.activeSelf && !m_cabbage.activeSelf)
				{
					if (HitVat(hit))			return; // バットにインタラクトした場合
				
					if (HitCuttingBoard(hit))	return; // まな板にインタラクトしたとき
					
					if (HitFryer(hit))			return; // フライヤーの網にインタラクトした場合
				}

				// 皿以外を持っている場合通らない
				if (m_cardBoard == null && m_haveFoodNum == NonefryFood && !m_cabbage.activeSelf)
				{
					if (HitDishTower(hit))	return; // 皿の塔にインタラクトした場合
				}

				if (HitFryerButton(hit))	return; // フライヤーのボタンにインタラクトした場合

				if (HitTrashBox(hit))		return; // ゴミ箱にインタラクトした場合

				if (HitCabinet(hit))		return; // 棚にインタラクトした場合
				
				if (HitProvideButton(hit))	return; // 提供ボタンにインタラクトした場合


				//================================//
				// Have

				if (HaveDish(hit))		return; // 手に皿を持っている場合

				if (HaveCardBoard(hit))	return; // 手に段ボールを持っている場合
			}
			// 右クリック
			if (Input.GetMouseButtonDown(1))
			{
				if (OpenCardBoard(hit))	return; // 段ボールにインタラクトした場合

				if (CutKatsu(hit))		return; // カツにインタラクトした場合
			}
		}
		// Rayが当たっていない
		else if (!Cursor.visible)
		{
			for (int i = 0; i < m_rayObject.Length; ++i)
			{
				m_rayMesh[i].SetInvisible(false);
			}
		}

		if (m_setCamera) SetCamera();
	}

	private void SetCamera()
	{
		m_cameraPos += Time.deltaTime;
		Camera.main.transform.position =
					Vector3.Lerp(Camera.main.transform.position, m_orderPC.GetPos().transform.position, m_cameraPos);
		Camera.main.transform.rotation = m_orderPC.GetPos().transform.rotation;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		if (m_cameraPos >= 1) m_setCamera = false;
	}

	public void ReturnCamera()
	{
		if (m_orderPC == null) return;
		m_setCamera = false;	// カメラを戻す

		// ポジション向きを直す
		Camera.main.transform.localPosition = new Vector3(0, 0, 0);
		Camera.main.transform.localRotation = Quaternion.Euler(0, 0, 0);

		// 移動を戻す
		m_playerCon.enabled = true;

		// カーソルを消す
		Cursor.visible = false;　
		Cursor.lockState = CursorLockMode.Locked;

		// PCを閉じる
		m_orderPC.ShutDownPC();
	}

	private void SetRayObject(RaycastHit hit)
	{
		for (int i = 0; i < m_rayObject.Length; ++i)
		{
			m_rayMesh[i].SetInvisible(false);
		}
		// レイが当たった場所に置く予定のアイテムを表示
		// 皿を持っている
		if (m_dish.activeSelf)
		{
			if (hit.collider.CompareTag("SetItemPos") || hit.collider.CompareTag("Provide"))
			{
				m_rayMesh[DishIndex].SetInvisible(true, true);
				m_rayObject[DishIndex].transform.rotation = Quaternion.Euler(0, m_dish.transform.eulerAngles.y, 0);
				m_rayObject[DishIndex].transform.position = hit.point + new Vector3(0, ObjectOffset, 0);
				// カツが乗っている
				if (m_haveFoodNum != NonefryFood)
				{
					m_rayMesh[m_haveFoodNum].SetInvisible(true);
					m_rayObject[m_haveFoodNum].transform.rotation = Quaternion.Euler(0, m_fryFood[m_haveFoodNum].transform.eulerAngles.y, 0);
					m_rayObject[m_haveFoodNum].transform.GetChild(0).gameObject.SetActive(!m_cut);
					m_rayObject[m_haveFoodNum].transform.GetChild(1).gameObject.SetActive(m_cut);
					// カツ(true)の置くポジションを取得
					if (m_dish.activeSelf) m_rayObject[m_haveFoodNum].transform.position = rayDish.GetRayPos(true).position;
				}
				// キャベツが乗っている
				if (m_cabbage.activeSelf)
				{
					m_rayMesh[CabbageIndex].SetInvisible(true);
					m_rayObject[CabbageIndex].transform.rotation = Quaternion.Euler(0, m_cabbage.transform.eulerAngles.y, 0);
					// キャベツ(false)の置くポジションを取得
					if (m_dish.activeSelf) m_rayObject[CabbageIndex].transform.position = rayDish.GetRayPos(false).position;
				}
			}
		}
		// 皿を持っていなくてカツを持っている
		else if (m_haveFoodNum != NonefryFood)
		{
			m_rayObject[m_haveFoodNum].transform.GetChild(0).gameObject.SetActive(!m_cut);
			m_rayObject[m_haveFoodNum].transform.GetChild(1).gameObject.SetActive(m_cut);
			// Rayが当たったいるのがまな板
			if (hit.collider.CompareTag("CuttingBoard"))
			{
				m_rayMesh[m_haveFoodNum].SetInvisible(true, true);
				m_rayObject[m_haveFoodNum].transform.rotation = Quaternion.Euler(0, 0, 0);
				m_rayObject[m_haveFoodNum].transform.position = hit.point + new Vector3(0, ObjectOffset, 0);
			}
			// Rayが当たっているのがフライヤー
			else if (hit.collider.CompareTag("FryerPos"))
			{
				if (m_cut) return;
				m_rayMesh[m_haveFoodNum].SetInvisible(true);
				m_rayObject[m_haveFoodNum].transform.rotation = Quaternion.Euler(hit.transform.eulerAngles);
				m_rayObject[m_haveFoodNum].transform.position = hit.transform.position;
			}
			// Rayが当たっているのが皿
			else if (hit.collider.TryGetComponent(out Dish dish))
			{
				m_rayMesh[m_haveFoodNum].SetInvisible(dish.ActiveRayObject(true));
				m_rayObject[m_haveFoodNum].transform.rotation = Quaternion.Euler(dish.GetRayPos(true).eulerAngles);
				m_rayObject[m_haveFoodNum].transform.position = dish.GetRayPos(true).position;
			}
			// Rayが当たっているのがバット
			else if (hit.collider.TryGetComponent(out Vat vat))
			{
				if (m_cut) return;  // 切っていたらreturn
				if (m_fryFood[m_haveFoodNum].GetComponent<Tonkatsu>().GetFryTime() > 0) return; // 少しでも揚げていたらreturn
				if (m_fryFood[m_haveFoodNum].GetComponent<FoodType>().Type != vat.GetFoodType()) return;	// バットの種類と持っているカツが違うなら
				GameObject createPos = vat.GetPos();
				if (createPos != null)
				{
					m_rayMesh[m_haveFoodNum].SetInvisible(true);
					m_rayObject[m_haveFoodNum].transform.rotation = Quaternion.Euler(createPos.transform.eulerAngles);
					m_rayObject[m_haveFoodNum].transform.position = createPos.transform.position;
				}
			}
		}
		// 皿もカツも持っていなくてキャベツを持っている
		else if (m_cabbage.activeSelf)
		{
			if (hit.collider.TryGetComponent(out Dish dish))
			{
				m_rayMesh[CabbageIndex].SetInvisible(dish.ActiveRayObject(false));
				m_rayObject[CabbageIndex].transform.rotation = Quaternion.Euler(0, m_cabbage.transform.eulerAngles.y, 0);
				// キャベツ(false)の置くポジションを取得
				m_rayObject[CabbageIndex].transform.position = dish.GetRayPos(false).position;
			}
		}
		// 手に段ボールを持っているとき
		else if (m_cardBoard != null)
		{
			// 床に当たっているとき
			if (hit.collider.CompareTag("Floor"))
			{
				// 指定した半径にはでない
				if (CirCleSet(hit.point, transform, 0.7f)) return;

				m_rayMesh[CardBoardIndex].SetInvisible(true, true);
				// Rayのポジションに設置
				m_rayObject[CardBoardIndex].transform.position = hit.point + new Vector3(0, CardBoardOffset, 0);
				m_rayObject[CardBoardIndex].transform.rotation = Quaternion.Euler(0, m_cardBoard.transform.eulerAngles.y, 0);
			}
		}
	}

	private bool HitFryerButton(RaycastHit hit)
	{
		if (hit.collider.TryGetComponent(out FryButtonType fryButton))
		{
			// フライヤー本体に揚げ時間と左右のどっちかを送る
			hit.transform.transform.gameObject.GetComponent<FryButtonType>().
				PutButton((int)fryButton.Type);

			return true;
		}
		return false;
	}

	private bool HitVat(RaycastHit hit)
	{
		if (hit.transform.TryGetComponent(out Vat vat))
		{			
			// 持っているカツとバットの種類が一緒の場合
			if ((FoodType.Food)m_haveFoodNum == vat.GetFoodType())
			{
				if (m_cut) return true;  // 切っていたらreturn
				if (m_fryFood[m_haveFoodNum].GetComponent<Tonkatsu>().GetFryTime() > 0) return true; // 少しでも揚げていたらreturn
				
				// バットのなかが空いていたら
				if (vat.SetKatsu(m_fryFood[m_haveFoodNum]))
				{
					m_fryFood[m_haveFoodNum].SetActive(false);
					m_haveFoodNum = NonefryFood;
				}
			}
			// 手持ちに何もなかった場合
			else if (m_haveFoodNum == NonefryFood)
			{
				// バットに一つでもカツがあったら
				m_haveFoodNum = (int)vat.GetKatsu();
				if (m_haveFoodNum != NonefryFood)
				{
					m_fryFood[m_haveFoodNum].TryGetComponent(out Tonkatsu katsu);
					m_haveFoodTime = 0;
					m_cut = false;
					katsu.SetTime(m_haveFoodTime);
					katsu.CutKatsu(m_cut);
					m_fryFood[m_haveFoodNum].SetActive(true);
				}
			}
			return true;
		}
		return false;
	}

	private bool HitKatsu(RaycastHit hit)
	{
		if (hit.collider.TryGetComponent(out FoodType type))
		{
			// 皿に乗っているなら全部の情報を消す
			if (hit.transform.parent != null && hit.transform.parent.TryGetComponent(out Dish hitdish))
			{
				hitdish.SetFood(null);
				hitdish.SetTime(0);
				hitdish.SetType(NonefryFood);
			}
			hit.collider.TryGetComponent(out Tonkatsu hitKatsu);
			m_haveFoodTime = hitKatsu.GetFryTime();

			// 一度持っているオブジェクトを全部消す
			for (int i = 0; i < m_fryFood.Length; ++i) m_fryFood[i].SetActive(false);
			m_haveFoodNum = (int)type.Type;
			m_fryFood[m_haveFoodNum].SetActive(true);
			// 取ったカツと手に持つカツの揚げ時間を一緒にする
			m_fryFood[m_haveFoodNum].TryGetComponent(out Tonkatsu myKatsu);
			myKatsu.SetTime(m_haveFoodTime);
			// 切っていたら手持ちも切る
			m_cut = hitKatsu.IsCutKatsu();
			myKatsu.CutKatsu(m_cut);

			Destroy(hit.transform.gameObject);
			return true;
		}
		return false;
	}

	private bool HitFryer(RaycastHit hit)
	{
		if (hit.transform.CompareTag("FryerPos") && !m_cut)
		{
			if (m_haveFoodNum == NonefryFood) return true;   // 食べ物を持っていなければreturn
			
			// フライヤーに入っているときはreturn
			if (!hit.transform.parent.parent.GetComponent<FryerNet>().
				IsEmptySet(hit, m_haveFoodNum, m_fryFood[m_haveFoodNum], m_haveFoodTime, m_cut)) return true;

			// 持っているオブジェクトを消す
			m_fryFood[m_haveFoodNum].gameObject.SetActive(false);
			m_haveFoodNum = NonefryFood;   // 内部上の所持アイテムをなくす
			return true;
		}
		return false;
	}

	private bool HitDishTower(RaycastHit hit)
	{
		if (hit.collider.CompareTag("Dish"))
		{
			m_dish.SetActive(!m_dish.activeSelf);
			return true;
		}
		return false;
	}

	private bool HaveDish(RaycastHit hit)
	{
		if (m_dish.activeSelf)
		{
			// 机の上か提供台以外ならreturn
			if (!hit.collider.CompareTag("SetItemPos") && !hit.collider.CompareTag("Provide")) return true;
			// 設置予定が出ていなければreturn
			if (!m_rayMesh[DishIndex].GetCanSet()) return true;

			// 皿を置く
			GameObject setDish = Instantiate(m_dish, m_rayObject[DishIndex].transform.position, Quaternion.Euler(0, m_dish.transform.eulerAngles.y, 0));
			setDish.GetComponent<Dish>().SetType(m_haveFoodNum);
			m_dish.SetActive(false);

			// 手にカツを持っている場合カツも置く
			if (m_haveFoodNum != NonefryFood)
			{
				setDish.GetComponent<Dish>().SetKatsu(setDish, m_haveFoodNum, m_fryFood[m_haveFoodNum], m_haveFoodTime, m_cut);

				// 持っているオブジェクトを消す
				m_fryFood[m_haveFoodNum].gameObject.SetActive(false);
				m_haveFoodNum = NonefryFood;   // 内部上の所持アイテムをなくす
			}
			// 手にキャベツを持っている場合キャベツも置く
			if (m_cabbage.activeSelf)
			{
				if (setDish.GetComponent<Dish>().SetCabbage(setDish.transform.gameObject, m_cabbage))
				{
					m_cabbage.SetActive(false);
				}
			}
			// 提供台だった場合
			if (hit.collider.TryGetComponent(out ProvideTable table))
			{
				setDish.transform.SetParent(hit.collider.transform);
				table.SetProvide();
			}
			return true;
		}
		return false;
	}

	private bool HitDish(RaycastHit hit)
	{
		if (hit.collider.TryGetComponent(out Dish dish))
		{
			// 手に何も持っていない場合皿をとる
			if (m_haveFoodNum == NonefryFood && !m_cabbage.activeSelf && !m_dish.activeSelf)
			{
				m_dish.SetActive(true);
				// 皿にカツがのっている場合
				if (dish.Type != Food.None)
				{
					m_haveFoodNum = (int)dish.Type;      // カツの種類を取得
					m_fryFood[m_haveFoodNum].SetActive(true);   // カツを手に表示

					m_fryFood[m_haveFoodNum].TryGetComponent(out Tonkatsu myKatsu);
					// 取ったカツの揚げ時間をセット
					m_haveFoodTime = dish.Time;
					myKatsu.SetTime(m_haveFoodTime);
					// 切っていたら皿のカツも切る
					m_cut = dish.IsCutKatsu();
					myKatsu.CutKatsu(m_cut);
				}
				// キャベツがのっているかどうか
				if (dish.OnCabbage())
				{
					m_cabbage.SetActive(true);
				}

				// 皿が提供台においてあった場合
				if (hit.transform.parent != null && hit.transform.parent.TryGetComponent(out ProvideTable table))
				{
					table.TakeProvide();
				}
				Destroy(hit.transform.gameObject);  // 取ったオブジェクトを消す
			}
			// 手にカツを持っている
			else if (m_haveFoodNum != NonefryFood)
			{
				// 皿の上にカツを生成(type,time,parent)全て
				if (!dish.SetKatsu(hit.transform.gameObject, m_haveFoodNum, m_fryFood[m_haveFoodNum], m_haveFoodTime, m_cut)) return true;

				// 手に持っているカツを消す
				m_fryFood[m_haveFoodNum].SetActive(false);

				m_haveFoodNum = NonefryFood;// 内部上の所持アイテムをなくす
			}
			// キャベツを持っている
			else if (m_cabbage.activeSelf)
			{
				// 皿にキャベツがのっていなかったら乗せる
				if (dish.SetCabbage(hit.transform.gameObject, m_cabbage))
				{
					m_cabbage.SetActive(false);
				}
			}

			return true;
		}
		return false;
	}

	private bool HitCabbage(RaycastHit hit)
	{
		if (hit.collider.CompareTag("Cabbage"))
		{			
			// 皿に乗ったキャベツなら
			if (hit.transform.parent.TryGetComponent(out Dish hitDish))
			{
				if (m_cabbage.activeSelf) return true;	// 手にキャベツを持っている場合return

				m_cabbage.SetActive(true);
				Destroy(hit.transform.gameObject);
				return true;
			}

			// キャベツ持っていない場合取る　キャベツを持っている場合キャベツを消す
			m_cabbage.SetActive(!m_cabbage.activeSelf);

			return true;
		}
		return false;
	}

	private bool HitCuttingBoard(RaycastHit hit)
	{
		if (hit.collider.CompareTag("CuttingBoard"))
		{
			// 設置予定が出ていなければreturn
			if (m_haveFoodNum == NonefryFood || !m_rayMesh[m_haveFoodNum].GetCanSet()) return true;
			// 生成
			GameObject goKatsu = Instantiate(m_fryFood[m_haveFoodNum], hit.point + new Vector3(0, 0.05f, 0), Quaternion.identity);
			goKatsu.transform.SetParent(hit.transform);
			goKatsu.TryGetComponent(out Tonkatsu katsu);

			// 情報を設定
			katsu.SetTime(m_haveFoodTime);
			katsu.CutKatsu(m_cut);
			m_fryFood[m_haveFoodNum].SetActive(false);
			m_haveFoodNum = NonefryFood;

			return true;
		}
		return false;
	}

	private bool HitTrashBox(RaycastHit hit)
	{
		if (hit.transform.CompareTag("TrashBox"))
		{
			// かつをもっている　
			if (m_haveFoodNum != NonefryFood)
			{
				m_fryFood[m_haveFoodNum].SetActive(false);
				m_cut = false;
				m_haveFoodTime = NonefryFood;
				m_haveFoodNum = NonefryFood;
			}
			// キャベツを持っている
			if (m_cabbage.activeSelf)
			{
				m_cabbage.SetActive(false);
			}
			// 段ボールを持っている
			if (m_cardBoard	!= null)
			{
				Destroy(m_cardBoard);
				m_cardBoard = null;
			}
			return true;
		}
		return false;
	}

	private bool HitCabinet(RaycastHit hit)
	{
		if (hit.collider.TryGetComponent(out Cabinet cabinet))
		{
			cabinet.Drawer();
			return true;
		}
		return false;
	}

	private bool HitProvideButton(RaycastHit hit)
	{
		if (hit.collider.TryGetComponent(out ProvideButton button))
		{
			if (!hit.transform.parent.TryGetComponent(out ProvidePanel panel)) return true;   // パネル
			GameObject provide = panel.Provide(button.GetFood, button.GetFry, button.GetCabbage, button.GetCut, button.GetTimeUp);

			// 提供完了
			if (provide != null)
			{
				// 注文の前詰め
				panel.DecNum(hit.transform.gameObject);

				// 売上加算
				m_storeMoney.ProductSales(button.GetFood);

				// 指定の注文と提供物を削除
				Destroy(hit.transform.gameObject);
				Destroy(provide);
			}
			return true;
		}
		return false;
	}

	private bool HitCardBoard(RaycastHit hit)
	{
		if (hit.collider.TryGetComponent(out CardBoard cardBoard))
		{
			// 手持ちに置く
			m_cardBoard = hit.transform.gameObject;
			m_cardBoard.transform.position = m_cardBoardPos.transform.position;
			m_cardBoard.transform.rotation = m_cardBoardPos.transform.rotation;

			// 棚に並んだ段ボールなら
			if (m_cardBoard.transform.parent != null &&
				m_cardBoard.transform.parent.parent != null && 
				m_cardBoard.transform.parent.parent.TryGetComponent(out CardBoardShelf shelf))
			{
				// 棚から取る
				shelf.GetCardBoard(m_cardBoard);
			}
			m_cardBoard.transform.SetParent(Camera.main.transform);
			m_cardBoard.GetComponent<CardBoard>().SetCollider(false);

			return true;
		}
		return false;
	}

	private bool HaveCardBoard(RaycastHit hit)
	{
		// 手に段ボールを持っている
		if (m_cardBoard != null)
		{
			// 床以外には置かない
			if (!hit.transform.CompareTag("Floor")) return true;
			// 設置予定が出ていなければreturn
			if (!m_rayMesh[CardBoardIndex].GetCanSet()) return true;

			// 円形範囲内の判定
			if (CirCleSet(hit.point, transform, 0.7f)) return true;

			// カメラを親じゃなくする
			m_cardBoard.transform.parent = null;
			// Rayのポジションに設置
			m_cardBoard.transform.position = hit.point + new Vector3(0, CardBoardOffset, 0);
			m_cardBoard.transform.rotation = Quaternion.Euler(0, m_cardBoard.transform.eulerAngles.y, 0);
			m_cardBoard.GetComponent<CardBoard>().SetCollider(true);
			m_cardBoard = null;
			return true;
		}
		return false;
	}
	// 円形範囲内におけるかどうか(現状段ボールのみ)
	private bool CirCleSet(Vector3 hit, Transform center, float radius)
	{
		// 半径Xの内側かどうか
		if ((hit.x - center.position.x) * (hit.x - center.position.x) +
			(hit.z - center.position.z) * (hit.z - center.position.z) <
			radius * radius) return true;

		return false;
	}

	private bool HitPC(RaycastHit hit)
	{
		if (!m_setCamera && hit.collider.TryGetComponent(out OrderPC pc))
		{
			m_setCamera = true; // カメラの位置をセット
			m_orderPC = pc;
			m_playerCon.enabled = false; // 移動を制限
			
			pc.UsePC(); // パソコンをつける
			m_cameraPos = 0;
			return true;
		}
		return false;
	}

	private bool OpenCardBoard(RaycastHit hit)
	{
		if (hit.collider.TryGetComponent(out CardBoard cardBoard))
		{
			// 段ボールを開ける
			cardBoard.Open();
			return true;
		}
		return false;
	}

	private bool CutKatsu(RaycastHit hit)
	{
		if (hit.collider.TryGetComponent(out FoodType type) && hit.transform.parent != null &&
					hit.transform.parent.CompareTag("CuttingBoard"))
		{
			// ヒレか唐揚げならreturn
			if (type.Type == Food.Hire || type.Type == Food.Karaage) return true;
			hit.collider.TryGetComponent(out Tonkatsu katsu);
			katsu.CutKatsu(true, true);
			return true;
		}
		return false;
	}
}
