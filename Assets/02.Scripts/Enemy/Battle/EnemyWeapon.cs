using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private EnemyAnimation _enemyAnimation;
    private Enemy _enemy; 

    public Collider collider;

    private void Start()
    {
        _enemyAnimation = GetComponentInParent<EnemyAnimation>();
        _enemy = GetComponentInParent<Enemy>();

        collider = GetComponent<Collider>();
    }


    public void OnTriggerEnter(Collider other)
    {
        int player = LayerMask.NameToLayer("Player");
        int craft = LayerMask.NameToLayer("Craft"); ;
        bool isAttack;

        // 플레이어 공격 성공
        if (other.gameObject.layer == player && _enemyAnimation.isAttackPlaying) 
        {
            // 플레이어 공격
            //CharacterManager.Instance.Player.condition.uiCondition.health.Subtract(_enemy.damage);
            if (other.gameObject.TryGetComponent(out IDamagable damagable))
            {
                Debug.Log("플레이어 공격");

                damagable.TakePhysicalDamage(_enemy.damage);
            }
            collider.enabled = false;
        }
        Debug.Log($"레이어 :   {other.gameObject.layer}");

        // 방해물 공격 성공
        if (other.gameObject.layer == craft && _enemyAnimation.isAttackPlaying)
        {
            if (other.gameObject.TryGetComponent(out IDamagable damagable))
            {
                Debug.Log("방해물 공격");

                damagable.TakePhysicalDamage(_enemy.damage);
            }
            collider.enabled = false;
        }

    }
}
