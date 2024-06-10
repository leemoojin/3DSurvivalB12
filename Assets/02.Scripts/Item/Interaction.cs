using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// 상호 작용 가능한 오브젝트에 대한 인터페이스
public interface IInteractable
{
    public string GetInteractPrompt(); // 상호 작용 프롬프트 반환
    public void OnInteract();          // 상호 작용 시 호출될 함수
}

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;     // 상호 작용 체크 간격
    private float lastCheckTime;        // 마지막으로 체크한 시간
    public float maxCheckDistance;      // 상호 작용 거리
    public LayerMask layerMask;         // 상호 작용 레이어 마스크

    public GameObject curInteractGameObject;  // 현재 상호 작용 중인 게임 오브젝트
    private IInteractable curInteractable;     // 현재 상호 작용 가능한 인터페이스

    public TextMeshProUGUI promptText;  // 상호 작용 프롬프트를 표시할 텍스트 메시 프로
    private Camera camera;              // 메인 카메라

    void Start()
    {
        camera = Camera.main;
    }

    // 매 프레임마다 실행되는 함수
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); // 카메라 시선을 화면 중앙으로부터 발사
            RaycastHit hit;

            // 레이캐스트를 통해 상호 작용 대상을 체크
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != curInteractGameObject)
                {
                    // 현재 상호 작용 대상 업데이트
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();                    
                    SetPromptText(); // 프롬프트 텍스트 설정 - 임시 주석처리**
                }
            }
            else
            {
                // 상호 작용 대상이 없으면 초기화
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false); // 프롬프트 텍스트 비활성화
            }
        }
    }

    // 상호 작용 프롬프트 텍스트 설정
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true); // 프롬프트 텍스트 활성화
        promptText.text = curInteractable.GetInteractPrompt(); // 인터랙터블로부터 프롬프트 텍스트 가져와 설정
    }

    // 상호 작용 입력 처리
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            // 상호 작용 시작 시 현재 상호 작용 대상과 인터랙터블 초기화 및 프롬프트 텍스트 비활성화
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
