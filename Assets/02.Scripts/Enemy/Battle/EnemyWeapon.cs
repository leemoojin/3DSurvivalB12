using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private EnemyAnimation _enemyAnimation;
    static public Collider collider;

    private void Start()
    {
        _enemyAnimation = GetComponentInParent<EnemyAnimation>();
        collider = GetComponent<Collider>();
    }


    public void OnTriggerEnter(Collider other)
    {
        int player = 6;
        bool isAttack;

        // 플레이어 공격 성공
        if (other.gameObject.layer == player && _enemyAnimation.isAttackPlaying) 
        {
            Debug.Log($"NewBehaviourScript.cs - OnTriggerEnter() - 플레이어 공격 성공!");
            //CharacterManager.Instance.Player.controller.GetComponent<IDamagable>().TakePhysicalDamage(damage);
            collider.enabled = false;
        }

        // 방해물 공격 성공


    }
}
