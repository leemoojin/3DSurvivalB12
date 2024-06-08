using UnityEngine;

// UI���� �÷��̾��� ���¸� �����ϴ� Ŭ����
public class UICondition : MonoBehaviour
{
    public Condition health;    // ü�� ���¸� ��Ÿ���� Condition ������Ʈ
    public Condition hunger;    // ������ ���¸� ��Ÿ���� Condition ������Ʈ
    public Condition stamina;   // ���¹̳� ���¸� ��Ÿ���� Condition ������Ʈ

    private void Start()
    {
        // �÷��̾��� ���� ������ UICondition���� ����
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
