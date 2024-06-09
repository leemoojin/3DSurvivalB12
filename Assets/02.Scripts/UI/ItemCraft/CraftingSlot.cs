using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingSlot : MonoBehaviour
{
    public CraftingRecipe recipe;  // 이 슬롯에 할당된 레시피
    public Image itemIcon;         // 아이템 아이콘 이미지
    public TextMeshProUGUI itemName;  // 아이템 이름 텍스트
    public Button craftButton;     // 제작 버튼

    private Making making;         // Making 스크립트 참조

    private void Start()
    {
        // Making 스크립트를 찾아 참조
        making = FindObjectOfType<Making>();

        // 슬롯 UI 업데이트
        if (recipe != null)
        {
            itemIcon.sprite = recipe.craftedItem.icon;
            itemName.text = recipe.craftedItem.displayName;
            craftButton.onClick.AddListener(OnCraftButtonClicked);
        }
    }

    // 제작 버튼 클릭 이벤트 핸들러
    private void OnCraftButtonClicked()
    {
        making.OnCraftButtonClicked(recipe);
    }
}
