using UnityEngine;

// 각 아이템에 대한 정보를 담는 클래스
[System.Serializable]
public class Craft
{
    public string craftName; // 아이템 이름
    public GameObject go_prefab; // 실제 생성될 프리팹
    public GameObject go_PreviewPrefab; // 미리 보기에 사용될 프리팹
}

// 조합 메뉴얼을 관리하는 클래스
public class CraftManual : MonoBehaviour
{
    private bool isActivated = false; // CraftManual UI의 활성화 상태를 나타냄
    private bool isPreviewActivated = false; // 미리 보기의 활성화 상태를 나타냄
    private PlayerController controller;

    [SerializeField]
    private GameObject go_BaseUI; // 기본 베이스 UI

    [SerializeField]
    private Craft[] craft_fire; // 불 탭에 있는 슬롯들을 나타내는 배열

    private GameObject go_Preview; // 미리 보기 프리팹을 저장할 변수
    private GameObject go_Prefab; // 실제 생성될 프리팹을 저장할 변수 

    [SerializeField]
    private Transform tf_Player; // 플레이어의 위치를 나타냄

    private RaycastHit hitInfo; // 레이캐스트를 통해 얻은 정보를 저장할 변수
    [SerializeField]
    private LayerMask layerMask; // 레이캐스트에서 충돌 검사할 레이어 마스크
    [SerializeField]
    private float range; // 레이캐스트의 최대 거리

    private void Start()
    {
        controller = FindObjectOfType<PlayerController>(); // PlayerController 인스턴스를 찾음
        if (controller != null)
        {
            controller.craft += Toggle; // PlayerController의 craft 이벤트에 Toggle 메서드를 연결
        }
    }

    // 슬롯 클릭 시 호출되는 메서드
    public void SlotClick(int _slotNumber)
    {
        // UI를 닫음
        Window(false); // UI를 닫음
        // 메인 카메라의 위치를 기반으로 미리보기 프리팹을 생성함
        go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, Camera.main.transform.position, Quaternion.identity);
        go_Prefab = craft_fire[_slotNumber].go_prefab;
        isPreviewActivated = true;
    }



    void Update()
    {
        if (isPreviewActivated)
        {
            PreviewPositionUpdate(); // 미리보기의 위치를 업데이트함
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Build(); // 빌드를 실행함
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel(); // 취소를 실행함
        }
    }

    // 미리보기의 위치를 업데이트하는 메서드
    private void PreviewPositionUpdate()
    {
        // 플레이어의 시야 방향으로 레이캐스트를 발사하여 미리보기 프리팹의 위치를 조정함
        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;
                go_Preview.transform.position = _location;
            }
        }
    }

    // 빌드를 실행하는 메서드
    private void Build()
    {
        if (isPreviewActivated)
        {
            Instantiate(go_Prefab, hitInfo.point, Quaternion.identity);
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
            // UI를 다시 활성화함
            Window(true);
        }
    }

    // 창을 열거나 닫는 메서드
    private void Window(bool toggle)
    {
        if (!toggle)
        {
            if (go_BaseUI.activeSelf) // UI가 활성화되어 있을 때만 화면 고정 해제
            {
                controller.ToggleCursor(false);
            }
        }

        if (isActivated)
        {
            OpenWindow();
        }
        else
        {
            CloseWindow();
        }
    }


    // 창을 열기 위한 메서드
    private void OpenWindow()
    {
        isActivated = true;
        go_BaseUI.SetActive(true); // UI를 활성화함
    }

    // 창을 닫기 위한 메서드
    private void CloseWindow()
    {
        isActivated = false;
        go_BaseUI.SetActive(false); // UI를 비활성화함
    }

    // 취소를 실행하는 메서드
    private void Cancel()
    {
        if (isPreviewActivated)
        {
            Destroy(go_Preview); // 미리보기를 제거함
        }

        isActivated = false;
        isPreviewActivated = false;

        go_Preview = null;
        go_Prefab = null;

        go_BaseUI.SetActive(false); // UI를 비
    }

    // UI를 토글하는 메서드
    public void Toggle()
    {
        Debug.Log("toggle open");
        if (go_BaseUI.activeInHierarchy)
        {
            go_BaseUI.SetActive(false); // UI를 비활성화함
        }
        else
        {
            go_BaseUI.SetActive(true); // UI를 활성화함
        }
    }

}
