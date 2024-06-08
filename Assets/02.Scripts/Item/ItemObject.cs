using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data; // 아이템 데이터

    // 상호 작용 프롬프트를 반환하는 메서드
    public string GetInteractPrompt()
    {
        // 아이템의 이름과 설명을 문자열로 반환
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    // 상호 작용 시 호출되는 메서드
    public void OnInteract()
    {
        // 플레이어의 아이템 데이터를 설정하고, 아이템을 추가하는 이벤트 호출 후, 자기 자신을 파괴
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}
