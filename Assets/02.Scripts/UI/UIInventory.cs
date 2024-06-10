using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;           // 인벤토리 슬롯 배열

    public GameObject inventoryWindow; // 인벤토리 창
    public Transform slotPanel;        // 슬롯 패널
    public Transform dropPosition;     // 아이템을 떨어뜨릴 위치

    // 선택된 아이템 정보를 표시하기 위한 UI 요소들
    [Header("Selected Item")]
    private ItemSlot selectedItem;              // 선택된 아이템
    private int selectedItemIndex;              // 선택된 아이템의 인덱스
    public TextMeshProUGUI selectedItemName;    // 선택된 아이템 이름
    public TextMeshProUGUI selectedItemDescription; // 선택된 아이템 설명
    public TextMeshProUGUI selectedItemStatName; // 선택된 아이템 스탯 이름
    public TextMeshProUGUI selectedItemStatValue; // 선택된 아이템 스탯 값
    public GameObject useButton;                // 사용 버튼
    public GameObject equipButton;              // 장착 버튼
    public GameObject unEquipButton;            // 장착 해제 버튼
    public GameObject dropButton;               // 아이템 버리기 버튼

    private int curEquipIndex;                  // 현재 장착된 아이템의 인덱스

    private PlayerController controller;         // 플레이어 컨트롤러
    private PlayerCondition condition;           // 플레이어 상태

    void Start()
    {
        // 컨트롤러와 상태 참조 설정
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        // 인벤토리 토글 및 아이템 추가 이벤트 설정
        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;

        // 인벤토리 창 비활성화 및 슬롯 초기화
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].Clear();
        }

        ClearSelectedItemWindow();
    }

    // 인벤토리 토글 메서드
    public void Toggle()
    {
        if (inventoryWindow.activeInHierarchy)
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    // 아이템을 추가하는 메서드
    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        // 아이템 데이터가 null인지 확인
        if (data == null)
        {
            Debug.LogWarning("Item data is null. Cannot add item.");
            return;
        }

        // 스택 가능한 아이템인 경우
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

        // 빈 슬롯에 아이템 추가
        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        // 아이템을 던지기
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }


    // 아이템을 던지는 메서드
    public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefabs, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    // 인벤토리 UI 업데이트 메서드
    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }

    // 스택 가능한 아이템을 찾는 메서드
    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    // 빈 슬롯을 찾는 메서드
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    // 아이템을 선택하는 메서드
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        // 선택된 아이템의 정보를 UI에 표시
        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        // 선택된 아이템의 속성을 표시
        for (int i = 0; i < selectedItem.item.consumables.Length; i++)
        {
            selectedItemStatName.text += selectedItem.item.consumables[i].type.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.item.consumables[i].value.ToString() + "\n";
        }

        // 사용, 장착, 버리기 버튼 활성화 설정
        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !slots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);
    }

    // 선택된 아이템 창 초기화 메서드
    void ClearSelectedItemWindow()
    {
        selectedItem = null;

        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    // 사용 버튼 클릭 이벤트 핸들러
    public void OnUseButton()
    {
        if (selectedItem.item.type == ItemType.Consumable)
        {
            // 선택된 아이템이 소비 가능한 경우, 해당 아이템을 사용하고 UI 업데이트
            for (int i = 0; i < selectedItem.item.consumables.Length; i++)
            {
                switch (selectedItem.item.consumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.item.consumables[i].value); break; // 체력 회복
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.item.consumables[i].value); break; // 포만도 증가
                }
            }
            RemoveSelctedItem(); // 사용한 아이템 삭제
        }
    }

    // 버리기 버튼 클릭 이벤트 핸들러
    public void OnDropButton()
    {
        // 선택된 아이템을 버리고 UI 업데이트
        ThrowItem(selectedItem.item);
        RemoveSelctedItem();
    }

    // 선택된 아이템 삭제 및 UI 업데이트 메서드
    void RemoveSelctedItem()
    {
        selectedItem.quantity--;

        if (selectedItem.quantity <= 0)
        {
            // 선택된 아이템의 수량이 0이하인 경우, 해당 아이템 삭제
            selectedItem.item = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }

        UpdateUI(); // UI 업데이트
    }

    // 아이템이 있는지 확인하는 메서드
    public bool HasItem(ItemData item, int quantity)
    {
        int itemCount = 0;

        // 모든 슬롯을 순회하면서 해당 아이템의 수량을 확인
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item)
            {
                itemCount += slots[i].quantity;
            }

            // 필요한 수량을 만족하면 true를 반환
            if (itemCount >= quantity)
            {
                return true;
            }
        }

        // 필요한 수량을 만족하지 못하면 false를 반환
        return false;
    }


    public void RemoveItem(ItemData item, int quantity)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item)
            {
                slots[i].quantity -= quantity;
                if (slots[i].quantity <= 0)
                {
                    slots[i].item = null;
                    UpdateUI();
                }
                return;
            }
        }
    }
    public void OnEquipButton()
    {
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }

        slots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;
        CharacterManager.Instance.Player.equip.EquipNew(selectedItem.item);
        UpdateUI();

        SelectItem(selectedItemIndex);
    }

    void UnEquip(int index)
    {
        slots[index].equipped = false;
        CharacterManager.Instance.Player.equip.UnEquip();
        UpdateUI();

        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }

}
