using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public int damage;
    public float damageRate;
    public float slowDownFactor; // �ӵ� ���� ����

    private List<IDamagable> things = new List<IDamagable>();

    private void Start()
    {
        InvokeRepeating("DealDamage", 0, damageRate);
    }

    void DealDamage()
    {
        for (int i = 0; i < things.Count; i++)
        {
            things[i].TakePhysicalDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            things.Add(damagable);
        }

        // �÷��̾��� �ӵ��� ����
        if (other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            playerController.SetSpeed(playerController.moveSpeed * slowDownFactor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            things.Remove(damagable);
        }

        // �÷��̾��� �ӵ��� ������� ����
        if (other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            playerController.ResetSpeed();
        }
    }
}
