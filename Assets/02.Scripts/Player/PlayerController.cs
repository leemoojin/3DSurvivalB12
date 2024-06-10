using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;                 // 이동 속도
    private float originalMoveSpeed;        // 원래의 이동 속도 저장
    private Vector2 curMovementInput;       // 현재 이동 입력
    public float jumptForce;                // 점프 힘
    public LayerMask groundLayerMask;       // 바닥 레이어 마스크

    [Header("Look")]
    public Transform cameraContainer;       // 카메라 컨테이너
    public float minXLook;                  // 최소 카메라 상하 각도
    public float maxXLook;                  // 최대 카메라 상하 각도
    private float camCurXRot;               // 현재 카메라 X 회전값
    public float lookSensitivity;           // 시선 감도

    private Vector2 mouseDelta;             // 마우스 입력 값

    [HideInInspector]
    public bool canLook = true;             // 시선 제어 가능 여부

    public Action inventory;                // 인벤토리 액션
    public Action craft;
    private Rigidbody rigidbody;            // 리지드바디

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        originalMoveSpeed = moveSpeed;      // 원래의 이동 속도 저장
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move(); // 이동 함수 호출
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook(); // 시선 제어 함수 호출
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>(); // 마우스 입력값 가져오기
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>(); // 이동 입력값 가져오기
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero; // 이동 입력값 초기화
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rigidbody.AddForce(Vector2.up * jumptForce, ForceMode.Impulse); // 점프 힘 적용
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x; // 이동 방향 계산
        dir *= moveSpeed; // 이동 속도 적용
        dir.y = rigidbody.velocity.y;
        Debug.Log("점프 중"+curMovementInput);
        rigidbody.velocity = dir; // 리지드바디에 속도 적용
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity; // 마우스 Y 입력에 따라 카메라 X 회전값 조절
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); // 최소, 최대 회전값 사이로 제한
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); // 카메라 각도 설정

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0); // 플레이어 회전값 적용
    }

    bool IsGrounded()
    {
        // 플레이어 주변의 네 방향으로 레이캐스트를 쏴서 바닥에 닿았는지 검사
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
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked; // 커서 상태 설정
        canLook = !toggle; // 시선 제어 가능 여부 업데이트
    }

    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed; // 이동 속도 설정
    }

    public void ResetSpeed()
    {
        moveSpeed = originalMoveSpeed; // 원래의 이동 속도로 설정
    }

    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            inventory?.Invoke(); // 인벤토리 액션 호출
            ToggleCursor(); // 커서 토글
        }
    }

    public void OnCraftButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            craft?.Invoke(); // 인벤토리 액션 호출
            ToggleCursor(); // 커서 토글
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked; // 커서 상태 확인
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked; // 커서 상태 토글
        canLook = !toggle; // 시선 제어 가능 여부 업데이트
    }
}
