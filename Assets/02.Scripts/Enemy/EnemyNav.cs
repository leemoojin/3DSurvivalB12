using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;
using static UnityEditor.PlayerSettings;

public enum AIState
{
    Idle,
    Wandering,
    Chasing,
    Attacking,
    Revenge,
    Lost,
    Invade
}

public class EnemyNav : MonoBehaviour
{

    public AIState aiState;
    private NavMeshAgent _agent;
    private Enemy _enemyInfo;
    private SateMark _sateMark;

    private float _toPlayerDistance;
    private float _viewAngle;
    private float _viewRadius;
    private float _roamRadius;
    private float _walkSpeed;
    private bool _isDayTimeMode;
    private bool _isInvadeArrive;

    private Vector3 _startPosition;

    //public GameObject curInteractGameObject;
    // 하나로 합쳐서 구현하도록 수정**
    public RaycastHit curHit;
    public ViewCastInfo curViewCastInfo; 


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _enemyInfo = GetComponent<Enemy>();
        _sateMark = GetComponent<SateMark>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //_startPosition = EnemyManager.Instance.enemyInfo.startPosition;
        //_roamRadius = EnemyManager.Instance.enemyInfo.roamRadius;
        //_walkSpeed = EnemyManager.Instance.enemyInfo.walkSpeed;

        _startPosition = _enemyInfo.startPosition;
        _roamRadius = _enemyInfo.roamRadius;
        _walkSpeed = _enemyInfo.walkSpeed;
        _isInvadeArrive = _enemyInfo.isInvadeArrive;
        _viewRadius = _enemyInfo.viewRadius;

        aiState = AIState.Wandering;
        //SetState(_aiState);
        //_sateMark.CloseStateMark();

    }

    // Update is called once per frame
    void Update()
    {
        //_toPlayerDistance = EnemyManager.Instance.enemyInfo.toPlayerDistance;
        //_isDayTimeMode = EnemyManager.Instance.enemyInfo.isDayTimeMode;

        _toPlayerDistance = _enemyInfo.toPlayerDistance;
        _isDayTimeMode = _enemyInfo.isDayTimeMode;

        // 낮, 밤에 따른 몬스터들의 상태변화 임시 코드 - 추후 수정**
        if (!_isDayTimeMode && !_isInvadeArrive)
        {
            aiState = AIState.Invade;
        }

        SetState(aiState);
       
    }

    private void SetState(AIState state)
    {        

        switch (state)
        {
            case AIState.Idle:
                //PassiveUpdate();
                break;
            case AIState.Wandering:
                _agent.speed = _walkSpeed;
                _agent.isStopped = false;
                WanderingUpdate();
                break;
            case AIState.Chasing:
                _agent.speed = _walkSpeed;
                _agent.isStopped = false;
                ChasingUpdate();
                //AttackingUpdate();
                break;
            case AIState.Attacking:
                _agent.isStopped = true;
                AttackingUpdate();
                break;
            case AIState.Revenge:
                //FleeingUpdate();
                break;
            case AIState.Lost:
                //FleeingUpdate();
                break;
            case AIState.Invade:
                InvadeUpdate();
                _agent.isStopped = false;
                break;
        }

        //animator.speed = agent.speed / walkSpeed;
    }

    private void AttackingUpdate()
    {
        Debug.Log("공격 시작");
    }

    private void WanderingUpdate()
    {
        Debug.Log($"EnemyNav.cs - WanderingUpdate()");


        if (_isDayTimeMode)
        {
            if (_agent.remainingDistance < 0.1f)
            {
                //_agent.SetDestination(GetRandomPoint(_startPosition, _roamRadius));

                Vector3 newPos = GetRandomPoint(_startPosition, _roamRadius);
                _agent.SetDestination(newPos);
            }

            aiState = AIState.Wandering;
        }
        else 
        {
            if (_agent.remainingDistance < 0.1f)
            {
                //_agent.SetDestination(GetRandomPoint(_startPosition, _roamRadius));

                Vector3 newPos = GetRandomPoint(transform.position, _viewRadius);
                _agent.SetDestination(newPos);
            }

            aiState = AIState.Wandering;

        }

        
    }

    private void ChasingUpdate()
    {
        if (_agent.remainingDistance < 0.1f)
        {
            Debug.Log($"EnemyNav.cs - ChasingUpdate()");

            _sateMark.ShowExclamationMark();
            _agent.SetDestination(curHit.point);

        }

        FindEnemy();
    }

    private void FindEnemy()
    {
        float toEnemy = Vector3.Distance(transform.position, curHit.point);
        Debug.Log(IsFind(curHit.collider.gameObject.transform.position));

        if (_viewRadius > toEnemy && IsFind(curHit.collider.gameObject.transform.position))
        {
            Debug.Log($"EnemyNav.cs - FindEnemy() - 공격 범위 안");

            aiState = AIState.Attacking;
        }
    }


    private void InvadeUpdate()
    {
        Debug.Log($"EnemyNav.cs - InvadeUpdate()");


        if (_agent.remainingDistance < 0.1f)
        {
            Debug.Log($"EnemyNav.cs - InvadeUpdate() - 도착");

            aiState = AIState.Wandering;
            _isInvadeArrive = true;
            Debug.Log($"EnemyNav.cs - InvadeUpdate() - _aiState: {aiState}");

            return;
        }

        int waveZoneArea = 1 << NavMesh.GetAreaFromName("WaveZone");
        int farmingZoneArea = 1 << NavMesh.GetAreaFromName("FarmingZone");
        int safeZoneArea = 1 << NavMesh.GetAreaFromName("SafeZone");

        int combinedMask = waveZoneArea | farmingZoneArea | safeZoneArea;

        _agent.areaMask = combinedMask;
        //_agent.SetDestination(EnemyManager.Instance.enemyInfo.Target.position);
        _agent.SetDestination(_enemyInfo.Target.position);

            
    }


    private Vector3 GetRandomPoint(Vector3 center, float radius)
    {

        Vector3 randomPos = Random.insideUnitSphere * radius;
        randomPos.y = 0;
        randomPos += center;

        // 내비게이션 메쉬 상에서 유효한 위치 샘플링
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, radius, NavMesh.AllAreas))
        {
            //hit.position = new Vector3(hit.position.x, 0, hit.position.z);
            //Debug.Log($"EnemyNav.cs - GetRandomPoint() - hit.position: {hit.position}");
            return hit.position;
        }


        return randomPos;
    }

    
    bool IsFind(Vector3 enemy)
    {        
        Vector3 directionToEnemy = enemy - transform.position;        
        float angle = Vector3.Angle(transform.forward, directionToEnemy);        
        return angle < _viewAngle * 0.5f;
    }




}
