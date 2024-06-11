using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data; // ������ ������

    // ��ȣ �ۿ� ������Ʈ�� ��ȯ�ϴ� �޼���
    public string GetInteractPrompt()
    {
        // �������� �̸��� ������ ���ڿ��� ��ȯ
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    // ��ȣ �ۿ� �� ȣ��Ǵ� �޼���
    public void OnInteract()
    {
        // �÷��̾��� ������ �����͸� �����ϰ�, �������� �߰��ϴ� �̺�Ʈ ȣ�� ��, �ڱ� �ڽ��� �ı�
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}
