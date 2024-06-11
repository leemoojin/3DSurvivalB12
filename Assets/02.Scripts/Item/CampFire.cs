using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public int damage;            // ���ط�
    public float damageRate;      // ���ظ� ������ �ֱ�
    public float slowDownFactor;  // �÷��̾��� �̵� �ӵ��� ���ҽ�Ű�� ����

    private List<IDamagable> things = new List<IDamagable>(); // ���ظ� ���� ������ �����ϴ� ����Ʈ

    private void Start()
    {
        // ���� �ֱ⸶�� DealDamage �Լ��� ȣ���Ͽ� �ֺ��� �ִ� ���鿡�� ���ظ� ����
        InvokeRepeating("DealDamage", 0, damageRate);
    }

    // ���� �ֱ⸶�� ȣ��Ǿ� �ֺ��� �ִ� ���鿡�� ���ظ� ������ �Լ�
    void DealDamage()
    {
        for (int i = 0; i < things.Count; i++)
        {
            things[i].TakePhysicalDamage(damage);
        }
    }

    // Collider�� CampFire�� ������ �� ȣ��Ǵ� �Լ�
    private void OnTriggerEnter(Collider other)
    {
        // ������ ��ü�� IDamagable ������Ʈ�� ������ �ִ��� Ȯ���ϰ�, ������ �ִٸ� ����Ʈ�� �߰�
        if (other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            things.Add(damagable);
        }

        // ������ ��ü�� �÷��̾��� ���, �̵� �ӵ��� ���ҽ�Ŵ
        if (other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            playerController.SetSpeed(playerController.moveSpeed * slowDownFactor);
        }

        if (other.gameObject.TryGetComponent(out EnemyNav enemy))
        {
            Debug.Log("���ο�");
            enemy.agent.speed = 0.2f;

        }
        else
        {
            Debug.Log("���ο� ����");

        }
    }

    // Collider�� CampFire���� ���� �� ȣ��Ǵ� �Լ�
    private void OnTriggerExit(Collider other)
    {
        // ���� ��ü�� IDamagable ������Ʈ�� ������ �ִ��� Ȯ���ϰ�, ������ �ִٸ� ����Ʈ���� ����
        if (other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            things.Remove(damagable);
        }

        // ���� ��ü�� �÷��̾��� ���, �̵� �ӵ��� ������� ����
        if (other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            playerController.ResetSpeed();
        }

        if (other.gameObject.TryGetComponent(out EnemyNav enemy))
        {
            Debug.Log("���ο� ����");

            if (enemy.aiState == AIState.Chasing) enemy.agent.speed = 1.5f;
            else enemy.agent.speed = 0.5f;

        }
    }
}
