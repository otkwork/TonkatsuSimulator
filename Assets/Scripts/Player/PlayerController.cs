using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private CharacterController characterController;  // CharacterController�^�̕ϐ�
	private Vector3 moveVelocity;  // �L�����N�^�[�R���g���[���[�𓮂������߂�Vector3�^�̕ϐ�
	[SerializeField] private Animator animator;
	[SerializeField] private Transform verRot;  //�c�̎��_�ړ��̕ϐ�(�J�����ɍ��킹��)
	[SerializeField] private Transform horRot;  //���̎��_�ړ��̕ϐ�(�v���C���[�ɍ��킹��)
	[SerializeField] private float moveSpeed;  //�ړ����x
	[SerializeField] private float sensX = 2f;
	[SerializeField] private float sensY = 2f;
	private float rotationY, rotationX;

	[SerializeField] private float jumpPower;  //�W�����v��

	void Start()
	{
		characterController = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		// �Q�[���̏I��
		EndGame();

		Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X") * sensX,
			Input.GetAxis("Mouse Y") * sensY);

		rotationX -= mouseInput.y;
		rotationY += mouseInput.x;
		rotationY %= 360; // ��Βl���傫���Ȃ肷���Ȃ��悤��

		// �㉺�̎��_�ړ��ʂ�Clamp
		rotationX = Mathf.Clamp(rotationX, -90, 90);

		// ���A�̂̌����̓K�p
		verRot.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
		horRot.transform.localRotation = Quaternion.Euler(0, rotationY, 0);

		//W�L�[�������ꂽ��
		if (Input.GetKey(KeyCode.W))
		{
			characterController.Move(this.gameObject.transform.forward * moveSpeed * Time.deltaTime);
		}
		//S�L�[�������ꂽ��
		if (Input.GetKey(KeyCode.S))
		{
			characterController.Move(this.gameObject.transform.forward * -1f * moveSpeed * Time.deltaTime);
		}
		//A�L�[�������ꂽ��
		if (Input.GetKey(KeyCode.A))
		{
			characterController.Move(this.gameObject.transform.right * -1 * moveSpeed * Time.deltaTime);
		}
		//D�L�[�������ꂽ��
		if (Input.GetKey(KeyCode.D))
		{
			characterController.Move(this.gameObject.transform.right * moveSpeed * Time.deltaTime);
		}

		// �ڒn���Ă���Ƃ�
		if (characterController.isGrounded)
		{
			// �W�����v
			if (Input.GetKeyDown(KeyCode.Space))
			{
				moveVelocity.y = jumpPower;
			}
		}
		// �󒆂ɂ��鎞
		else
		{
			// �d�͂�������
			moveVelocity.y += Physics.gravity.y * Time.deltaTime;
		}

		// �L�����N�^�[�𓮂���
		characterController.Move(moveVelocity * Time.deltaTime);

		// �ړ��X�s�[�h���A�j���[�^�[�ɔ��f����
		//animator.SetFloat("MoveSpeed", new Vector3(moveVelocity.x, 0, moveVelocity.z).magnitude);
	}

	//�Q�[���I��
	private void EndGame()
	{
		//Esc�������ꂽ��
		if (Input.GetKey(KeyCode.Escape))
		{

#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
		}

	}
}