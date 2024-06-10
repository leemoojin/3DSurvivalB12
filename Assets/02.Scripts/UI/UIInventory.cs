using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;           // �κ��丮 ���� �迭

    public GameObject inventoryWindow; // �κ��丮 â
    public Transform slotPanel;        // ���� �г�
    public Transform dropPosition;     // �������� ����߸� ��ġ

    // ���õ� ������ ������ ǥ���ϱ� ���� UI ��ҵ�
    [Header("Selected Item")]
    private ItemSlot selectedItem;              // ���õ� ������
    private int selectedItemIndex;              // ���õ� �������� �ε���
    public TextMeshProUGUI selectedItemName;    // ���õ� ������ �̸�
    public TextMeshProUGUI selectedItemDescription; // ���õ� ������ ����
    public TextMeshProUGUI selectedItemStatName; // ���õ� ������ ���� �̸�
    public TextMeshProUGUI selectedItemStatValue; // ���õ� ������ ���� ��
    public GameObject useButton;                // ��� ��ư
    public GameObject equipButton;              // ���� ��ư
    public GameObject unEquipButton;            // ���� ���� ��ư
    public GameObject dropButton;               // ������ ������ ��ư

    private int curEquipIndex;                  // ���� ������ �������� �ε���

    private PlayerController controller;         // �÷��̾� ��Ʈ�ѷ�
    private PlayerCondition condition;           // �÷��̾� ����

    void Start()
    {
        // ��Ʈ�ѷ��� ���� ���� ����
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        // �κ��丮 ��� �� ������ �߰� �̺�Ʈ ����
        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;

        // �κ��丮 â ��Ȱ��ȭ �� ���� �ʱ�ȭ
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

    // �κ��丮 ��� �޼���
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

    // �κ��丮�� ���� �ִ��� Ȯ���ϴ� �޼���
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    // �������� �߰��ϴ� �޼���
    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        // ���� ������ �������� ���
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

        // �� ���Կ� ������ �߰�
        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        // �������� ������
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    // �������� ������ �޼���
    public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefabs, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    // �κ��丮 UI ������Ʈ �޼���
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

    // ���� ������ �������� ã�� �޼���
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

    // �� ������ ã�� �޼���
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

    // �������� �����ϴ� �޼���
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        // ���õ� �������� ������ UI�� ǥ��
        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        // ���õ� �������� �Ӽ��� ǥ��
        for (int i = 0; i < selectedItem.item.consumables.Length; i++)
        {
            selectedItemStatName.text += selectedItem.item.consumables[i].type.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.item.consumables[i].value.ToString() + "\n";
        }

        // ���, ����, ������ ��ư Ȱ��ȭ ����
        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !slots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);
    }

    // ���õ� ������ â �ʱ�ȭ �޼���
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

    // ��� ��ư Ŭ�� �̺�Ʈ �ڵ鷯
    public void OnUseButton()
    {
        if (selectedItem.item.type == ItemType.Consumable)
        {
            // ���õ� �������� �Һ� ������ ���, �ش� �������� ����ϰ� UI ������Ʈ
            for (int i = 0; i < selectedItem.item.consumables.Length; i++)
            {
                switch (selectedItem.item.consumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.item.consumables[i].value); break; // ü�� ȸ��
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.item.consumables[i].value); break; // ������ ����
                }
            }
            RemoveSelctedItem(); // ����� ������ ����
        }
    }

    // ������ ��ư Ŭ�� �̺�Ʈ �ڵ鷯
    public void OnDropButton()
    {
        // ���õ� �������� ������ UI ������Ʈ
        ThrowItem(selectedItem.item);
        RemoveSelctedItem();
    }

    // ���õ� ������ ���� �� UI ������Ʈ �޼���
    void RemoveSelctedItem()
    {
        selectedItem.quantity--;

        if (selectedItem.quantity <= 0)
        {
            // ���õ� �������� ������ 0������ ���, �ش� ������ ����
            selectedItem.item = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }

        UpdateUI(); // UI ������Ʈ
    }

    // �������� �ִ��� Ȯ���ϴ� �޼���
    public bool HasItem(ItemData item, int quantity)
    {
        return false;
    }
}
