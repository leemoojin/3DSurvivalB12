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
    public NavMeshAgent agent;
    private Enemy _enemy;
    private SateMark _sateMark;
    private EnemyAnimation _animationManager;

    private Coroutine _coroutine;

    private float _toPlayerDistance;
    private float _viewAngle;
    private float _viewRadius;
    private float _roamRadius;
    private float _runSpeed;
    private float _walkSpeed;

    private float _attackDistance;
    private bool _isDayTimeMode;
    private bool _isInvadeArrive;

    private Vector3 _startPosition;

    //public GameObject curInteractGameObject;
    // 하나로 합쳐서 구현하도록 수정**
    //public RaycastHit curHit;
    public ViewCastInfo curViewCastInfo;
    // 추적 대상 여부
    public bool isChase = false;



    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _enemy = GetComponent<Enemy>();
        _sateMark = GetComponent<SateMark>();
        _animationManager = GetComponent<EnemyAnimation>();


    }

    // Start is called before the first frame update
    void Start()
    {
        //_startPosition = EnemyManager.Instance.enemyInfo.startPosition;
        //_roamRadius = EnemyManager.Instance.enemyInfo.roamRadius;
        //_walkSpeed = EnemyManager.Instance.enemyInfo.walkSpeed;

        _startPosition = _enemy.startPosition;
        _roamRadius = _enemy.roamRadius;
        _walkSpeed = _enemy.walkSpeed;
        _runSpeed = _enemy.runSpeed;
        _isInvadeArrive = _enemy.isInvadeArrive;
        _viewRadius = _enemy.viewRadius;
        _viewAngle = _enemy.viewAngle;
        _attackDistance = _enemy.attackDistance;

        aiState = AIState.Wandering;
        //SetState(_aiState);
        //_sateMark.CloseStateMark();

    }

    // Update is called once per frame
    void Update()
    {        
        //_isDayTimeMode = EnemyManager.Instance.enemyInfo.isDayTimeMode;

        _toPlayerDistance = _enemy.toPlayerDistance;
        _isDayTimeMode = _enemy.isDayTimeMode;        


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
                Debug.Log("EnemyNav.cs -SetState() - 대기");
                agent.isStopped = true;     
                aiState = AIState.Wandering;
                break;
            case AIState.Wandering:
                agent.speed = _walkSpeed;
                agent.isStopped = false;
                WanderingUpdate();
                break;
            case AIState.Chasing:
                agent.speed = _runSpeed;
                agent.isStopped = false;
                ChasingUpdate();                
                break;
            case AIState.Attacking:                
                agent.isStopped = true;
                AttackingUpdate();
                break;
            case AIState.Revenge:
                //FleeingUpdate();
                break;
            case AIState.Lost:
                //FleeingUpdate();
                break;
            case AIState.Invade:
                agent.speed = _runSpeed;
                InvadeUpdate();
                agent.isStopped = false;
                break;
        }

        //animator.speed = agent.speed / walkSpeed;
    }

    private void AttackingUpdate()
    {
        // 공격 범위 안에 있으면 공격
        Debug.Log("EnemyNav.cs - AttackingUpdate()");
        

        Transform target = curViewCastInfo.hit.collider.gameObject.transform;
        float toEnemy = Vector3.Distance(transform.position, target.position);


        if (!_animationManager.isAttackPlaying)
        {
            // 목표 방향 계산
            Vector3 direction = target.position - transform.position;
            // 목표 회전 계산
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 2f * Time.deltaTime);
        }

        if (curViewCastInfo.hit.collider.gameObject.layer == 9)
        {
            if (curViewCastInfo.hit.collider.gameObject.GetComponent<WallCraftItem>().health == 0)
            {
                aiState = AIState.Idle;
                isChase = false;
                return;
            }
        }
        
        // 공격범위를 벗어나면 추적
        if (_attackDistance < toEnemy)
        {
            Debug.Log("EnemyNav.cs - AttackingUpdate() - 공격 벗어남 다시 추적");
            aiState = AIState.Chasing;
            return;
        }

        // 목표물을 놓쳤을때
        if (IsLostEnemy())
        {
            return;
        }


    }

    private void WanderingUpdate()
    {
        Debug.Log($"EnemyNav.cs - WanderingUpdate()");


        if (_isDayTimeMode)
        {
            if (agent.remainingDistance < 0.1f)
            {
                //_agent.SetDestination(GetRandomPoint(_startPosition, _roamRadius));

                Vector3 newPos = GetRandomPoint(_startPosition, _roamRadius);
                agent.SetDestination(newPos);
            }

            aiState = AIState.Wandering;
        }
        else 
        {
            if (agent.remainingDistance < 0.1f)
            {
                //_agent.SetDestination(GetRandomPoint(_startPosition, _roamRadius));

                Vector3 newPos = GetRandomPoint(transform.position, _viewRadius);
                agent.SetDestination(newPos);
            }

            aiState = AIState.Wandering;
        }        
    }

    private void ChasingUpdate()
    {
        int player = LayerMask.NameToLayer("Player"); ;
        int craft = LayerMask.NameToLayer("Craft"); ;


        //Debug.Log($"EnemyNav.cs - ChasingUpdate()");
        //Debug.Log($"EnemyNav.cs - ChasingUpdate() - layer: {curViewCastInfo.hit.collider.gameObject.layer}");

        // 플레이어 쫓을때
        if (curViewCastInfo.hit.collider.gameObject.layer == player)
        {
            Debug.Log($"EnemyNav.cs - ChasingUpdate() - 플레이어 추적");
            //Debug.Log($"EnemyNav.cs - ChasingUpdate() - _viewRadius: {_viewRadius}, _toPlayerDistance: {_toPlayerDistance}");


            _sateMark.ShowExclamationMark();
        }

        // 장애물 쫓을때
        if (curViewCastInfo.hit.collider.gameObject.layer == craft)
        {
            Debug.Log($"EnemyNav.cs - ChasingUpdate() - 장애물 추적");
            
        }

        // 목표물로 접근
        agent.SetDestination(curViewCastInfo.hit.collider.gameObject.transform.position);


        if (IsLostEnemy())
        {
            return;
        }

        CatchEnemy();
    }

    private void CatchEnemy()
    {
        //Debug.Log($"EnemyNav.cs - CatchEnemy()");     

        float toEnemy = Vector3.Distance(transform.position, curViewCastInfo.hit.collider.gameObject.transform.position);
        //Debug.Log($"EnemyNav.cs - FindEnemy() - {curViewCastInfo.point}");

        //Debug.Log($"EnemyNav.cs - CatchEnemy() - _attackDistance: {_attackDistance}, toEnemy: {toEnemy}, IsFind: {IsFind(curViewCastInfo.hit.collider.gameObject.transform.position)}");

        if (_attackDistance >= toEnemy && IsFind(curViewCastInfo.hit.collider.gameObject.transform.position))
        {
            Debug.Log($"EnemyNav.cs - CatchEnemy() - 공격 범위 안");

            aiState = AIState.Attacking;
        }
    }

    private bool IsLostEnemy() 
    {   
        int player = LayerMask.NameToLayer("Player"); ;

        // 목표물을 놓쳤을 때
        if (_viewRadius < _toPlayerDistance && curViewCastInfo.hit.collider.gameObject.layer == player)
        {
            Debug.Log($"EnemyNav.cs - ChasingUpdate() - 플레이어 놓침");
            //Debug.Log($"EnemyNav.cs - ChasingUpdate() - _viewRadius: {_viewRadius}, _toPlayerDistance: {_toPlayerDistance}");

            _coroutine = StartCoroutine(_sateMark.ShowQuestionMark());            
            aiState = AIState.Idle;
            isChase = false;
            return true;
        }

        return false;
    }


    private void InvadeUpdate()
    {
        Debug.Log($"EnemyNav.cs - InvadeUpdate()");
        isChase = false;
        _isInvadeArrive = true;


        if (agent.remainingDistance < 0.1f)
        {
            Debug.Log($"EnemyNav.cs - InvadeUpdate() - 도착");

            aiState = AIState.Wandering;
            Debug.Log($"EnemyNav.cs - InvadeUpdate() - _aiState: {aiState}");

            return;
        }

        int waveZoneArea = 1 << NavMesh.GetAreaFromName("WaveZone");
        int farmingZoneArea = 1 << NavMesh.GetAreaFromName("FarmingZone");
        int safeZoneArea = 1 << NavMesh.GetAreaFromName("SafeZone");

        int combinedMask = waveZoneArea | farmingZoneArea | safeZoneArea;

        agent.areaMask = combinedMask;
        //_agent.SetDestination(EnemyManager.Instance.enemyInfo.Target.position);
        agent.SetDestination(_enemy.Target.position);

            
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

    
    private bool IsFind(Vector3 enemy)
    {        
        Vector3 directionToEnemy = enemy - transform.forward;        
        float angle = Vector3.Angle(transform.position, directionToEnemy);        
        return angle < _viewAngle * 0.5f;
    }


    //private void Tension()
    //{
    //    Debug.Log($"EnemyNav.cs - Tension() - {_toPlayerDistance} < {_viewRadius} , {IsFind(CharacterManager.Instance.Player.transform.position)}");


    //    if (_toPlayerDistance < _viewRadius && !IsFind(CharacterManager.Instance.Player.transform.position))
    //    {
    //        Debug.Log($"EnemyNav.cs - Tension() - 근처에 플레이어 있음");

    //        _coroutine = StartCoroutine(_sateMark.ShowQuestionMark());

    //    }
    //}

}
