using Cinemachine;
using UnityEngine;

public class FPSMove : MonoBehaviour
{
	[SerializeField] private Transform head;
	[SerializeField] private Transform body;
	[SerializeField] private float sensX = 5f;
	[SerializeField] private float sensY = 5f;
	private float rotationY, rotationX;

	[SerializeField] GameObject player;
	float playerSpeed;

	Rigidbody m_rigidbody;
	float inputHorizontal;
	float inputVertical;
	void Start()
	{
		m_rigidbody = GetComponent<Rigidbody>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		playerSpeed = 5.0f;
	}

	private void Update()
	{
		Look();

		inputHorizontal = Input.GetAxisRaw("Horizontal");
		inputVertical = Input.GetAxisRaw("Vertical");
	}

	private void FixedUpdate()
	{
		Walk();
	}

	// �}�E�X���_�ړ�
	private void Look()
	{
		Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X") * sensX,
			Input.GetAxis("Mouse Y") * sensY);

		rotationX -= mouseInput.y;
		rotationY += mouseInput.x;
		rotationY %= 360; // ��Βl���傫���Ȃ肷���Ȃ��悤��

		// �㉺�̎��_�ړ��ʂ�Clamp
		rotationX = Mathf.Clamp(rotationX, -90, 90);

		// ���A�̂̌����̓K�p
		head.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
		body.transform.localRotation = Quaternion.Euler(0, rotationY, 0);
	}

	// ����
	private void Walk()
	{
		playerSpeed = Input.GetKey(KeyCode.LeftShift) ? 10 : 5;

		// �J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
		Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

		// �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
		Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

		// �ړ������ɃX�s�[�h���|����B�W�����v�◎��������ꍇ�́A�ʓrY�������̑��x�x�N�g���𑫂��B
		m_rigidbody.velocity = moveForward * playerSpeed + new Vector3(0, m_rigidbody.velocity.y, 0);

		// �L�����N�^�[�̌�����i�s������
		if (moveForward != Vector3.zero)
		{
			transform.rotation = Quaternion.LookRotation(moveForward);
		}
	}
}