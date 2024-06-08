using UnityEngine;

// UI에서 플레이어의 상태를 관리하는 클래스
public class UICondition : MonoBehaviour
{
    public Condition health;    // 체력 상태를 나타내는 Condition 컴포넌트
    public Condition hunger;    // 포만감 상태를 나타내는 Condition 컴포넌트
    public Condition stamina;   // 스태미너 상태를 나타내는 Condition 컴포넌트

    private void Start()
    {
        // 플레이어의 상태 참조를 UICondition으로 설정
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
