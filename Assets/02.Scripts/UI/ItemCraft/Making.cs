using UnityEngine;
using UnityEngine.InputSystem;

public class Making : MonoBehaviour
{
    public GameObject craftingCanvas; // ���� ĵ����
    public CraftingSlot[] slots;      // ���� ���� �迭

    private PlayerController playerController; // �÷��̾� ��Ʈ�ѷ�
    private UIInventory playerInventory;       // �÷��̾� �κ��丮

    private void Start()
    {
        craftingCanvas.SetActive(false); // ���� �� ĵ���� ��Ȱ��ȭ
        playerController = FindObjectOfType<PlayerController>(); // �÷��̾� ��Ʈ�ѷ� ã��
        playerInventory = FindObjectOfType<UIInventory>(); // �÷��̾� �κ��丮 ã��
    }

    private void Update()
    {
        // G Ű�� ������ ���� ĵ������ ���
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            ToggleCraftingCanvas();
        }
    }

    void ToggleCraftingCanvas()
    {
        bool isActive = craftingCanvas.activeSelf;
        craftingCanvas.SetActive(!isActive); // ĵ���� Ȱ��/��Ȱ��ȭ

        // ĵ������ Ȱ��ȭ�Ǿ��� ��
        if (!isActive)
        {
            Cursor.lockState = CursorLockMode.None; // ���콺 ��� ����
            Cursor.visible = true; // ���콺 Ŀ�� ���̱�
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // ���콺 ��� ����
            Cursor.visible = false; // ���콺 Ŀ�� �����
        }

        // �÷��̾� ��Ʈ�ѷ����� ���콺 Ŀ�� ���¸� ������Ʈ
        if (playerController != null)
        {
            playerController.ToggleCursor(!isActive); // ���콺 Ŀ�� ���� ������Ʈ
        }
    }

    // ���� ��ư Ŭ�� �̺�Ʈ �ڵ鷯
    public void OnCraftButtonClicked(CraftingRecipe recipe)
    {
        // �ʿ��� ��ᰡ ��� �ִ��� Ȯ��
        bool hasAllMaterials = CheckMaterials(recipe);

        if (hasAllMaterials)
        {
            // ���� ������ ���, ������ �Ϸ��ϰ� �κ��丮�� ������ �߰�
            CraftItem(recipe);
        }
        else
        {
            // ���� �Ұ����� ���, ������ �ȳ� �޽��� ǥ��
            Debug.Log("Not enough materials to craft this item!");
        }
    }

    // �ʿ��� ��ᰡ ��� �ִ��� Ȯ���ϴ� �Լ�
    bool CheckMaterials(CraftingRecipe recipe)
    {
        // �÷��̾� �κ��丮���� �ʿ��� ��� �������� ������ Ȯ���ϰ�, ������ ��� false�� ��ȯ
        foreach (var requiredItem in recipe.requiredMaterials)
        {
            if (!playerInventory.HasItem(requiredItem.item, requiredItem.quantity))
            {
                return false;
            }
        }
        return true;
    }

    // �������� �����ϴ� �Լ�
    void CraftItem(CraftingRecipe recipe)
    {
        // ��Ḧ �κ��丮���� ����
        foreach (var requiredItem in recipe.requiredMaterials)
        {
            playerInventory.RemoveItem(requiredItem.item, requiredItem.quantity);
        }

        // ���۵� �������� �κ��丮�� �߰�
        // ������ ������ ��ſ� �������� �ε����� ����
        for (int i = 0; i < playerInventory.slots.Length; i++)
        {
            // ������ ����ִ� ��� �ش� ���Կ� �������� �߰�
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
