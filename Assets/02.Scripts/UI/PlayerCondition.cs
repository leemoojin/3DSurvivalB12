using System;
using UnityEngine;

// 물리적 피해를 받을 수 있는 오브젝트를 정의하는 인터페이스
public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount); // 물리적 피해를 받는 메서드
}

// 플레이어의 상태를 관리하는 클래스
public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition; // UI 상태 참조

    // 각 상태의 참조 프로퍼티
    Condition health { get { return uiCondition.health; } }      // 체력
    Condition hunger { get { return uiCondition.hunger; } }      // 포만감
    Condition stamina { get { return uiCondition.stamina; } }    // 스태미너

    public float noHungerHealthDecay; // 포만감이 없을 때 체력이 줄어드는 비율
    public event Action onTakeDamage;  // 피해를 입었을 때 발생하는 이벤트

    private void Update()
    {
        // 포만감이 시간에 따라 감소하고 스태미너는 회복됨
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        // 포만감이 없으면 체력이 감소함
        if (hunger.curValue <= 0.0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        // 체력이 0보다 작으면 죽음 메서드 호출
        if (health.curValue < 0.0f)
        {
            Die();
        }
    }

    // 체력을 회복하는 메서드
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    // 음식을 먹어 포만감을 채우는 메서드
    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    // 플레이어가 죽는 메서드
    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }

    // 물리적 피해를 받을 때 호출되는 메서드
    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount); // 체력 감소
        onTakeDamage?.Invoke();        // 피해를 입었음을 알리는 이벤트 발생
    }
}
