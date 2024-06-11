using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public int damage;            // 피해량
    public float damageRate;      // 피해를 입히는 주기
    public float slowDownFactor;  // 플레이어의 이동 속도를 감소시키는 비율

    private List<IDamagable> things = new List<IDamagable>(); // 피해를 입을 대상들을 저장하는 리스트

    private void Start()
    {
        // 일정 주기마다 DealDamage 함수를 호출하여 주변에 있는 대상들에게 피해를 입힘
        InvokeRepeating("DealDamage", 0, damageRate);
    }

    // 일정 주기마다 호출되어 주변에 있는 대상들에게 피해를 입히는 함수
    void DealDamage()
    {
        for (int i = 0; i < things.Count; i++)
        {
            things[i].TakePhysicalDamage(damage);
        }
    }

    // Collider가 CampFire에 진입할 때 호출되는 함수
    private void OnTriggerEnter(Collider other)
    {
        // 진입한 물체가 IDamagable 컴포넌트를 가지고 있는지 확인하고, 가지고 있다면 리스트에 추가
        if (other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            things.Add(damagable);
        }

        // 진입한 물체가 플레이어인 경우, 이동 속도를 감소시킴
        if (other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            playerController.SetSpeed(playerController.moveSpeed * slowDownFactor);
        }

        if (other.gameObject.TryGetComponent(out EnemyNav enemy))
        {
            Debug.Log("슬로우");
            enemy.agent.speed = 0.2f;

        }
        else
        {
            Debug.Log("슬로우 안함");

        }
    }

    // Collider가 CampFire에서 나갈 때 호출되는 함수
    private void OnTriggerExit(Collider other)
    {
        // 나간 물체가 IDamagable 컴포넌트를 가지고 있는지 확인하고, 가지고 있다면 리스트에서 제거
        if (other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            things.Remove(damagable);
        }

        // 나간 물체가 플레이어인 경우, 이동 속도를 원래대로 돌림
        if (other.gameObject.TryGetComponent(out PlayerController playerController))
        {
            playerController.ResetSpeed();
        }

        if (other.gameObject.TryGetComponent(out EnemyNav enemy))
        {
            Debug.Log("슬로우 해제");

            if (enemy.aiState == AIState.Chasing) enemy.agent.speed = 1.5f;
            else enemy.agent.speed = 0.5f;

        }
    }
}
