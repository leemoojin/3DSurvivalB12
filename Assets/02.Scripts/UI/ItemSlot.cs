using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;            // ���Կ� �ִ� ������ ������
    public UIInventory inventory;    // UI �κ��丮 ����
    public Button button;            // ��ư ������Ʈ ����
    public Image icon;               // ������ �̹��� ������Ʈ ����
    public TextMeshProUGUI quatityText; // ������ ��Ÿ���� �ؽ�Ʈ ������Ʈ
    private Outline outline;         // �ܰ��� ������Ʈ ����

    public int index;                // ������ �ε���
    public bool equipped;            // ��� ����
    public int quantity;             // ����

    private void Awake()
    {
        outline = GetComponent<Outline>(); // �ܰ��� ������Ʈ ����
    }

    private void OnEnable()
    {
        outline.enabled = equipped; // Ȱ��ȭ�� �� �ܰ��� ǥ�� ���� ����
    }

    // ���Կ� �������� �����ϴ� �޼���
    public void Set()
    {
        // �������� Ȱ��ȭ�ϰ� ������ ������ ǥ��
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty; // ������ ǥ��

        if (outline != null)
        {
            outline.enabled = equipped; // ���� ���ο� ���� �ܰ��� ǥ�� ���� ����
        }
    }

    // ������ ���� �޼���
    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false); // ������ ��Ȱ��ȭ
        quatityText.text = string.Empty;   // ���� �ؽ�Ʈ ����
    }

    // ��ư�� Ŭ������ �� ȣ��Ǵ� �޼���
    public void OnClickButton()
    {
        inventory.SelectItem(index); // �ش� �������� ����
    }
}
