using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Craft
{
    public string craftName; // 이름
    public GameObject go_prefab; // 실제 설치 될 프리팹
    public GameObject go_PreviewPrefab; // 미리 보기 프리팹
}

public class CraftManual : MonoBehaviour
{
    private bool isActivated = false;  // CraftManual UI 활성 상태
    private bool isPreviewActivated = false; // 미리 보기 활성화 상태
    private PlayerController controller;

    [SerializeField]
    private GameObject go_BaseUI; // 기본 베이스 UI

    [SerializeField]
    private Craft[] craft_fire;  // 불 탭에 있는 슬롯들. 

    private GameObject go_Preview; // 미리 보기 프리팹을 담을 변수
    private GameObject go_Prefab; // 실제 생성될 프리팹을 담을 변수 

    [SerializeField]
    private Transform tf_Player;  // 플레이어 위치

    private RaycastHit hitInfo;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float range;

    private void Start()
    {
        controller = FindObjectOfType<PlayerController>(); // PlayerController 인스턴스 찾기
        if (controller != null)
        {
            controller.craft += Toggle; // PlayerController의 craft 액션에 Toggle 메서드 연결
        }
    }

    void Update()
    {
        Debug.Log("Update method called"); // Update 메서드 호출 확인

        if (isPreviewActivated)
        {
            Debug.Log("Preview is activated"); // 미리보기가 활성화된 상태 확인
            PreviewPositionUpdate();
        }

        if (Input.GetButtonDown("Fire1"))
            Build();

        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel();
    }

    public void SlotClick(int _slotNumber)
    {
        // 메인 카메라의 위치를 사용하여 미리보기 프리팹 생성
        go_Preview = Instantiate(craft_fire[_slotNumber].go_PreviewPrefab, Camera.main.transform.position, Quaternion.identity);
        go_Prefab = craft_fire[_slotNumber].go_prefab;
        isPreviewActivated = true;
        go_BaseUI.SetActive(false);
    }

    private void PreviewPositionUpdate()
    {
        Debug.Log("PreviewPositionUpdate called"); // PreviewPositionUpdate 메서드 호출 확인

        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;
                go_Preview.transform.position = _location;

                Debug.Log("Hit location: " + _location); // 레이캐스트 히트 위치 확인
                Debug.Log("Preview position: " + go_Preview.transform.position); // 미리보기 프리팹 위치 확인
            }
            else
            {
                Debug.Log("Raycast hit nothing"); // 레이캐스트가 아무것도 맞추지 못함
            }
        }
        else
        {
            Debug.Log("Raycast did not hit"); // 레이캐스트가 아무것도 맞추지 못함
        }
    }


    private void Build()
    {
        if (isPreviewActivated)
        {
            // 실제 프리팹을 생성하고 미리보기 프리팹을 제거
            Instantiate(go_Prefab, hitInfo.point, Quaternion.identity);
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
        }
    }


    private void Window()
    {
        if (!isActivated)
            OpenWindow();
        else
            CloseWindow();
    }

    private void OpenWindow()
    {
        isActivated = true;
        go_BaseUI.SetActive(true);
    }

    private void CloseWindow()
    {
        isActivated = false;
        go_BaseUI.SetActive(false);
    }

    private void Cancel()
    {
        if (isPreviewActivated)
            Destroy(go_Preview);

        isActivated = false;
        isPreviewActivated = false;

        go_Preview = null;
        go_Prefab = null;

        go_BaseUI.SetActive(false);
    }

    public void Toggle()
    {
        if (go_BaseUI.activeInHierarchy)
        {
            go_BaseUI.SetActive(false);
        }
        else
        {
            go_BaseUI.SetActive(true);
        }
    }
}
