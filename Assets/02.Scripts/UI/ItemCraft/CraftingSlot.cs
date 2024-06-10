using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingSlot : MonoBehaviour
{
    public CraftingRecipe recipe;  // �� ���Կ� �Ҵ�� ������
    public Image itemIcon;         // ������ ������ �̹���
    public TextMeshProUGUI itemName;  // ������ �̸� �ؽ�Ʈ
    public Button craftButton;     // ���� ��ư

    private Making making;         // Making ��ũ��Ʈ ����

    private void Start()
    {
        // Making ��ũ��Ʈ�� ã�� ����
        making = FindObjectOfType<Making>();

        // ���� UI ������Ʈ
        if (recipe != null)
        {
            itemIcon.sprite = recipe.craftedItem.icon;
            itemName.text = recipe.craftedItem.displayName;
            craftButton.onClick.AddListener(OnCraftButtonClicked);
        }
    }

    // ���� ��ư Ŭ�� �̺�Ʈ �ڵ鷯
    private void OnCraftButtonClicked()
    {
        making.OnCraftButtonClicked(recipe);
    }
}
