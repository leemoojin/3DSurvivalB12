using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class CraftingMaterial
{
    public string itemName; // ������ �̸�
    public int quantity;    // �ʿ��� ����
}

public class Making : MonoBehaviour
{
    public GameObject craftingCanvas; // ���� ĵ����
    public ItemData craftedItem;      // ������ ������ ������
    public ItemData[] requiredMaterials; // �ʿ��� ��� ������ ������ �迭

    private PlayerController playerController; // �÷��̾� ��Ʈ�ѷ�
    private UIInventory playerInventory; // �÷��̾� �κ��丮
    private void Start()
    {
        craftingCanvas.SetActive(false); // ���� �� ĵ���� ��Ȱ��ȭ
        playerController = FindObjectOfType<PlayerController>(); // �÷��̾� ��Ʈ�ѷ� ã��

        playerInventory = FindObjectOfType<UIInventory>();         // �÷��̾� �κ��丮 ã��
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
    public void OnCraftButtonClicked()
    {
        // �ʿ��� ��ᰡ ��� �ִ��� Ȯ��
        bool hasAllMaterials = CheckMaterials();

        if (hasAllMaterials)
        {
            // ���� ������ ���, ������ �Ϸ��ϰ� �κ��丮�� ������ �߰�
            CraftItem();
        }
        else
        {
            // ���� �Ұ����� ���, ������ �ȳ� �޽��� ǥ��
            Debug.Log("Not enough materials to craft this item!");
        }
    }

    // �ʿ��� ��ᰡ ��� �ִ��� Ȯ���ϴ� �Լ�
    bool CheckMaterials()
    {
        // �÷��̾� �κ��丮���� �ʿ��� ��� �������� ������ Ȯ���ϰ�, ������ ��� false�� ��ȯ
        foreach (ItemData requiredItem in requiredMaterials)
        {
            if (!playerInventory.HasItem(requiredItem, 1))
            {
                return false;
            }
        }
        return true;
    }

    // �������� �����ϴ� �Լ�
    void CraftItem()
    {
        // �ʿ��� ��Ḧ �÷��̾� �κ��丮���� ����
        foreach (ItemData requiredItem in requiredMaterials)
        {
            playerInventory.RemoveItem(requiredItem, 1);
        }

        // ���۵� �������� �κ��丮�� �߰�
        CharacterManager.Instance.Player.itemData = craftedItem; // ���۵� ������ �����͸� ����
        playerInventory.AddItem(); // �μ� ���� ȣ��

        // ���� ĵ������ ����
        ToggleCraftingCanvas();
    }

}
