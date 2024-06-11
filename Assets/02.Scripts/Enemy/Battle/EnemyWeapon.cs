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

        // �÷��̾� ���� ����
        if (other.gameObject.layer == player && _enemyAnimation.isAttackPlaying) 
        {
            // �÷��̾� ����
            //CharacterManager.Instance.Player.condition.uiCondition.health.Subtract(_enemy.damage);
            if (other.gameObject.TryGetComponent(out IDamagable damagable))
            {
                Debug.Log("�÷��̾� ����");

                damagable.TakePhysicalDamage(_enemy.damage);
            }
            collider.enabled = false;
        }
        Debug.Log($"���̾� :   {other.gameObject.layer}");

        // ���ع� ���� ����
        if (other.gameObject.layer == craft && _enemyAnimation.isAttackPlaying)
        {
            if (other.gameObject.TryGetComponent(out IDamagable damagable))
            {
                Debug.Log("���ع� ����");

                damagable.TakePhysicalDamage(_enemy.damage);
            }
            collider.enabled = false;
        }

    }
}
