using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;                 // �̵� �ӵ�
    private float originalMoveSpeed;        // ������ �̵� �ӵ� ����
    private Vector2 curMovementInput;       // ���� �̵� �Է�
    public float jumptForce;                // ���� ��
    public LayerMask groundLayerMask;       // �ٴ� ���̾� ����ũ

    [Header("Look")]
    public Transform cameraContainer;       // ī�޶� �����̳�
    public float minXLook;                  // �ּ� ī�޶� ���� ����
    public float maxXLook;                  // �ִ� ī�޶� ���� ����
    private float camCurXRot;               // ���� ī�޶� X ȸ����
    public float lookSensitivity;           // �ü� ����

    private Vector2 mouseDelta;             // ���콺 �Է� ��

    [HideInInspector]
    public bool canLook = true;             // �ü� ���� ���� ����

    public Action inventory;                // �κ��丮 �׼�
    public Action craft;
    private Rigidbody rigidbody;            // ������ٵ�

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        originalMoveSpeed = moveSpeed;      // ������ �̵� �ӵ� ����
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move(); // �̵� �Լ� ȣ��
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook(); // �ü� ���� �Լ� ȣ��
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>(); // ���콺 �Է°� ��������
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>(); // �̵� �Է°� ��������
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero; // �̵� �Է°� �ʱ�ȭ
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rigidbody.AddForce(Vector2.up * jumptForce, ForceMode.Impulse); // ���� �� ����
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x; // �̵� ���� ���
        dir *= moveSpeed; // �̵� �ӵ� ����
        dir.y = rigidbody.velocity.y;

        rigidbody.velocity = dir; // ������ٵ� �ӵ� ����
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity; // ���콺 Y �Է¿� ���� ī�޶� X ȸ���� ����
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); // �ּ�, �ִ� ȸ���� ���̷� ����
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); // ī�޶� ���� ����

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0); // �÷��̾� ȸ���� ����
    }

    bool IsGrounded()
    {
        // �÷��̾� �ֺ��� �� �������� ����ĳ��Ʈ�� ���� �ٴڿ� ��Ҵ��� �˻�
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked; // Ŀ�� ���� ����
        canLook = !toggle; // �ü� ���� ���� ���� ������Ʈ
    }

    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed; // �̵� �ӵ� ����
    }

    public void ResetSpeed()
    {
        moveSpeed = originalMoveSpeed; // ������ �̵� �ӵ��� ����
    }

    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            inventory?.Invoke(); // �κ��丮 �׼� ȣ��
            ToggleCursor(); // Ŀ�� ���
        }
    }

    public void OnCraftButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            craft?.Invoke(); // �κ��丮 �׼� ȣ��
            ToggleCursor(); // Ŀ�� ���
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked; // Ŀ�� ���� Ȯ��
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked; // Ŀ�� ���� ���
        canLook = !toggle; // �ü� ���� ���� ���� ������Ʈ
    }
}
