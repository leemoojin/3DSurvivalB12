using UnityEngine;
using UnityEngine.InputSystem;

public class Making : MonoBehaviour
{
    public GameObject craftingCanvas; // 제작 캔버스
    public CraftingSlot[] slots;      // 제작 슬롯 배열

    private PlayerController playerController; // 플레이어 컨트롤러
    private UIInventory playerInventory;       // 플레이어 인벤토리

    private void Start()
    {
        craftingCanvas.SetActive(false); // 시작 시 캔버스 비활성화
        playerController = FindObjectOfType<PlayerController>(); // 플레이어 컨트롤러 찾기
        playerInventory = FindObjectOfType<UIInventory>(); // 플레이어 인벤토리 찾기
    }

    private void Update()
    {
        // G 키를 누르면 제작 캔버스를 토글
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            ToggleCraftingCanvas();
        }
    }

    void ToggleCraftingCanvas()
    {
        bool isActive = craftingCanvas.activeSelf;
        craftingCanvas.SetActive(!isActive); // 캔버스 활성/비활성화

        // 캔버스가 활성화되었을 때
        if (!isActive)
        {
            Cursor.lockState = CursorLockMode.None; // 마우스 잠금 해제
            Cursor.visible = true; // 마우스 커서 보이기
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금 설정
            Cursor.visible = false; // 마우스 커서 숨기기
        }

        // 플레이어 컨트롤러에서 마우스 커서 상태를 업데이트
        if (playerController != null)
        {
            playerController.ToggleCursor(!isActive); // 마우스 커서 상태 업데이트
        }
    }

    // 제작 버튼 클릭 이벤트 핸들러
    public void OnCraftButtonClicked(CraftingRecipe recipe)
    {
        // 필요한 재료가 모두 있는지 확인
        bool hasAllMaterials = CheckMaterials(recipe);

        if (hasAllMaterials)
        {
            // 제작 가능한 경우, 제작을 완료하고 인벤토리에 아이템 추가
            CraftItem(recipe);
        }
        else
        {
            // 제작 불가능한 경우, 적절한 안내 메시지 표시
            Debug.Log("Not enough materials to craft this item!");
        }
    }

    // 필요한 재료가 모두 있는지 확인하는 함수
    bool CheckMaterials(CraftingRecipe recipe)
    {
        // 플레이어 인벤토리에서 필요한 재료 아이템의 수량을 확인하고, 부족한 경우 false를 반환
        foreach (var requiredItem in recipe.requiredMaterials)
        {
            if (!playerInventory.HasItem(requiredItem.item, requiredItem.quantity))
            {
                return false;
            }
        }
        return true;
    }

    // 아이템을 제작하는 함수
    void CraftItem(CraftingRecipe recipe)
    {
        // 재료를 인벤토리에서 제거
        foreach (var requiredItem in recipe.requiredMaterials)
        {
            playerInventory.RemoveItem(requiredItem.item, requiredItem.quantity);
        }

        // 제작된 아이템을 인벤토리에 추가
        // 아이템 데이터 대신에 아이템의 인덱스를 전달
        for (int i = 0; i < playerInventory.slots.Length; i++)
        {
            // 슬롯이 비어있는 경우 해당 슬롯에 아이템을 추가
            if (playerInventory.slots[i].item == null)
            {
                playerInventory.slots[i].item = recipe.craftedItem;
                playerInventory.slots[i].quantity = 1;
                playerInventory.UpdateUI();
                return;
            }
        }
    }
}
