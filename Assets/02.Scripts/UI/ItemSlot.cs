using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;            // 슬롯에 있는 아이템 데이터
    public UIInventory inventory;    // UI 인벤토리 참조
    public Button button;            // 버튼 컴포넌트 참조
    public Image icon;               // 아이콘 이미지 컴포넌트 참조
    public TextMeshProUGUI quatityText; // 수량을 나타내는 텍스트 컴포넌트
    private Outline outline;         // 외곽선 컴포넌트 참조

    public int index;                // 슬롯의 인덱스
    public bool equipped;            // 장비 여부
    public int quantity;             // 수량

    private void Awake()
    {
        outline = GetComponent<Outline>(); // 외곽선 컴포넌트 참조
    }

    private void OnEnable()
    {
        outline.enabled = equipped; // 활성화될 때 외곽선 표시 여부 설정
    }

    // 슬롯에 아이템을 설정하는 메서드
    public void Set()
    {
        // 아이콘을 활성화하고 아이템 정보를 표시
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty; // 수량을 표시

        if (outline != null)
        {
            outline.enabled = equipped; // 장착 여부에 따라 외곽선 표시 여부 설정
        }
    }

    // 슬롯을 비우는 메서드
    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false); // 아이콘 비활성화
        quatityText.text = string.Empty;   // 수량 텍스트 비우기
    }

    // 버튼을 클릭했을 때 호출되는 메서드
    public void OnClickButton()
    {
        inventory.SelectItem(index); // 해당 아이템을 선택
    }
}
