using System;
using UnityEngine;

// ������ ���ظ� ���� �� �ִ� ������Ʈ�� �����ϴ� �������̽�
public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount); // ������ ���ظ� �޴� �޼���
}

// �÷��̾��� ���¸� �����ϴ� Ŭ����
public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition; // UI ���� ����

    // �� ������ ���� ������Ƽ
    Condition health { get { return uiCondition.health; } }      // ü��
    Condition hunger { get { return uiCondition.hunger; } }      // ������
    Condition stamina { get { return uiCondition.stamina; } }    // ���¹̳�

    public float noHungerHealthDecay; // �������� ���� �� ü���� �پ��� ����
    public event Action onTakeDamage;  // ���ظ� �Ծ��� �� �߻��ϴ� �̺�Ʈ

    private void Update()
    {
        // �������� �ð��� ���� �����ϰ� ���¹̳ʴ� ȸ����
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        // �������� ������ ü���� ������
        if (hunger.curValue <= 0.0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        // ü���� 0���� ������ ���� �޼��� ȣ��
        if (health.curValue < 0.0f)
        {
            Die();
        }
    }

    // ü���� ȸ���ϴ� �޼���
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    // ������ �Ծ� �������� ä��� �޼���
    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    // �÷��̾ �״� �޼���
    public void Die()
    {
        Debug.Log("�÷��̾ �׾���.");
    }

    // ������ ���ظ� ���� �� ȣ��Ǵ� �޼���
    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount); // ü�� ����
        onTakeDamage?.Invoke();        // ���ظ� �Ծ����� �˸��� �̺�Ʈ �߻�
    }
}
