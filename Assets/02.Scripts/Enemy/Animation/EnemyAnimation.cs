using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    // 애니메이션
    private Animator _animator;
    private EnemyNav _enemyNav;
    private Enemy _enemy;   
    private Collider _collider;

    private float _lastAttackTime;

    public bool isAttackPlaying;




    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();               
        _enemyNav = GetComponent<EnemyNav>();
        _enemy = GetComponent<Enemy>();        
        _collider = _enemy.weapon.GetComponent<Collider>();
    }

    


    // Update is called once per frame
    private void Update()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        isAttackPlaying = stateInfo.IsName("Attack01");

        MovingAnime();
        AttackAnime();
        ChasingAnime();
    }

    public void MovingAnime()
    {
        Debug.Log($"EnemyAnimation.cs - MovingAnime - aiState: {_enemyNav.aiState}");

        if (_enemyNav.aiState == AIState.Wandering)
        {            
            _animator.SetBool("IsChasing", false);
            _animator.SetBool("IsMoving", true);
        }
    }

    public void ChasingAnime()
    {
        if (_enemyNav.aiState == AIState.Chasing || _enemyNav.aiState == AIState.Invade)
        {
            _animator.SetBool("IsMoving", false);
            _animator.SetBool("IsChasing", true);
        }
    }

    public void AttackAnime()
    {
        if (isAttackPlaying) return;

        if (_enemyNav.aiState != AIState.Attacking) return;

        //공격 대기시간
        if (Time.time - _lastAttackTime > _enemy.attackRate)
        {
            _collider.enabled = true;
            _lastAttackTime = Time.time;

            _animator.SetBool("IsChasing", false);
            _animator.SetBool("IsMoving", false);
            _animator.SetTrigger("Attack");
           
        }
        
    }



}
