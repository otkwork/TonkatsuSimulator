using UnityEngine;

public class DragObject : MonoBehaviour
{
	private Vector3 mOffset;
	private float mZCoord;
	Rigidbody m_rigidbody;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
	{
		// ‚Â‚©‚ñ‚¾Žž‚Ì‹——£
		mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
		mOffset = gameObject.transform.position - GetMouseWorldPos();
        m_rigidbody.useGravity = false;
        m_rigidbody.isKinematic = true;
	}

	private void OnMouseUp()
	{
        m_rigidbody.useGravity = true;
        m_rigidbody.isKinematic = false;
	}

	private Vector3 GetMouseWorldPos()
	{
		Vector3 mousePoint = Input.mousePosition;

		Ray ray = Camera.main.ScreenPointToRay(mousePoint);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, 100.0f, ~(1 << 6 | 1 << 3)))
		{
			mousePoint.z = mZCoord < hit.distance ? mZCoord : hit.distance - 0.1f;
//			Debug.Log(hit.point);
		}

//		mousePoint.z = mZCoord;

		return Camera.main.ScreenToWorldPoint(mousePoint);
	}

	void OnMouseDrag()
	{
		transform.position = GetMouseWorldPos() + mOffset;
	}
}